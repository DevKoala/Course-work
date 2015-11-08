using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkWithExcel;
using System.Data.SqlClient;

namespace work
{
    public partial class MainForm : Form
    {

        private ExcelTools excelTool = null;
        private String path = null;
        private int rowIndex = 0;
        Color red = ColorTranslator.FromHtml("#ff3b3b");
        Color black = Color.Black;
        Color white = Color.White;

        private int currentTable = 0;
        public MainForm()
        {
            InitializeComponent();
            this.Height = 520;
            this.Width = 555;
            dataGridView1.DefaultCellStyle.SelectionBackColor = red;
            dataGridView1.DefaultCellStyle.SelectionForeColor = white;

        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Шрифт_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowColor = true;

            fontDialog1.Font = dataGridView1.Font;
            fontDialog1.Color = dataGridView1.ForeColor;

            if (fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dataGridView1.Font = fontDialog1.Font;
                dataGridView1.ForeColor = fontDialog1.Color;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //SaveFileDialog fileDialog = new SaveFileDialog();
            //fileDialog.Title = "Вкажіть ім'я під яким слід створити новий файл";
            //fileDialog.Filter = "*.xlsx|*.xlsx";
            //if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    path = fileDialog.FileName;
            //    excelTool = new ExcelTools(path);
            //}
            //else return;
            //dataGridView.DataSource = excelTool.dataSet.Tables[0];
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "*.xls, *.xlsx|*.xlsx; *.xlsx";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = openFile.FileName;
                excelTool = new ExcelTools(path);
                dataGridView1.DataSource = excelTool.dataSet.Tables[currentTable];
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

        }

        // =======================================================================================


        // SAVING A TABLE
        private void зберегтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (path == null) return;
            excelTool.saveExcelFile();

        }


        // SHOWING A CCONTEXT MENU ITEM
        private void dataGridView_CellMouseUp_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.dataGridView1.Rows[e.RowIndex].Selected = true;
                this.rowIndex = e.RowIndex;
                this.dataGridView1.CurrentCell = this.dataGridView1.Rows[e.RowIndex].Cells[1];
                this.contextMenuStrip1.Show(this.dataGridView1, e.Location);
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        //  DELETING A ROW
        private void deleteRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView1.Rows[this.rowIndex].IsNewRow)
            {
                this.dataGridView1.Rows.RemoveAt(this.rowIndex);
            }
        }

        // SEARCH
        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            string search_text = textSearch.Text;
            BindingSource bs = new BindingSource();
            bs.DataSource = dataGridView1.DataSource as DataTable;
            dataGridView1.ClearSelection();
            dataGridView1.DefaultCellStyle.BackColor = white;

            try {
                foreach(DataGridViewColumn col in dataGridView1.Columns)
                {
                    bs.Position = bs.Find(col.Name.ToString(), search_text);
                    if (search_text != "")
                    {
                        if (bs.Position == 0)
                        {
                            MessageBox.Show("Заданого елементу не знайдено");
                            break;
                        }
                        dataGridView1.DefaultCellStyle.SelectionBackColor = red;
                        dataGridView1.DefaultCellStyle.BackColor = black;
                        dataGridView1.Rows[bs.Position].Selected = true;
                        dataGridView1.Rows[0].Selected = false;
                    }
                    else
                        break;
                }   
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        // SEARCH BUTTON
        private void btnSearch_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dataGridView1.Columns[1].ToString());
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           DialogResult dialog_result = MessageBox.Show("Ви впевнені, що хочете вийти?", "Вихід",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog_result == DialogResult.No)
                e.Cancel = true;          
        }
    }
}
