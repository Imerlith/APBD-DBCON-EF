using DeansOffice.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeansOffice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DAL.EfServiceDb connection;
        public MainWindow()
        {
            InitializeComponent();
            InitializeDBConn();
            

        }

        private void InitializeDBConn()
        {
           
                connection = new DAL.EfServiceDb();
                var source = new ObservableCollection<Student>();
                foreach(Student student in connection.GetStudents())
                {
                source.Add(student);
                }
            DataGrid.ItemsSource = source;
        
            
        }
        

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCount = DataGrid.SelectedItems.Count;
            if (selectedCount > 0)
            {
                var selected = DataGrid.SelectedItems.Cast<Student>().ToList();
                
                var source = DataGrid.ItemsSource as ObservableCollection<Student>;
                
               
                if (source != null)
                {
                    if(MessageBox.Show($"Czy napewno chcesz usunąć {selectedCount} studentów ?", "Question",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        
                        foreach (Student student in selected)
                        {
                           source.Remove(student);
                            context.Students.Remove(student);
                        }
                        
                    }
                   
                }
                
            }
            context.SaveChangesAsync();
        }

        private void AddNewStudentButton_Click(object sender, RoutedEventArgs e)
        {
            var addStudnetWindow = new AddStudentWindow();
            addStudnetWindow.AddStudent += new AddStudentHandler(AddStudentHandler);
            addStudnetWindow.ShowDialog();
            
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dg = sender as DataGrid;
            var slectedCount = dg.SelectedItems.Count;
            if (slectedCount > 1)
            {
                HowManySelectedLabel.Content = "Wybrałeś " + slectedCount + " studentów";
            }
            else
            {
                HowManySelectedLabel.Content = "";
            }

        }
        private void AddStudentHandler(object sender, Student nStudent)
        {
            var source = DataGrid.ItemsSource as ObservableCollection<Student>;
            if (source != null)
            {
                source.Add(nStudent);
                context.Students.Add(nStudent);
                context.SaveChangesAsync();
            }
           
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dg = sender as DataGrid;
            var selected = dg.SelectedItem;
            if (selected != null)
            {
                var student = selected as Student;
                var addWindow = new AddStudentWindow(student);
                addWindow.UpdateStudent += new UpdateStudentHandler(UpdateStudentHandler);
                addWindow.ShowDialog();
            }
        }
        private void UpdateStudentHandler(object sender, Student uStudent)
        {
            var source = DataGrid.ItemsSource as ObservableCollection<Student>;
            var index = source.ToList().FindIndex(s => s.IdStudent == uStudent.IdStudent);
            var toUpdate = source[index];
            toUpdate.FirstName = uStudent.FirstName;
            toUpdate.LastName = uStudent.LastName;
            toUpdate.IndexNumber = uStudent.IndexNumber;
            DataGrid.Items.Refresh();
            
        }
    }
}
