using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;

namespace work
{
    public partial class MainForm : Form
    {
        // perfomance for odbc connection
        private string Excel03ConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
        private string Excel07ConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";


        // List of variables
        DataGridViewRow row = new DataGridViewRow();
        private int rowIndex = 0;

        Color green = ColorTranslator.FromHtml("#40bd74");
        Color black = Color.Black;
        Color white = Color.White;

        string path = null;

        //===================================================================================
        // INITIALIZE
        public MainForm()
        {
            InitializeComponent();
            this.Height = 520;
            this.Width = 550;
            this.dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;

            dataGridView1.DefaultCellStyle.SelectionBackColor = green;
            dataGridView1.DefaultCellStyle.SelectionForeColor = white;
            rbHeaderYes.Checked = true;
            toolStripButton3.Enabled = false;
            btn_Print.Enabled = false;
            font_btn.Enabled = false;
            правкаToolStripMenuItem.Enabled = false;
            delete_row.Enabled = false;
            btn_Insert.Enabled = false;
            btn_Copy.Enabled = false;
        }

        // Change a font
        private void font_btn_Click(object sender, EventArgs e)
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

        //OPENING
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

                case ".xlsx": //Excel 07 ++
                    conStr = string.Format(Excel07ConString, filePath, header);
                    break;
            }

            // Отримання назви першого листа

            try {
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
                // Читання даних з першого листа
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

                            // Record in datagridView
                            dataGridView1.DataSource = dt;

                            btnSearch.Enabled = true;
                            textSearch.Enabled = true;
                            radioButton2.Enabled = false;
                            rbHeaderYes.Enabled = false;
                            btn_CloseDocument.Enabled = true;
                            toolStripButton3.Enabled = true;
                            btn_Print.Enabled = true;
                            font_btn.Enabled = true;
                            правкаToolStripMenuItem.Enabled = true;
                            label1.Hide();
                            delete_row.Enabled = true;
                            btn_Insert.Enabled = true;
                            btn_Copy.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
}

        //Creating a new file
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
            //dataGridView1.DataSource = excelTool.dataSet.Tables[0];
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        // =======================================================================================

        // SAVING A TABLE
        private void зберегтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Doucments *.xlsx|*.xlsx";
            sfd.FileName = "rock.xlsx";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();

                // creating new WorkBook within Excel application
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);

                // creating new Excelsheet in workbook
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                // see the excel sheet behind the program
                app.Visible = true;

                // get the reference of first sheet. By default its name is Sheet1.
                // store its reference to worksheet
                worksheet = workbook.Sheets["Лист1"];
                worksheet = workbook.ActiveSheet;

                // changing the name of active sheet
                worksheet.Name = "Exported from gridview";


                // storing header part in Excel
                for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                {
                    worksheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
                }



                // storing Each row and column value to excel sheet
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }


                // save the application
                workbook.SaveAs(sfd.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                // Exit from the application
                app.Quit();

            }


            //if (path == null) return;
            //excelTool.saveExcelFile();

        }


        // SHOWING A CONTEXT MENU ITEM
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
                if (MessageBox.Show("Ви впевнені, що хочете видалити рядок?", "Видалення рядка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    this.dataGridView1.Rows.RemoveAt(this.rowIndex);
                else
                    return;
            }
        }

        private void delete_row_Click(object sender, EventArgs e)
        {
            if (!dataGridView1.Rows[rowIndex].IsNewRow)
            {
                if (MessageBox.Show("Ви впевнені, що хочете видалити рядок?", "Видалення рядка", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    dataGridView1.Rows.RemoveAt(rowIndex);
                else
                    return;
            }
        }


        // ====================================================================================


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
            int count = 0;
            dataGridView1.ClearSelection();
            dataGridView1.DefaultCellStyle.BackColor = white;

            try
            {
                if(search_text.Length == 0)
                {
                    MessageBox.Show("Поле для пошуку пусте!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {

                        // Regular expression
                        Regex regex = new Regex("(" + search_text + ")",
                            RegexOptions.IgnoreCase);


                        if (regex.IsMatch(dataGridView1.Rows[i].Cells[j].Value.ToString()))
                        {
                            dataGridView1.DefaultCellStyle.SelectionBackColor = green;
                            dataGridView1.DefaultCellStyle.BackColor = black;
                            dataGridView1.Rows[i].Selected = true;
                            count++;
                            break;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                //MessageBox.Show(exc.Message);
            }
            finally
            {
                if(search_text.Length != 0) {
                    if (count == 0) MessageBox.Show("По вашому запиту результатів не знайдено!",
                        "Результат пошуку", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    else if (count == 1) MessageBox.Show("По вашому запиту знайдено " + count + " результат.",
                        "Результат пошуку", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (count > 1 && count <= 4) MessageBox.Show("По вашому запиту знайдено " + count + " результати.", 
                        "Результат пошуку", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (count >= 5) MessageBox.Show("По вашому запиту знайдено " + count + " результатів.", 
                        "Результат пошуку", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
            }
        }

        // ====================================================================================

        // Copy row function
        public void copyRow()
        {
            if (dataGridView1.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                try
                {
                    Clipboard.SetDataObject(this.dataGridView1.GetClipboardContent());
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void rowCopy_Click(object sender, EventArgs e)
        {
            copyRow();
        }
        private void btn_Copy_Click(object sender, EventArgs e)
        {
            copyRow();
        }

        // Insert row function
        public void insertRow()
        {
            try
            {

                char[] rowSplitter = { '\r', '\n' };
                char[] columnSplitter = { '\t' };

                //get the text from clipboard
                IDataObject dataInClipboard = Clipboard.GetDataObject();
                string stringInClipboard = (string)dataInClipboard.GetData(DataFormats.Text);

                //split it into lines
                string[] rowsInClipboard = stringInClipboard.Split(rowSplitter, StringSplitOptions.RemoveEmptyEntries);

                //get the row and column of selected cell in grid
                int r = dataGridView1.SelectedCells[0].RowIndex;
                int c = dataGridView1.SelectedCells[0].ColumnIndex;

                //add rows into grid to fit clipboard lines
                if (dataGridView1.Rows.Count < (r + rowsInClipboard.Length))
                    dataGridView1.Rows.Add(r + rowsInClipboard.Length - dataGridView1.Rows.Count);

                // loop through the lines, split them into cells and place the values in the corresponding cell.
                for (int iRow = 0; iRow < rowsInClipboard.Length; iRow++)
                {
                    //split row into cell values
                    string[] valuesInRow = rowsInClipboard[iRow].Split(columnSplitter);

                    //cycle through cell values
                    for (int iCol = 0; iCol < valuesInRow.Length; iCol++)
                    {
                        //assign cell value, only if it within columns of the grid
                        if (dataGridView1.ColumnCount - 1 >= c + iCol)
                            dataGridView1.Rows[r + iRow].Cells[c + iCol].Value = valuesInRow[iCol];
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void rowInsert_Click(object sender, EventArgs e)
        {
            insertRow();
        }
        private void btn_Insert_Click(object sender, EventArgs e)
        {
            insertRow();
        }

        // ====================================================================================

        //Closing document
        public void closeDoc()
        {
            try
            {
                DialogResult dialog_result = MessageBox.Show("Ви впевнені, що хочете закрити поточний документ?", "Закриття документу",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialog_result == DialogResult.Yes)
                {
                    dataGridView1.DataSource = "";
                    label1.Show();

                    rbHeaderYes.Checked = true;
                    toolStripButton3.Enabled = false;
                    btn_Print.Enabled = false;
                    font_btn.Enabled = false;
                    radioButton2.Enabled = true;
                    rbHeaderYes.Enabled = true;
                    btn_CloseDocument.Enabled = false;
                    правкаToolStripMenuItem.Enabled = false;
                    textSearch.Enabled = false;
                    delete_row.Enabled = false;
                    btn_Insert.Enabled = false;
                    btn_Copy.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_CloseDocument_Click(object sender, EventArgs e)
        {
            closeDoc();
        }

        private void закритиПоточнийДокументToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeDoc();
        }

        // Form closing
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog_result = MessageBox.Show("Ви впевнені, що хочете вийти?", "Вихід",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialog_result == DialogResult.No)
                e.Cancel = true;
        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
        }

        private void btn_Undo_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.RejectChanges();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bmp = new Bitmap(dataGridView1.Size.Width + 15, dataGridView1.Size.Height + 10);
            dataGridView1.DrawToBitmap(bmp, dataGridView1.Bounds);
            e.Graphics.DrawImage(bmp, 0, 0);
        }

        //==================================================================================================
    }
}
