using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class ClassesFillability
    {
        public string ClassName { get; set; }
        public int CountOfPupils { get; set; }

        public ClassesFillability() { }

        public ClassesFillability(string name, int count)
        {
            ClassName = name;
            CountOfPupils = count;
        }
    }
}