using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class Subject
    {
        public int Subject_ID { get; set; }
        public string Subject_Name { get; set; }

        public Subject(int subject_ID, string subject_Name)
        {
            Subject_ID = subject_ID;
            Subject_Name = subject_Name;
        }
    }
}