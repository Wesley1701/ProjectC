using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class InfoHours
    {
        public static void InformationHours(DateTime d, DateTime e)
        {
            using (var contextt = new CompanyContext())
            {
                Console.WriteLine("Clearing database...");;
                contextt.Hours.RemoveRange(contextt.Hours);
                contextt.SaveChanges();
                Console.WriteLine("Done!");
            }
        }
        public void AddStartTime(DateTime s, string pID,string sID, string htID, string uID)
        {
            using (var contextt = new CompanyContext())
            {
                contextt.Add(new Hours()
                {
                    EmployeeID = uID,
                    ProjectID = pID,
                    ServiceID= sID,
                    HoursTypeID= htID,
                    StartTime = s.ToString(),
                    EndTime = "",
                    Note = ""
                }) ;
                contextt.SaveChanges();
            }
        }

        public void AddEndTime(DateTime e, string pID, string uID)
        {
            using (var contextt = new CompanyContext())
            {
                var result = contextt.Hours.Where(s => s.EmployeeID.Equals(uID) && s.EndTime.Equals("")).ToArray();
                var h = result[0];
                if (h != null)
                {
                    h.EndTime = e.ToString();
                    Console.WriteLine();
                }
                contextt.SaveChanges();
            }
        }
    }
}