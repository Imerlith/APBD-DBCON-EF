﻿using DeansOffice.Models;
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
      public  PjatkDB context = new PjatkDB();
       
       

        public ICollection<Student> GetStudents()
        {
            var list = new List<Student>();
            try
            {
                var students = context.Students.AsEnumerable()
                    .Join(context.Studies, stu => stu.IdStudies, stad => stad.IdStudies, (stu, stad) => new Student {
                    FirstName = stu.FirstName,LastName= stu.LastName,IndexNumber = stu.IndexNumber,Address=stu.Address,Study =stad
                }).ToList();
                
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
            context.Students.Remove(toRemove);
            
        }

        public void Commit()
        {
            context.SaveChanges();
        }
        
    }
}
