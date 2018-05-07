using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class Pupil
    {
        public int Pupil_ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public int Class_ID { get; set; }

        public Pupil(int id, string name, string surname, string patronymic, int class_ID)
        {
            Pupil_ID = id;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Class_ID = class_ID;
        }
    }
}