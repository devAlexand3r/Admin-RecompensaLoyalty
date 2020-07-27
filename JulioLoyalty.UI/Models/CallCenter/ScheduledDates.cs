using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JulioLoyalty.UI.Models.CallCenter
{
    public class ScheduledDates
    {
        private static Random r = new Random();
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
        public bool Active { get; set; }
        public double Discount { get; set; }

        private static int randomInt(int max)
        {
            return (int)Math.Floor(r.NextDouble() * (max + 1));
        }

        public static IEnumerable<ScheduledDates> GetData(int cnt)
        {
            string[] names = "Alejandro Jiménmez,Juan Carlos Reyes,Victor Hernandez,Daniel Hernandez,Roberto Garcia,Carlos Rios".Split(',');
            string[] cells = "5578597415,1293405983,2934059284,0923812394,1293405938,5938493789".Split(',');
            string[] comments = "Wijmo,Aoba,Xuni,Olap".Split(',');
            List<ScheduledDates> result = new List<ScheduledDates>();
            for (var i = 0; i < cnt; i++)
            {
                result.Add(new ScheduledDates
                {
                    Id = i,
                    FullName = names[randomInt(names.Length - 1)],
                    PhoneNumber = cells[randomInt(cells.Length - 1)],
                    Date = new DateTime(DateTime.Now.Year, randomInt(5) + 1, randomInt(27) + 1),
                    Age = randomInt(100),
                    Comments = comments[randomInt(comments.Length - 1)],
                    Active = randomInt(1) == 1 ? true : false,
                    Discount = r.NextDouble()
                });
            }
            return result;
        }
    }
}