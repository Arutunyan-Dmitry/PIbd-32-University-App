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
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace DepartmentApp
{
    /// <summary>
    /// Логика взаимодействия для GroupCreateUpdateWindow.xaml
    /// </summary>
    public partial class GroupCreateUpdateWindow : Window
    {
        private MainController controller;
        public List<StudentViewModel> listStudents;
        public List<StudentViewModel> listUpdatedStudents;
        private List<StudentViewModel> listOldStudent;
        public int Id { set { id = value; } }
        public int? id;
        public GroupCreateUpdateWindow(int? id)
        {
            InitializeComponent();
            this.id = id;
            controller = new MainController();
            listStudents = new List<StudentViewModel>();
            listUpdatedStudents = new List<StudentViewModel>();
            listOldStudent = new List<StudentViewModel>();

            LoadDataStudents();
        }

        public void LoadDataStudents()
        {
            ListViewStudents.ItemsSource = null;
            if (id.HasValue)
            {
                GroupViewModel group = controller.GetGroup(id.Value);
                TextBoxNameGroup.Text = group.Name;
                TextBoxCourseGroup.Text = group.Course.ToString();
                var sourceStudents = controller.GetStudents(id.Value);
                ListViewStudents.ItemsSource = sourceStudents;
            }
            else
            {
                ListViewStudents.ItemsSource = null;
            }
        }
        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            GroupsWindowxaml groupsWindowxaml = new GroupsWindowxaml();
            groupsWindowxaml.Show();
            this.Close();
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            var elem = ((Button)sender).Tag.ToString();
            StudentCreateOrUpdateWindow studentCreateOrUpdateWindow = new StudentCreateOrUpdateWindow(Convert.ToInt32(elem));
            studentCreateOrUpdateWindow.ShowDialog();
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            GroupsWindowxaml groupsWindowxaml = new GroupsWindowxaml();
            groupsWindowxaml.Show();
            this.Close();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (!TextBoxNameGroup.Text.Equals("") && !TextBoxCourseGroup.Text.Equals(""))
            {
                if (id.HasValue)
                {
                    try
                    {
                        controller.CreateOrUpdateGroup(new GroupBindingModel
                        {
                            Id = id.Value,
                            DepartmentId = APIClient.DepartmentId,
                            Name = TextBoxNameGroup.Text,
                            Course = Convert.ToInt32(TextBoxCourseGroup.Text)

                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    MessageBox.Show("Группа успешно обновлена", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    try
                    {
                        controller.CreateOrUpdateGroup(new GroupBindingModel
                        {
                            DepartmentId = APIClient.DepartmentId,
                            Name = TextBoxNameGroup.Text,
                            Course = Convert.ToInt32(TextBoxCourseGroup.Text)

                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                GroupsWindowxaml groupsWindowxaml = new GroupsWindowxaml();
                groupsWindowxaml.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля формы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
