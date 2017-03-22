using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManager
{
    public class School
    {
        private const int NumberOfClasses = 8;
        private const int MaxStudentsInClass = 5;
        private const int NumberOfMinutesLastAte = 60;
        private int m_LastId = 0;

        private List<Class> m_Classes = new List<Class>(NumberOfClasses);
        private List<Student> m_Students = new List<Student>();
        private List<Teacher> m_Teachers = new List<Teacher>();

        public School()
        {
            for (int i = 0; i < NumberOfClasses; i++)
            {
                m_Classes.Add(new Class(i));
            }
        }

        public string AddStudent(string name, string phone, int age, int classID)
        {
            Class currentClass = GetValidClass(classID);
            if (currentClass == null)
            {
                return "-1cant add student";
            }
            int id = Interlocked.Increment(ref m_LastId);
            currentClass.StudentsIDs.Add(id);

            Student student = new Student(name, phone, age, classID, id); ;
            m_Students.Add(student);
            return id.ToString();
        }

        public string AddTeacher(string name, string phone, int classID)
        {
            Class currentClass = m_Classes.FirstOrDefault(c => c.ID == classID);
            int id = Interlocked.Increment(ref m_LastId);

            //TODO: check if can add teacher to class?
            Teacher teacher = new Teacher(name, phone, classID, id);
            m_Teachers.Add(teacher);
            return id.ToString();
        }

        public string EnterClass(int studentID, int classID)
        {
            Class currentClass = m_Classes.FirstOrDefault(c => c.ID == classID);
            if (currentClass == null)
            {
                return "-1cant enter class";
            }
            currentClass.ClassEvents.Add(new ClassEvent { EventExecutorID = studentID, EventOfType = EventType.Enter });
            return "OK";
        }

        public string ExitClass(int studentID, int classID)
        {
            Class currentClass = m_Classes.FirstOrDefault(c => c.ID == classID);
            if (currentClass == null)
            {
                return "-1cant exit class";
            }
            currentClass.ClassEvents.Add(new ClassEvent { EventExecutorID = studentID, EventOfType = EventType.Exit });
            return "OK";
        }

        public string Eating(int studentID)
        {
            Student student = m_Students.FirstOrDefault(s => s.ID == studentID);
            if (student == null)
            {
                return "-1no student";
            }
            student.SchoolEvents.Add(new Event { EventOfType = EventType.Eat });
            return "OK";
        }

        public string Chat(int studentID1, int studentID2)
        {
            Student student1 = m_Students.FirstOrDefault(s => s.ID == studentID1);
            Student student2 = m_Students.FirstOrDefault(s => s.ID == studentID2);
            if (student1 == null || student2 == null)
            {
                return "-1no student";
            }
            student1.SchoolEvents.Add(new Event { EventOfType = EventType.Chat });
            student2.SchoolEvents.Add(new Event { EventOfType = EventType.Chat });
            return "OK";
        }

        public string GetStudent()
        {
            string res = string.Empty;
            m_Students.ForEach(s => res += string.Format("{0} {1} {2}\n", s.ID, s.Name, s.Age));
            return res;
        }

        public string GetTeachers()
        {
            string res = string.Empty;
            m_Teachers.ForEach(s => res += string.Format("{0} {1} {2}\n", s.ID, s.Name, s.ClassID));
            return res;
        }

        public string WhoAte()
        {
            string res = string.Empty;
            m_Students.Where(s => s.SchoolEvents.Any(e => e.EventOfType == EventType.Eat && (DateTime.Now - e.EventTime).TotalMinutes <= NumberOfMinutesLastAte)).ToList().
                ForEach(s => res += string.Format("{0} {1}\n", s.ID, s.Name));
            return res;
        }

        public string ClassPresence()
        {
            string res = string.Empty;
            m_Classes.ForEach(c =>
            {
                c.ClassEvents.Where(e => e.EventOfType == EventType.Enter).ToList().ForEach(enterEvent =>
                {
                    res += string.Format("{0} {1} {2} {3}\n", enterEvent.EventExecutorID, c.ID, enterEvent.EventTime, GetExitTime(c.ClassEvents, enterEvent));
                });
            });
            return res;
        }

        private DateTime GetExitTime(List<ClassEvent> classEvents, ClassEvent enterEvent)
        {
            ClassEvent firstExitEvent = classEvents.OrderBy(e => e.EventTime).
                FirstOrDefault(e => e.EventExecutorID == enterEvent.EventExecutorID && e.EventTime > enterEvent.EventTime && e.EventOfType == EventType.Exit);
            return firstExitEvent != null ? firstExitEvent.EventTime : new DateTime();
        }

        private Class GetValidClass(int classID)
        {
            Class currentClass = m_Classes.FirstOrDefault(c => c.ID == classID);
            if (currentClass == null || currentClass.StudentsIDs.Count >= MaxStudentsInClass)
            {
                return null;
            }
            return currentClass;
        }

    }
}
