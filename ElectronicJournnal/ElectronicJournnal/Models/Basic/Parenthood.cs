using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class Parenthood
    {
        public int Parenthood_ID { get; set; }
        public int Parent_ID { get; set; }
        public int Pupil_ID { get; set; }

        public Parenthood(int parenthood_ID, int parent_ID, int pupil_ID)
        {
            Parenthood_ID = parenthood_ID;
            Parent_ID = parent_ID;
            Pupil_ID = pupil_ID;
        }
    }
}