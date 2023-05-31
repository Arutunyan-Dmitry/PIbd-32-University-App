
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO.Packaging;
using DepartmentHTTPClient;
using DepartmentHTTPClient.Controllers;
using UniversityContracts.ViewModels;

namespace DepartmentApp
{
    /// <summary>
    /// Логика взаимодействия для ImportStudentWindow.xaml
    /// </summary>
    public partial class ImportStudentWindow : Window
    {
        MainController controller;
        public ImportStudentWindow()
        {
            InitializeComponent();
            controller = new MainController();
        }
        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ButtonImportStudent_Click(object sender, RoutedEventArgs e)
        {
            List<StudentBindingModel> newStudents = new List<StudentBindingModel>();

            var dialog = new OpenFileDialog { Filter = "xlsx|*.xlsx" };
            if (dialog.ShowDialog() == true)
            {

                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(dialog.FileName, false))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                    string text;

                    int rowCount = sheetData.Elements<Row>().Count();

                    string nameGroup = "";
                    int courseGroup = 0;
                    foreach (Row r in sheetData.Elements<Row>())
                    {
                        int indexCell = 1;
                        StudentBindingModel newStudent = new StudentBindingModel();

                        foreach (Cell c in r.Elements<Cell>())
                        {

                            List<GroupViewModel> groups = controller.GetGroups(APIClient.DepartmentId);
                            string value = "empty";
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                value = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText;
                                if (value.Contains("Фамилия"))
                                {
                                    break;
                                }
                                if (value.Contains("Список группы"))
                                {
                                    string inputString = value;
                                    string searchText = "группы ";

                                    int startIndex = inputString.IndexOf(searchText) + searchText.Length;
                                    int endIndex = inputString.IndexOf(" (", startIndex);

                                    string result = inputString.Substring(startIndex, endIndex - startIndex);

                                    nameGroup = result;

                                    string inputNameGroup = nameGroup;

                                    char numCourse = inputString.Where(char.IsDigit).FirstOrDefault();
                                    courseGroup = Convert.ToInt32(numCourse.ToString());

                                    break;
                                }
                                if (indexCell == 3)
                                {
                                    newStudent = new StudentBindingModel
                                    {
                                        Flm = value,
                                    };
                                }
                                if (indexCell == 4)
                                {
                                    newStudent = new StudentBindingModel
                                    {
                                        Flm = newStudent.Flm + " " + value,
                                    };
                                }
                                if (indexCell == 5)
                                {
                                    newStudent = new StudentBindingModel
                                    {
                                        Flm = newStudent.Flm + " " + value,
                                    };
                                }
                                if (indexCell == 6)
                                {
                                    GroupViewModel? checkGroup = groups.FirstOrDefault(x => x.Name == nameGroup);
                                    if (checkGroup == null)
                                    {
                                        controller.CreateOrUpdateGroup(new GroupBindingModel
                                        {
                                            DepartmentId = APIClient.DepartmentId,
                                            Name = nameGroup,
                                            Course = courseGroup
                                        });
                                        GroupViewModel group = controller.GetGroupsByName(nameGroup, APIClient.DepartmentId)[0];
                                        newStudent = new StudentBindingModel
                                        {
                                            DepartmentId = APIClient.DepartmentId,
                                            Flm = newStudent.Flm,
                                            GroupId = group.Id,
                                            Basement = (TypeEducationBasement)Enum.Parse(typeof(TypeEducationBasement), value, true)
                                        };
                                    }
                                    else
                                    {
                                        newStudent = new StudentBindingModel
                                        {
                                            DepartmentId = APIClient.DepartmentId,
                                            Flm = newStudent.Flm,
                                            GroupId = checkGroup.Id,
                                            Basement = (TypeEducationBasement)Enum.Parse(typeof(TypeEducationBasement), value, true)
                                        };
                                    }
                                    break;
                                }
                            }
                            indexCell++;
                        }
                        newStudents.Add(newStudent);
                    }
                }
            }
            List<StudentViewModel> listAllStudent = controller.GetStudentsByDepartment(APIClient.DepartmentId);
            foreach (StudentBindingModel student in newStudents)
            {
                if (student.Flm != null)
                {
                    StudentViewModel? checkStudent = listAllStudent.FirstOrDefault(x => x.Flm == student.Flm);
                    if (checkStudent == null)
                    {
                        controller.CreateOrUpdateStudent(student);
                    }
                }
            }
            MessageBox.Show("everything is good", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

        }

    }
}
