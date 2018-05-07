using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class PupilWithAvgMark
    {
        public int PupilId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public int ClassId { get; set; }
        public double AvgMark { get; set; }

        public PupilWithAvgMark() { }

        public PupilWithAvgMark(int id, string name, string surname, string patronymic, int classID, double avgMark)
        {
            PupilId = id;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            ClassId = classID;
            AvgMark = avgMark;
        }
    }
}