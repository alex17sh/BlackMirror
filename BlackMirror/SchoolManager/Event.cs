using System;

namespace SchoolManager
{
    public class Event
    {
        public DateTime EventTime { get; set; }
        public EventType EventOfType { get; set; }

        public Event()
        {
            EventTime = DateTime.Now;
        }
    }
}