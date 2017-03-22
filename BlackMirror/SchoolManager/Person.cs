using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager
{
    public abstract class Person
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public List<Event> SchoolEvents { get; set; }
        public int ID { get; set; }
        public int ClassID { get; set; }

        protected Person(string name, string phone,int classID, int id)
        {
            Name = name;
            Phone = phone;
            ClassID = classID;
            SchoolEvents = new List<Event>();
            ID = id;
        }
    }
}
