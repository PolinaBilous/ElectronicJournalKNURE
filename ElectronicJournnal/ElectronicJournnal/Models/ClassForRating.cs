using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class ClassForRating
    {
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        
        public double AvgMark { get; set; }

        public ClassForRating() { }

        public ClassForRating(int id, string name, double avgMark)
        {
            ClassID = ClassID;
            ClassName = name;
            AvgMark = avgMark;
        }
    }
}