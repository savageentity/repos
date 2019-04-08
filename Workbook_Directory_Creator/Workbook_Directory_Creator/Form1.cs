using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.Reflection;

namespace PartClass_Directory_Creator
{
    public partial class Form1 : Form
    {        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dir = folderBrowserDialog1;
            dir.ShowDialog();
            DialogClose(dir.SelectedPath);
        }

        private void DialogClose(string str)
        {
            textBox1.Text = str;
            textBox1.Focus();
            return;
        }
        private string FindUserClass(Excel.Worksheet sheet)
        {
            Excel.Range currentFind = null;
            Excel.Range firstFind = null;
            string strTest = "";
            Excel.Range range = sheet.Range["A1", "Y7"];

            // You should specify all these parameters every time you call this method,
            // since they can be overridden in the user interface. 
            currentFind = range.Find("UserClassName", Missing.Value,
                Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart,
                Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, false,false,Missing.Value);

            while (currentFind != null)
            {
                dynamic obj = currentFind.Column;
                int col = (int)obj;
                obj = currentFind.Row;
                int row = (int)obj;
                for(int i = 1; i < 5; i++)
                {
                    //dynamic testout = xlWorkSheets[16].Cells(row+i, col);
                    dynamic testout = ((Excel.Range)sheet.Cells[row + i, col]);
                    // Keep track of the first range you find. 
                    strTest = testout.text;
                    if (strTest != null && strTest!="") { currentFind = null; break; }
                }
                currentFind = null;
                
                //if (firstFind == null)
                //{
                //    firstFind = currentFind;
                //}
                //// If you didn't move to a new range, you are done.
                //else if (currentFind.get_Address(Excel.XlReferenceStyle.xlA1)
                //      == firstFind.get_Address(Excel.XlReferenceStyle.xlA1))
                //{
                //    break;
                //}

                //currentFind.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                //currentFind.Font.Bold = true;

                //currentFind = range.FindNext(currentFind);
            }return strTest;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=SP3DSMP7;Initial Catalog=db_S3DSupport;Integrated Security=True");
            con.Open();
            string[] subdirectoryEntries = Directory.GetDirectories(textBox1.Text);
            foreach (string subdirectory in subdirectoryEntries)

                try
                {
                    LoadSubDirs(subdirectory,con);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            //string[] Files = Directory.GetFiles(textBox1.Text);
            //foreach(string file in Files)
            //{
            //    if (CheckExtension(file))
            //    {
            //        LoadFiletoTable(file,con);
            //    };
            //}
            con.Close();
            MessageBox.Show("DONE!");
        }
        private void LoadSubDirs(string dir, SqlConnection con)

        {
            try
            {
                //Console.WriteLine(dir);
                SaveInfoforFiles(dir, con);
                string[] subdirectoryEntries = Directory.GetDirectories(dir);

                foreach (string subdirectory in subdirectoryEntries)

                {

                    LoadSubDirs(subdirectory, con);

                }
            }
            catch (Exception) { }
        }

        private void SaveInfoforFiles(string dir, SqlConnection con)

        {
            try
            {
                string[] Files = Directory.GetFiles(dir);
                foreach (string file in Files)
                {
                    if (CheckExtension(file))
                    {
                        IEnumerable<char> c = (IEnumerable<char>)file;
                        string c2 = String.Concat(c.Reverse());
                        string check = c2.Substring(0, c2.IndexOf("\\"));
                        IEnumerable<char> e = (IEnumerable<char>)check;
                        string strWorkbookName = String.Concat(e.Reverse());
                        //Excel.Application xlApp;
                        //Excel.Workbook xlWorkBook;
                        ////Excel.Sheets xlWorkSheets;

                        //xlApp = new Excel.Application();
                        //xlWorkBook = xlApp.Workbooks.Open(file, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                        //string strWorkbookName = xlWorkBook.Name;
                        //xlWorkBook.Close();
                        WritetoTable(strWorkbookName, dir, con);
                    };
                }
            }
            catch (Exception) { }

        }

        //private void LoadFiletoTable(string file, string dir, SqlConnection con)
        //{
        //    //Excel.Application xlApp;
        //    //Excel.Workbook xlWorkBook;
        //    //Excel.Sheets xlWorkSheets;

        //    //xlApp = new Excel.Application();
        //    //xlWorkBook = xlApp.Workbooks.Open(file, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
        //    //string strWorkbookName = xlWorkBook.Name;
        //    //xlWorkSheets = xlWorkBook.Worksheets;
        //    //foreach(Excel.Worksheet sheet in xlWorkSheets)
        //    //{
        //        //string strPartClassName = sheet.Name;
        //        //string strUserClassName = FindUserClass(sheet);
        //        //MessageBox.Show(strUserClassName);
        //        WritetoTable(file,dir,con);
        //    //}
        //    //xlWorkBook.Close();

        //}

        private void WritetoTable(string file, string dir, SqlConnection con)
        {
            try
            {
                string strSSPath = dir.Substring(dir.IndexOf("SmartSupport Content") + 21).Replace(@"\", " > ");
                string str = "insert into dbo.WorkbookDirectory (Workbook,Path,SSPath) values ('" + file + "','" + dir+ @"\" + file + "','"+ strSSPath +"')";
                SqlCommand xp = new SqlCommand(str, con);
                xp.ExecuteNonQuery();
            }
            catch (Exception) { }
        }

        private Boolean CheckExtension(string file)
        {
            Boolean bTest = false;
            IEnumerable<char> c = (IEnumerable<char>)file;
            string c2 = String.Concat(c.Reverse());
            string check = c2.Substring(0, c2.IndexOf("."));

            switch(check)
            {
                case "slx":
                case "xslx":
                    bTest = true;
                    break;
            }
            return bTest;
        }
    }
}
