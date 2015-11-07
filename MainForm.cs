﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkWithExcel;

namespace work
{
    public partial class MainForm : Form
    {

        private ExcelTools excelTool = null;
        private String path = null;
        private int rowIndex = 0;
   
        private int currentTable = 0;
        public MainForm()
        {
            InitializeComponent();
            this.Height = 520;
            this.Width = 648;

        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Шрифт_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowColor = true;

            fontDialog1.Font = dataGridView.Font;
            fontDialog1.Color = dataGridView.ForeColor;

            if (fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dataGridView.Font = fontDialog1.Font;
                dataGridView.ForeColor = fontDialog1.Color;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "Вкажіть ім'я під яким слід створити новий файл";
            fileDialog.Filter = "*.xlsx|*.xlsx";
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = fileDialog.FileName;
                excelTool = new ExcelTools(path);
            }
            else return;
            dataGridView.DataSource = excelTool.dataSet.Tables[0];
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "*.xls, *.xlsx|*.xlsx; *.xlsx";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = openFile.FileName;
                excelTool = new ExcelTools(path);
                dataGridView.DataSource = excelTool.dataSet.Tables[currentTable];
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

        }

        // =======================================================================================


        // saving a table
        private void зберегтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (path == null) return;
            excelTool.saveExcelFile();
        }


        // showing a context menu item
        private void dataGridView_CellMouseUp_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.dataGridView.Rows[e.RowIndex].Selected = true;
                this.rowIndex = e.RowIndex;
                this.dataGridView.CurrentCell = this.dataGridView.Rows[e.RowIndex].Cells[1];
                this.contextMenuStrip1.Show(this.dataGridView, e.Location);
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        //  DELETING A ROW
        private void deleteRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView.Rows[this.rowIndex].IsNewRow)
            {
                this.dataGridView.Rows.RemoveAt(this.rowIndex);
            }
        }
    }
}