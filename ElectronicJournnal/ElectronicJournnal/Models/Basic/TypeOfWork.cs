using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class TypeOfWork
    {
        public int TypeOfWork_ID { get; set; }
        public string TypeOfWork_Name { get; set; }
        public int Coefficient { get; set; }

        public TypeOfWork(int typeOfWork_ID, string typeOfWork_Name, int coefficient)
        {
            TypeOfWork_ID = typeOfWork_ID;
            TypeOfWork_Name = typeOfWork_Name;
            Coefficient = coefficient;
        }
    }
}