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
            this.path = path;
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

        public static implicit operator string (ExcelTools v)
        {
            throw new NotImplementedException();
        }
    }
}