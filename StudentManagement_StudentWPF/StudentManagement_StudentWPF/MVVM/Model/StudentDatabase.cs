using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement_StudentWPF.Core
{
    public class StudentDatabase
    {
        private List<Student> students = new List<Student>();
        private int nextID = 1;

        public StudentDatabase()
        {
            InitializeSampleStudents();
        }
        
        private void InitializeSampleStudents()
        {
            students.Add(new Student { ID = 1, Name = "A", Age = 18, Address = "A", PhoneNumber = "0", Country = "A" });
            students.Add(new Student { ID = 2, Name = "B", Age = 19, Address = "B", PhoneNumber = "1", Country = "B" });
            students.Add(new Student { ID = 3, Name = "C", Age = 20, Address = "C", PhoneNumber = "2", Country = "C" });
            students.Add(new Student { ID = 4, Name = "D", Age = 21, Address = "D", PhoneNumber = "3", Country = "D" });
            students.Add(new Student { ID = 5, Name = "E", Age = 22, Address = "E", PhoneNumber = "4", Country = "E" });
        }

        public void AddStudent(Student student)
        {
            student.ID = nextID++;
            students.Add(student);
        }
        public List<Student> GetAllStudents()
        {
            return students;
        }

        public void RemoveStudents(IEnumerable<Student> studentsToRemove)
        {
            foreach (var student in studentsToRemove)
            {
                students.Remove(student);
            }
        }

        public void UpdateStudent(Student updatedStudent)
        {
            var existingStudent = students.FirstOrDefault(s => s.ID == updatedStudent.ID);
            if (existingStudent != null)
            {
                existingStudent.Name = updatedStudent.Name;
                existingStudent.Age = updatedStudent.Age;
            }
        }
    }
}
