using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class Mark
    {
        public int Mark_ID { get; set; }
        public int Pupil_ID { get; set; }
        public int Lesson_ID { get; set; }
        public int? TypeOfWork_ID { get; set; }
        public int? PupilMark { get; set; }

        public Mark(int mark_ID, int pupil_ID, int lesson_ID, int? typeOfWork_ID, int? pupilMark)
        {
            Mark_ID = mark_ID;
            Pupil_ID = pupil_ID;
            Lesson_ID = lesson_ID;
            TypeOfWork_ID = typeOfWork_ID;
            PupilMark = pupilMark;
        }

        public Mark() { }
    }
}