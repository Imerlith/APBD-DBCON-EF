using DeansOffice.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeansOffice.DAL
{
    class EfServiceDb
    {
        public PjatkDB context;
       public EfServiceDb()
        {
            context =  new PjatkDB();
            context.Configuration.LazyLoadingEnabled = false;
        }
       

        public ICollection<Student> GetStudents()
        {
            var list = new List<Student>();
            try
            {
                /*
                var students = context.Students.AsEnumerable()
                    .Join(context.Studies, stu => stu.IdStudies, stad => stad.IdStudies, (stu, stad) => new Student {
                    FirstName = stu.FirstName,LastName= stu.LastName,IndexNumber = stu.IndexNumber,Address=stu.Address,Study =stad
                }).ToList();
                */
                var students = context.Students
                                .Include("Study")
                                .ToList();
                return students;
            }
            catch (Exception)
            {
                MessageBox.Show("Błąd łączenia z bazą danych");
                return list;
            }
        }
        public ICollection<Study> GetStudies()
        {
            var list = new List<Study>();
            try
            {
                var studies = context.Studies.ToList();

                return studies;
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
            if (student.IdStudies == 0) student.IdStudies = 1;
            context.Students.Add(student);
            Commit();
        }
        public void RemoveStudentFromDB(Student toRemove)
        {
            var stu = new Student {IdStudent= toRemove.IdStudent };
            context.Students.Attach(stu);
            context.Students.Remove(stu);
        }

        public void Commit()
        {
            bool saveFailed;
            do
            {
                saveFailed = false;
                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                   
                    ex.Entries.Single().Reload();
                }
            } while (saveFailed);
           

        }

    }
}
