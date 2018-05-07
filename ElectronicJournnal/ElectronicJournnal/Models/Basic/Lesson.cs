using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class Lesson
    {
        public int Lesson_ID { get; set; }
        public int Class_ID { get; set; }
        public int Teacher_ID { get; set; }
        public int Subject_ID { get; set; }
        public DateTime Date { get; set; }

        public Lesson(int lesson_ID, int class_ID, int teacher_ID, int subject_ID, DateTime date)
        {
            Lesson_ID = lesson_ID;
            Class_ID = class_ID;
            Teacher_ID = teacher_ID;
            Subject_ID = subject_ID;
            Date = date;
        }

        public Lesson() { }
    }
}