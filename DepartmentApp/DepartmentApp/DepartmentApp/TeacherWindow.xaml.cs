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
using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace DepartmentApp
{
    /// <summary>
    /// Логика взаимодействия для TeacherWindow.xaml
    /// </summary>
    public partial class TeacherWindow : Window
    {
        private MainController controller;

        public TeacherWindow()
        {
            InitializeComponent();
            controller = new MainController();
            LoadData();
        }

        private void LoadData()
        {
            ListViewTeachers.ItemsSource = null;
            try
            {
                var sourceTeachers = controller.GetTeachers(APIClient.DepartmentId);
                if (sourceTeachers != null)
                {
                    ListViewTeachers.ItemsSource = sourceTeachers;
                }
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

        private void ButtonCreateUpdateTeacher_Click(object sender, RoutedEventArgs e)
        {
            TeacherCreateUpdateWindow teacherCreateUpdate = new TeacherCreateUpdateWindow(null);
            teacherCreateUpdate.Show();
            this.Close();
        }
        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            var elem = ((Button)sender).Tag.ToString();
            TeacherCreateUpdateWindow teacherCreateUpdate = new TeacherCreateUpdateWindow(Convert.ToInt32(elem));
            teacherCreateUpdate.Show();
            this.Close();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var elem = ((Button)sender).Tag.ToString();
            int idTeacher = Convert.ToInt32(elem);
            controller.DeleteTeacher(idTeacher);
            LoadData();
        }
    }
}