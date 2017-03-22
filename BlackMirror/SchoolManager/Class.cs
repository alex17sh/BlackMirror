using System.Collections.Generic;

namespace SchoolManager
{
    public class Class
    {
        public int ID { get; set; }
        public List<int> StudentsIDs { get; set; }
        public List<ClassEvent> ClassEvents { get; set; }

        public Class(int id)
        {
            ID = id;
            StudentsIDs = new List<int>();
            ClassEvents = new List<ClassEvent>();
        }
    }
}