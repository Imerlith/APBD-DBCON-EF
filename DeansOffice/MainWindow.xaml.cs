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
        public MainWindow()
        {
            InitializeComponent();
            var students = DAL.StudentDBService.GetStudentData();
            DataGrid.ItemsSource = students;

        }

        

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCount = DataGrid.SelectedItems.Count;
            if (selectedCount > 0)
            {
                var selected = DataGrid.SelectedItems.Cast<Structures.Student>().ToList();
                
                var source = DataGrid.ItemsSource as ObservableCollection<Structures.Student>;
                if (source != null)
                {
                    if(MessageBox.Show($"Czy napewno chcesz usunąć {selectedCount} studentów ?", "Question",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        
                        foreach (Structures.Student student in selected)
                        {
                            
                           
                            //if (source.Contains(student))
                           source.Remove(student);
                        }
                        DAL.StudentDBService.DeleteFromDB(selected);
                    }
                   
                }
                
            }

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
        private void AddStudentHandler(object sender, Structures.Student nStudent)
        {
            var source = DataGrid.ItemsSource as ObservableCollection<Structures.Student>;
            source.Add(nStudent);
            DAL.StudentDBService.AddToDB(nStudent);
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dg = sender as DataGrid;
            var selected = dg.SelectedItem;
            if (selected != null)
            {
                var student = selected as Structures.Student;
                var addWindow = new AddStudentWindow(student);
                addWindow.UpdateStudent += new UpdateStudentHandler(UpdateStudentHandler);
                addWindow.ShowDialog();
            }
        }
        private void UpdateStudentHandler(object sender, Structures.Student uStudent)
        {
            var source = DataGrid.ItemsSource as ObservableCollection<Structures.Student>;
            var index = source.ToList().FindIndex(s => s.id == uStudent.id);
            var toUpdate = source[index];
            toUpdate.FirstName = uStudent.FirstName;
            toUpdate.LastName = uStudent.LastName;
            toUpdate.IndexNumber = uStudent.IndexNumber;
            DataGrid.Items.Refresh();
            DAL.StudentDBService.UpdateDB(uStudent);
        }
    }
}
