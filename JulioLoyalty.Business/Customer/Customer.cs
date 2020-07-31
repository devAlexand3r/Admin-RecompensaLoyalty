using System;
using System.Collections.Generic;
using System.Linq;
using JulioLoyalty.Business.Parameters;
using JulioLoyalty.Entities;
using JulioLoyalty.Model;
using JulioLoyalty.Model.EntitiesModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data;
using JulioLoyalty.Entities.MailChimp;
using System.Configuration;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Customer
{
    public class Customer : IRepositoryCustomer
    {
        private ResultJson result = new ResultJson();
        public Customer() { }

        public ResultJson AddCustomer(RequestCustomer customer)
        {
            try
            {
                using (DbContextJulio db = new DbContextJulio())
                {
                    //Verificar existencia (número de tarjeta) en almacen_tarjetas
                    var _almacenTarjetas = db.almacen_tarjetas.Where(s => s.tarjeta == customer.Num_tarjeta).FirstOrDefault();
                    if (_almacenTarjetas == null)
                    {
                        result.Success = false;
                        result.Message = $"El número de tarjeta ({customer.Num_tarjeta}) no existe en el catalogo almacen_tarjetas";
                        return result;
                    }
                    // Validar fecha de nacimiento
                    DateTime maxDate = new DateTime((DateTime.Now.Year - 18), 1, 1);
                    DateTime minDate = new DateTime((DateTime.Now.Year - 70), 1, 1);
                    if (customer.Fecha_nacimiento != null)
                    {
                        DateTime birthday = new DateTime(customer.Fecha_nacimiento.Value.Year, customer.Fecha_nacimiento.Value.Month, customer.Fecha_nacimiento.Value.Day);
                        if (birthday > maxDate)
                        {
                            result.Success = false;
                            result.Message = $"La fecha de nacimiento no puede ser mayor a {maxDate.ToString("dd/MM/yyyy")}";
                            return result;
                        }
                        else if (birthday < minDate)
                        {
                            result.Success = false;
                            result.Message = $"La fecha de nacimiento no puede ser menor a {minDate.ToString("dd/MM/yyyy")}";
                            return result;
                        }
                    }
                    // Verificar si la tarjeta ya esta relacionada con un participante y con status activo
                    var _participanteTarjeta = db.participante_tarjeta.Where(s => s.tarjeta == customer.Num_tarjeta).FirstOrDefault();
                    if (_participanteTarjeta != null)
                    {
                        var _Participante = db.participante.Find(_participanteTarjeta.participante_id);
                        if (_Participante.status_participante_id == 2)
                        {
                            // Número de tarjeta -> participante activo
                            result.Success = false;
                            result.Message = $"El número de tarjeta({customer.Num_tarjeta}) esta en uso, intentelo con una núeva";
                            return result;
                        }
                        if (_Participante.status_participante_id != 1 && _Participante.status_participante_id != 3 && _Participante.status_participante_id != 5)
                        {
                            result.Success = false;
                            result.Message = $"Por el momento la tarjeta no se encuentra disponible, intente con una nueva tarjeta.";
                            return result;
                        }
                        // Tarjeta valida -> participante
                        #region Registrar, Actualizar datos del participante
                        var _userRegistro = db.AspNetUsers.Where(s => s.UserName == customer.UserName).FirstOrDefault();
                        // Actualizar datos del participante
                        _Participante.clave = customer.Num_tarjeta;
                        _Participante.nombre = customer.Nombre;
                        _Participante.apellido_paterno = customer.Ape_paterno;
                        _Participante.apellido_materno = customer.Ape_materno;
                        _Participante.correo_electronico = customer.Correo;
                        _Participante.fecha_nacimiento = customer.Fecha_nacimiento;
                        _Participante.tipo_participante_id = 2;
                        _Participante.usuario_alta = Guid.Parse(_userRegistro.Id); // Usuario que registra el participante                       
                        _Participante.status_participante_id = 2;// 2 => Participante activo
                        _Participante.fecha_status = DateTime.Now;
                        _Participante.ocupacion_id = customer.Ocupacion_id;
                        db.Entry(_Participante).State = System.Data.Entity.EntityState.Modified;
                        if (string.IsNullOrEmpty(customer.Tel_celular))
                        {
                            customer.Tel_celular = "0";
                        }
                        #region Actualizar o agregar participante direccion
                        var direc = db.participante_direccion.Where(s => s.participante_id == _Participante.id).FirstOrDefault();
                        if (direc != null)
                        {
                            direc.estado = customer.Estado;
                            direc.fecha_status = DateTime.Now;
                            direc.codigo_postal = customer.Codigo_postal;
                            db.Entry(direc).State = System.Data.Entity.EntityState.Modified;
                        }
                        else
                        {
                            // No existe datos en participante dirección, Agregar
                            participante_direccion partDireccion = new participante_direccion()
                            {
                                tipo_direccion_id = 1,
                                participante_id = _Participante.id,
                                codigo_postal = customer.Codigo_postal,
                                status_id = 1, // Activo 
                                fecha_status = DateTime.Now,
                                usuario_id = Guid.Parse(_userRegistro.Id),
                                estado = customer.Estado,
                            };
                            db.participante_direccion.Add(partDireccion);
                            db.SaveChanges();
                        }
                        #endregion
                        #region Actualizar o agregar participante telefonos
                        var _participanteTelefonos = db.participante_telefono.Where(s => s.participante_id == _Participante.id).ToList();
                        if (_participanteTelefonos.Count > 0)
                        {
                            foreach (var _objTelefono in _participanteTelefonos)
                            {
                                if (string.IsNullOrEmpty(_objTelefono.telefono))
                                {
                                    _objTelefono.telefono = "0";
                                }
                                if (string.IsNullOrEmpty(_objTelefono.lada))
                                {
                                    _objTelefono.lada = "0";
                                }
                                if (_objTelefono.tipo_telefono_id == 3)
                                    _objTelefono.telefono = customer.Tel_celular;
                                db.Entry(_objTelefono).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                        else
                        {
                            // No existe datos en participante telefonos, Agregar
                            List<participante_telefono> partTelefono = new List<participante_telefono>();
                            partTelefono.Add(new participante_telefono()
                            {
                                tipo_telefono_id = 3,
                                participante_id = _Participante.id,
                                lada = "0",
                                telefono = customer.Tel_celular,
                            });
                            db.participante_telefono.AddRange(partTelefono);
                            db.SaveChanges();
                        }
                        #endregion
                        // Número de tarjeta ->  Le asignamos un número de participante
                        _almacenTarjetas.participante_id = _Participante.id;
                        db.Entry(_almacenTarjetas).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        #endregion
                        string _userId = "";
                        using (ApplicationDbContext context = new ApplicationDbContext())
                        {
                            #region Registro para al acceso
                            var year = DateTime.Now.Year;
                            if (customer.Fecha_nacimiento != null)
                            {
                                year = Convert.ToDateTime(customer.Fecha_nacimiento).Year;
                            }
                            // Generar password, 3 ultimos digitos (tarjeta) + ? + 2 primeras letras (nombre) + - + año de nacimiento
                            string password = $"{customer.Num_tarjeta.Substring((customer.Num_tarjeta.Length - 3), 3)}?{customer.Nombre.Substring(0, 2).ToUpper()}-{year}";
                            if (customer.Fecha_nacimiento != null)
                            {
                                password = customer.Fecha_nacimiento.Value.ToString("ddMMyyyy");
                            }
                            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                            var user = new ApplicationUser()
                            {
                                UserName = customer.Num_tarjeta,
                                Email = customer.Correo,
                                EmailConfirmed = false,
                                Profiles = new Profiles()
                                {
                                    FirstName = customer.Nombre,
                                    MiddleName = customer.Ape_paterno,
                                    LastName = customer.Ape_materno,
                                    Age = 0
                                }
                            };
                            var userResult = UserManager.Create(user, password);
                            userResult = UserManager.SetLockoutEnabled(user.Id, false);
                            _userId = user.Id;
                            var rolesForUser = UserManager.GetRoles(user.Id);
                            if (!rolesForUser.Contains("Participante"))
                            {
                                var result = UserManager.AddToRole(user.Id, "Participante");
                            }
                            #endregion
                            Business.EmailService.EParameters parameterE = new Business.EmailService.EParameters()
                            {
                                IdUnique = 1,
                                FullName = $"{customer.Nombre} {customer.Ape_paterno}",
                                Email = customer.Correo,
                                Link = customer.URL,
                                currentTime = DateTime.Now,
                                UserName = customer.Num_tarjeta,
                                Password = password
                            };
                            Business.EmailService.IRepositoryEmail service = new Business.EmailService.Email(new Business.EmailService.Templates(1), parameterE);
                            if (!string.IsNullOrEmpty(customer.Correo))
                            {
                                //service.SubmitExit();
                                // Busca en la tabla historico_emails el correo y si esta actualiza el participante_id y status
                                var _historico_emails = db.historico_emails.Where(s => s.correo_electronico == customer.Correo && s.participante_id == 0).FirstOrDefault();
                                if (_historico_emails != null)
                                {
                                    _historico_emails.participante_id = _Participante.id;
                                    _historico_emails.status = "OK";
                                    db.Entry(_historico_emails).State = System.Data.Entity.EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }
                        // Guardar relación AspNetUsers -> Participante
                        AspNetUsers_Participante _userParticipante = new AspNetUsers_Participante()
                        {
                            UserId = _userId,
                            ParticipanteId = _Participante.id,
                        };
                        db.AspNetUsers_Participante.Add(_userParticipante);
                        db.SaveChanges();
                        // Busca en la tabla historico_emails el correo y si esta actualiza el participante_id y status
                        if (!string.IsNullOrEmpty(customer.Correo))
                        {
                            var _historico_emails = db.historico_emails.Where(s => s.correo_electronico == customer.Correo && s.participante_id == 0).FirstOrDefault();
                            if (_historico_emails != null)
                            {
                                _historico_emails.participante_id = _Participante.id;
                                _historico_emails.status = "OK";
                                db.Entry(_historico_emails).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        try
                        {
                            // Después de guardar los datos, hay que actualizar la lista MailChimp
                            Funciones.csMailChimp lista = new Funciones.csMailChimp();
                            cRespAddOrUpdateListMember.RootObject parsed = new cRespAddOrUpdateListMember.RootObject();
                            cMerge_Fields merge_fields = new cMerge_Fields();
                            merge_fields.EMAIL = customer.Correo; // Es el correo_electronico a cambiar
                            merge_fields.NUMSOCIA = customer.Num_tarjeta;
                            merge_fields.NOMBRE = customer.Nombre;
                            merge_fields.APESOCIA = customer.Ape_paterno + ' ' + customer.Ape_materno;
                            merge_fields.MMERGE18 = DateTime.Parse(customer.Fecha_nacimiento.ToString()).ToString("MM/dd");  // Formato yyyy-mm-dd                         
                            merge_fields.MOVIL1 = customer.Tel_celular;
                            merge_fields.IDLOYALTY = "LOYALTY";
                            //var _ocupacion = db.ocupacion.Where(s => s.id == customer.Ocupacion_id).FirstOrDefault();
                            //merge_fields.OCUPA = _ocupacion.descripcion;
                            //merge_fields.CP = customer.Codigo_postal;
                            merge_fields.ESTSOCIA = customer.Estado;
                            string sLista = lista.AddOrUpdateListMember(ConfigurationManager.AppSettings["DataCenter"].ToString(), ConfigurationManager.AppSettings["MailChimpApiKey"].ToString(), ConfigurationManager.AppSettings["List_Id"].ToString(), merge_fields, "subscribed", customer.Correo); // Este es el correo_electronico actual
                            parsed = (cRespAddOrUpdateListMember.RootObject)JsonConvert.DeserializeObject(sLista, typeof(cRespAddOrUpdateListMember.RootObject));
                        }
                        catch (Exception ex)
                        {
                        }
                        // Envia Mail de Bienvenida al Participante Recien Registrado	
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("@tarjeta", customer.Num_tarjeta);
                        parameters.Add("@tienda", "NEZ, TLH, PNT, LRM, OGD, PAG, PUE, LM2");
                        DataSet setTables = db.GetDataSet("[dbo].[usp_Consulta_Plantilla_Bienvenida]", CommandType.StoredProcedure, parameters);
                        DataTable dtConsulta = setTables.Tables[0];
                        if (dtConsulta != null)
                        {
                            if (dtConsulta.Rows.Count > 0)
                            {
                                Funciones.envioMail envio = new Funciones.envioMail();
                                switch (dtConsulta.Rows[0]["numPlantilla"].ToString())
                                {
                                    case "1":
                                        envio.envioMailParticipante("~/Plantillas/avisoBienvenida100.html", "¡Bienvenida a JULIO Loyalty! ¡Has recibido 100 puntos!", customer.Nombre, customer.Correo);
                                        break;
                                    case "2":
                                        envio.envioMailParticipante("~/Plantillas/avisoBienvenida10Porc.html", "¡Bienvenida a JULIO Loyalty! ¡Recibiste un bono especial!", customer.Nombre, customer.Correo);
                                        break;
                                    case "3":
                                        envio.envioMailParticipante("~/Plantillas/avisoBienvenida5Porc.html", "¡Bienvenida a JULIO Loyalty!", customer.Nombre, customer.Correo);
                                        break;
                                }
                            }
                        }
                        //
                        result.Message = "Registro de socia exitoso";
                        return result;
                    }
                    else
                    {
                        // No existe ningun participante
                        result.Success = false;
                        result.Message = "La tarjeta no tiene ventas registradas";
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error en la transacción, vuelva intentarlo mas tarde";
                result.InnerException = $"{ex.Message}";
                return result;
            }
        }

        public ResultJson UpdateCustomer(RequestCustomer customer)
        {
            try
            {
                using (DbContextJulio db = new DbContextJulio())
                {
                    // Para Actualizar la lista Mailchimp se requiere obtener el correo_electronico actual, antes de actualizar dicho correo
                    var _correo_actual = db.participante.Where(s => s.clave == customer.Num_tarjeta).FirstOrDefault();
                    string correo_actual = _correo_actual.correo_electronico;
                    var _userActualiza = db.AspNetUsers.Where(s => s.UserName == customer.UserName).FirstOrDefault();
                    // Validar fecha de nacimiento
                    DateTime maxDate = new DateTime((DateTime.Now.Year - 18), 1, 1);
                    DateTime minDate = new DateTime((DateTime.Now.Year - 70), 1, 1);
                    if (customer.Fecha_nacimiento != null)
                    {
                        DateTime birthday = new DateTime(customer.Fecha_nacimiento.Value.Year, customer.Fecha_nacimiento.Value.Month, customer.Fecha_nacimiento.Value.Day);
                        if (birthday > maxDate)
                        {
                            result.Success = false;
                            result.Message = $"La fecha de nacimiento no puede ser mayor a {maxDate.ToString("dd/MM/yyyy")}";
                            return result;
                        }
                        else if (birthday < minDate)
                        {
                            result.Success = false;
                            result.Message = $"La fecha de nacimiento no puede ser menor a {minDate.ToString("dd/MM/yyyy")}";
                            return result;
                        }
                    }
                    var _Participante = db.participante.Find(customer.Id);
                    if (_Participante == null)
                    {
                        result.Success = false;
                        result.Message = "Operación cancelada, Socia no encontrado";
                        return result;
                    }
                    _Participante.clave = customer.Num_tarjeta;
                    _Participante.nombre = customer.Nombre;
                    _Participante.apellido_paterno = customer.Ape_paterno;
                    _Participante.apellido_materno = customer.Ape_materno;
                    _Participante.fecha_nacimiento = customer.Fecha_nacimiento;
                    _Participante.correo_electronico = customer.Correo;
                    _Participante.ocupacion_id = customer.Ocupacion_id;
                    if (_Participante.status_participante_id == 3 || _Participante.status_participante_id == 5)
                    {
                        _Participante.status_participante_id = 2;
                    }
                    db.Entry(_Participante).State = System.Data.Entity.EntityState.Modified;
                    if (string.IsNullOrEmpty(customer.Tel_celular))
                    {
                        customer.Tel_celular = "0";
                    }
                    #region Actualizar o agregar participante direccion
                    var direc = db.participante_direccion.Where(s => s.participante_id == _Participante.id).FirstOrDefault();
                    if (direc != null)
                    {
                        direc.estado = customer.Estado;
                        direc.codigo_postal = customer.Codigo_postal;
                        db.Entry(direc).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        // No existe datos en participante dirección, Agregar
                        participante_direccion partDireccion = new participante_direccion()
                        {
                            tipo_direccion_id = 1,
                            participante_id = _Participante.id,
                            codigo_postal = customer.Codigo_postal,
                            status_id = 1, // Activo 
                            fecha_status = DateTime.Now,
                            usuario_id = Guid.Parse(_userActualiza.Id),
                            estado = customer.Estado,
                        };
                        db.participante_direccion.Add(partDireccion);
                        db.SaveChanges();
                    }
                    #endregion
                    #region Actualizar o agregar participante telefonos                    
                    var _participanteTelefonoCel = db.participante_telefono.Where(s => s.participante_id == _Participante.id && s.tipo_telefono_id == 3).ToList();
                    if (_participanteTelefonoCel.Count > 0)
                    {
                        foreach (var _objTelefono in _participanteTelefonoCel)
                        {
                            if (string.IsNullOrEmpty(_objTelefono.telefono))
                            {
                                _objTelefono.telefono = "0";
                            }
                            if (string.IsNullOrEmpty(_objTelefono.lada))
                            {
                                _objTelefono.lada = "0";
                            }
                            if (_objTelefono.telefono != "0")
                                _objTelefono.telefono = customer.Tel_celular;
                            if (customer.Tel_celular != "0")
                                _objTelefono.telefono = customer.Tel_celular;
                            db.Entry(_objTelefono).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                    else
                    {
                        // No existe datos en participante telefonos, Agregar
                        List<participante_telefono> partTelefono = new List<participante_telefono>();
                        partTelefono.Add(new participante_telefono()
                        {
                            tipo_telefono_id = 3,
                            participante_id = _Participante.id,
                            lada = "0",
                            telefono = customer.Tel_celular,
                        });
                        db.participante_telefono.AddRange(partTelefono);
                        db.SaveChanges();
                    }
                    #endregion
                    var aspNetRelacion = db.AspNetUsers_Participante.Where(s => s.ParticipanteId == customer.Id).FirstOrDefault();
                    if (aspNetRelacion != null)
                    {
                        var _aspNetUser = db.AspNetUsers.Find(aspNetRelacion.UserId);
                        _aspNetUser.AspNetProfiles.FirstName = customer.Nombre;
                        _aspNetUser.AspNetProfiles.MiddleName = customer.Ape_paterno;
                        _aspNetUser.AspNetProfiles.LastName = customer.Ape_materno;
                        _aspNetUser.Email = customer.Correo;
                        db.Entry(_aspNetUser).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    db.SaveChanges();
                    // Busca en la tabla historico_emails el correo y si esta actualiza el participante_id y status
                    if (!string.IsNullOrEmpty(customer.Correo))
                    {
                        var _historico_emails = db.historico_emails.Where(s => s.correo_electronico == customer.Correo && s.participante_id == 0).FirstOrDefault();
                        if (_historico_emails != null)
                        {
                            _historico_emails.participante_id = _Participante.id;
                            _historico_emails.status = "OK";
                            db.Entry(_historico_emails).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    try
                    {
                        // Después de guardar los datos, hay que actualizar la lista MailChimp
                        Funciones.csMailChimp lista = new Funciones.csMailChimp();
                        cRespAddOrUpdateListMember.RootObject parsed = new cRespAddOrUpdateListMember.RootObject();
                        cMerge_Fields merge_fields = new cMerge_Fields();
                        merge_fields.EMAIL = customer.Correo; // Es el correo_electronico a cambiar
                        merge_fields.NUMSOCIA = customer.Num_tarjeta;
                        merge_fields.NOMBRE = customer.Nombre;
                        merge_fields.APESOCIA = customer.Ape_paterno + ' ' + customer.Ape_materno;
                        merge_fields.MMERGE18 = DateTime.Parse(customer.Fecha_nacimiento.ToString()).ToString("MM/dd");  // Formato yyyy-mm-dd                         //DateTime.Parse(customer.Fecha_nacimiento.ToString()).ToString("yyyy-MM-dd");  // Formato yyyy-mm-dd                         
                        merge_fields.MOVIL1 = customer.Tel_celular;
                        merge_fields.IDLOYALTY = "LOYALTY";
                        //var _ocupacion = db.ocupacion.Where(s => s.id == customer.Ocupacion_id).FirstOrDefault();
                        //merge_fields.OCUPA = _ocupacion.descripcion;
                        //merge_fields.CP = customer.Codigo_postal;
                        merge_fields.ESTSOCIA = customer.Estado;
                        string sLista = lista.AddOrUpdateListMember(ConfigurationManager.AppSettings["DataCenter"].ToString(), ConfigurationManager.AppSettings["MailChimpApiKey"].ToString(), ConfigurationManager.AppSettings["List_Id"].ToString(), merge_fields, "subscribed", correo_actual); // Este es el correo_electronico actual
                        parsed = (cRespAddOrUpdateListMember.RootObject)JsonConvert.DeserializeObject(sLista, typeof(cRespAddOrUpdateListMember.RootObject));
                        //
                    }
                    catch (Exception ex)
                    {

                    }
                }
                result.Message = "Actualización de socia exitoso";
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error en la transacción, vuelva intentarlo mas tarde";
                result.InnerException = $"{ex.Message}";
                return result;
            }
        }

        public ResultJson UnifyCustomer(RequestCustomer customer)
        {
            try
            {
                // Codigo replicado, registro de socia
                using (DbContextJulio db = new DbContextJulio())
                {
                    //Verificar existencia (número de tarjeta) en almacen_tarjetas
                    var _almacenTarjetas = db.almacen_tarjetas.Where(s => s.tarjeta == customer.Num_tarjeta).FirstOrDefault();
                    if (_almacenTarjetas == null)
                    {
                        result.Success = false;
                        result.Message = $"El número de tarjeta ({customer.Num_tarjeta}) no existe en el catalogo almacen_tarjetas";
                        return result;
                    }
                    // Validar fecha de nacimiento
                    DateTime maxDate = new DateTime((DateTime.Now.Year - 18), 1, 1);
                    DateTime minDate = new DateTime((DateTime.Now.Year - 70), 1, 1);
                    if (customer.Fecha_nacimiento != null)
                    {
                        DateTime birthday = new DateTime(customer.Fecha_nacimiento.Value.Year, customer.Fecha_nacimiento.Value.Month, customer.Fecha_nacimiento.Value.Day);
                        if (birthday > maxDate)
                        {
                            result.Success = false;
                            result.Message = $"La fecha de nacimiento no puede ser mayor a {maxDate.ToString("dd/MM/yyyy")}";
                            return result;
                        }
                        else if (birthday < minDate)
                        {
                            result.Success = false;
                            result.Message = $"La fecha de nacimiento no puede ser menor a {minDate.ToString("dd/MM/yyyy")}";
                            return result;
                        }
                    }
                    // Verificar si la tarjeta ya esta relacionada con un participante Y con status activo
                    var _participanteTarjeta = db.participante_tarjeta.Where(s => s.tarjeta == customer.Num_tarjeta).FirstOrDefault();
                    if (_participanteTarjeta != null)
                    {
                        var _Participante = db.participante.Find(_participanteTarjeta.participante_id);
                        if (_Participante.status_participante_id == 2)
                        {
                            // Número de tarjeta -> participante activo
                            result.Success = false;
                            result.Message = $"El número de tarjeta({customer.Num_tarjeta}) esta en uso, intentelo con una núeva";
                            return result;
                        }
                        if (_Participante.status_participante_id != 1 && _Participante.status_participante_id != 3 && _Participante.status_participante_id != 5)
                        {
                            result.Success = false;
                            result.Message = $"Por el momento la tarjeta no se encuentra disponible, intente con una nueva tarjeta.";
                            return result;
                        }
                        // Tarjeta valida -> participante
                        #region Registrar, Actualizar datos del participante
                        var _userRegistro = db.AspNetUsers.Where(s => s.UserName == customer.UserName).FirstOrDefault();
                        // Actualizar datos del participante
                        _Participante.clave = customer.Num_tarjeta;
                        _Participante.nombre = customer.Nombre;
                        _Participante.apellido_paterno = customer.Ape_paterno;
                        _Participante.apellido_materno = customer.Ape_materno;
                        _Participante.correo_electronico = customer.Correo;
                        _Participante.fecha_nacimiento = customer.Fecha_nacimiento;
                        _Participante.tipo_participante_id = 2;
                        _Participante.usuario_alta = Guid.Parse(_userRegistro.Id); // Usuario que registra el participante                       
                        _Participante.status_participante_id = 2;// 2 => Participante activo
                        _Participante.fecha_status = DateTime.Now;
                        _Participante.ocupacion_id = customer.Ocupacion_id;
                        db.Entry(_Participante).State = System.Data.Entity.EntityState.Modified;
                        if (string.IsNullOrEmpty(customer.Tel_celular))
                        {
                            customer.Tel_celular = "0";
                        }
                        #region Actualizar o agregar participante direccion
                        var direc = db.participante_direccion.Where(s => s.participante_id == _Participante.id).FirstOrDefault();
                        if (direc != null)
                        {
                            direc.estado = customer.Estado;
                            direc.fecha_status = DateTime.Now;
                            direc.codigo_postal = customer.Codigo_postal;
                            db.Entry(direc).State = System.Data.Entity.EntityState.Modified;
                        }
                        else
                        {
                            // No existe datos en participante dirección, Agregar
                            participante_direccion partDireccion = new participante_direccion()
                            {
                                tipo_direccion_id = 1,
                                participante_id = _Participante.id,
                                codigo_postal = customer.Codigo_postal,
                                status_id = 1, // Activo 
                                fecha_status = DateTime.Now,
                                usuario_id = Guid.Parse(_userRegistro.Id),
                                estado = customer.Estado,
                            };
                            db.participante_direccion.Add(partDireccion);
                            db.SaveChanges();
                        }
                        #endregion
                        #region Actualizar o agregar participante telefonos
                        var _participanteTelefonos = db.participante_telefono.Where(s => s.participante_id == _Participante.id).ToList();
                        if (_participanteTelefonos.Count > 0)
                        {
                            foreach (var _objTelefono in _participanteTelefonos)
                            {
                                if (string.IsNullOrEmpty(_objTelefono.telefono))
                                {
                                    _objTelefono.telefono = "0";
                                }
                                if (string.IsNullOrEmpty(_objTelefono.lada))
                                {
                                    _objTelefono.lada = "0";
                                }
                                if (_objTelefono.tipo_telefono_id == 3)
                                    _objTelefono.telefono = customer.Tel_celular;
                                db.Entry(_objTelefono).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                        else
                        {
                            // No existe datos en participante telefonos, Agregar
                            List<participante_telefono> partTelefono = new List<participante_telefono>();
                            partTelefono.Add(new participante_telefono()
                            {
                                tipo_telefono_id = 3,
                                participante_id = _Participante.id,
                                lada = "0",
                                telefono = customer.Tel_celular,
                            });
                            db.participante_telefono.AddRange(partTelefono);
                            db.SaveChanges();
                        }
                        #endregion
                        // Número de tarjeta ->  Le asignamos un número de participante
                        _almacenTarjetas.participante_id = _Participante.id;
                        db.Entry(_almacenTarjetas).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        #endregion
                        string _userId = "";
                        using (ApplicationDbContext context = new ApplicationDbContext())
                        {
                            #region Registro para al acceso
                            var year = DateTime.Now.Year;
                            if (customer.Fecha_nacimiento != null)
                            {
                                year = Convert.ToDateTime(customer.Fecha_nacimiento).Year;
                            }
                            // Generar password, 3 ultimos digitos (tarjeta) + ? + 2 primeras letras (nombre) + - + año de nacimiento
                            string password = $"{customer.Num_tarjeta.Substring((customer.Num_tarjeta.Length - 3), 3)}?{customer.Nombre.Substring(0, 2).ToUpper()}-{year}";
                            if (customer.Fecha_nacimiento != null)
                            {
                                password = customer.Fecha_nacimiento.Value.ToString("ddMMyyyy");
                            }
                            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                            var user = new ApplicationUser()
                            {
                                UserName = customer.Num_tarjeta,
                                Email = customer.Correo,
                                EmailConfirmed = true,
                                Profiles = new Profiles()
                                {
                                    FirstName = customer.Nombre,
                                    MiddleName = customer.Ape_paterno,
                                    LastName = customer.Ape_materno,
                                    Age = 0
                                }
                            };
                            var userResult = UserManager.Create(user, password);
                            userResult = UserManager.SetLockoutEnabled(user.Id, false);
                            _userId = user.Id;
                            var rolesForUser = UserManager.GetRoles(user.Id);
                            if (!rolesForUser.Contains("Participante"))
                            {
                                var result = UserManager.AddToRole(user.Id, "Participante");
                            }
                            #endregion
                            Business.EmailService.EParameters parameterE = new Business.EmailService.EParameters()
                            {
                                IdUnique = 1,
                                FullName = $"{customer.Nombre} {customer.Ape_paterno}",
                                Email = customer.Correo,
                                Link = customer.URL,
                                currentTime = DateTime.Now,
                                UserName = customer.Num_tarjeta,
                                Password = password
                            };
                            Business.EmailService.IRepositoryEmail service = new Business.EmailService.Email(new Business.EmailService.Templates(1), parameterE);
                            if (!string.IsNullOrEmpty(customer.Correo))
                            {
                                //service.UnifySubmitExit();
                                // Busca en la tabla historico_emails el correo y si esta actualiza el participante_id y status
                                var _historico_emails = db.historico_emails.Where(s => s.correo_electronico == customer.Correo && s.participante_id == 0).FirstOrDefault();
                                if (_historico_emails != null)
                                {
                                    _historico_emails.participante_id = _Participante.id;
                                    _historico_emails.status = "OK";
                                    db.Entry(_historico_emails).State = System.Data.Entity.EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }
                        // Guardar relación AspNetUsers -> Participante
                        AspNetUsers_Participante _userParticipante = new AspNetUsers_Participante()
                        {
                            UserId = _userId,
                            ParticipanteId = _Participante.id,
                        };
                        db.AspNetUsers_Participante.Add(_userParticipante);
                        db.SaveChanges();
                        result.Message = "Unificación de socia exitoso";
                        return result;
                    }
                    else
                    {
                        // No existe ningun participante
                        result.Success = false;
                        result.Message = "La tarjeta no tiene ventas registradas";
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error en la transacción, vuelva intentarlo mas tarde";
                result.InnerException = $"{ex.Message}";
                return result;
            }
        }

        public ResultJson AddUser(RequesUser objUser)
        {
            try
            {
                string usuarioId = "";
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                    var user = UserManager.FindByName(objUser.Username);
                    if (user != null)
                    {
                        result.Success = false;
                        result.Message = "El nombre de usuario esta en uso";
                        return result;
                    }
                    user = new ApplicationUser()
                    {
                        UserName = objUser.Username,
                        Email = objUser.Email,
                        EmailConfirmed = true,
                        Profiles = new Profiles()
                        {
                            FirstName = objUser.Nombre,
                            MiddleName = objUser.Ape_paterno,
                            LastName = objUser.Ape_materno,
                            Age = objUser.Age
                        }
                    };
                    var userResult = UserManager.Create(user, objUser.Password);
                    userResult = UserManager.SetLockoutEnabled(user.Id, false);
                    var rolesForUser = UserManager.GetRoles(user.Id);
                    usuarioId = user.Id;
                    var Roles = objUser.Roles.Split(',');
                    foreach (var rol in Roles)
                    {
                        if (!rolesForUser.Contains(rol))
                        {
                            var result = UserManager.AddToRole(user.Id, rol);
                            AddCustomMenu(user.Id, rol);
                        }
                    }
                }

                // Agregar distribuidores por usuario
                using (DbContextJulio db = new DbContextJulio())
                {
                    #region Eliminar relación de usuario distribuidor  
                    var oldDistribuidores = db.AspNetUsers_Distribuidor.Where(s => s.IdUser == usuarioId).ToList();
                    foreach (var distribuidor in oldDistribuidores)
                    {
                        db.Entry(distribuidor).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                    }
                    #endregion
                    #region Agregar la nueva relación de usuario distribuidor
                    if (!string.IsNullOrEmpty(objUser.Distribuidor))
                    {
                        var distribuidores = objUser.Distribuidor.Split(',');
                        List<AspNetUsers_Distribuidor> addDistribuidor = new List<AspNetUsers_Distribuidor>();
                        if (distribuidores.Count() > 0)
                        {
                            foreach (var item in distribuidores)
                            {
                                addDistribuidor.Add(new AspNetUsers_Distribuidor() { IdUser = usuarioId, IdDistribuidor = decimal.Parse(item) });
                            }
                            db.AspNetUsers_Distribuidor.AddRange(addDistribuidor);
                            db.SaveChanges();
                        }
                    }
                    #endregion
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error en la transacción, vuelva intentarlo mas tarde";
                result.InnerException = $"{ex.Message}";
                return result;
            }
        }

        public ResultJson UpdateUser(RequesUser objUser)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var user = db.Profiles.Find(objUser.Key);
                    if (user == null)
                    {
                        result.Success = false;
                        result.Message = "Usuario no encontrado";
                        return result;
                    }
                    user.FirstName = objUser.Nombre;
                    user.MiddleName = objUser.Ape_paterno;
                    user.LastName = objUser.Ape_materno;
                    user.ApplicationUser.Email = objUser.Email;
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error en la transacción, vuelva intentarlo mas tarde";
                result.InnerException = $"{ex.Message}";
                return result;
            }
        }

        private void AddCustomMenu(string key, string Rol)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@UserId", key);
                parameters.Add("@RoleName", Rol);
                DataSet setTables = context.GetDataSet("[dbo].[usp_menu_personalizado]", CommandType.StoredProcedure, parameters);
            }
        }

        public ResultJson UpdateStutus(RequestComments comm)
        {
            try
            {
                using (DbContextJulio db = new DbContextJulio())
                {
                    var _aspnetUsers = db.AspNetUsers.Where(s => s.UserName == comm.userName).FirstOrDefault();
                    var _Participante = db.participante.Find(comm.participante_id);
                    // Validar si es posible actualizar status del participante
                    bool isValid = true;
                    decimal[] ignore = new decimal[] { 8, 9, 10, 12, 16 };
                    foreach (decimal val in ignore)
                    {
                        if (val == _Participante.status_participante_id)
                        {
                            isValid = false;
                        }
                    }
                    // Actualizar status del participante
                    if (isValid)
                    {
                        _Participante.status_participante_id = comm.status_id;
                        db.Entry(_Participante).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        if (!string.IsNullOrEmpty(comm.comentarios))
                        {
                            participante_status_comentarios psc = new participante_status_comentarios()
                            {
                                participante_id = _Participante.id,
                                comentarios = comm.comentarios,
                                fecha_alta = DateTime.Now,
                                usuario_alta_id = Guid.Parse(_aspnetUsers.Id)
                            };
                            db.participante_status_comentarios.Add(psc);
                            db.SaveChanges();
                        }
                        result.acumula = _Participante.status_participante.acumula;
                        result.status = _Participante.status_participante.descripcion;
                        result.acumula_mensaje = _Participante.status_participante.acumula_mensaje;
                        result.Message = "El status del participante fue cambiado con exito";
                    }
                    else
                    {
                        // No actualizar status del participante
                        result.acumula = _Participante.status_participante.acumula;
                        result.status = _Participante.status_participante.descripcion;
                        result.acumula_mensaje = _Participante.status_participante.acumula_mensaje;
                        result.Success = false;
                        result.Message = "La actualización no se pudo realizar, comunicate a corporativo";
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error en la transacción, vuelva intentarlo mas tarde";
                result.InnerException = $"{ex.Message}";
                return result;
            }
        }

        public decimal ObtieneStatusPartipanteId(RequestCustomer customer)
        {
            decimal status_participante_id;
            try
            {
                using (DbContextJulio db = new DbContextJulio())
                {
                    var _participante_tarjeta = db.participante_tarjeta.Where(s => s.tarjeta == customer.Num_tarjeta && s.status_tarjeta_id == 1).FirstOrDefault();
                    if (_participante_tarjeta == null)
                        return 0;
                    else
                    {
                        var _participante = db.participante.Where(s => s.id == _participante_tarjeta.participante_id).FirstOrDefault();
                        if (_participante == null)
                            return 0;
                        else
                        {
                            status_participante_id = decimal.Parse(_participante.status_participante_id.ToString());
                            return status_participante_id;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool ActualizaStatusParticipante(RequestCustomer customer, decimal status_participante_id)
        {
            try
            {
                using (DbContextJulio db = new DbContextJulio())
                {
                    var _participante_tarjeta = db.participante_tarjeta.Where(s => s.tarjeta == customer.Num_tarjeta && s.status_tarjeta_id == 1).FirstOrDefault();
                    if (_participante_tarjeta == null)
                        return false;
                    else
                    {
                        var _participante = db.participante.Where(s => s.id == _participante_tarjeta.participante_id).FirstOrDefault();
                        if (_participante == null)
                            return false;
                        else
                        {
                            _participante.status_participante_id = status_participante_id;
                            db.Entry(_participante).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ResultJson> ThemeAsync(string theme)
        {
            try
            {
                using (var db = new DbContextJulio())
                {
                    var pais = await db.pais.FirstOrDefaultAsync();
                    pais.theme = theme;
                    db.Entry(pais).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    result.jsonObject = theme;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                return result;
            }
        }
    }
}