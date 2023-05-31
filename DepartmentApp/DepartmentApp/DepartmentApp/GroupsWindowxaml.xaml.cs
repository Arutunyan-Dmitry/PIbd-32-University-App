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
using UniversityContracts.ViewModels;

namespace DepartmentApp
{
    /// <summary>
    /// Логика взаимодействия для GroupsWindowxaml.xaml
    /// </summary>
    public partial class GroupsWindowxaml : Window
    {
        MainController controller;
        public GroupsWindowxaml()
        {
            InitializeComponent();
            controller = new MainController();
            LoadData();
        }
        private void LoadData()
        {
            ListViewGroups.ItemsSource = null;
            try
            {
                var sourceGroups = controller.GetGroups(APIClient.DepartmentId);
                if (sourceGroups != null)
                {
                    ListViewGroups.ItemsSource = sourceGroups;
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

        private void ButtonCreateUpdateGroup_Click(object sender, RoutedEventArgs e)
        {
            GroupCreateUpdateWindow groupCreateUpdate = new GroupCreateUpdateWindow(null);
            groupCreateUpdate.Show();
            this.Close();
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            var elem = ((Button)sender).Tag.ToString();
            GroupCreateUpdateWindow groupCreateUpdate = new GroupCreateUpdateWindow(Convert.ToInt32(elem));
            groupCreateUpdate.Show();
            this.Close();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var elem = ((Button)sender).Tag.ToString();
            try
            {
                var result = controller.DeleteGroup(Convert.ToInt32(elem));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadData();
        }
    }
}
