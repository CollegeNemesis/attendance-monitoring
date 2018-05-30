using GemBox.Spreadsheet;
using Microsoft.Office.Core;
using Microsoft.Win32;
using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SJBCS.GUI.Dialogs;
using System.Configuration;
using System.Reflection;
using System.Threading;

namespace SJBCS.GUI.Report
{
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : UserControl
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string strDateFrom, strDateTo, strParams;
        string DEFAULT_PATH = ConfigurationManager.AppSettings["reportPath"];
        string DEFAULT_FILENAME = ConfigurationManager.AppSettings["tempReport"];
        string LOGO_PATH = ConfigurationManager.AppSettings["logoPath"];

        const string appName = "EntityFramework";
        const string providerName = "System.Data.SqlClient";

        string metaData = ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Metadata;
        string dataSource = ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Hostname;
        string initialCatalog = ConnectionHelper.Config.AppConfiguration.Settings.DataSource.InitialCatalog;
        string userId = ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Username;
        string password = ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Password;

        SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

        public ReportView()
        {
            try
            {
                InitializeComponent();
                sqlBuilder.DataSource = dataSource;
                sqlBuilder.InitialCatalog = initialCatalog;
                sqlBuilder.MultipleActiveResultSets = true;
                sqlBuilder.IntegratedSecurity = false;
                sqlBuilder.UserID = userId;
                sqlBuilder.Password = password;
                sqlBuilder.ApplicationName = appName;

                BindControls();

                if (!Directory.Exists(DEFAULT_PATH))
                {
                    Directory.CreateDirectory(DEFAULT_PATH);
                }
            }
            catch (Exception error)
            {
                LogError(error);
                Logger.Error(error);
            }
        }

        private async void LogError(Exception error)
        {
            var result = await DialogHelper.ShowDialog(DialogType.Error, error.Message);
        }

        private void FillDataGrid()
        {
            string CmdString = GetSP_Name();
            using (SqlConnection con = new SqlConnection(sqlBuilder.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@startDate", SqlDbType.Date).Value = strDateFrom;
                cmd.Parameters.Add("@endDate", SqlDbType.Date).Value = strDateTo;

                switch (strParams.ToLower())
                {
                    case "grade":
                        cmd.Parameters.Add("@GradeLevel", SqlDbType.NVarChar).Value = cboGrade.SelectedValue.ToString();
                        break;
                    case "gradesec":
                        cmd.Parameters.Add("@GradeLevel", SqlDbType.NVarChar).Value = cboGrade.SelectedValue.ToString();
                        cmd.Parameters.Add("@Section", SqlDbType.NVarChar).Value = cboSection.SelectedValue.ToString();
                        break;
                    case "fname":
                        cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = txtFirstName.Text.Trim();
                        break;
                    case "lname":
                        cmd.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = txtLastName.Text.Trim();
                        break;
                    case "studid":
                        cmd.Parameters.Add("@StudentID", SqlDbType.NVarChar).Value = txtStudentID.Text.Trim();
                        break;
                }

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Reports");
                sda.Fill(dt);
                dgResults.ItemsSource = dt.DefaultView;
            }
        }

        private string GetSP_Name()
        {
            return GetValues("dbo.PRC_SPName", "Report_SP", string.Empty, cboReports.SelectedValue.ToString(), cboFilter.SelectedIndex.ToString()).First();
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ExportToExcel();
            }
            catch (Exception error)
            {
                LogError(error);
                Logger.Error(error);
            }
        }

        private void CopyAlltoClipboard()
        {
            dgResults.SelectAllCells();
            ApplicationCommands.Copy.Execute(null, dgResults);
            string result = (string)Clipboard.GetData(DataFormats.Text);

            if (string.IsNullOrEmpty(result))
                Clipboard.SetDataObject(result);
        }

        private void ExportToExcel(bool isPDF = false)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            sfd.FilterIndex = 1;

            if (isPDF)
            {
                sfd.FileName = DEFAULT_PATH + DEFAULT_FILENAME;
                Mouse.OverrideCursor = Cursors.Wait;
                GenerateExcel(isPDF, sfd);
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            else
            {
                sfd.FileName = cboReports.SelectedValue.ToString().Trim() + "_" + GetFilterUsed() + DateTime.Today.ToString("dd-MMM-yyyy") + ".xlsx";
                if (sfd.ShowDialog() == true)
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    GenerateExcel(isPDF, sfd);
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }

        }

        private void GenerateExcel(bool isPDF, SaveFileDialog sfd)
        {
            CopyAlltoClipboard();

            object misValue = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.Application xlexcel = new Microsoft.Office.Interop.Excel.Application();

            xlexcel.DisplayAlerts = false;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook = xlexcel.Workbooks.Add(misValue);
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);


            int colStart = 1;
            int addCol = 0;
            int logoCellLeft = 0;

            if (cboReports.SelectedValue.ToString() == "Absentees Report" || cboReports.SelectedValue.ToString() == "No Time Out Report")
            {
                addCol = 2;
                colStart += addCol;
                logoCellLeft = 100;
            }
            else if (cboReports.SelectedValue.ToString() == "Attendance Report")
            {
                logoCellLeft = 12;
            }

            Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[8, colStart];

            xlWorkSheet.Shapes.AddPicture(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @LOGO_PATH, MsoTriState.msoFalse, MsoTriState.msoCTrue, logoCellLeft, 0, 90, 90);

            xlWorkSheet.get_Range("A1", "A1").Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            xlWorkSheet.get_Range("A1", "A1").Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;

            CR.Select();

            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

            CR.Rows.AutoFit();
            CR.get_Range("A1").Columns.EntireColumn.AutoFit();
            CR.get_Range("C1", "K1").Columns.EntireColumn.AutoFit();
            CR.get_Range("B1").Columns.ColumnWidth = 35;

            if (cboReports.SelectedValue.ToString().Trim() == "Attendance Report")
            {
                CR.get_Range("D1", "K1").Columns.EntireColumn.AutoFit();
                CR.get_Range("C1").Columns.ColumnWidth = 20;
            }


            CR.get_Range("B1").Cells.Style.WrapText = true;
            xlWorkSheet.Cells[3, 3 + addCol - 1] = "'                   " + cboReports.SelectedValue.ToString();
            xlWorkSheet.Cells[3, 3 + addCol - 1].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

            xlWorkSheet.Cells[4, 3 + addCol - 1] = "'                   " + dtFrom.SelectedDate.Value.ToString("dd MMMMM yyyy") + " to " + dtTo.SelectedDate.Value.ToString("dd MMMM yyyy");
            xlWorkSheet.Cells[4, 3 + addCol - 1].Cells.Style.WrapText = false;
            xlWorkSheet.Cells[4, 3 + addCol - 1].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

            xlWorkSheet.Cells[5, 3 + addCol - 1] = "'                   " + GetFilterUsed().Replace("_", "");
            xlWorkSheet.Cells[5, 3 + addCol - 1].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

            int ctr = 1;
            while (ctr <= dgResults.Columns.Count)
            {
                ((Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[8, ctr + addCol]).Interior.Color = ColorTranslator.ToOle(Color.AliceBlue);
                ctr++;
            }

            for (int ctrC = 1; ctrC <= dgResults.Columns.Count; ctrC++)
            {
                for (int ctrR = 8; ctrR <= dgResults.Items.Count + 8; ctrR++)
                {
                    ((Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[ctrR, ctrC + addCol]).BorderAround(LineStyle.Thin, Microsoft.Office.Interop.Excel.XlBorderWeight.xlHairline, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, ColorTranslator.ToOle(Color.AliceBlue));
                }

            }

            Microsoft.Office.Interop.Excel.Range rg = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, colStart];
            rg.EntireColumn.NumberFormat = "yyyy/MM/dd";

            if (cboReports.SelectedValue.ToString() == "Consolidated Report")
            {
                rg.EntireColumn.NumberFormat = "@";
            }

            xlWorkBook.SaveAs(sfd.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlexcel.DisplayAlerts = true;
            xlWorkBook.Close(true, misValue, misValue);
            xlexcel.Quit();

            ReleaseObject(xlWorkSheet);
            ReleaseObject(xlWorkBook);
            ReleaseObject(xlexcel);

            dgResults.UnselectAllCells();

            Clipboard.Clear();
            if (File.Exists(sfd.FileName) && !isPDF)
            {
                CheckIfFileIsOpen(sfd);
                //System.Diagnostics.Process.Start(sfd.FileName);
                System.Diagnostics.Process.Start(Path.GetDirectoryName(sfd.FileName));
            }
        }

        private async void CheckIfFileIsOpen(SaveFileDialog sfd)
        {
            FileInfo file = new FileInfo(sfd.FileName);
            if (IsFileLocked(file))
            {
                await DialogHelper.ShowDialog(DialogType.Informational, "File with the same name is in use by another process.");
            }
        }

        private string GetFilterUsed()
        {
            string strFilter = cboFilter.SelectedValue.ToString();
            string filterUsed = string.Empty;

            if (strFilter == "By Grade")
            {
                filterUsed = cboGrade.SelectedValue.ToString().Trim() + "_";
            }
            else if (strFilter == "By Grade and Section")
            {
                filterUsed = cboGrade.SelectedValue.ToString().Trim() + " - " + cboSection.SelectedValue.ToString().Trim() + "_";
            }


            return filterUsed;
        }

        private async void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception error)
            {
                obj = null;
                var result = await DialogHelper.ShowDialog(DialogType.Error, "Exception occurred while releasing object " + error.Message);
                Logger.Error(error);
            }
            finally
            {
                GC.Collect();
            }
        }

        private void dgResults_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Date")
            {
                DataGridTextColumn col = e.Column as DataGridTextColumn;
                col.Binding = new Binding(e.PropertyName) { StringFormat = "yyyy/MM/dd" };
            }
        }

        private async void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InputValidation();
                FillDataGrid();

                dgResults.Visibility = Visibility.Visible;

                if (dgResults.Items.Count <= 0)
                {
                    EnableExportButtons(false);
                    dgResults.Visibility = Visibility.Hidden;
                    await DialogHelper.ShowDialog(DialogType.Informational, "No results found.");
                }
                else
                {
                    EnableExportButtons(true);
                    dgResults.Visibility = Visibility.Visible;
                }
            }
            catch (Exception error)
            {
                LogError(error);
                Logger.Error(error);
            }
        }

        private void EnableExportButtons(bool isEnabled)
        {
            btnExport.IsEnabled = isEnabled;
            btnExportPDF.IsEnabled = isEnabled;
        }

        private void btnExportPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = cboReports.SelectedValue.ToString().Trim() + "_" + GetFilterUsed() + DateTime.Today.ToString("dd-MMM-yyyy") + ".pdf";
                ExportWorkbookToPdf(DEFAULT_PATH + DEFAULT_FILENAME, sfd);
                if (File.Exists(DEFAULT_PATH + DEFAULT_FILENAME))
                {
                    File.Delete(DEFAULT_PATH + DEFAULT_FILENAME);
                }
            }
            catch (Exception error)
            {
                LogError(error);
                Logger.Error(error);
            }
        }

        public bool ExportWorkbookToPdf(string workbookPath, SaveFileDialog sfd)
        {
            sfd.Filter = "All files (*.pdf*)|*.*";
            sfd.FilterIndex = 1;

            var exportSuccessful = true;

            if (sfd.ShowDialog() == true)
            {
                ExportToExcel(true);
                if (string.IsNullOrEmpty(workbookPath) || string.IsNullOrEmpty(sfd.FileName))
                {
                    return false;
                }

                Microsoft.Office.Interop.Excel.Application excelApplication;
                Microsoft.Office.Interop.Excel.Workbook excelWorkbook;

                excelApplication = new Microsoft.Office.Interop.Excel.Application();
                excelApplication.ScreenUpdating = false;
                excelApplication.DisplayAlerts = false;
                excelWorkbook = excelApplication.Workbooks.Open(workbookPath);

                if (excelWorkbook == null)
                {
                    excelApplication.Quit();
                    excelApplication = null;
                    excelWorkbook = null;

                    return false;
                }

                try
                {
                    Microsoft.Office.Interop.Excel.XlPageOrientation orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
                    if (cboReports.SelectedValue.ToString() == "Absentees Report" || cboReports.SelectedValue.ToString() == "No Time Out Report" || cboReports.SelectedValue.ToString() == "Consolidated Report")
                    {
                        orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlPortrait;
                    }

                    ((Microsoft.Office.Interop.Excel._Worksheet)excelWorkbook.ActiveSheet).PageSetup.Orientation = orientation;
                    ((Microsoft.Office.Interop.Excel._Worksheet)excelWorkbook.ActiveSheet).PageSetup.PaperSize = Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperLegal;

                    excelWorkbook.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, sfd.FileName, Microsoft.Office.Interop.Excel.XlFixedFormatQuality.xlQualityStandard, true, true, 1);
                }

                catch (System.Exception error)
                {
                    exportSuccessful = false;
                    //var result =  await DialogHelper.ShowDialog(DialogType.Error, "Export to PDF failed. --" + error.Message);
                    LogError(error);
                    Logger.Error(error);
                }
                finally
                {
                    excelWorkbook.Close();
                    excelApplication.Quit();

                    excelApplication = null;
                    excelWorkbook = null;
                }

                if (System.IO.File.Exists(sfd.FileName))
                {
                    CheckIfFileIsOpen(sfd);
                    //System.Diagnostics.Process.Start(sfd.FileName);
                    System.Diagnostics.Process.Start(Path.GetDirectoryName(sfd.FileName));
                }

            }
            return exportSuccessful;
        }

        private void DatePickerFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var now = DateTime.Now;
            strDateFrom = GetDateRange(sender);
            dgResults.Visibility = Visibility.Hidden;
            btnExport.IsEnabled = false;
            btnExportPDF.IsEnabled = false;
            dtTo.SelectedDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
        }

        private void DatePickerTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            strDateTo = GetDateRange(sender);
            dgResults.Visibility = Visibility.Hidden;
            btnExport.IsEnabled = false;
            btnExportPDF.IsEnabled = false;
        }

        private string GetDateRange(object sender)
        {
            var picker = sender as DatePicker;
            DateTime? date = picker.SelectedDate;

            if (date == null)
            {
                return string.Empty;
            }
            else
            {
                return date.Value.ToShortDateString();
            }
        }

        private void BindControls()
        {
            cboReports.ItemsSource = GetValues("dbo.PRC_ReportsList", "ReportName");
            cboFilter.ItemsSource = GetValues("dbo.PRC_FiltersList", "FilterName", string.Empty, cboReports.SelectedValue.ToString());
            dtFrom.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtTo.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        }

        private void cboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                isPopulated(cboFilter);
                string strFilter = cboFilter.SelectedValue.ToString();

                if (strFilter == "By Grade")
                {
                    pnlGrade.Visibility = Visibility.Visible;
                    pnlGradeSection.Visibility = Visibility.Hidden;
                    pnlFirstName.Visibility = Visibility.Hidden;
                    pnlLastName.Visibility = Visibility.Hidden;
                    pnlStudentID.Visibility = Visibility.Hidden;

                    cboGrade.ItemsSource = GetValues("dbo.PRC_GradeList", "GradeLevel");
                    strParams = "grade";
                    isPopulated(cboGrade);
                }
                if (strFilter == "By Grade and Section")
                {
                    pnlGrade.Visibility = Visibility.Visible;
                    pnlGradeSection.Visibility = Visibility.Visible;
                    pnlFirstName.Visibility = Visibility.Hidden;
                    pnlLastName.Visibility = Visibility.Hidden;
                    pnlStudentID.Visibility = Visibility.Hidden;

                    cboGrade.ItemsSource = GetValues("dbo.PRC_GradeList", "GradeLevel");
                    isPopulated(cboGrade);

                    cboSection.ItemsSource = GetValues("dbo.PRC_SectionList", "SectionName", cboGrade.SelectedValue.ToString());
                    isPopulated(cboSection);

                    strParams = "gradeSec";
                }
                if (strFilter == "By First Name")
                {
                    pnlGrade.Visibility = Visibility.Hidden;
                    pnlGradeSection.Visibility = Visibility.Hidden;
                    pnlFirstName.Visibility = Visibility.Visible;
                    pnlLastName.Visibility = Visibility.Hidden;
                    pnlStudentID.Visibility = Visibility.Hidden;
                    strParams = "fname";
                }
                if (strFilter == "By Last Name")
                {
                    pnlGrade.Visibility = Visibility.Hidden;
                    pnlGradeSection.Visibility = Visibility.Hidden;
                    pnlFirstName.Visibility = Visibility.Hidden;
                    pnlLastName.Visibility = Visibility.Visible;
                    pnlStudentID.Visibility = Visibility.Hidden;
                    strParams = "lname";
                }
                if (strFilter == "By Student ID")
                {
                    pnlGrade.Visibility = Visibility.Hidden;
                    pnlGradeSection.Visibility = Visibility.Hidden;
                    pnlFirstName.Visibility = Visibility.Hidden;
                    pnlLastName.Visibility = Visibility.Hidden;
                    pnlStudentID.Visibility = Visibility.Visible;
                    strParams = "studID";
                }

                dgResults.Visibility = Visibility.Hidden;
                EnableExportButtons(false);
            }
            catch (Exception error)
            {
                LogError(error);
                Logger.Error(error);
            }
        }

        private void cboGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (pnlGradeSection.Visibility == 0)
                {
                    cboSection.ItemsSource = GetValues("dbo.PRC_SectionList", "SectionName", cboGrade.SelectedValue.ToString());
                    isPopulated(cboSection);
                    cboSection.SelectedIndex = 0;
                }

                dgResults.Visibility = Visibility.Hidden;
                EnableExportButtons(false);
            }
            catch (Exception error)
            {
                LogError(error);
                Logger.Error(error);
            }
        }

        private List<string> GetValues(string script, string colName, string paramGL = "", string paramRN = "", string paramFi = "")
        {
            List<String> List = new List<String>();

            string CmdString = script;
            using (SqlConnection con = new SqlConnection(sqlBuilder.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (!string.IsNullOrEmpty(paramGL))
                { cmd.Parameters.Add("@GradeLevel", SqlDbType.NVarChar).Value = paramGL; }

                if (!string.IsNullOrEmpty(paramRN))
                { cmd.Parameters.Add("@ReportType", SqlDbType.NVarChar).Value = paramRN; }

                if (!string.IsNullOrEmpty(paramFi))
                { cmd.Parameters.Add("@Filter", SqlDbType.NVarChar).Value = paramFi; }

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                sda.Fill(dt);
                List = dt.AsEnumerable()
                    .Select(r => r.Field<string>(colName)).Distinct()
                    .ToList();
            }

            return List;
        }

        private void cboReports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cboFilter.SelectedIndex = 0;
                cboFilter.ItemsSource = GetValues("dbo.PRC_FiltersList", "FilterName", string.Empty, cboReports.SelectedValue.ToString());

                isPopulated(cboFilter);
                cboFilter_SelectionChanged(sender, e);

                dgResults.Visibility = Visibility.Hidden;
                EnableExportButtons(false);
            }
            catch (Exception error)
            {
                LogError(error);
                Logger.Error(error);
            }
        }

        private void cboSection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                dgResults.Visibility = Visibility.Hidden;
                EnableExportButtons(false);
            }
            catch (Exception error)
            {
                LogError(error);
                Logger.Error(error);
            }
        }

        private async void isPopulated(ComboBox cb)
        {
            if (cb.Items.Count < 1)
            {
                btnGenerate.IsEnabled = false;
                cb.IsEnabled = false;
                var result = await DialogHelper.ShowDialog(DialogType.Error, "Some fields do not have values. Please contact system administrator.");
            }
            else
            {
                cb.IsEnabled = true;
                btnGenerate.IsEnabled = true;
            }
        }

        private void InputValidation()
        {

            if (dtFrom.SelectedDate == null || dtTo.SelectedDate == null)
            {
                throw new Exception("Date From and Date To are required.");
            }

            if (dtTo.SelectedDate < dtFrom.SelectedDate)
            {
                throw new Exception("Invalid date range.");
            }

            if (pnlFirstName.Visibility == 0)
            {
                if (string.IsNullOrEmpty(txtFirstName.Text.Trim()))
                {
                    throw new Exception("First Name is required.");
                }
            }

            if (pnlLastName.Visibility == 0)
            {
                if (string.IsNullOrEmpty(txtLastName.Text.Trim()))
                {
                    throw new Exception("Last Name is required.");
                }
            }

            if (pnlStudentID.Visibility == 0)
            {
                if (string.IsNullOrEmpty(txtStudentID.Text.Trim()))
                {
                    throw new Exception("Student ID is required.");
                }
            }

            if (cboGrade.SelectedIndex < 0)
            {
                throw new Exception("Please select Grade.");
            }

            if (cboSection.SelectedIndex < 0)
            {
                throw new Exception("Please select Section.");
            }

        }

        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }
    }
}
