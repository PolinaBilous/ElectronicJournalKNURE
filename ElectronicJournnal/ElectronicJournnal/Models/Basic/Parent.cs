using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class Parent
    {
        public int Parent_ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }

        public Parent(int parent_ID, string name, string surname, string patronymic, string email)
        {
            Parent_ID = parent_ID;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Email = email;
        }

        public Parent() { }
    }
}