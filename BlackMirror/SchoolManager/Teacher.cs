﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager
{
    public class Teacher : Person
    {
        

        public Teacher(string name, string phone, int classID, EventType eventType) : base(name, phone, eventType, classID)
        {
        }
    }
}
