using UniversityBusinessLogic.OfficePackage.HelperEnums;
using UniversityBusinessLogic.OfficePackage.HelperModels;
using UniversityContracts.ViewModels;

namespace UniversityBusinessLogic.OfficePackage
{
    public abstract class AbstractSaveToWord
    {
        public void CreatePlanDoc(ReportPlanViewModel model, string filename)
        {
            CreateWord(filename);
            CreateParagraph(new WordParagraph
            {
                Texts = new List<(string, WordTextProperties)> {
                (model.Title, new WordTextProperties { Bold = true, Size = "24", })},
                TextProperties = new WordTextProperties
                {
                    Size = "24",
                    JustificationType = WordJustificationType.Center
                }
            });
            CreateParagraph(new WordParagraph
            {
                Texts = new List<(string, WordTextProperties)> {
                (model.PlanName, new WordTextProperties { Bold = true, Size = "20", })},
                TextProperties = new WordTextProperties
                {
                    Size = "20",
                    JustificationType = WordJustificationType.Both
                }
            });
            CreateParagraph(new WordParagraph
            {
                Texts = new List<(string, WordTextProperties)> {
                (" ", new WordTextProperties { Bold = true, Size = "20", })},
                TextProperties = new WordTextProperties
                {
                    Size = "20",
                    JustificationType = WordJustificationType.Both
                }
            });
            List<string> head = new List<string>
            {
                "ФИО"
            };
            foreach (var testing in model.Items[0].Item2)
            {
                head.Add(testing.Item1);
            }
            CreateTable(head);
            foreach (var items in model.Items)
            {
                var list = new List<string>
                {
                    items.Item1
                };
                foreach (var subItem in items.Item2)
                {
                    list.Add(subItem.Item2.ToString());
                }
                CreateRow(list);
            }
            CreateParagraph(new WordParagraph
            {
                Texts = new List<(string, WordTextProperties)> {
                (" ", new WordTextProperties { Bold = true, Size = "20", })},
                TextProperties = new WordTextProperties
                {
                    Size = "20",
                    JustificationType = WordJustificationType.Both
                }
            });
            CreateParagraph(new WordParagraph
            {
                Texts = new List<(string, WordTextProperties)> {
                (model.Footer, new WordTextProperties { Bold = false, Size = "20", })},
                TextProperties = new WordTextProperties
                {
                    Size = "20",
                    JustificationType = WordJustificationType.Both
                }
            });
            CreateParagraph(new WordParagraph
            {
                Texts = new List<(string, WordTextProperties)> {
                (DateTime.Now.ToShortDateString(), new WordTextProperties { Bold = false, Size = "20", })},
                TextProperties = new WordTextProperties
                {
                    Size = "20",
                    JustificationType = WordJustificationType.Both
                }
            });
            SaveWord();
        }
        public void CreateFullDoc(ReportFullViewModel model, string filename)
        {
            int maxWidth = 13000;

            CreateWord(filename);
            CreateParagraph(new WordParagraph
            {
                Texts = new List<(string, WordTextProperties)> {
                (model.Title, new WordTextProperties { Bold = true, Size = "24", })},
                TextProperties = new WordTextProperties
                {
                    Size = "24",
                    JustificationType = WordJustificationType.Center
                }
            });
            CreateParagraph(new WordParagraph
            {
                Texts = new List<(string, WordTextProperties)> {
                ("Дисциплина: " + model.DisciplineName, new WordTextProperties { Bold = true, Size = "20", })},
                TextProperties = new WordTextProperties
                {
                    Size = "20",
                    JustificationType = WordJustificationType.Both
                }
            });

            //-------------body------------------
            foreach (var group in model.Items)
            {
                CreateParagraph(new WordParagraph
                {
                    Texts = new List<(string, WordTextProperties)> {
                (" ", new WordTextProperties { Bold = true, Size = "20", })},
                    TextProperties = new WordTextProperties
                    {
                        Size = "20",
                        JustificationType = WordJustificationType.Both
                    }
                });
                CreateParagraph(new WordParagraph
                {
                    Texts = new List<(string, WordTextProperties)> {
                ("Группа: " + group.Item1, new WordTextProperties { Bold = true, Size = "20", })},
                    TextProperties = new WordTextProperties
                    {
                        Size = "20",
                        JustificationType = WordJustificationType.Both
                    }
                });

                var join = new List<(int, int, string)>();
                int count = 1;
                for (int i = 0; i < group.Item2[0].Item2.Count(); i++)
                {
                    join.Add((
                        count,
                        count + group.Item2[0].Item2[i].Item2.Count() - 1,
                        group.Item2[0].Item2[i].Item1));
                    count += group.Item2[0].Item2[i].Item2.Count();
                }

                var columns = new List<string> { "ФИО" };
                foreach (var subitem in group.Item2[0].Item2)
                {
                    foreach (var subitem2 in subitem.Item2)
                    {
                        columns.Add(subitem2.Item1);
                    }
                }

                var width = new int[columns.Count()];
                for (int i = 0; i < width.Length; i++)
                {
                    width[i] = maxWidth / width.Length;
                }
                CreateComplexTable(width, join, columns);
                foreach (var subItem in group.Item2)
                {
                    var row = new List<string>{ subItem.Item1 };
                    foreach(var subItem2 in subItem.Item2)
                    {
                        foreach(var subItem3 in subItem2.Item2)
                        {
                            row.Add(subItem3.Item2.ToString());
                        }
                    }
                    CreateRow(row);
                }
            }
            //--------------------------------------------------

            CreateParagraph(new WordParagraph
            {
                Texts = new List<(string, WordTextProperties)> {
                (" ", new WordTextProperties { Bold = true, Size = "20", })},
                TextProperties = new WordTextProperties
                {
                    Size = "20",
                    JustificationType = WordJustificationType.Both
                }
            });
            CreateParagraph(new WordParagraph
            {
                Texts = new List<(string, WordTextProperties)> {
                (model.Footer, new WordTextProperties { Bold = false, Size = "20", })},
                TextProperties = new WordTextProperties
                {
                    Size = "20",
                    JustificationType = WordJustificationType.Both
                }
            });
            CreateParagraph(new WordParagraph
            {
                Texts = new List<(string, WordTextProperties)> {
                (DateTime.Now.ToShortDateString(), new WordTextProperties { Bold = false, Size = "20", })},
                TextProperties = new WordTextProperties
                {
                    Size = "20",
                    JustificationType = WordJustificationType.Both
                }
            });
            SaveWord();
        }
        /// <summary>
        /// Создание doc-файла
        /// </summary>
        /// <param name="info"></param>
        protected abstract void CreateWord(string info);
        /// <summary>
        /// Создание абзаца с текстом
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        protected abstract void CreateParagraph(WordParagraph paragraph);
        /// <summary>
        /// Создание заголовка таблицы с текстом
        /// </summary>
        /// <param name="tableHeader"></param>
        protected abstract void CreateTable(List<string> tableHeader);
        /// <summary>
        /// Создание сложного заголовка таблицы с текстом
        /// </summary>
        /// <param name="tableHeader"></param>
        protected abstract void CreateComplexTable(int[] width, List<(int, int, string)> joined,
            List<string> columns);
        /// <summary>
        /// Создание строки таблицы с текстом
        /// </summary>
        /// <param name="tableRow"></param>
        protected abstract void CreateRow(List<string> tableRow);
        /// <summary>
        /// Сохранение файла
        /// </summary>
        protected abstract void SaveWord();
    }
}
