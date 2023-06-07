using DepartmentHTTPClient;
using DepartmentHTTPClient.Controllers;
using DocumentFormat.OpenXml;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
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
    /// Логика взаимодействия для MessageCreateWindow.xaml
    /// </summary>
    public partial class MessageCreateWindow : Window
    {
        MainController controller;

        private List<TeacherViewModel> listAllTeachers;
        private List<DisciplineViewModel> listAllDisciplines;
        private List<PlanViewModel> listAllPlans;
        List<string> displaynames;
        public MessageCreateWindow()
        {
            InitializeComponent();
            controller = new MainController();
            listAllTeachers = new List<TeacherViewModel>();
            listAllDisciplines = new List<DisciplineViewModel>();
            listAllPlans = new List<PlanViewModel>();

            /*
            enumValue.GetType()
                        .GetMember(enumValue.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        .GetName();

            var attributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);

            */
            // ComboBoxTypeReport.ItemsSource = Enum.GetValues(typeof(ReportTypes)).Cast<ReportTypes>();

            displaynames = new List<string>();
            var names = Enum.GetNames(typeof(ReportTypes));
            foreach (var name in names)
            {
                var field = (typeof(ReportTypes)).GetField(name);
                var fds = field.GetCustomAttributes(typeof(DisplayAttribute), true);

                if (fds.Length == 0)
                {
                    displaynames.Add(name);
                }

                foreach (DisplayAttribute fd in fds)
                {
                    displaynames.Add(fd.Name);
                }
            }



            ComboBoxTypeReport.ItemsSource = displaynames;


            LoadDataTeachers(null);
            LoadDataDisciplines();
            LoadDataTypePlan();


            ComboBoxTypePlan.Visibility = Visibility.Hidden;
            ComboBoxTeachers.Visibility = Visibility.Hidden;
            ComboBoxDisciplines.Visibility = Visibility.Hidden;
        }

        private void LoadDataTypePlan()
        {
            ComboBoxTypePlan.ItemsSource = null;
            try
            {
                var sourceComboboxTypePlan = controller.GetDepartmentPlans(APIClient.DepartmentId);
                listAllPlans = sourceComboboxTypePlan;
                ComboBoxTypePlan.ItemsSource = listAllPlans;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDataDisciplines()
        {
             try
            {
                var sourceComboboxDisciplines = controller.GetDisciplines(APIClient.DepartmentId);
                listAllDisciplines = sourceComboboxDisciplines;
                ComboBoxDisciplines.ItemsSource = listAllDisciplines;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDataTeachers(int? disciplineId)
        {
            if (disciplineId.HasValue)
            {
                ComboBoxTeachers.ItemsSource = null;
                var sourceComboboxTeachers = controller.GetTeacherByDiscipline(disciplineId.Value);
                listAllTeachers = sourceComboboxTeachers;
                ComboBoxTeachers.ItemsSource = listAllTeachers;
            }
            else
            {
                ComboBoxTeachers.ItemsSource = null;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageWindow messageWindow = new MessageWindow();
            messageWindow.Show();
            this.Close();
        }
        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            MessageWindow messageWindow = new MessageWindow();
            messageWindow.Show();
            this.Close();
        }

        private void ComboBoxTypeReport_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string currentItem = (string)ComboBoxTypeReport.SelectedItem;

            if (currentItem.ToString().Equals("Отчёт по занятию"))
            {
                ComboBoxTypePlan.Visibility = Visibility.Visible;
                ComboBoxTeachers.Visibility = Visibility.Hidden;
                ComboBoxDisciplines.Visibility = Visibility.Hidden;
            }
            else if (currentItem.ToString().Equals("Итоговый отчёт по дисциплине"))
            {
                ComboBoxTypePlan.Visibility = Visibility.Hidden;
                ComboBoxTeachers.Visibility = Visibility.Visible;
                ComboBoxDisciplines.Visibility = Visibility.Visible;
            }
        }

        private void ComboBoxDisciplines_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DisciplineViewModel currentItem = (DisciplineViewModel)ComboBoxDisciplines.SelectedItem;
            LoadDataTeachers(currentItem.Id);            
        }


        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (((string)ComboBoxTypeReport.SelectedItem).Equals("Итоговый отчёт по дисциплине"))
            {
                controller.CreateMessage(new MessageBindingModel
                {
                    TeacherId = ((TeacherViewModel) ComboBoxTeachers.SelectedItem).Id,
                    DisciplineId = ((DisciplineViewModel) ComboBoxDisciplines.SelectedItem).Id,
                    DepartmentId = APIClient.DepartmentId,
                    ReportType = null
                });
            }
            else if (((string)ComboBoxTypeReport.SelectedItem).Equals("Отчёт по занятию"))
            {
                controller.CreateMessage(new MessageBindingModel
                {
                    PlanId = ((PlanViewModel) ComboBoxTypePlan.SelectedItem).Id,
                    DepartmentId = APIClient.DepartmentId,
                    ReportType = null
                });
            }

            MessageWindow messageWindow = new MessageWindow();
            messageWindow.Show();
            this.Close();
        }
    }
}
