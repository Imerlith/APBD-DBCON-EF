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
                return context.Students.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Błąd łączenia z bazą danych");
                return list;
            }
        }
        public void AddStudentToDB(Student student)
        {
            context.Students.Add(student);
        }
        public void RemoveStudentFromDB(Student toRemove)
        {
            context.Students.Remove(toRemove);
        }
        
    }
}
