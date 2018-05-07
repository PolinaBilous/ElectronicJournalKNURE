using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class PupilWithClass
    {
        public int Pupil_ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Class { get; set; }

        public PupilWithClass(int id, string name, string surname, string patronymic, string Pclass)
        {
            Pupil_ID = id;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Class = Pclass;
        }
    }
}