using DepartmentHTTPClient;
using DepartmentHTTPClient.Controllers;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using UniversityContracts.ViewModels;

namespace DepartmentApp
{
    /// <summary>
    /// Логика взаимодействия для StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {

        public List<StudentViewModel> listStudents;
        private MainController controller;
        public StudentWindow()
        {
            InitializeComponent();
            controller = new MainController();
            listStudents = new List<StudentViewModel>();
            LoadDataStudents();
        }

        private void LoadDataStudents()
        {
            ListViewStudents.ItemsSource = null;
            try
            {
                var sourceStudents = controller.GetStudentsByDepartment(APIClient.DepartmentId);
                if (sourceStudents != null)
                {
                    ListViewStudents.ItemsSource = sourceStudents;
                }
                ListViewStudents.ItemsSource = sourceStudents;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void ButtonCreateUpdateStudent_Click(object sender, RoutedEventArgs e)
        {
            StudentCreateOrUpdateWindow studentCreateOrUpdateWindow = new StudentCreateOrUpdateWindow(null);
            studentCreateOrUpdateWindow.Show();
            this.Close();
        }
        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            var elem = ((Button)sender).Tag.ToString();
            StudentCreateOrUpdateWindow studentCreateOrUpdateWindow = new StudentCreateOrUpdateWindow(Convert.ToInt32(elem));
            studentCreateOrUpdateWindow.ShowDialog();
            this.Close();
        }
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var elem = ((Button)sender).Tag.ToString();
            try
            {
                var result = controller.DeleteStudent(Convert.ToInt32(elem));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadDataStudents();
        }
    }
}
