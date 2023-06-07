using DepartmentHTTPClient;
using DepartmentHTTPClient.Controllers;
using DocumentFormat.OpenXml.Presentation;
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
using UniversityContracts.Enums;
using UniversityContracts.ViewModels;

namespace DepartmentApp
{
    /// <summary>
    /// Логика взаимодействия для MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        MainController controller;
        List<MessageViewModel> listAllMessages;

        public MessageWindow()
        {
            InitializeComponent();
            controller = new MainController();
            listAllMessages = new List<MessageViewModel>();
            LoadDataMessages();
        }
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield return (T)Enumerable.Empty<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);
                if (ithChild == null) continue;
                if (ithChild is T t) yield return t;
                foreach (T childOfChild in FindVisualChildren<T>(ithChild)) yield return childOfChild;
            }
        }
        private void LoadDataMessages()
        {
            ListViewMessages.ItemsSource = null;
            try
            {
                var sourceMessages = controller.GetDepartmentMessages(APIClient.DepartmentId);
                if (sourceMessages != null)
                {
                    listAllMessages = sourceMessages;
                    ListViewMessages.ItemsSource = sourceMessages;
                   // CheckStatus(sourceMessages);
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            CheckStatus(listAllMessages);
            // Your code here.
        }

        private void CheckStatus(List<MessageViewModel> sourceMessages)
        {
            foreach (MessageViewModel elem in sourceMessages)
            {
                if (elem.Status != Status.Проверяется)
                {
                    foreach (Button button in FindVisualChildren<Button>(this))
                    {
                        if (button.Tag != null && Convert.ToInt32(button.Tag) == elem.Id)
                        {
                            button.Visibility = Visibility.Hidden;
                        }
                    }
                }
            }
        }

        private void ButtonCheck_Click(object sender, RoutedEventArgs e)
        {
            var elem = ((Button)sender).Tag.ToString();
            MessageCheckWindow messageCheckWindow = new MessageCheckWindow(Convert.ToInt32(elem));
            messageCheckWindow.Show();
            this.Close();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ButtonCreateUpdateMessage_Click(object sender, RoutedEventArgs e)
        {
            MessageCreateWindow messageCreateWindow = new MessageCreateWindow();
            messageCreateWindow.Show();
            this.Close();
        }
    }
}
