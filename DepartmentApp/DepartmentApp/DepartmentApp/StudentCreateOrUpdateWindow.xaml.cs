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
using UniversityContracts.Enums;
using UniversityContracts.ViewModels;

namespace DepartmentApp
{
    /// <summary>
    /// Логика взаимодействия для StudentCreateOrUpdateWindow.xaml
    /// </summary>
    public partial class StudentCreateOrUpdateWindow : Window
    {
        
        public int Id { set { id = value; } }
        private int? id;
        StudentViewModel? oldStudent;
        MainController controller;
        List<GroupViewModel> groups;
        GroupCreateUpdateWindow group;
        public StudentCreateOrUpdateWindow(int? id)
        {
            InitializeComponent();
            oldStudent = new StudentViewModel();
            controller = new MainController();

            LoadGroupStudent();
            ComboBoxBasementStudent.ItemsSource = Enum.GetValues(typeof(TypeEducationBasement)).Cast<TypeEducationBasement>();
            this.id = id;
            if (id.HasValue)
            {
                oldStudent = controller.GetStudent(id.Value);
                TextBoxFLMStudent.Text = oldStudent.Flm;
                GroupViewModel group = controller.GetGroup(oldStudent.GroupId);
                if (group != null)
                {
                    int indexSelectedGroup = 0;
                    for(int i=0; i<groups.Count();i++)
                    {
                        if (groups[i].Id == group.Id)
                        {
                            ComboBoxGroupStudent.SelectedIndex = i;
                        }
                    }
                }
                ComboBoxBasementStudent.SelectedValue = oldStudent.Basement;
            }
        }

        private void LoadGroupStudent()
        {
            ComboBoxGroupStudent.ItemsSource = null;
            try
            {
                List<GroupViewModel> sourceComboboxGroupPlan = controller.GetGroups(APIClient.DepartmentId);
                if (sourceComboboxGroupPlan.Count() > 0)
                {
                    groups = sourceComboboxGroupPlan;
                }
                ComboBoxGroupStudent.ItemsSource = sourceComboboxGroupPlan;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (!TextBoxFLMStudent.Text.Equals("") && ComboBoxBasementStudent.SelectedValue != null && ComboBoxGroupStudent.SelectedValue != null)
            {
                if (id.HasValue)
                {
                    try
                    {
                        StudentBindingModel student = new StudentBindingModel
                        {
                            Id = id.Value,
                            DepartmentId = APIClient.DepartmentId,
                            GroupId = ((GroupViewModel)ComboBoxGroupStudent.SelectedItem).Id,
                            Flm = TextBoxFLMStudent.Text,
                            Basement = (TypeEducationBasement)ComboBoxBasementStudent.SelectedItem
                        };
                        controller.CreateOrUpdateStudent(student);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    StudentBindingModel student = new StudentBindingModel
                    {
                        DepartmentId = APIClient.DepartmentId,
                        GroupId = ((GroupViewModel)ComboBoxGroupStudent.SelectedItem).Id,
                        Flm = TextBoxFLMStudent.Text,
                        Basement = (TypeEducationBasement)ComboBoxBasementStudent.SelectedItem
                    };
                    controller.CreateOrUpdateStudent(student);
                }
                StudentWindow studentWindow = new StudentWindow();
                studentWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля формы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            StudentWindow studentWindow = new StudentWindow();
            studentWindow.Show();
            this.Close();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            StudentWindow studentWindow = new StudentWindow();
            studentWindow.Show();
            this.Close();
        }
    }
}
