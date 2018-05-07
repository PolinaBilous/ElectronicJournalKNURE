using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class ClassWithManagment
    {
        public int Class_ID { get; set; }
        public string Class_Name { get; set; }
        public DateTime Formation_Date { get; set; }
        public int ClassManagment_ID { get; set; }
        public int Teacher_ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }

        public ClassWithManagment(int class_ID, string class_Name, DateTime formation_Date, int classManagment_ID, int teacher_ID, string name, string surname, string patronymic)
        {
            Class_ID = class_ID;
            Class_Name = class_Name;
            Formation_Date = formation_Date;
            Teacher_ID = teacher_ID;
            ClassManagment_ID = classManagment_ID;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
        }

        public ClassWithManagment() { }
    }
}