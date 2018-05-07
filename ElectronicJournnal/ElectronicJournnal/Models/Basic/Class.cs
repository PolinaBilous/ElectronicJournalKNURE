using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class Class
    {
        public int Class_ID { get; set; }
        public string Class_Name { get; set; }
        public DateTime Formation_Date { get; set; }

        public Class(int class_ID, string class_Name, DateTime formation_Date)
        {
            Class_ID = class_ID;
            Class_Name = class_Name;
            Formation_Date = formation_Date;
        }

        public Class() { }
    }
}