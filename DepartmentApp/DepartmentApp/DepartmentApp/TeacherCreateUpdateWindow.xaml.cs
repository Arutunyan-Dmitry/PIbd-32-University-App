using DepartmentHTTPClient;
using DepartmentHTTPClient.Controllers;
using Microsoft.AspNetCore.Mvc;
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
    /// Логика взаимодействия для TeacherCreateUpdateWindow.xaml
    /// </summary>
    public partial class TeacherCreateUpdateWindow : Window
    {
        MainController controller;
        public int Id { set { id = value; } }
        private int? id;
        TeacherViewModel? oldTeacher;
        public TeacherCreateUpdateWindow(int? id)
        {
            InitializeComponent();
            controller = new MainController();
            this.id = id;
            if (id.HasValue)
            {
                try
                {
                    oldTeacher = controller.GetTeacher(id.Value, null, null);
                    if (oldTeacher != null)
                    {
                        TextBoxFlmTeacher.Text = oldTeacher.Flm;
                        TextBoxLoginTeacher.Text = oldTeacher.Login;
                        TextBoxPasswordTeacher.Text = oldTeacher.Password;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Извините, сервер временно не работает", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (!TextBoxFlmTeacher.Text.Equals("") && !TextBoxLoginTeacher.Text.Equals("") && !TextBoxPasswordTeacher.Text.Equals(""))
            {
                try
                {
                    if (id.HasValue)
                    {
                        controller.CreateOrUpdateTeacher(new TeacherBindingModel
                        {
                            Id = id.Value,
                            DepartmentId = APIClient.DepartmentId,
                            Flm = TextBoxFlmTeacher.Text,
                            Login = TextBoxLoginTeacher.Text,
                            Password = TextBoxPasswordTeacher.Text,
                            TeacherDisciplines = oldTeacher.TeacherDisciplines.Keys.ToList(),
                        });
                        MessageBox.Show("Информация обновлена", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        controller.CreateOrUpdateTeacher(new TeacherBindingModel
                        {
                            DepartmentId = APIClient.DepartmentId,
                            Flm = TextBoxFlmTeacher.Text,
                            Login = TextBoxLoginTeacher.Text,
                            Password = TextBoxPasswordTeacher.Text,
                            TeacherDisciplines = new List<int>()
                        });
                        MessageBox.Show("Новый преподаватель успешно добавлен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Извините, сервер временно не работает", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                TeacherWindow teacherWindow = new TeacherWindow();
                teacherWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля формы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            TeacherWindow teacherWindow = new TeacherWindow();
            teacherWindow.Show();
            this.Close();
        }
    }
}
