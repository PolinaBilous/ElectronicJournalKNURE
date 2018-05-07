using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class PupilFullInformation
    {
        public int Pupil_ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Class { get; set; }
        public DateTime Date_Of_Birth { get; set; }
        public int Age { get; set; }
        public string Parent_Phone_Number { get; set; }
        public string Pupil_Phone_Number { get; set; }
        public string Address { get; set; }

        public PupilFullInformation(int id, string name, string surname, string patronymic, string Pclass, 
                                    DateTime date_Of_Bith, int age, string parent_Phone_Number, string pupil_Phone_Number, string address)
        {
            Pupil_ID = id;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Class = Pclass;
            Date_Of_Birth = date_Of_Bith;
            Age = age;
            Parent_Phone_Number = parent_Phone_Number;
            Pupil_Phone_Number = pupil_Phone_Number;
            Address = address;
        }

        public PupilFullInformation() { }    
    }
}