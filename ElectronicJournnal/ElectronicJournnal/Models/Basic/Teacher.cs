using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class Teacher
    {
        public int Teacher_ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }

        public Teacher(int id, string name, string surname, string patronymic)
        {
            Teacher_ID = id;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
        }
    }
}