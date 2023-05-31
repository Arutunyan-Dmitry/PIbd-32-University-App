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
    /// Логика взаимодействия для DisciplineCreateUpdateWindow.xaml
    /// </summary>
    public partial class DisciplineCreateUpdateWindow : Window
    {
        MainController controller;
        public int Id { set { id = value; } }
        private int? id;
        public List<TeacherViewModel> listSelectedTeachers;
        public List<PlanViewModel> listPlans;

        public DisciplineViewModel? oldDiscipline;
        public List<PlanViewModel> listNewPlans;
        public DisciplineCreateUpdateWindow(int? id)
        {
            InitializeComponent();
            controller = new MainController();
            listPlans = new List<PlanViewModel>();
            listSelectedTeachers = new List<TeacherViewModel>();
            listNewPlans = new List<PlanViewModel>();
            LoadDataTeachersCombobox();
            this.id = id;
            if (id.HasValue)
            {
                oldDiscipline = controller.GetDiscipline(id.Value);
                TextBoxNameDiscipline.Text = oldDiscipline.Name;
                List<TeacherViewModel> teachers = controller.GetTeacherByDiscipline(oldDiscipline.Id);
                listSelectedTeachers = teachers;
                List<PlanViewModel> plans = controller.GetPlansByDiscipline(oldDiscipline.Id);
                listPlans = plans;
            }
            LoadDataTeachers();
            LoadDataPlans();
        }

        private void LoadDataTeachers()
        {
            ListViewTeachers.ItemsSource = null;
            if (listSelectedTeachers != null)
            {
                var sourceTeacher = listSelectedTeachers;
                ListViewTeachers.ItemsSource = sourceTeacher;
            }
        }

        public void LoadDataPlans()
        {
            ListViewPlans.ItemsSource = null;
            try
            {
                var sourcePlan = listPlans;
                if (id.HasValue)
                {
                    if (listNewPlans.Count() > 0)
                    {
                        foreach (var elem in listNewPlans)
                        {
                            PlanViewModel? checkUniq = listPlans.FirstOrDefault(x => x.GroupName == elem.GroupName);
                            if (checkUniq == null)
                            {
                                listPlans.Add(elem);
                            }
                        }
                    }
                }
                ListViewPlans.ItemsSource = listPlans;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDataTeachersCombobox()
        {
            ComboBoxTeachers.ItemsSource = null;
            try
            {
                var sourceComboBoxTeachers = controller.GetTeachers(APIClient.DepartmentId);
                ComboBoxTeachers.ItemsSource = sourceComboBoxTeachers;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            DisciplineWindow disciplineWindow = new DisciplineWindow();
            disciplineWindow.Show();
            this.Close();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (!TextBoxNameDiscipline.Text.Equals(""))
            {
                if (id.HasValue)
                {
                    try
                    {
                        controller.CreateOrUpdateDiscipline(new DisciplineBindingModel
                        {
                            Id = id.Value,
                            DepartmentId = APIClient.DepartmentId,
                            Name = TextBoxNameDiscipline.Text
                        });
                        if (listSelectedTeachers != null)
                        {
                            foreach (var teacher in listSelectedTeachers)
                            {
                                controller.AddTeachersToDiscipline(oldDiscipline.Id, teacher.Id);
                            }
                        }
                        if (listNewPlans != null)
                        {
                            foreach (var plan in listNewPlans)
                            {
                                controller.CreateOrUpdatePlan(new PlanBindingModel
                                {
                                    DepartmentId = plan.DepartmentId,
                                    TeacherId = plan.TeacherId,
                                    DisciplineId = oldDiscipline.Id,
                                    GroupId = plan.GroupId,
                                    Name = "",
                                    Hours = plan.Hours,
                                    Type = plan.Type

                                });
                            }
                        }
                        MessageBox.Show("Дисциплина успешно обновлена", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    try
                    {
                        controller.CreateOrUpdateDiscipline(new DisciplineBindingModel
                        {
                            DepartmentId = APIClient.DepartmentId,
                            Name = TextBoxNameDiscipline.Text
                        });
                        DisciplineViewModel discipline = controller.GetDisciplinesByName(TextBoxNameDiscipline.Text, APIClient.DepartmentId)[0];
                        if (listSelectedTeachers != null)
                        {
                            foreach (var teacher in listSelectedTeachers)
                            {
                                controller.AddTeachersToDiscipline(discipline.Id, teacher.Id);
                            }
                        }

                        if (listPlans != null)
                        {
                            foreach (var plan in listPlans)
                            {
                                controller.CreateOrUpdatePlan(new PlanBindingModel
                                {
                                    DepartmentId = plan.DepartmentId,
                                    TeacherId = plan.TeacherId,
                                    DisciplineId = discipline.Id,
                                    GroupId = plan.GroupId,
                                    Name = "",
                                    Hours = plan.Hours,
                                    Type = plan.Type

                                });
                            }
                        }

                        MessageBox.Show("Новая дисциплина успешно добавлена", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                DisciplineWindow disciplineWindow = new DisciplineWindow();
                disciplineWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля формы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DisciplineWindow disciplineWindow = new DisciplineWindow();
            disciplineWindow.Show();
            this.Close();
        }

        private void ButtonAddPlan_Click(object sender, RoutedEventArgs e)
        {
            PlanCreateUpdateWindow planCreateUpdateWindow = new PlanCreateUpdateWindow(this);
            planCreateUpdateWindow.Show();
        }

        private void ButtonAddTeacher_Click(object sender, RoutedEventArgs e)
        {
            TeacherViewModel newTeacher = (TeacherViewModel)ComboBoxTeachers.SelectedItem;

            if (!listSelectedTeachers.Contains(newTeacher))
            {
                TeacherViewModel? checkUniq = listSelectedTeachers.FirstOrDefault(x => x.Flm == newTeacher.Flm);
                if (checkUniq == null)
                {
                    listSelectedTeachers.Add(newTeacher);

                    MessageBox.Show("Преподаватель добавлен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadDataTeachers();
                }
                else
                {
                    MessageBox.Show("Преподаватель уже добавлен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Преподаватель уже добавлен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void ButtonDelTeacher_Click(object sender, RoutedEventArgs e)
        {
            TeacherViewModel newTeacher = (TeacherViewModel)ComboBoxTeachers.SelectedItem;

            TeacherViewModel? checkUniq = listSelectedTeachers.FirstOrDefault(x => x.Flm == newTeacher.Flm);
            if (checkUniq != null)
            {
                listSelectedTeachers.Remove(checkUniq);
                MessageBox.Show("Преподаватель удален", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                if (id.HasValue && newTeacher.TeacherDisciplines.Keys.Contains(id.Value))
                {
                    controller.RemoveTeacerFromDiscipline(newTeacher.Id, id.Value);
                }
                LoadDataTeachers();
            }
            else
            {
                MessageBox.Show("Преподаватель еще не был добавлен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
