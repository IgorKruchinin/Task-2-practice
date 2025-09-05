using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace Task_1_2__template
{
    public partial class formWordAutomatisationPart2 : Form
    {
        private string settingsFile = "settings.txt";
        private string[] parameters = new string[17];
        public formWordAutomatisationPart2()
        {
            InitializeComponent();
            loadSettings();
            updateControlsFromParameters();
            
        }

        private void loadSettings()
        {
            if (File.Exists(settingsFile))
            {
                string[] lines = File.ReadAllLines(settingsFile);
                if (lines.Length > 0)
                {
                    for (int i = 0; i < lines.Length && i < parameters.Length; i++)
                    {
                        parameters[i] = lines[i].Trim();
                    }
                }
            }
        }

        private void updateControlsFromParameters()
        {
            if (parameters == null || parameters.Length < 17)
                return;

            // Проверка на null и установка значений
            if (parameters[0] != null) fldAgreementNum.Text = parameters[0];
            if (parameters[1] != null) fldCity.Text = parameters[1];
            if (parameters[2] != null) fldTenantFullName.Text = parameters[2];
            if (parameters[3] != null) fldTenantPassportSeries.Text = parameters[3];
            if (parameters[4] != null) fldTenantPassportNum.Text = parameters[4];
            if (parameters[5] != null) fldTenantPassportGiveDepartament.Text = parameters[5];
            if (parameters[6] != null) fldTenantPassportGiveDate.Text = parameters[6];
            if (parameters[7] != null) fldTenantAddress.Text = parameters[7];
            if (parameters[8] != null) fldLandlordFullName.Text = parameters[8];
            if (parameters[9] != null) fldLandlordPassportSeries.Text = parameters[9];
            if (parameters[10] != null) fldLandlordPassportNum.Text = parameters[10];
            if (parameters[11] != null) fldLandlordPassportGiveDepartament.Text = parameters[11];
            if (parameters[12] != null) fldLandlordPassportGiveDate.Text = parameters[12];
            if (parameters[13] != null) fldLandlordAddress.Text = parameters[13];
            if (parameters[14] != null) fldRentedAddress.Text = parameters[14];
            if (parameters[16] != null) fldDate.Text = parameters[16];

            // Для NumericUpDown (этаж) — проверка, что значение числовое
            if (parameters[15] != null && int.TryParse(parameters[15], out int floor))
            {
                // Ограничиваем значение в пределах Minimum и Maximum
                if (floor >= (int)fldRentedFloor.Minimum && floor <= (int)fldRentedFloor.Maximum)
                {
                    fldRentedFloor.Value = floor;
                }
                else
                {
                    // На случай, если значение вне диапазона — устанавливаем ближайшее
                    fldRentedFloor.Value = Math.Max((int)fldRentedFloor.Minimum,
                                                   Math.Min((int)fldRentedFloor.Maximum, floor));
                }
            }
        }

        private void updateParametersFromControls()
        {
            parameters[0] = fldAgreementNum.Text;
            parameters[1] = fldCity.Text;
            parameters[2] = fldTenantFullName.Text;
            parameters[3] = fldTenantPassportSeries.Text;
            parameters[4] = fldTenantPassportNum.Text;
            parameters[5] = fldTenantPassportGiveDepartament.Text;
            parameters[6] = fldTenantPassportGiveDate.Text;
            parameters[7] = fldTenantAddress.Text;
            parameters[8] = fldLandlordFullName.Text;
            parameters[9] = fldLandlordPassportSeries.Text;
            parameters[10] = fldLandlordPassportNum.Text;
            parameters[11] = fldLandlordPassportGiveDepartament.Text;
            parameters[12] = fldLandlordPassportGiveDate.Text;
            parameters[13] = fldLandlordAddress.Text;
            parameters[14] = fldRentedAddress.Text;
            parameters[15] = $"{fldRentedFloor.Value}";
            parameters[16] = fldDate.Text;
        }

        private void saveTXT()
        {
            File.WriteAllLines(settingsFile, parameters.Take(16).Where(p => p != null).ToArray());
            MessageBox.Show("Настройки сохранены!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void fillWordDocument(Word.Document doc, string[] parameters)
        {
            doc.Content.Delete();

            Word.Range range = doc.Content;
            range.Font.Name = "Times New Roman";
            range.Font.Size = 12;

            // === ЗАГОЛОВОК ===
            range.InsertAfter($"ДОГОВОР НАЙМА КВАРТИРЫ № {parameters[0]}");
            Word.Paragraph titlePara = range.Paragraphs.Last;

            // Используем встроенный стиль
            try
            {
                titlePara.set_Style(doc.Styles[Word.WdBuiltinStyle.wdStyleHeading1]);
            }
            catch
            {
                // Если стиль недоступен — форматируем вручную
                titlePara.Range.Font.Bold = -1;
                titlePara.Range.Font.Size = 16;
            }
            titlePara.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            range.InsertParagraphAfter();

            // === ДАТА ===
            range.InsertAfter($"г. {parameters[1]}");
            Word.Paragraph cityPara = range.Paragraphs.Last;
            cityPara.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;


            range.InsertParagraphAfter();
            range.InsertAfter($"{parameters[16]} г.");
            Word.Paragraph datePara = range.Paragraphs.Last;
            datePara.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
            range.InsertParagraphAfter();
            range.InsertParagraphAfter();

            // === СТОРОНЫ ===
            string tenantPassport = $"{parameters[3]} {parameters[4]}, выдан {parameters[5]} {parameters[6]}";
            string landlordPassport = $"{parameters[9]} {parameters[10]}, выдан {parameters[11]} {parameters[12]}";

            range.InsertAfter($"Гражданин {parameters[2]}, паспорт (серия, номер, выдан) {tenantPassport},");
            range.Paragraphs.Last.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
            range.InsertParagraphAfter();
            range.InsertAfter($"проживающий по адресу {parameters[7]}, именуемый в дальнейшем «Наниматель», с одной стороны, и гражданин {parameters[8]}, паспорт (серия, номер, выдан) {landlordPassport},");
            range.Paragraphs.Last.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
            range.InsertParagraphAfter();
            range.InsertAfter($"проживающий по адресу {parameters[13]}, именуемый в дальнейшем «Наймодатель», с другой стороны, именуемые в дальнейшем «Стороны», заключили настоящий договор, в дальнейшем «Договор», о нижеследующем:");
            range.Paragraphs.Last.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
            range.InsertParagraphAfter();
            range.InsertParagraphAfter();

            // === 1. ПРЕДМЕТ ДОГОВОРА (жирный заголовок) ===
            range.InsertAfter("1. ПРЕДМЕТ ДОГОВОРА");
            Word.Paragraph sectionPara = range.Paragraphs.Last;
            sectionPara.Range.Font.Bold = -1;
            sectionPara.Range.Font.Size = 14;
            sectionPara.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            range.InsertParagraphAfter();
            range.InsertParagraphAfter();

            // === 1.1 ===
            range.InsertAfter($"1.1. Наймодатель обязуется предоставить Нанимателю квартиру, расположенную по адресу: {parameters[14]}");
            range.Paragraphs.Last.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
            range.InsertParagraphAfter();
            range.InsertAfter($"расположенную на {parameters[15]} этаже, в дальнейшем именуемую «Квартира», за плату во временное пользование для проживания.");
            range.Paragraphs.Last.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
            range.InsertParagraphAfter();
            range.InsertParagraphAfter();

            // === 1.2 ===
            range.InsertAfter("1.2. Наймодатель распоряжается Квартирой по праву собственности, что подтверждается свидетельством о государственной регистрации права");
            range.Paragraphs.Last.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
            range.InsertParagraphAfter();
            range.InsertParagraphAfter();


            // === Общее форматирование ===
            foreach (Word.Paragraph para in doc.Paragraphs)
            {
                if (para.Range.Text.Trim().Length == 0) continue;

                para.Range.Font.Name = "Times New Roman";
                para.Range.Font.Size = 12;
                para.SpaceAfter = 6;
            }

           
        }

        private void showPreviewInWord(string tempFilePath)
        {
            Word.Application wordApp = null;
            Word.Document wordDoc = null;
            try
            {
                wordApp = new Word.Application();
                wordDoc = wordApp.Documents.Add();
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = tempFilePath,
                    UseShellExecute = true
                });

                MessageBox.Show($"Документ открыт в Word.\nФайл: {tempFilePath}",
                    "Предварительный просмотр", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании или открытии документа: " + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                wordDoc?.Close();
                wordApp?.Quit();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            updateParametersFromControls();
            saveTXT();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            updateParametersFromControls();
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Сохранить титульный лист";
                sfd.Filter = "Документы Word|*.docx|Все файлы|*.*";
                sfd.DefaultExt = "docx";
                sfd.FileName = "TitlePage.docx"; // предложенное имя

                if (sfd.ShowDialog() != DialogResult.OK)
                    return; // пользователь нажал "Отмена"

                string savePath = sfd.FileName;

                // Проверим, не занят ли файл
                if (File.Exists(savePath))
                {
                    try
                    {
                        File.Delete(savePath); // пытаемся удалить старый (если не открыт)
                    }
                    catch
                    {
                        MessageBox.Show("Файл используется другим приложением.", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Создаём документ Word
                Word.Application wordApp = null;
                Word.Document wordDoc = null;

                try
                {
                    wordApp = new Word.Application();
                    wordDoc = wordApp.Documents.Add();

                    // Заполняем титульный лист (уже есть)
                    fillWordDocument(wordDoc, parameters);

                    // Сохраняем в выбранный путь
                    wordDoc.SaveAs2(savePath);
                    wordDoc.Close();
                    wordApp.Quit();

                    MessageBox.Show($"Документ успешно сохранён:\n{savePath}",
                        "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при создании документа: " + ex.Message,
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    wordDoc?.Close();
                    wordApp?.Quit();
                }
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            updateParametersFromControls();
            Word.Application wordApp = null;
            Word.Document wordDoc = null;
            string tempFilePath = Path.Combine(Path.GetTempPath(), "Preview_TitlePage.docx");

            try
            {
                wordApp = new Word.Application();
                wordDoc = wordApp.Documents.Add();

                // Заполняем титульный лист (тот же код!)
                fillWordDocument(wordDoc, parameters);

                // Сохраняем временный файл
                wordDoc.SaveAs2(tempFilePath);
                wordDoc.Close();
                wordApp.Quit();

                // Открываем в Word
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = tempFilePath,
                    UseShellExecute = true
                });

                MessageBox.Show($"Документ открыт в Word.\nФайл: {tempFilePath}",
                    "Предварительный просмотр", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании или открытии документа: " + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                wordDoc?.Close();
                wordApp?.Quit();
            }
        }

        private void lblPassportNum_Click(object sender, EventArgs e)
        {

        }

        private void fldPassportGiveDepartament_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
