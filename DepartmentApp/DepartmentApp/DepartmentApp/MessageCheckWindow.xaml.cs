using DepartmentHTTPClient;
using DepartmentHTTPClient.Controllers;
using DocumentFormat.OpenXml.EMMA;
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
    /// Логика взаимодействия для MessageCheckWindow.xaml
    /// </summary>
    public partial class MessageCheckWindow : Window
    {
        public int Id { set { id = value; } }
        private int? id;
        MessageViewModel? messageReport;
        MainController controller;
        /*
         * "Отчёт по учебному плану " + model.PlanName + ", преподаватель: " + APIClient.teacher.Flm
"Итоговый отчёт по дисциплине " + model.DisciplineName + ", преподаватель: " + APIClient.teacher.Flm,
         */
        public MessageCheckWindow(int? id)
        {
            InitializeComponent();
            this.id = id;
            messageReport = new MessageViewModel();
            controller = new MainController();
            if (id.HasValue)
            {
                messageReport = controller.GetMessage(id.Value);
                TeacherViewModel? teacher = controller.GetTeacher(messageReport.TeacherId, null, null);
                if(messageReport != null)
                {
                    if(messageReport.ReportType == ReportTypes.SumReport)
                    {
                        TextBlockMessageTopic.Text = "Итоговый отчёт по дисциплине " + messageReport.DisciplineName + ", преподаватель: " + teacher.Flm;
                    }else if(messageReport.ReportType == ReportTypes.LessonReport)
                    {
                        TextBlockMessageTopic.Text = "Отчёт по учебному плану " + messageReport.PlanName + ", преподаватель: " + teacher.Flm;
                    }
                }
            }
            else
            {

            }
        }
        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            MessageWindow messageWindow = new MessageWindow();
            messageWindow.Show();
            this.Close();
        }

        private void ButtonRecheck_Click(object sender, RoutedEventArgs e)
        {
            controller.MessageRollBack(new MessageBindingModel
            {
                Id = messageReport.Id,
                DepartmentId = APIClient.DepartmentId
            });
            MessageWindow messageWindow = new MessageWindow();
            messageWindow.Show();
            this.Close();
        }

        private void ButtonAgree_Click(object sender, RoutedEventArgs e)
        {
            controller.CloseRequest(new MessageBindingModel
            {
                Id = messageReport.Id,
                DepartmentId = APIClient.DepartmentId
            });
            MessageWindow messageWindow = new MessageWindow();
            messageWindow.Show();
            this.Close();
        }
    }
}
