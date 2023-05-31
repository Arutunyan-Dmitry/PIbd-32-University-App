using DepartmentHTTPClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UniversityContracts.ViewModels;

namespace DepartmentApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (APIClient.DepartmentName != null)
                EntryButton.Header = APIClient.DepartmentName;
        }
        private void MenuItemDiscipline_Click(object sender, RoutedEventArgs e)
        {
            if (APIClient.DepartmentName != null)
            {
                DisciplineWindow disciplineWindow = new DisciplineWindow();
                disciplineWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Авторизируйтесь в системе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItemGroup_Click(object sender, RoutedEventArgs e)
        {
            if (APIClient.DepartmentName != null)
            {
                GroupsWindowxaml groupsWindowxaml = new GroupsWindowxaml();
                groupsWindowxaml.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Авторизируйтесь в системе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItemTeacher_Click(object sender, RoutedEventArgs e)
        {
            if (APIClient.DepartmentName != null)
            {
                TeacherWindow teacherWindow = new TeacherWindow();
                teacherWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Авторизируйтесь в системе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItemAuthorization_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            this.Close();
        }

        private void MenuItemStudent_Click(object sender, RoutedEventArgs e)
        {
            if (APIClient.DepartmentName != null)
            {
                StudentWindow studentWindow = new StudentWindow();
                studentWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Авторизируйтесь в системе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MenuItemImportStudents_Click(object sender, RoutedEventArgs e)
        {
            if (APIClient.DepartmentName != null)
            {
                ImportStudentWindow importStudent = new ImportStudentWindow();
                importStudent.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Авторизируйтесь в системе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MenuItemMessages_Click(object sender, RoutedEventArgs e)
        {
            if (APIClient.DepartmentName != null)
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Авторизируйтесь в системе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
