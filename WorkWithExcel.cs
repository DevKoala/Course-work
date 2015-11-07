/*
 * The first you must add to current project the references "Microsoft Ecxel Library 15.0"
*/

using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WorkWithExcel
{
    public class ExcelTools
    {
        public DataSet dataSet = null;
        private String path = null;

        //constructor used for read all date from Excel file to dataSet
        public ExcelTools(String path)
        {
            loadDataSet(path);
            this.path = path;
        }

        public void loadDataSet(String path)
        {
            //Created variable to connection
            OleDbConnection MyConnection = null;

            //Created variable to using command in connection
            System.Data.OleDb.OleDbDataAdapter MyCommand;

            try
            {
                //get file extension to find correct provider
                String fileExtension = System.IO.Path.GetExtension(path);

                //search on existing file
                bool fileExist = new System.IO.FileInfo(path).Exists;

                //Connecting variable
                String connString = null;
                if (fileExtension == ".xls")
                    connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path
                        + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
                if (fileExtension == ".xlsx")
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path
                        + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";

                //creating connection
                MyConnection = new OleDbConnection(connString);

                //open connection
                MyConnection.Open();

                if (!fileExist)
                {
                    var cmd = MyConnection.CreateCommand();

                    cmd.CommandText = "CREATE TABLE `Населення країн` (" +
                        "`Країна` NVARCHAR(100), `Частина світу` NVARCHAR(100)," +
                        "`Населення` DOUBLE, `Площа` DOUBLE," +
                        "`Густота населення` DOUBLE,`Президент` NVARCHAR(100))";
                    cmd.ExecuteNonQuery();

                  }
                dataSet = new DataSet();

                //add all data to dataSet from Excel file
                foreach (DataRow row in MyConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows)
                {
                    string sheetName = row["TABLE_NAME"].ToString();
                    if (!sheetName.EndsWith("$") && !sheetName.EndsWith("$'"))
                        continue;

                    dataSet.Tables.Add(row["TABLE_NAME"].ToString());
                    MyCommand = new OleDbDataAdapter("select * from [" + row["TABLE_NAME"].ToString() + "]", MyConnection);
                    MyCommand.Fill(dataSet, row["TABLE_NAME"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                // Clean up.
                if (MyConnection != null)
                {
                    MyConnection.Close();
                    MyConnection.Dispose();
                }
            }
        }

        public void saveExcelFile()
        {
            Excel.Application xlApp = null;
            try
            {
                xlApp = new Excel.Application();
                if (xlApp == null || path == null) return;
                Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(
                    path,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing);
                xlApp.Visible = false;

                int tableCount = 0;
                foreach (DataTable dt in dataSet.Tables)
                {
                    tableCount++;
                    Excel.Worksheet xlWorkSheet = xlWorkBook.Worksheets.get_Item(tableCount);
                    xlWorkSheet.Columns.ClearContents();
                    int i = 1;
                    bool bl = true;

                    //Write data from dataSet to Excel File

                    foreach (DataRow dr in dt.Rows)
                    {
                        int j = 1;
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (bl)
                            {
                                int j2 = 1;
                                foreach (DataColumn insertColumn in dt.Columns)
                                {
                                    xlWorkSheet.Cells[1, j2] = insertColumn.ColumnName;
                                    j2++;
                                }
                                xlWorkSheet.Cells[i + 1, j] = dr[dc].ToString();
                                bl = false;
                            }
                            else xlWorkSheet.Cells[i + 1, j] = dr[dc].ToString();
                            j++;
                        }
                        i++;
                    }
                    bl = true;

                }
                xlWorkBook.Close(true, Type.Missing, Type.Missing);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace.ToString());
                MessageBox.Show("Помилка збереження");
            }
        }

        public void printTable(int tableID)
        {
            Excel.Application xlApp = new Excel.Application();
            if (xlApp == null || path == null) return;
            Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(
                path,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing);
            Excel.Worksheet workSheet = xlWorkBook.Worksheets.get_Item(tableID);
            //((Excel.Worksheet)Application.ActiveSheet).PrintPreview();
            xlApp.Visible = true;
            workSheet.PrintPreview();
            xlWorkBook.Close(true, Type.Missing, Type.Missing);
        }
      }
}