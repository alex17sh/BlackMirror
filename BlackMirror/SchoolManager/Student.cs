using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager
{
    public class Student : Person
    {
        public int Age { get; set; }

        public Student(string name, string phone, int age, int classID, int id) : base(name, phone, classID, id)
        {
            Age = age;
        }
    }
}
