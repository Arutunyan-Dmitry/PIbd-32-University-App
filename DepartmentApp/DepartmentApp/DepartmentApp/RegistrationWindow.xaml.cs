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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private MainController controller;
        public RegistrationWindow()
        {
            InitializeComponent();
            controller = new MainController();
        }

        private void ButtonEntry_Click(object sender, RoutedEventArgs e)
        {
            if (!TextBoxNameDepartment.Text.Equals("") && !TextBoxEmailDepartment.Text.Equals("") &&
                !TextBoxPasswordDepartment1.Password.Equals("") && !TextBoxPasswordDepartment2.Password.Equals(""))
            {
                if (TextBoxPasswordDepartment1.Password.Equals(TextBoxPasswordDepartment2.Password))
                {
                    try
                    {
                        controller.CreateOrUpdateDepartment(new DepartmentBindingModel
                        {
                            Name = TextBoxNameDepartment.Text,
                            Login = TextBoxEmailDepartment.Text,
                            Password = TextBoxPasswordDepartment1.Password
                        });
                        MessageBox.Show("Регистрация прошла успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                        AuthorizationWindow authorization = new AuthorizationWindow();
                        authorization.Show();
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Название кафедры должно быть уникальным. " +
                            "Пароль должен быть длиной от 10 до 50 символов и состоять из цифр, букв и небуквенных символов" +
                            "В качестве логина должна быть указана почта", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Введенные пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}
