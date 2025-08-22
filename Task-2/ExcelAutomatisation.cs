using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task_2
{
    public partial class ExcelAutomatisation : Form
    {
        public ExcelAutomatisation()
        {
            InitializeComponent();
        }

        private List<string[]> _tableData;

        private void btnPreview_Click(object sender, EventArgs e)
        {
            var previewForm = new PreviewForm(_tableData);
            previewForm.ShowDialog();
        }

        private void btnFillData_Click(object sender, EventArgs e)
        {
            string[] lines = File.ReadAllLines("data.txt");
            var data = new List<string[]>();

            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    data.Add(line.Split(';'));
                }
            }

            // Сохраняем данные для использования в Excel
            _tableData = data;
            MessageBox.Show("Данные успешно загружены!");
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (_tableData == null || _tableData.Count == 0)
            {
                MessageBox.Show("Сначала загрузите данные!");
                return;
            }

            try
            {
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = excelApp.Workbooks.Add();
                Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.ActiveSheet;

                // Заголовок таблицы
                worksheet.Cells[1, 1] = "Драгоценный материал (металл, камень)";
                worksheet.Range["A1:H1"].Merge(true);
                worksheet.Range["A1:H1"].Font.Bold = true;
                worksheet.Range["A1:H1"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                worksheet.Range["A1:H1"].Font.Size = 12;

                // Объединение ячеек для "Единица измерения"
                worksheet.Range["D2:E2"].Merge(true);
                worksheet.Range["D2:E2"].Value = "Единица измерения";

               
                // Мерджим всё (2 и 3 ряд) кроме единицы измерения
                worksheet.Cells[2, 1] = "Наименование";
                worksheet.Range["A2:A3"].Merge(true);

                worksheet.Range["A2:A3"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                worksheet.Range["A2:A3"].VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                worksheet.Cells[2, 2] = "Вид";
                worksheet.Range["B2:B3"].Merge(true);

                worksheet.Range["B2"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                worksheet.Range["B2"].VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;


                worksheet.Cells[2, 3] = "Номенклатурный номер";
                worksheet.Range["C2:C3"].Merge(true);

                worksheet.Range["F2"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                worksheet.Range["F2"].VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                // D и E не мерджим, вместо этого пишем заголовок 3-го уровня
                worksheet.Cells[3, 4] = "кол";
                worksheet.Cells[3, 5] = "наименование";

                worksheet.Cells[2, 6] = "Количество (масса)";
                worksheet.Range["F2:F3"].Merge(true);

                worksheet.Range["F2"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                worksheet.Range["F2"].VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;


                worksheet.Cells[2, 7] = "Номер паспорта";
                worksheet.Range["G2:G3"].Merge(true);

                worksheet.Range["G2"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                worksheet.Range["G2"].VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;


                // Нумеруем столбцы как в шаблоне в ТЗ
                for (int i = 1; i <= 7; ++i)
                {
                    // Индексация у нас с единицы и сначала номер ряда, потом столбца
                    worksheet.Cells[4, i] = i;
                }

                // Установка формата заголовков
                worksheet.Range["A1:H4"].Font.Bold = true; // шрифт жирный
                worksheet.Range["A1:H4"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter; // выравниваем заголовок по центру


                // Заполнение данными
                int row = 5;
                foreach (var item in _tableData)
                {
                    worksheet.Cells[row, 1] = item[0];
                    worksheet.Cells[row, 2] = item[1];
                    worksheet.Cells[row, 3] = item[2];
                    worksheet.Cells[row, 4] = item[3];
                    worksheet.Cells[row, 5] = item[4];
                    worksheet.Cells[row, 6] = item[5];
                    worksheet.Cells[row, 7] = item[6];
                    row++;
                }

                // Автоподбор ширины столбцов
                worksheet.Columns.AutoFit();

                // Сохранение
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveDialog.FileName = "Драгоценные_материалы.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Таблица успешно сохранена!");
                }

                workbook.Close();
                excelApp.Quit();

                // Очистка объектов
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
