using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class TeacherForm
    {
        public int TeacherForm_ID { get; set; }
        public int Teacher_ID { get; set; }
        public DateTime Date_Of_Birth { get; set; }
        public int Age { get; set; }
        public int Experience { get; set; }
        public string Address { get; set; }
        public string Phone_Number { get; set; }
        public string Email { get; set; }

        public TeacherForm(int teacherForm_ID, int teacher_ID, DateTime date_Of_Birth, int age, int experience, string address, string phone_Number, string email)
        {
            TeacherForm_ID = teacherForm_ID;
            Teacher_ID = teacher_ID;
            Date_Of_Birth = date_Of_Birth;
            Age = age;
            Experience = experience;
            Address = address;
            Phone_Number = phone_Number;
            Email = email;
        }


    }
}