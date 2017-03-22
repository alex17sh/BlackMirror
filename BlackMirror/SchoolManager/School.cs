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
        private int m_LastId = 0;

        private List<Class> m_Classes = new List<Class>(NumberOfClasses);
        private List<Student> m_Students = new List<Student>();
        private List<Teacher> m_Teachers = new List<Teacher>();

        public School()
        {
            for (int i = 0; i < NumberOfClasses; i++)
            {
                m_Classes[i] = new Class { ID = i, StudentsIDs = new List<int>() };
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

            Student student = new Student { Age = age, Name = name, Phone = phone, ID = id, SchoolEvents = new List<Event>() };
            m_Students.Add(student);
            return id.ToString();
        }

        public string AddTeacher(string name, string phone, int classID)
        {
            Class currentClass = m_Classes.FirstOrDefault(c => c.ID == classID);
            int id = Interlocked.Increment(ref m_LastId);

            //TODO: check if can add teacher to class?
            Teacher teacher = new Teacher { Name = name, Phone = phone, ID = id, SchoolEvents = new List<Event>() { new Event { EventOfType = EventType.Enter } } };
            m_Teachers.Add(teacher);
            return id.ToString();
        }

        public string EnterClass(int studentID, int classID)
        {
            Class currentClass = GetValidClass(classID);
            if (currentClass == null)
            {
                return "-1cant enter class";
            }
            //TODO: check if can add student to class?
            currentClass.StudentsIDs.Add(studentID);
            return "OK";
        }

        public string ExitClass(int studentID, int classID)
        {
            Class currentClass = GetValidClass(classID);
            if (currentClass == null)
            {
                return "-1cant exit class";
            }
            currentClass.StudentsIDs.Remove(studentID);
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
