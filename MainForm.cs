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
using System.Data.OleDb;
using System.IO;

namespace work
{
    public partial class MainForm : Form
    {

        private string Excel03ConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
        private string Excel07ConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";

        private ExcelTools excelTool = null;
        private String path = null;
        private int rowIndex = 0;
        Color green = ColorTranslator.FromHtml("#40bd74");
        Color black = Color.Black;
        Color white = Color.White;

        private int currentTable = 0;
        public MainForm()
        {
            InitializeComponent();
            this.Height = 520;
            this.Width = 555;
            dataGridView1.DefaultCellStyle.SelectionBackColor = green;
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
            openFileDialog1.ShowDialog();
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
                dataGridView1.Update();
            }
        }

        // SEARCH
        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            string search_text = textSearch.Text;
            dataGridView1.ClearSelection();
            if (search_text == "")
            {
                dataGridView1.DefaultCellStyle.BackColor = white;
            }
        }

        // SEARCH BUTTON
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string search_text = textSearch.Text;
            BindingSource bs = new BindingSource();
            bs.DataSource = dataGridView1.DataSource as DataTable;
            dataGridView1.ClearSelection();
            dataGridView1.DefaultCellStyle.BackColor = white;

            try
            {
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    bs.Position = bs.Find(col.Name.ToString(), search_text);
                    if (search_text != "")
                    {

                        dataGridView1.DefaultCellStyle.SelectionBackColor = green;
                        dataGridView1.DefaultCellStyle.BackColor = black;
                        dataGridView1.Rows[bs.Position].Selected = true;
                        dataGridView1.Rows[0].Selected = false;
                    }
                    else
                        break;
                }
                if (bs.Position == 0 && search_text != "")
                {
                    dataGridView1.DefaultCellStyle.BackColor = white;
                    MessageBox.Show("По вашому запиту нічого не знайдено!");
                }

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           DialogResult dialog_result = MessageBox.Show("Ви впевнені, що хочете вийти?", "Вихід",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog_result == DialogResult.No)
                e.Cancel = true;          
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string filePath = openFileDialog1.FileName;
            string extension = Path.GetExtension(filePath);
            string header = rbHeaderYes.Checked ? "YES" : "NO";
            string conStr, sheetName;

            conStr = string.Empty;
            switch (extension)
            {

                case ".xls": //Excel 97-03
                    conStr = string.Format(Excel03ConString, filePath, header);
                    break;

                case ".xlsx": //Excel 07
                    conStr = string.Format(Excel07ConString, filePath, header);
                    break;
            }

            //Get the name of the First Sheet.
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    cmd.Connection = con;
                    con.Open();
                    DataTable dtExcelSchema = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                    con.Close();
                }
            }

            //Read Data from the First Sheet.
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    using (OleDbDataAdapter oda = new OleDbDataAdapter())
                    {
                        DataTable dt = new DataTable();
                        cmd.CommandText = "SELECT * From [" + sheetName + "]";
                        cmd.Connection = con;
                        con.Open();
                        oda.SelectCommand = cmd;
                        oda.Fill(dt);
                        con.Close();

                        //Populate DataGridView.
                        dataGridView1.DataSource = dt;
                    }
                }
            }
        }
    }
}
