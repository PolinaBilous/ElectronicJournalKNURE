using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class MarkFullInf
    {
        public int Mark_ID { get; set; }
        public string Pupil_Name { get; set; }
        public string Pupil_Surname { get; set; }
        public string Pupil_Patonymic { get; set; }
        public string Teacher_Name { get; set; }
        public string Teacher_Surname { get; set; }
        public string Teacher_Patonymic { get; set; }
        public string Subject_Name { get; set; }
        public string TypeOfWork_Name { get; set; }
        public DateTime Date { get; set; }
        public int? Mark { get; set; }

        public MarkFullInf() { }

        public MarkFullInf(int markID, string pupil_Name, string pupil_Surname, string pupil_Patonymic, string teacher_Name, 
                           string teacher_Surname, string teacher_Patonymic, string subject_Name, string typeOfWork_Name, DateTime date, int? mark)
        {
            Mark_ID = markID;
            Pupil_Name = pupil_Name;
            Pupil_Surname = pupil_Surname;
            Pupil_Patonymic = pupil_Patonymic;
            Teacher_Name = teacher_Name;
            Teacher_Surname = teacher_Surname;
            Teacher_Patonymic = teacher_Patonymic;
            Subject_Name = subject_Name;
            TypeOfWork_Name = typeOfWork_Name;
            Date = date;
            Mark = mark;
        }
    }
}