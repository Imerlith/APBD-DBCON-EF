using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DeansOffice.Models;
namespace DeansOffice
{
    /// <summary>
    /// Interaction logic for AddStudentWindow.xaml
    /// </summary>

    public delegate void AddStudentHandler(object sender, Student nStudent);
    public delegate void UpdateStudentHandler(object sender, Student uStudent);
    public partial class AddStudentWindow : Window
    {
        public event AddStudentHandler AddStudent;
        public event UpdateStudentHandler UpdateStudent;
        private Student received;
        public AddStudentWindow(List<Study> studies)
        {
            InitializeComponent();
            StudiesComboBox.ItemsSource = studies;
        }
        public AddStudentWindow(Student student,List<Study> studies)
        {
            InitializeComponent();
            received = student;
            FillForm();
            StudiesComboBox.ItemsSource = studies;
            
        }

        private void FillForm()
        {
            LastNameTxtBox.Text = received.LastName;
            FirstNameTxtBox.Text = received.FirstName;
            IndexTxtBox.Text = received.IndexNumber;
            
          

        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
            {
                MessageBox.Show("Blędne dane");
            }
            else
            {
                var nStudent = new Student
                {
                    FirstName = NormalizeInput(FirstNameTxtBox.Text),
                    LastName = NormalizeInput(LastNameTxtBox.Text),
                    IndexNumber = IndexTxtBox.Text,
                    
                };
                if (received == null)
                {
                    
                    AddStudent(this, nStudent);
                }
                else
                {
                    if (IsChanged(nStudent))
                    {
                        
                        UpdateStudent(this, nStudent);
                    }
                }
               
                Close();
            }
        }
        private bool IsChanged(Student student)
        {
            return student.IndexNumber != received.IndexNumber || student.FirstName != received.FirstName
                || student.LastName != received.LastName;
        }

        private bool ValidateInput()
        {
            var lName = LastNameTxtBox.Text;
            var fName = FirstNameTxtBox.Text;
            var index = IndexTxtBox.Text;
            if(string.IsNullOrWhiteSpace(lName) || string.IsNullOrWhiteSpace(fName) || string.IsNullOrWhiteSpace(index))
            {
                return false;
            }
            string indexPattern = "[s]\\d{5}";
            Regex regex = new Regex(indexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var matches = regex.Match(index);
            if (!matches.Success)
            {
                return false;
            }

            return true;
        }
        private string NormalizeInput(string input)
        {
           input = Regex.Replace(input, @"\s+", "");
            return input.First().ToString().ToUpper() + input.Substring(1).ToLower();
        }

        private void DelStudentButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
           
            
        }
    }
}
