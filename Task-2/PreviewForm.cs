using System.Drawing;
using System.Windows.Forms;

namespace Task_2
{

    public partial class PreviewForm : Form
    {
        private DataGridView dataGridView;

        public PreviewForm()
        {
            InitializeComponent();
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.RowHeadersVisible = false;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.ReadOnly = true;

            // Настройка столбцов
            dataGridView.ColumnCount = 7;
            dataGridView.Columns[0].Name = "Наименование";
            dataGridView.Columns[1].Name = "Вид";
            dataGridView.Columns[2].Name = "Номенклатурный номер";
            dataGridView.Columns[3].Name = "Кол";
            dataGridView.Columns[4].Name = "Наименование единицы";
            dataGridView.Columns[5].Name = "Количество";
            dataGridView.Columns[6].Name = "Номер паспорта";

            // Ширина
            dataGridView.Columns[0].Width = 100;
            dataGridView.Columns[1].Width = 80;
            dataGridView.Columns[2].Width = 120;
            dataGridView.Columns[3].Width = 60;
            dataGridView.Columns[4].Width = 100;
            dataGridView.Columns[5].Width = 100;
            dataGridView.Columns[6].Width = 120;

            // Отключаем стандартные заголовки — будем рисовать свои
            dataGridView.ColumnHeadersVisible = false;

            // Добавляем "чердак" — строку с объединёнными заголовками
            dataGridView.Rows.Insert(0); // Строка 0 — верхняя
            dataGridView.Rows.Insert(1); // 2-й уровень шапки
            dataGridView.Rows.Insert(1); // 3-й уровень шапки - нумерация



            dataGridView.Rows[0].Cells[0].Value = "Наименование";
            dataGridView.Rows[0].Cells[1].Value = "Вид";
            dataGridView.Rows[0].Cells[2].Value = "Номенк. номер";
            dataGridView.Rows[0].Cells[3].Value = "Единица измерения";
            dataGridView.Rows[1].Cells[3].Value = "кол";
            dataGridView.Rows[1].Cells[4].Value = "наименование";
            dataGridView.Rows[0].Cells[5].Value = "Кол-во (масса)";
            dataGridView.Rows[0].Cells[6].Value = "Номер паспорта";


            // Стиль для "чердака"
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = Color.LightGray;
            headerStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            for (int i = 0; i < 7; i++)
            {
                dataGridView.Rows[0].Cells[i].Style = headerStyle;
                dataGridView.Rows[1].Cells[i].Style = headerStyle;
                dataGridView.Rows[2].Cells[i].Style = headerStyle;
                dataGridView.Rows[2].Cells[i].Value = i + 1;

            }

            // Добавляем контрол на форму
            this.Controls.Add(dataGridView);
        }
    }
}
