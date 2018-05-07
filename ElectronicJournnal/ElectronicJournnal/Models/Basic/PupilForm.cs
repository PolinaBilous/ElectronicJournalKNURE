using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class PupilForm
    {
        public int PupilForm_ID { get; set; }
        public int Pupil_ID { get; set; }
        public DateTime Date_Of_Birth { get; set; }
        public int Age { get; set; }
        public string Parent_Phone_Number { get; set; }
        public string Pupil_Phone_Number { get; set; }
        public string Address { get; set; }

        public PupilForm(int id, int pupil_ID, DateTime date_Of_Bith, int age, string parent_Phone_Number, string pupil_Phone_Number, string address)
        {
            PupilForm_ID = id;
            Pupil_ID = pupil_ID;
            Date_Of_Birth = date_Of_Bith;
            Age = age;
            Parent_Phone_Number = parent_Phone_Number;
            Pupil_Phone_Number = pupil_Phone_Number;
            Address = address;
        }
    }
}