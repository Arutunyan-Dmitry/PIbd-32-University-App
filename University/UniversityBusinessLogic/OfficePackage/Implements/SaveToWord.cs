using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using UniversityBusinessLogic.OfficePackage.HelperEnums;
using UniversityBusinessLogic.OfficePackage.HelperModels;

namespace UniversityBusinessLogic.OfficePackage.Implements
{
    public class SaveToWord : AbstractSaveToWord
    {
        private WordprocessingDocument _wordDocument;
        private Body _docBody;
        private Table _table;
        /// <summary>
        /// Получение типа выравнивания
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static JustificationValues GetJustificationValues(WordJustificationType type)
        {
            return type switch
            {
                WordJustificationType.End => JustificationValues.End,
                WordJustificationType.Both => JustificationValues.Both,
                WordJustificationType.Center => JustificationValues.Center,
                _ => JustificationValues.Left,
            };
        }
        /// <summary>
        /// Настройки страницы
        /// </summary>
        /// <returns></returns>
        private static SectionProperties CreateSectionProperties()
        {
            var properties = new SectionProperties();
            var pageSize = new PageSize
            {
                Orient = PageOrientationValues.Portrait
            };
            properties.AppendChild(pageSize);
            return properties;
        }
        /// <summary>
        /// Задание форматирования для абзаца
        /// </summary>
        /// <param name="paragraphProperties"></param>
        /// <returns></returns>
        private static ParagraphProperties CreateParagraphProperties(WordTextProperties paragraphProperties)
        {
            if (paragraphProperties != null)
            {
                var properties = new ParagraphProperties();
                properties.AppendChild(new Justification()
                {
                    Val = GetJustificationValues(paragraphProperties.JustificationType)
                });
                properties.AppendChild(new SpacingBetweenLines
                {
                    LineRule = LineSpacingRuleValues.Auto
                });
                properties.AppendChild(new Indentation());
                var paragraphMarkRunProperties = new ParagraphMarkRunProperties();
                if (!string.IsNullOrEmpty(paragraphProperties.Size))
                {
                    paragraphMarkRunProperties.AppendChild(new FontSize
                    {
                        Val = paragraphProperties.Size
                    });
                }
                properties.AppendChild(paragraphMarkRunProperties);
                return properties;
            }
            return null;
        }
        protected override void CreateWord(string info)
        {
            _wordDocument = WordprocessingDocument.Create(info, WordprocessingDocumentType.Document);
            MainDocumentPart mainPart = _wordDocument.AddMainDocumentPart();
            mainPart.Document = new Document();
            _docBody = mainPart.Document.AppendChild(new Body());
        }
        protected override void CreateParagraph(WordParagraph paragraph)
        {
            if (paragraph != null)
            {
                var docParagraph = new Paragraph();

                docParagraph.AppendChild(CreateParagraphProperties(paragraph.TextProperties));
                foreach (var run in paragraph.Texts)
                {
                    var docRun = new Run();
                    var properties = new RunProperties();
                    properties.AppendChild(new FontSize { Val = run.Item2.Size });
                    if (run.Item2.Bold)
                    {
                        properties.AppendChild(new Bold());
                    }
                    docRun.AppendChild(properties);
                    docRun.AppendChild(new Text
                    {
                        Text = run.Item1,
                        Space = SpaceProcessingModeValues.Preserve
                    });
                    docParagraph.AppendChild(docRun);
                }
                _docBody.AppendChild(docParagraph);
            }
        }
        protected override void CreateTable(List<string> tableHeader)
        {
            _table = new Table();
            TableProperties tblProp = new TableProperties(
                new TableBorders(
                    new TopBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 8
                    },
                    new BottomBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 8
                    },
                    new LeftBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 8
                    },
                    new RightBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 8
                    },
                    new InsideHorizontalBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 8
                    },
                    new InsideVerticalBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 8
                    }
                )
            );
            _table.AppendChild(tblProp);
            TableRow tableRowHeader = new TableRow();

            foreach (string stringHeaderCell in tableHeader)
            {
                TableCell cellHeader = new TableCell();
                cellHeader.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3400" }));
                cellHeader.Append(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(new RunProperties(new Bold()), new Text(stringHeaderCell))));
                tableRowHeader.Append(cellHeader);
            }

            _table.Append(tableRowHeader);
            _docBody.AppendChild(_table);
        }

        protected override void CreateComplexTable(int[] width, List<(int, int, string)> joined,
            List<string> columns)
        {
            _table = new Table();
            TableProperties tblProp = new TableProperties(
                new TableBorders(
                    new TopBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 8
                    },
                    new BottomBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 8
                    },
                    new LeftBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 8
                    },
                    new RightBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 8
                    },
                    new InsideHorizontalBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 8
                    },
                    new InsideVerticalBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 8
                    }
                )
            );
            _table.AppendChild(tblProp);
            TableRow tableRowHeaderOne = new TableRow();
            for (int i = 0; i < width.Length; i++)
            {
                TableCell cellHeader = new TableCell();
                bool flag = true;
                foreach (var join in joined)
                {
                    if (i == join.Item1)
                    {
                        cellHeader.Append(new TableCellProperties(new HorizontalMerge() { Val = MergedCellValues.Restart }));
                        cellHeader.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = width[i].ToString() }));
                        cellHeader.Append(new Paragraph(new ParagraphProperties(new
                            Justification()
                        { Val = JustificationValues.Center }),
                            new Run(new RunProperties(new Bold()),
                            new Text(join.Item3))));
                        flag = false;
                    }
                    else if (i > join.Item1 && i <= join.Item2)
                    {
                        cellHeader.Append(new TableCellProperties(new HorizontalMerge() { Val = MergedCellValues.Continue }));
                        cellHeader.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = width[i].ToString() }));
                        flag = false;
                    }
                }
                if (flag)
                {
                    cellHeader.Append(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Restart }));
                    cellHeader.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = width[i].ToString() }));
                    cellHeader.Append(new Paragraph(new ParagraphProperties(new Justification()
                    { Val = JustificationValues.Center }),
                            new Run(new RunProperties(new Bold()),
                            new Text(columns[i]))));
                }
                tableRowHeaderOne.Append(cellHeader);
            }
            TableRow tableRowHeaderTwo = new TableRow();
            for (int i = 0; i < width.Length; i++)
            {
                TableCell cellHeader = new TableCell();
                cellHeader.Append(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Continue }));
                cellHeader.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = width[i].ToString() }));
                cellHeader.Append(new Paragraph(new ParagraphProperties(new Justification()
                { Val = JustificationValues.Center }),
                    new Run(new RunProperties(new Bold()),
                    new Text(columns[i]))));
                tableRowHeaderTwo.Append(cellHeader);
            }
            _table.Append(tableRowHeaderOne);
            _table.Append(tableRowHeaderTwo);
            _docBody.AppendChild(_table);
        }

        protected override void CreateRow(List<string> tableRowInfo)
        {
            TableRow tableRow = new TableRow();
            
            TableCell tableCell = new TableCell();
            tableRow.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3400" }));
            tableRow.Append(new Paragraph(new Run(new Text(" " + tableRowInfo[0]))));
            tableRow.Append(tableCell);

            tableRowInfo.Remove(tableRowInfo[0]);

            foreach (string celltext in tableRowInfo)
            {
                TableCell tableCellmain = new TableCell();
                tableRow.Append(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3400" }));
                tableRow.Append(new Paragraph (new ParagraphProperties( 
                    new Justification() { Val = JustificationValues.End }), (new Run(new Text(celltext + " ")))));
                tableRow.Append(tableCellmain);
            }
            _table.Append(tableRow);
        }

        protected override void SaveWord()
        {
            _docBody.AppendChild(CreateSectionProperties());
            _wordDocument.MainDocumentPart.Document.Save();
            _wordDocument.Close();
        }
    }
}
