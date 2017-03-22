using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager
{
    public class ClassEvent : Event
    {
        public int EventExecutorID { get; set; }

        public ClassEvent() : base()
        {

        }
    }
}
