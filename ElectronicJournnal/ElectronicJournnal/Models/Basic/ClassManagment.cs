using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class ClassManagment
    {
        public int ClassManagment_ID { get; set; }
        public int Class_ID { get; set; }
        public int Teacher_ID { get; set; }
        public DateTime Begin_Date { get; set; }
        public DateTime Finish_Date { get; set; }

        public ClassManagment(int classManagment_ID, int class_ID, int teacher_ID, DateTime begin_Date, DateTime finish_Date)
        {
            ClassManagment_ID = classManagment_ID;
            Class_ID = class_ID;
            Teacher_ID = teacher_ID;
            Begin_Date = begin_Date;
            Finish_Date = finish_Date;
        }
    }
}