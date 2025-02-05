﻿
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Augustine.VietnameseCalendar.Core.LuniSolarCalendar;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using System.Collections;
using System.Windows.Controls.Primitives;
using System.Data;
using HRMS.HR.Model.Database;
using iTextSharp.text.pdf;
using iTextSharp.text;
using ClosedXML.Excel;
using System.Reflection;
using HRMS.HR.Model;
using HRMS.HR.View;
using HRMS.HR.uCon;

namespace HRMS.HR.ViewModel
{
    class ReportViewModel : BaseViewModel
    {
        //Export to PDF
        public ICommand ExportPDFCommand { get; set; }
        //Export to Excel
        public ICommand ExportExcelCommand { get; set; }
        public ICommand showDetailCommand { get; set; }
        
        public ReportViewModel(int ID)
        {
            LoadComboboxTypeList();
            LoadMonth();
            LoadDeptList();
            hrmsEntities db = new hrmsEntities();
            var employee = db.EMPLOYEEs.Where(x => x.EMPLOYEE_ID == ID).SingleOrDefault();
            //Chức năng export to pdf
            ExportPDFCommand = new RelayCommand<DataGrid>(p => { if (SELECTMONTHTYPE.MONTH == DateTime.Now.Month && SELECTMONTHTYPE.YEAR == DateTime.Now.Year) { return false; } else { return true; } }, p => ExportPDF(p, employee.NAME));

            //Chức năng export to excel
            ExportExcelCommand = new RelayCommand<DataGrid>(p => { if (SELECTMONTHTYPE.MONTH == DateTime.Now.Month && SELECTMONTHTYPE.YEAR == DateTime.Now.Year) { return false; } else { return true; } }, p => ExportExcel(p));

            showDetailCommand = new RelayCommand<ContentControl>(p => { return true; }, p => { p.Content = new uConTimekeepingDetail(ID, SELECTEDITEM); });
        }
        
        //Binding tới datagrid của Salary List
        private ObservableCollection<TimekeepingData> _TimekeepingList;
        public ObservableCollection<TimekeepingData> TimekeepingList { get => _TimekeepingList; set { _TimekeepingList = value; OnPropertyChanged(); } }

        //Để lưu trữ bản sao để có thể sao chép khi cần thiết
        private ObservableCollection<TimekeepingData> _TimekeepingTest;
        public ObservableCollection<TimekeepingData> TimekeepingTest { get => _TimekeepingTest; set { _TimekeepingTest = value; OnPropertyChanged(); } }

        //Binding dữ liệu vào combobox của chọn loại để lọc
        private ObservableCollection<ComboboxModel> _ListType;
        public ObservableCollection<ComboboxModel> ListType { get => _ListType; set { _ListType = value; OnPropertyChanged(); } }

        //Binding dữ liệu với select trong comboox chọn loại lọc
        private ComboboxModel _SELECTEDTYPE;
        public ComboboxModel SELECTEDTYPE { get => _SELECTEDTYPE; set { _SELECTEDTYPE = value; OnPropertyChanged(); } }

        //Binding dữ liệu với Search Text
        private string _SEARCH_TEXT;
        public string SEARCH_TEXT
        {
            get => _SEARCH_TEXT; set
            {
                _SEARCH_TEXT = value;
                OnPropertyChanged();

                TimekeepingList = TimekeepingTest;

                //Kiểm tra SearchText có khác null không
                if (!string.IsNullOrEmpty(SEARCH_TEXT))
                {
                    //Kiểm tra ComboBox chọn loại để lọc có khác null không
                    if (SELECTEDTYPE != null)
                    {
                        //Chọn kiểu lọc
                        switch (SELECTEDTYPE.NAME)
                        {
                            //Lọc theo ID
                            case "ID":
                                TimekeepingList = new ObservableCollection<TimekeepingData>(TimekeepingList.Where(x => x.EMPLOYEE_ID.ToString().Contains(SEARCH_TEXT)));
                                break;

                            //Lọc theo MONTH
                            case "NAME":
                                TimekeepingList = new ObservableCollection<TimekeepingData>(TimekeepingList.Where(x => x.NAME.Contains(SEARCH_TEXT) ||
                                                                                                        x.NAME.ToLower().Contains(SEARCH_TEXT) ||
                                                                                                        x.NAME.ToUpper().Contains(SEARCH_TEXT)));
                                break;
                            //Lọc theo Department
                            case "DEPARTMENT":
                                TimekeepingList = new ObservableCollection<TimekeepingData>(TimekeepingList.Where(x => x.DEPT.Contains(SEARCH_TEXT) ||
                                                                                                        x.DEPT.ToLower().Contains(SEARCH_TEXT) ||
                                                                                                        x.DEPT.ToUpper().Contains(SEARCH_TEXT)));
                                break;

                            //Lọc theo role
                            case "ROLE":
                                TimekeepingList = new ObservableCollection<TimekeepingData>(TimekeepingList.Where(x => x.ROLE.Contains(SEARCH_TEXT) ||
                                                                                                        x.ROLE.ToLower().Contains(SEARCH_TEXT) ||
                                                                                                        x.ROLE.ToUpper().Contains(SEARCH_TEXT)));
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }
        //Lấy ngày trong tháng
        public static int GetDaybyMonth(int month, int year)
        {
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    return 31;
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                default:
                    return ((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0)) ? 29 : 28;
            }
        }
        //Kiểm tra ngày nghỉ
        public static bool IsHoliday(int day, int month, int year)
        {
            //Kiểm tra ngày hôm đó có phải ngày nghỉ không nếu ngày nghỉ rơi vào thứ 7 hoặc chủ nhật thì được nghỉ bù vào thứ 2

            //Kiểm tra ngày 1/1
            if (day == 1 && month == 1)
                return true;
            DateTime date = new DateTime(year, 1, 1);
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                if (day == 3 && month == 1)
                    return true;
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                if (day == 2 && month == 1)
                    return true;
            }

            //Kiểm tra ngày 30/4
            if (day == 30 && month == 4)
                return true;
            date = new DateTime(year, 4, 30);
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                if (day == 2 && month == 5)
                    return true;
                if (day == 3 && month == 5)
                    return true;
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                if (day == 2 && month == 5)
                    return true;
            }

            //Kiểm tra ngày 1/5
            if (day == 1 && month == 5)
                return true;
            date = new DateTime(year, 5, 1);
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                if (day == 3 && month == 5)
                    return true;
            }

            //Kiểm tra ngày 2/9 (Quốc Khánh được nghỉ 2 ngày)
            if (day == 2 && month == 9)
                return true;
            date = new DateTime(year, 9, 2);
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                if (day == 4 && month == 9)
                    return true;
                if (day == 5 && month == 9)
                    return true;

            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                if (day == 4 && month == 9)
                    return true;
            }

            //Kiểm tra ngày 3/9
            if (day == 3 && month == 9)
                return true;
            date = new DateTime(year, 9, 3);
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                if (day == 5 && month == 9)
                    return true;
            }

            //Kiểm tra ngày âm (tết âm lịch đc nghỉ 5 ngày 29 1 2 3 4 và ngày 10/3) 
            //Kiểm tra ngày 1/1 có phải thứ 3 ko nếu phải thì ko đc nghỉ bù còn thứ 2 thì đc nghỉ bù 1 ngày, còn lai thì nghỉ bù 2 ngày
            LuniSolarDate<VietnameseLocalInfoProvider> lunnardate = LuniSolarCalendar<VietnameseLocalInfoProvider>.LuniSolarDateFromSolarDate(new DateTime(year, month, day));
            int lunnarday = lunnardate.Day;
            int lunarmonth = lunnardate.Month;
            LuniSolarDate<VietnameseLocalInfoProvider> datelunar = LuniSolarCalendar<VietnameseLocalInfoProvider>.LuniSolarDateFromLunarInfo(year, 1, false, 1);
            int tempday = datelunar.Day;
            int tempmonth = datelunar.Month;
            if (new DateTime(year, tempmonth, tempday).DayOfWeek != DayOfWeek.Tuesday)
            {
                if (lunnarday == 5 && lunarmonth == 1)
                    return true;
                if (lunnarday == 6 && lunarmonth == 1)
                    return true;
            }
            else if (new DateTime(year, tempmonth, tempday).DayOfWeek == DayOfWeek.Monday)
            {
                if (lunnarday == 5 && lunarmonth == 1)
                    return true;
            }

            DateTime temp = datelunar.SolarDate.AddDays(-1);
            if (temp.Day == day && temp.Month == month)
                return true;
            if (lunnarday == 1 && lunarmonth == 1)
                return true;
            if (lunnarday == 2 && lunarmonth == 1)
                return true;
            if (lunnarday == 3 && lunarmonth == 1)
                return true;
            if (lunnarday == 4 && lunarmonth == 1)
                return true;
            if (lunnarday == 10 && lunarmonth == 3)
                return true;

            datelunar = LuniSolarCalendar<VietnameseLocalInfoProvider>.LuniSolarDateFromLunarInfo(year, 3, false, 10);
            tempday = datelunar.Day;
            tempmonth = datelunar.Month;
            if (new DateTime(year, tempmonth, tempday).DayOfWeek == DayOfWeek.Saturday)
            {
                if (lunnarday == 12 && lunarmonth == 3)
                    return true;
            }
            else if (new DateTime(year, tempmonth, tempday).DayOfWeek == DayOfWeek.Sunday)
            {
                if (lunnarday == 11 && lunarmonth == 3)
                    return true;
            }

            //Kiểm tra ngày đó có phải chủ nhật ko
            if (new DateTime(year, month, day).DayOfWeek == DayOfWeek.Sunday)
                return true;

            return false;
        }

        //Tìm số ngày công chuẩn 5.5 ngày/ tuần
        public static float CalculateAverageDay(int month, int year)
        {
            float countAverage = 0;
            int Day = GetDaybyMonth(month, year);
            for (int i = 1; i <= Day; i++)
            {
                if (!IsHoliday(i, month, year))
                {
                    DateTime date = new DateTime(year, month, i);
                    if (date.DayOfWeek == DayOfWeek.Saturday)
                        countAverage++;
                    else
                        countAverage += 2;
                }
            }
            return countAverage / 2;
        }

        private ObservableCollection<ComboboxModel> _DEPTLIST;
        public ObservableCollection<ComboboxModel> DEPTLIST { get => _DEPTLIST; set { _DEPTLIST = value; OnPropertyChanged(); } }

        //Binding tới selected của ComboxBox chọn tháng
        private ComboboxModel _SELECTDEPTTYPE;
        public ComboboxModel SELECTDEPTTYPE
        {
            get => _SELECTDEPTTYPE; set
            {
                _SELECTDEPTTYPE = value;
                OnPropertyChanged();

                //Nếu selected khác null, nghĩa là tháng đã chọn thì show data theo select dó
                if (SELECTDEPTTYPE != null)
                {
                    LoadTimekeepingData();
                }
            }
        }
        //Binding tới ComboBox chọn Tháng
        private ObservableCollection<ComboboxModel> _MONTHLIST;
        public ObservableCollection<ComboboxModel> MONTHLIST { get => _MONTHLIST; set { _MONTHLIST = value; OnPropertyChanged(); } }

        //Binding tới selected của ComboxBox chọn tháng
        private ComboboxModel _SELECTMONTHTYPE;
        public ComboboxModel SELECTMONTHTYPE
        {
            get => _SELECTMONTHTYPE; set
            {
                _SELECTMONTHTYPE = value;
                OnPropertyChanged();

                //Nếu selected khác null, nghĩa là tháng đã chọn thì show data theo select dó
                if (SELECTMONTHTYPE != null && SELECTDEPTTYPE != null)
                {
                    LoadTimekeepingData();
                }
            }
        }
        private void LoadMonth()
        {
            //Chọn tháng từ database KHÔNG TRÙNG LẶP (chọn DATE_START và DATE_END để kiểm tra tháng bắt đầu và tháng kết thúc có hợp lệ không (nếu cách nhau không quá 31 ngày hợp lệ)
            var listmonth = (from month in HRMSEntities.Ins.DB.TIMEKEEPINGs
                             orderby month.MONTH descending
                             select new { Month = month.MONTH }).Distinct();

            //Khởi tạo biến MONTHLIST để chứa tháng
            MONTHLIST = new ObservableCollection<ComboboxModel>();

            //Đưa dữ liệu từ listmonth vào MONTHLIST
            foreach (var item in listmonth)
            {
                 DateTime date1 = (DateTime)item.Month;
                //Nếu điều kiện hợp lệ thì lưu dữ liệu vào ComboBox Month thông qua MONTHLIST
                 MONTHLIST.Add(new ComboboxModel(date1.Month, date1.Year, (date1.Month == DateTime.Now.Month && date1.Year == DateTime.Now.Year) ? true : false));
            }
            SELECTMONTHTYPE = MONTHLIST.Where(x => x.ISSELECTED == true).FirstOrDefault();
            if (SELECTMONTHTYPE == null)
            {
                SELECTMONTHTYPE = MONTHLIST.FirstOrDefault();
            }
        }

        private void LoadDeptList()
        {
            DEPTLIST = new ObservableCollection<ComboboxModel>();
            DEPTLIST.Add(new ComboboxModel("ALL", 0, true));
            DEPTLIST.Add(new ComboboxModel("HUMAN RESOURCE DEPT", 1, false));
            DEPTLIST.Add(new ComboboxModel("ACOUNTING DEPT", 2, false));
            DEPTLIST.Add(new ComboboxModel("DIRECTOR DEPT", 3, false));
            DEPTLIST.Add(new ComboboxModel("SOFTWARE DEPT", 4, false));
            DEPTLIST.Add(new ComboboxModel("QUALITY MANAGEMENT DEPT", 5, false));
            DEPTLIST.Add(new ComboboxModel("BUSSINESS DEPT", 6, false));
            DEPTLIST.Add(new ComboboxModel("SUPPORT DEPT", 7, false));
            SELECTDEPTTYPE = DEPTLIST.Where(x => x.ISSELECTED == true).FirstOrDefault();
        }

        private void LoadComboboxTypeList()
        {
            ListType = new ObservableCollection<ComboboxModel>();
            ListType.Add(new ComboboxModel("ID", true));
            ListType.Add(new ComboboxModel("NAME", false));
            ListType.Add(new ComboboxModel("DEPARTMENT", false));
            ListType.Add(new ComboboxModel("ROLE", false));
            SELECTEDTYPE = ListType.Where(x => x.ISSELECTED == true).FirstOrDefault();
        }

        //Load data vaof grid
        private void LoadTimekeepingData()
        {
            if (SELECTDEPTTYPE == null || SELECTMONTHTYPE == null)
            {
                return;
            }
            hrmsEntities db = new hrmsEntities();
            var list_filter_dept = from emp in db.EMPLOYEEs where SELECTDEPTTYPE.DEPT_ID > 0 ? emp.DEPT_ID == SELECTDEPTTYPE.DEPT_ID : true select emp;
            var list = (from emp in list_filter_dept
                        join tk in db.TIMEKEEPINGs on emp.EMPLOYEE_ID equals tk.EMPLOYEE_ID
                        where tk.MONTH.Value.Month == SELECTMONTHTYPE.MONTH && tk.MONTH.Value.Year == SELECTMONTHTYPE.YEAR
                        select tk).Distinct(); ;
            TimekeepingList = new ObservableCollection<TimekeepingData>();
            TimekeepingTest = new ObservableCollection<TimekeepingData>();
            foreach (var item in list)
            {
                item.NUMBER_OF_STANDARD_DAY = CalculateAverageDay(item.MONTH.Value.Month, item.MONTH.Value.Year);
                TimekeepingData data = new TimekeepingData();
                data.EMPLOYEE_ID = (int)item.EMPLOYEE_ID;
                data.NAME = item.EMPLOYEE.NAME;
                data.DEPT = item.EMPLOYEE.DEPARTMENT.DEPT_NAME;
                data.ROLE = item.EMPLOYEE.ROLE.ROLE_NAME;
                data.WORK = (double)item.NUMBER_OF_WORK_DAY;
                data.ABSENT = (double)item.NUMBER_OF_ABSENT_DAY;
                data.STANDARD = (double)item.NUMBER_OF_STANDARD_DAY;
                data.MONTH = (DateTime)item.MONTH;
                TimekeepingList.Add(data);
                TimekeepingTest.Add(data);
            }

        }

        //Chuyển từ datagrid sang datatable
        public static DataTable ToDataTable<T>(List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (var item in items)
            {
                var values = new object[properties.Length];
                for (var i = 0; i < properties.Length; i++)
                {
                    //inserting property values to data table rows
                    values[i] = properties[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check data table
            return dataTable;
        }
        //Lưu datagrid thàng pdf (name: UserName)
        private void ExportPDF(DataGrid dtgrid, string name)
        {
            var d = dtgrid.ItemsSource.Cast<TimekeepingData>();
            var data = ToDataTable(d.ToList());

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Pdf Files|*.pdf";
            saveFileDialog.Title = "Save Pdf file";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                System.IO.FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                Document document = new Document(iTextSharp.text.PageSize.A4);

                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                //Report Header
                BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font fntHead = new Font(bfntHead, 30, 1, BaseColor.GRAY);
                Paragraph prgHeading = new Paragraph();
                prgHeading.Alignment = Element.ALIGN_CENTER;
                prgHeading.Add(new Chunk(String.Format("TIMEKEEPING REPORT {0}/{1}", SELECTMONTHTYPE.MONTH, SELECTMONTHTYPE.YEAR), fntHead));
                document.Add(prgHeading);

                //Author
                Paragraph prgAuthor = new Paragraph();
                BaseFont btnAuthor = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font fntAuthor = new Font(btnAuthor, 8, 2, BaseColor.BLACK);
                prgAuthor.Alignment = Element.ALIGN_RIGHT;
                prgAuthor.Add(new Chunk(String.Format("Author : {0}", name), fntAuthor));
                prgAuthor.Add(new Chunk("\nRun Date : " + DateTime.Now.ToShortDateString(), fntAuthor));
                document.Add(prgAuthor);

                //Add a line seperation
                Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                document.Add(p);

                //Add line break
                document.Add(new Chunk("\n", fntHead));

                //Write the table
                PdfPTable table = new PdfPTable(data.Columns.Count);
                //Table header
                BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font fntColumnHeader = new Font(btnColumnHeader, 6, 1, BaseColor.WHITE);
                Font fntColumnData = new Font(btnColumnHeader, 5, 1, BaseColor.BLACK);
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    PdfPCell cell = new PdfPCell();
                    cell.BackgroundColor = BaseColor.GRAY;
                    String header = data.Columns[i].ColumnName.ToUpper();
                    string[] split_string = header.Split('_');
                    String name_temp = "";
                    foreach (var item in split_string)
                        name_temp = item + " ";

                    cell.AddElement(new Chunk(String.Format("{0}", name_temp), fntColumnHeader));
                    table.AddCell(cell);
                }
                //table Data
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    for (int j = 0; j < data.Columns.Count; j++)
                    {
                        DateTime date = DateTime.Now;
                        if (data.Rows[i][j].GetType() == date.GetType())
                        {
                            date = (DateTime)data.Rows[i][j];
                            PdfPCell cell_data = new PdfPCell();
                            string data_table = date.Day + "/" + date.Month + "/" + date.Year;
                            cell_data.AddElement(new Chunk(data_table, fntColumnData));
                            table.AddCell(cell_data);
                        }
                        else
                        {
                            PdfPCell cell_data = new PdfPCell();
                            string data_table = data.Rows[i][j].ToString();
                            cell_data.AddElement(new Chunk(data_table, fntColumnData));
                            table.AddCell(cell_data);
                        }
                    }
                }

                document.Add(table);
                document.Close();
                writer.Close();
                fs.Close();
            }
            String message = "Export data to " + saveFileDialog.FileName + " successful";
            MessageBox.Show(message, "MESSAGE", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //Lưu datagrid thành excel
        private void ExportExcel(DataGrid dtgrid)
        {
            var d = dtgrid.ItemsSource.Cast<TimekeepingData>();
            var data = ToDataTable(d.ToList());
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files(.xlsx)| *.xlsx";
            saveFileDialog.Title = "Save Excel file";

            if (saveFileDialog.ShowDialog() == true)
            {
                using (XLWorkbook workbook = new XLWorkbook())
                {
                    workbook.Worksheets.Add(data, "Month " + SELECTMONTHTYPE.MONTH + "-" + SELECTMONTHTYPE.YEAR);
                    workbook.SaveAs(saveFileDialog.FileName);
                }
                String message = "Export data to " + saveFileDialog.FileName + " successful";
                MessageBox.Show(message, "MESSAGE", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #region timekeepingDetail
        public ReportViewModel(int id, TimekeepingData data)
        {
            EMPLOYEENAME = data.NAME;
            EMPLOYEE_ID = data.EMPLOYEE_ID;
            LoadMonth(data);
            //Chức năng của BackCommand
            BackCommand = new RelayCommand<ContentControl>(p => { return true; },
            p => { p.Content = new uConReport(EMPLOYEE_ID); });
            showDetailCommand = new RelayCommand<ContentControl>(p => { return true; }, p => { p.Content = new uConTimekeepingDetail(EMPLOYEE_ID, SELECTEDITEM); });
        }
        private string _EMPLOYEENAME;
        public string EMPLOYEENAME { get => _EMPLOYEENAME; set { _EMPLOYEENAME = value; OnPropertyChanged(); } }

        private int _EMPLOYEE_ID;
        public int EMPLOYEE_ID { get => _EMPLOYEE_ID; set { _EMPLOYEE_ID = value; OnPropertyChanged(); } }
        public ICommand BackCommand { get; set; }
        private TimekeepingData _SELECTEDITEM;
        public TimekeepingData SELECTEDITEM
        {
            get => _SELECTEDITEM; set
            {
                _SELECTEDITEM = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> _workdayList { get; set; }
        public ObservableCollection<string> workdayList
        {
            get { return _workdayList; }
            set
            {
                _workdayList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> _absentdayList { get; set; }
        public ObservableCollection<string> absentdayList
        {
            get
            {
                return _absentdayList;
            }
            set
            {
                _absentdayList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> _overtimeList { get; set; }
        public ObservableCollection<string> overtimeList
        {
            get
            {
                return _overtimeList;
            }
            set
            {
                _overtimeList = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ComboboxModel> _MONTHDETAILLIST;
        public ObservableCollection<ComboboxModel> MONTHDETAILLIST { get => _MONTHDETAILLIST; set { _MONTHDETAILLIST = value; OnPropertyChanged(); } }
        private ComboboxModel _SELECTDETAILMONTHTYPE;
        public ComboboxModel SELECTDETAILMONTHTYPE
        {
            get => _SELECTDETAILMONTHTYPE; set
            {
                _SELECTDETAILMONTHTYPE = value;
                OnPropertyChanged();

                //Nếu selected khác null, nghĩa là tháng đã chọn thì show data theo select dó
                if (SELECTDETAILMONTHTYPE != null)
                {
                    LoadDetail(EMPLOYEE_ID, SELECTDETAILMONTHTYPE.MONTH, SELECTDETAILMONTHTYPE.YEAR);
                }
            }
        }
        private void LoadMonth(TimekeepingData data)
        {
            hrmsEntities db = new hrmsEntities();

            //Chọn tháng từ database KHÔNG TRÙNG LẶP (chọn DATE_START và DATE_END để kiểm tra tháng bắt đầu và tháng kết thúc có hợp lệ không (nếu cách nhau không quá 31 ngày hợp lệ)
            var listmonth = (from month in db.TIMEKEEPINGs
                             select new { Month = month.MONTH }).Distinct();

            //Khởi tạo biến MONTHLIST để chứa tháng
            MONTHDETAILLIST = new ObservableCollection<ComboboxModel>();
            //Đưa dữ liệu từ listmonth vào MONTHLIST
            foreach (var item in listmonth)
            {
                MONTHDETAILLIST.Add(new ComboboxModel(item.Month.Value.Month, item.Month.Value.Year, false));
            }
            SELECTDETAILMONTHTYPE = MONTHDETAILLIST.Where(x => x.MONTH == data.MONTH.Month && x.YEAR == data.MONTH.Year).First();
        }
        private void LoadDetail(int id, int month, int year)
        {
            hrmsEntities db = new hrmsEntities();
            var GetWorkdayList = ((from t in db.TIMEKEEPING_DETAIL
                                   where t.TIMEKEEPING.MONTH.Value.Month == month &&
                                   t.TIMEKEEPING.MONTH.Value.Year == year &&
                                   (t.TIMEKEEPING_DETAIL_TYPE == 1 || t.TIMEKEEPING_DETAIL_TYPE == 2) &&
                                   t.EMPLOYEE_ID == id
                                   orderby t.CHECK_DATE.Value.Day ascending
                                   select t).Distinct());
            workdayList = new ObservableCollection<string>();
            absentdayList = new ObservableCollection<string>();
            overtimeList = new ObservableCollection<string>();
            foreach (var item in GetWorkdayList)
            {
                string Session = "";
                if (item != null)
                {
                    if (item.SESSION == 1)
                    {
                        Session = "Morning";
                        workdayList.Add(item.CHECK_DATE.Value.Day + "/" + item.CHECK_DATE.Value.Month + "/" + item.CHECK_DATE.Value.Year + "(" + Session + ")");
                    }
                    else
                    {
                        Session = "Afternoon";
                        workdayList.Add(item.CHECK_DATE.Value.Day + "/" + item.CHECK_DATE.Value.Month + "/" + item.CHECK_DATE.Value.Year + "(" + Session + ")");
                    }
                }
            }

            var GetAbsentdayList = ((from t in db.TIMEKEEPING_DETAIL
                                     where t.TIMEKEEPING.MONTH.Value.Month == month &&
                                   t.TIMEKEEPING.MONTH.Value.Year == year && t.TIMEKEEPING_DETAIL_TYPE == 0
                                     && t.EMPLOYEE_ID == id
                                     select t).Distinct());
            foreach (var item in GetAbsentdayList)
            {
                string Session = "";
                if (item != null)
                {
                    if (item.SESSION == 1)
                    {
                        Session = "Morning";
                        absentdayList.Add(item.CHECK_DATE.Value.Day + "/" + item.CHECK_DATE.Value.Month + "/" + item.CHECK_DATE.Value.Year + "(" + Session + ")");
                    }
                    else
                    {
                        Session = "Afternoon";
                        absentdayList.Add(item.CHECK_DATE.Value.Day + "/" + item.CHECK_DATE.Value.Month + "/" + item.CHECK_DATE.Value.Year + "(" + Session + ")");
                    }
                }

            }

            var GetOvertimeList = ((from t in db.TIMEKEEPING_DETAIL
                                    where t.TIMEKEEPING.MONTH.Value.Month == month &&
                                   t.TIMEKEEPING.MONTH.Value.Year == year && t.TIMEKEEPING_DETAIL_TYPE == 3
                                    && t.EMPLOYEE_ID == id
                                    select t).Distinct());
            foreach (var item in GetOvertimeList)
            {
                if (item != null)
                {
                    string Session = "";
                    if (item.SESSION == 1)
                    {
                        Session = "Morning";
                        overtimeList.Add(item.CHECK_DATE.Value.Day + "/" + item.CHECK_DATE.Value.Month + "/" + item.CHECK_DATE.Value.Year + "(" + Session + ")");
                    }
                    else
                    {
                        Session = "Afternoon";
                        overtimeList.Add(item.CHECK_DATE.Value.Day + "/" + item.CHECK_DATE.Value.Month + "/" + item.CHECK_DATE.Value.Year + "(" + Session + ")");
                    }
                }
            }
        }
        #endregion
    }
}
