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
        public List<Event> Events { get; set; }
    }
}
