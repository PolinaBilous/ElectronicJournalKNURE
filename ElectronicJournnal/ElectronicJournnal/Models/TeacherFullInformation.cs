using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class TeacherFullInformation
    {
        public int Teacher_ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public DateTime Date_Of_Birth { get; set; }
        public int Age { get; set; }
        public int Experience { get; set; }
        public string Address { get; set; }
        public string Phone_Number { get; set; }
        public string Email { get; set; }

        public TeacherFullInformation(int id, string name, string surname, string patronymic, DateTime date_Of_Birth, int age, int experience, string address, string phone_Number, string email)
        {
            Teacher_ID = id;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Date_Of_Birth = date_Of_Birth;
            Age = age;
            Experience = Convert.ToInt32(experience);
            Address = address;
            Phone_Number = phone_Number;
            Email = email;
        }

        public TeacherFullInformation() { }
    }
}