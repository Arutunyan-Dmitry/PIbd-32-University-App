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
using UniversityContracts.Enums;
using UniversityContracts.ViewModels;

namespace DepartmentApp
{
    /// <summary>
    /// Логика взаимодействия для PlanCreateUpdateWindow.xaml
    /// </summary>
    public partial class PlanCreateUpdateWindow : Window
    {

        private List<TeacherViewModel> listAllTeachers;
        private List<GroupViewModel> listAllGroups;

        MainController controller;

        DisciplineCreateUpdateWindow discipline;
        public PlanCreateUpdateWindow(DisciplineCreateUpdateWindow discipline)
        {
            InitializeComponent();
            controller = new MainController();
            listAllGroups = new List<GroupViewModel>();
            listAllTeachers = new List<TeacherViewModel>();
            this.discipline = discipline;
            LoadTeacherPlan();
            LoadGroupPlan();
            ComboBoxTypePlan.ItemsSource = Enum.GetValues(typeof(PlanType)).Cast<PlanType>();
        }

        private void LoadTeacherPlan()
        {
            ComboBoxTeacherPlan.ItemsSource = null;
            try
            {
                var sourceComboboxTeacherPlan = discipline.listSelectedTeachers;
                listAllTeachers = sourceComboboxTeacherPlan;
                ComboBoxTeacherPlan.ItemsSource = listAllTeachers;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadGroupPlan()
        {
            ComboBoxGroupPlan.ItemsSource = null;
            try
            {
                var sourceComboboxGroupPlan = controller.GetGroups(APIClient.DepartmentId);
                listAllGroups = sourceComboboxGroupPlan;
                ComboBoxGroupPlan.ItemsSource = listAllGroups;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DisciplineWindow disciplineWindow = new DisciplineWindow();
            disciplineWindow.Show();
            this.Close();
        }
        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            DisciplineWindow disciplineWindow = new DisciplineWindow();
            disciplineWindow.Show();
            this.Close();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxTeacherPlan.SelectedValue != null && ComboBoxGroupPlan.SelectedValue != null && !TextBoxHours.Text.Equals("")
                && ComboBoxTypePlan.SelectedValue != null)
            {
                PlanViewModel newPlan = new PlanViewModel
                {
                    DepartmentId = APIClient.DepartmentId,
                    TeacherId = ((TeacherViewModel)ComboBoxTeacherPlan.SelectedItem).Id,
                    GroupId = ((GroupViewModel)ComboBoxGroupPlan.SelectedItem).Id,
                    GroupName = ((GroupViewModel)ComboBoxGroupPlan.SelectedItem).Name,
                    Hours = Convert.ToInt32(TextBoxHours.Text),
                    Type = (PlanType)ComboBoxTypePlan.SelectedItem
                };
                if (discipline.oldDiscipline != null)
                {
                    if (!discipline.listNewPlans.Contains(newPlan))
                    {
                        discipline.listNewPlans.Add(newPlan);
                        discipline.LoadDataPlans();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Данный учебный план уже добавлен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (!discipline.listPlans.Contains(newPlan))
                    {
                        discipline.listPlans.Add(newPlan);
                        discipline.LoadDataPlans();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Данный учебный план уже добавлен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля формы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
