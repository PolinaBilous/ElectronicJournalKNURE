using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class TeacherShortInf
    {
        public int Teacher_ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }   
        public int Experience { get; set; }

        public TeacherShortInf() { }

        public TeacherShortInf(int id, string name, string surname, string patronymic, int experience)
        {
            Teacher_ID = id;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Experience = experience;
        }
    }
}