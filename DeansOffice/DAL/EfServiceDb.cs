using DeansOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeansOffice.DAL
{
    class EfServiceDb
    {
        PjatkDB context = new PjatkDB();
       

        public ICollection<Student> GetStudents()
        {
            var list = new List<Student>();
            try
            {
                var students = context.Students.ToList();
                foreach(Student student in students)
                {
                    student.Study = context.Studies.Where(s => s.IdStudies == student.IdStudies) as Study;
                }
                return students;
            }
            catch (Exception)
            {
                MessageBox.Show("Błąd łączenia z bazą danych");
                return list;
            }
        }
        public void AddStudentToDB(Student student)
        {
            student.Address = "jakas";
            
            context.Students.Add(student);
            Commit();
        }
        public void RemoveStudentFromDB(Student toRemove)
        {
            context.Students.Remove(toRemove);
            
        }

        public void Commit()
        {
            context.SaveChanges();
        }
        
    }
}
