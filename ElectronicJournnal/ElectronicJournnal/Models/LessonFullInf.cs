using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class LessonFullInf
    {
        public int Lesson_ID { get; set; }
        public int Class_ID { get; set; }
        public string Class_Name { get; set; }
        public int Teacher_ID { get; set; }
        public string Teacher_Name { get; set; }
        public string Teacher_Surname { get; set; }
        public string Teacher_Patronymic { get; set; }
        public int Subject_ID { get; set; }
        public string Subject_Name { get; set; }
        public DateTime Date { get; set; }

        public LessonFullInf() { }

        public LessonFullInf(int lessonID, int classID, string className, int teacherID, 
                             string teacherName, string teacherSurname, string teacherPatronymic, 
                             int subjectID, string subjectName, DateTime date)
        {
            Lesson_ID = lessonID;
            Class_ID = classID;
            Class_Name = className;
            Teacher_ID = teacherID;
            Teacher_Name = teacherName;
            Teacher_Surname = teacherSurname;
            Teacher_Patronymic = teacherPatronymic;
            Subject_ID = subjectID;
            Subject_Name = subjectName;
            Date = date;
        }
    }
}