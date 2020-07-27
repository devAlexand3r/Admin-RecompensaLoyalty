using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using context = System.Web.HttpContext;

namespace JulioLoyalty.Model
{
    /// <summary>  
    /// Summary description for ExceptionLogging  
    /// </summary> 
    public static class ExceptionLogging
    {
        private static string ErrorlineNo, Errormsg, extype, exurl, hostIp, ErrorLocation, HostAdd;

        public static void SendErrorToText(DbEntityValidationException ex)
        {
            var line = Environment.NewLine + Environment.NewLine;

            ErrorlineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            Errormsg = ex.GetType().Name.ToString();
            extype = ex.GetType().ToString();
            exurl = context.Current.Request.Url.ToString();
            ErrorLocation = ex.Message.ToString();
            hostIp = context.Current.Request.UserHostAddress;
            try
            {
                //Text File Path
                string filepath = context.Current.Server.MapPath("~/AuditFiles/");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var date = DateTime.Now; // Text File Name
                filepath += $"DbEntity {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").Replace(':', '-')}.txt";

                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }

                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine("                  Exception Details on: " + DateTime.Now.ToString() + "");
                    sw.WriteLine("-----------------------------------------------------------------------------------");
                    sw.WriteLine();
                    sw.WriteLine("Error Line No   : " + ErrorlineNo);
                    sw.WriteLine("Error Message   : " + Errormsg);
                    sw.WriteLine("Exception Type  : " + extype);
                    sw.WriteLine("Error Location  : " + ErrorLocation);
                    sw.WriteLine("Error Page Url  : " + exurl);
                    sw.WriteLine("User Host IP    : " + hostIp);
                    sw.WriteLine();
                    sw.WriteLine("--------------------------------*Details*------------------------------------------");
                    sw.WriteLine();
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        var entity_type = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        sw.WriteLine(entity_type);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            sw.WriteLine(string.Format("- Property: \"{0}\"", ve.PropertyName));
                            sw.WriteLine(string.Format("- Error: \"{0}\"", ve.ErrorMessage));
                        }
                        var entry = eve.Entry;
                        // Rollback changes
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                entry.State = EntityState.Detached;
                                break;
                            case EntityState.Modified:
                                entry.CurrentValues.SetValues(entry.OriginalValues);
                                entry.State = EntityState.Unchanged;
                                break;
                            case EntityState.Deleted:
                                entry.State = EntityState.Unchanged;
                                break;
                        }
                    }
                    sw.WriteLine();
                    sw.WriteLine("--------------------------------*End exception*------------------------------------");
                    sw.Flush();
                    sw.Close();

                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
    }
}
