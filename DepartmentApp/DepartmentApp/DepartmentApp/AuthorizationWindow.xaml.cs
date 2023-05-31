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
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private MainController controller;

        public AuthorizationWindow()
        {
            InitializeComponent();
            controller = new MainController();
        }

        private void ButtonEntry_Click(object sender, RoutedEventArgs e)
        {
            DepartmentViewModel department = new DepartmentViewModel();
            if (!TextBoxLoginDepartment.Text.Equals("") && !TextBoxPasswordDepartment.Password.Equals(""))
            {
                try
                {
                    department = controller.AuthorizationDepartment(new DepartmentBindingModel
                    {
                        Login = TextBoxLoginDepartment.Text,
                        Password = TextBoxPasswordDepartment.Password
                    });
                    if (department == null)
                    {
                        MessageBox.Show("Кафедра не найдена, проверьте введенные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        APIClient.DepartmentId = department.Id;
                        APIClient.DepartmentName = department.Name;
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.ShowDialog();
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Проверьте введенные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля формы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ButtonRegistration_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }
    }
}
