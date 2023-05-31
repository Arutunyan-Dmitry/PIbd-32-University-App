using DepartmentHTTPClient;
using DepartmentHTTPClient.Controllers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Unity.Policy;
using UniversityContracts.BindingModels;
using UniversityContracts.ViewModels;

namespace DepartmentApp
{
    /// <summary>
    /// Логика взаимодействия для DisciplineWindow.xaml
    /// </summary>
    public partial class DisciplineWindow : Window
    {
        private MainController controller;
        private List<DisciplineViewModel> sourceDiscipline;
        public DisciplineWindow()
        {
            InitializeComponent();
            controller = new MainController();
            LoadData();
            sourceDiscipline = new List<DisciplineViewModel>();
        }

        private void LoadData()
        {
            ListViewDiscipline.ItemsSource = null;
            try
            {
                sourceDiscipline = controller.GetDisciplines(APIClient.DepartmentId);
                if (sourceDiscipline != null)
                {
                    ListViewDiscipline.ItemsSource = sourceDiscipline;
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

        private void ButtonAddDiscipline_Click(object sender, RoutedEventArgs e)
        {
            DisciplineCreateUpdateWindow createUpdateWindow = new DisciplineCreateUpdateWindow(null);
            createUpdateWindow.Show();
            this.Close();
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            var elem = ((Button)sender).Tag.ToString();
            DisciplineCreateUpdateWindow createUpdateWindow = new DisciplineCreateUpdateWindow(Convert.ToInt32(elem));
            createUpdateWindow.Show();
            this.Close();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var elem = ((Button)sender).Tag.ToString();
            controller.DeleteDiscipline(Convert.ToInt32(elem));
            LoadData();
        }
    }
}
