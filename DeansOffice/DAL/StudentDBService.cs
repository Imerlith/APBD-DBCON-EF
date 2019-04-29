using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DeansOffice.Structures;
namespace DeansOffice.DAL
{
    class StudentDBService
    {
        public static List<Studies> ListOfStudies;
        public static ObservableCollection<Subject> ListOfSubjects;
        private static readonly string ConString = "Data Source=db-mssql;Initial Catalog=s16540;Integrated Security=True";

        public  static  ObservableCollection<Student> GetStudentData()
        {
            var students = new ObservableCollection<Student>();
            ListOfStudies = new List<Studies>();
            ListOfSubjects = new ObservableCollection<Subject>();
            
            using (var conn= new SqlConnection(ConString))
            {
                try
                {


                    conn.Open();

                    using (var command = new SqlCommand("select * from apbd.student inner join apbd.Studies on " +
                        "apbd.student.idstudies = apbd.studies.idstudies", conn))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            students.Add(new Student
                            {
                                id = int.Parse(reader["idstudent"].ToString()),
                                IndexNumber = reader["IndexNumber"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Address = reader["Address"].ToString(),
                                Studies = new Studies { Name = reader["Name"].ToString() },
                                Subjects = new ObservableCollection<Subject>()
                            });
                        }
                    }
                    using (var command = new SqlCommand("select * from apbd.studies", conn))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ListOfStudies.Add(new Studies { Name = reader["Name"].ToString() });
                        }
                    }
                    using (var command = new SqlCommand("select * from apbd.subject", conn))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ListOfSubjects.Add(new Subject { Name = reader["Name"].ToString() });
                        }
                    }
                    foreach (Student student in students)
                    {
                        var subjects = new ObservableCollection<Subject>();
                        
                        using (var command = new SqlCommand("select name from apbd.Subject" +
                            " inner join apbd.Student_subject on apbd.Student_Subject.idsubject = apbd.Subject.idsubject" +
                            " where idstudent ="+student.id, conn))
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var subject = new Subject { Name = reader["name"].ToString(), IsChecked = true};
                                subjects.Add(subject);
                            }
                        }
                        
                        foreach(Subject subject in ListOfSubjects)
                        {
                            if (!subjects.Contains(subject))
                            {
                                subjects.Add(subject);
                            }
                        }

                        student.Subjects = subjects;
                    }
                }
                catch (SqlException)
                {
                    MessageBox.Show("Błąd połączenia z bazą danych");
                }
            }
           
            return students;
        }
       
        public static void DeleteFromDB(IEnumerable<Student> toDelete)
        {
            var sqlCommands = new List<string>();
            foreach(Student student in toDelete)
            {
                sqlCommands.Add($"delete from apbd.student_subject where idstudent = {student.id}");
                sqlCommands.Add($"delete from apbd.student where idstudent = {student.id}");
            }
            using (var con = new SqlConnection(ConString))
            {
                try
                {
                    con.Open();
                    using (var transaction = con.BeginTransaction())
                    {
                        foreach(string s in sqlCommands)
                        {
                            using(var command = new SqlCommand(s, con, transaction))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                        

                }
                catch (SqlException)
                {
                    MessageBox.Show("Błąd połączenia z bazą danych");
                }
            }

        }
        public static void AddToDB(Student student)
        {
            int nIndex = 0;
            
            using (var con = new SqlConnection(ConString))
            {
                try
                {
                    con.Open();
                    using (var command = new SqlCommand("select max(idstudent) as 'max' from apbd.student", con))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nIndex = int.Parse(reader["max"].ToString())+1;
                        }
                    }

                    var str = $"SET IDENTITY_INSERT apbd.student on ; " +
                        $"insert into apbd.student(idstudent,firstname,lastname,address,indexnumber,idstudies)" +
                        $" values({nIndex},'{student.FirstName}','{student.LastName}','nie podano','{student.IndexNumber}',1);" +
                        $" SET IDENTITY_INSERT apbd.student off ; ";
                    
                    using (var command = new SqlCommand(str,con))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException)
                {
                    MessageBox.Show("Błąd połączenia z bazą danych");
                }
            }

        }
        public static void UpdateDB(Student student)
        {
            using(var con = new SqlConnection(ConString))
            {
                try
                {
                    con.Open();
                    using(var command = new SqlCommand($"update apbd.student set " +
                        $"FirstName = '{student.FirstName}',LastName='{student.LastName}',IndexNumber='{student.IndexNumber}'" +
                        $" where idstudent = {student.id}", con))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException)
                {
                    MessageBox.Show("Błąd połączenia z bazą danych");
                }
            }
        }

    }
    
}
