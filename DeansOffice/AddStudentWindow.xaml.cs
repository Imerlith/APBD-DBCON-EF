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
using DeansOffice.Structures;
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
        public AddStudentWindow()
        {
            InitializeComponent();
            StudiesComboBox.ItemsSource = DAL.StudentDBService.ListOfStudies;
            SubjectListBox.ItemsSource = DAL.StudentDBService.ListOfSubjects;
        }
        public AddStudentWindow(Student student)
        {
            InitializeComponent();
            received = student;
            FillForm();
            
        }

        private void FillForm()
        {
            LastNameTxtBox.Text = received.LastName;
            FirstNameTxtBox.Text = received.FirstName;
            IndexTxtBox.Text = received.IndexNumber;
            StudiesComboBox.ItemsSource = DAL.StudentDBService.ListOfStudies;
            StudiesComboBox.SelectedItem = received.Studies;
           
            SubjectListBox.ItemsSource = received.Subjects;

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
                    Studies = StudiesComboBox.SelectedItem as Studies
                };
                if (received == null)
                {
                    nStudent.id = 9999;
                    AddStudent(this, nStudent);
                }
                else
                {
                    if (IsChanged(nStudent))
                    {
                        nStudent.id = received.id;
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
