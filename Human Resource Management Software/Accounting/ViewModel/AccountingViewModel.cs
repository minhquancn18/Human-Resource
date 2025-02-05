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
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Collections;
using System.Windows.Controls.Primitives;
using HRMS.Accouting.Model;
using ClosedXML.Excel;
using System.Data;
using HRMS.HR.ViewModel;
using HRMS.HR.Model.Database;
using HRMS.Accounting.View;

namespace HRMS.Accouting.ViewModel
{
    using ButtonContent = Tuple<TextBlock, PackIcon>;
    public class AccountingViewModel : BaseViewModel
    {

        #region Command
        //Chuyển content từ ListEmployee sang DeatilSalaryEmployee
        public ICommand showEmployeeCommand { get; set; }
        //Để lưu những thay đổi trong DetailSalaryEmployee
        public ICommand EditCommand { get; set; }
        //Để thực chức năng back trong DetailSalaryEmployee
        public ICommand BackCommand { get; set; }
        //Để thêm hình ảnh
        public ICommand AddImageCommand { get; set; }
        //Export to PDF
        public ICommand ExportPDFCommand { get; set; }
        //Export to Excel
        public ICommand ExportExcelCommand { get; set; }
        #endregion

        #region Data Binding Salary List 
        //Binding tới tài khoản hiện tại

        //Binding lấy thông tin USER để hiện thị cập nhật thay đổi và xuất PDF
        private int _USER_ID;
        public int USER_ID { get => _USER_ID; set { _USER_ID = value; OnPropertyChanged(); } }

        private string _USER_NAME;
        public string USER_NAME { get => _USER_NAME; set { _USER_NAME = value; OnPropertyChanged(); } }

        private int _USER_DEPT;
        public int USER_DEPT { get => _USER_DEPT; set { _USER_DEPT = value; OnPropertyChanged(); } }

        private int _USER_ROLE;
        public int USER_ROLE { get => _USER_ROLE; set { _USER_ROLE = value; OnPropertyChanged(); } }


        //Binding tới datagrid của Salary List
        private ObservableCollection<SalaryInformationData> _SalaryList;
        public ObservableCollection<SalaryInformationData> SalaryList { get => _SalaryList; set { _SalaryList = value; OnPropertyChanged(); } }

        //Để lưu trữ bản sao để có thể sao chép khi cần thiết
        private ObservableCollection<SalaryInformationData> _SalaryTest;
        public ObservableCollection<SalaryInformationData> SalaryTest { get => _SalaryTest; set { _SalaryTest = value; OnPropertyChanged(); } }

        //Binding tới datagrid selected trong Salary list
        private SalaryInformationData _SelectedItem;
        public SalaryInformationData SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                //Đưa dữ liệu khi double-left vào 1 row bất kì trong SalaryDetailEmployee
                if (SelectedItem != null)
                {
                    SOCIAL_INSURANCE = (long)SelectedItem.SOCIALINSURANCE;
                    HEALTH_INSURANCE = (int)SelectedItem.HEALTHINSURANCE;
                    OVERTIME_SALARY = (long)SelectedItem.OVERTIMESALARY;
                    BONUS = (long)SelectedItem.BONUS;
                    WELFARE = (long)SelectedItem.WELFARE;
                    TAX = (long)SelectedItem.TAX;
                    EMPLOYEE_ID = SelectedItem.EMPLOYEE_ID;
                    EMPLOYEE_NAME = SelectedItem.NAME;
                    DEPARTMENT_NAME = SelectedItem.DEPARTMENT;
                    ROLE_NAME = SelectedItem.ROLE;
                    BASIC_WAGE = (long)SelectedItem.BASICWAGE;
                    COEFFICIENT = SelectedItem.COEFFICIENT;
                    TOTAL_SALARY = SelectedItem.TOTALSALARY;
                    NOTE = SelectedItem.NOTE;
                    COFFEICENT_STRING = COEFFICIENT.ToString();
                    EMPLOYEE emp = HRMSEntities.Ins.DB.EMPLOYEEs.Where(x => x.EMPLOYEE_ID == SelectedItem.EMPLOYEE_ID).FirstOrDefault();
                    IMAGESOURCE = emp.IMAGE;
                    if (IMAGESOURCE == null)
                    {
                        BUTTONTHICKNESS = 1;
                        IMAGE_SOURCE = null;
                        BRUSH = Brushes.AliceBlue;
                    }
                    else
                    {
                        BUTTONTHICKNESS = 0;
                        IMAGE_SOURCE = AccountingClass.ToImage(IMAGESOURCE);
                        BRUSH = Brushes.Transparent;
                    }
                }
            }
        }

        //Binding dữ liệu vào combobox của chọn loại để lọc
        private ObservableCollection<ComboboxModel> _ListType;
        public ObservableCollection<ComboboxModel> ListType { get => _ListType; set { _ListType = value; OnPropertyChanged(); } }

        //Binding dữ liệu với select trong comboox chọn loại lọc
        private ComboboxModel _SELECTEDTYPE;
        public ComboboxModel SELECTEDTYPE { get => _SELECTEDTYPE; set { _SELECTEDTYPE = value; OnPropertyChanged(); SEARCH_TEXT = ""; } }

        //Binding dữ liệu vào combobox department của chọn loại để lọc
        private ObservableCollection<ComboboxModel> _ListDeptType;
        public ObservableCollection<ComboboxModel> ListDeptType { get => _ListDeptType; set { _ListDeptType = value; OnPropertyChanged(); } }

        //Binding dữ liệu với select trong comboox chọn loại lọc
        private ComboboxModel _SELECTEDDEPTTYPE;
        public ComboboxModel SELECTEDDEPTTYPE
        {
            get => _SELECTEDDEPTTYPE;
            set
            {
                _SELECTEDDEPTTYPE = value;
                OnPropertyChanged();

                //Trả search text về mặc định
                SEARCH_TEXT = "";

                if (SELECTEDDEPTTYPE != null)
                    LoadSalaryData();
            }
        }

        //Binding dữ liệu với Search Text
        private string _SEARCH_TEXT;
        public string SEARCH_TEXT
        {
            get => _SEARCH_TEXT; set
            {
                _SEARCH_TEXT = value;
                OnPropertyChanged();

                //Đưa SalaryTest vào trong SalaryList để dữ liệu được refresh mỗi lần nhập
                SalaryList = SalaryTest;

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
                                SalaryList = new ObservableCollection<SalaryInformationData>(SalaryList.Where(x => x.EMPLOYEE_ID.ToString().Contains(SEARCH_TEXT)));
                                break;

                            //Lọc theo tên
                            case "NAME":
                                SalaryList = new ObservableCollection<SalaryInformationData>(SalaryList.Where(x => x.NAME.Contains(SEARCH_TEXT) ||
                                                                                                        x.NAME.ToLower().Contains(SEARCH_TEXT) ||
                                                                                                        x.NAME.ToUpper().Contains(SEARCH_TEXT)));
                                break;

                            //Lọc theo Department
                            case "DEPARTMENT":
                                SalaryList = new ObservableCollection<SalaryInformationData>(SalaryList.Where(x => x.DEPARTMENT.Contains(SEARCH_TEXT) ||
                                                                                                        x.DEPARTMENT.ToLower().Contains(SEARCH_TEXT) ||
                                                                                                        x.DEPARTMENT.ToUpper().Contains(SEARCH_TEXT)));
                                break;

                            //Lọc theo role
                            case "ROLE":
                                SalaryList = new ObservableCollection<SalaryInformationData>(SalaryList.Where(x => x.ROLE.Contains(SEARCH_TEXT) ||
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
                //Trả search text về mặc định
                SEARCH_TEXT = "";
                //Nếu selected khác null, nghĩa là tháng đã chọn thì show data theo select dó
                if (SELECTMONTHTYPE != null)
                {
                    LoadSalaryData();
                }
            }
        }
        #endregion

        #region Data Binding SalaryDetailEmployee
        //Toàn bộ dữ liệu binding trong SalaryDetailEmployee

        private long _SOCIAL_INSURANCE;
        public long SOCIAL_INSURANCE { get => _SOCIAL_INSURANCE; set { _SOCIAL_INSURANCE = value; OnPropertyChanged(); TOTAL_SALARY = AccountingClass.CalculateSalary(SOCIAL_INSURANCE, HEALTH_INSURANCE, TAX, BONUS, WELFARE, BASIC_WAGE, OVERTIME_SALARY, COEFFICIENT, SelectedItem.WORKDAY, SelectedItem.OVERTIMEDAY, SelectedItem.DATESTART.Month, SelectedItem.DATESTART.Year); } }

        private long _OVERTIME_SALARY;
        public long OVERTIME_SALARY { get => _OVERTIME_SALARY; set { _OVERTIME_SALARY = value; OnPropertyChanged(); TOTAL_SALARY = AccountingClass.CalculateSalary(SOCIAL_INSURANCE, HEALTH_INSURANCE, TAX, BONUS, WELFARE, BASIC_WAGE, OVERTIME_SALARY, COEFFICIENT, SelectedItem.WORKDAY, SelectedItem.OVERTIMEDAY, SelectedItem.DATESTART.Month, SelectedItem.DATESTART.Year); } }

        private long _HEALTH_INSURANCE;
        public long HEALTH_INSURANCE { get => _HEALTH_INSURANCE; set { _HEALTH_INSURANCE = value; OnPropertyChanged(); TOTAL_SALARY = AccountingClass.CalculateSalary(SOCIAL_INSURANCE, HEALTH_INSURANCE, TAX, BONUS, WELFARE, BASIC_WAGE, OVERTIME_SALARY, COEFFICIENT, SelectedItem.WORKDAY, SelectedItem.OVERTIMEDAY, SelectedItem.DATESTART.Month, SelectedItem.DATESTART.Year); } }

        private long _BONUS;
        public long BONUS { get => _BONUS; set { _BONUS = value; OnPropertyChanged(); TOTAL_SALARY = AccountingClass.CalculateSalary(SOCIAL_INSURANCE, HEALTH_INSURANCE, TAX, BONUS, WELFARE, BASIC_WAGE, OVERTIME_SALARY, COEFFICIENT, SelectedItem.WORKDAY, SelectedItem.OVERTIMEDAY, SelectedItem.DATESTART.Month, SelectedItem.DATESTART.Year); } }

        private long _BASIC_WAGE;
        public long BASIC_WAGE { get => _BASIC_WAGE; set { _BASIC_WAGE = value; OnPropertyChanged(); TOTAL_SALARY = AccountingClass.CalculateSalary(SOCIAL_INSURANCE, HEALTH_INSURANCE, TAX, BONUS, WELFARE, BASIC_WAGE, OVERTIME_SALARY, COEFFICIENT, SelectedItem.WORKDAY, SelectedItem.OVERTIMEDAY, SelectedItem.DATESTART.Month, SelectedItem.DATESTART.Year); } }

        private long _WELFARE;
        public long WELFARE { get => _WELFARE; set { _WELFARE = value; OnPropertyChanged(); TOTAL_SALARY = AccountingClass.CalculateSalary(SOCIAL_INSURANCE, HEALTH_INSURANCE, TAX, BONUS, WELFARE, BASIC_WAGE, OVERTIME_SALARY, COEFFICIENT, SelectedItem.WORKDAY, SelectedItem.OVERTIMEDAY, SelectedItem.DATESTART.Month, SelectedItem.DATESTART.Year); } }

        private double _COEFFICIENT;
        public double COEFFICIENT { get => _COEFFICIENT; set { _COEFFICIENT = value; OnPropertyChanged(); TOTAL_SALARY = AccountingClass.CalculateSalary(SOCIAL_INSURANCE, HEALTH_INSURANCE, TAX, BONUS, WELFARE, BASIC_WAGE, OVERTIME_SALARY, COEFFICIENT, SelectedItem.WORKDAY, SelectedItem.OVERTIMEDAY, SelectedItem.DATESTART.Month, SelectedItem.DATESTART.Year); } }

        private long _TAX;
        public long TAX { get => _TAX; set { _TAX = value; OnPropertyChanged(); TOTAL_SALARY = AccountingClass.CalculateSalary(SOCIAL_INSURANCE, HEALTH_INSURANCE, TAX, BONUS, WELFARE, BASIC_WAGE, OVERTIME_SALARY, COEFFICIENT, SelectedItem.WORKDAY, SelectedItem.OVERTIMEDAY, SelectedItem.DATESTART.Month, SelectedItem.DATESTART.Year); } }

        private int _EMPLOYEE_ID;
        public int EMPLOYEE_ID { get => _EMPLOYEE_ID; set { _EMPLOYEE_ID = value; OnPropertyChanged(); } }

        private string _EMPLOYEE_NAME;
        public string EMPLOYEE_NAME { get => _EMPLOYEE_NAME; set { _EMPLOYEE_NAME = value; OnPropertyChanged(); } }

        private string _DEPARTMENT_NAME;
        public string DEPARTMENT_NAME { get => _DEPARTMENT_NAME; set { _DEPARTMENT_NAME = value; OnPropertyChanged(); } }

        private string _ROLE_NAME;
        public string ROLE_NAME { get => _ROLE_NAME; set { _ROLE_NAME = value; OnPropertyChanged(); } }

        private string _NOTE;
        public string NOTE { get => _NOTE; set { _NOTE = value; OnPropertyChanged(); } }

        private long _TOTAL_SALARY;
        public long TOTAL_SALARY { get => _TOTAL_SALARY; set { _TOTAL_SALARY = value; OnPropertyChanged(); } }

        private string _COFFEICENT_STRING;
        public string COFFEICENT_STRING
        {
            get => _COFFEICENT_STRING;
            set
            {
                _COFFEICENT_STRING = value;
                OnPropertyChanged();
                double number;
                bool success = double.TryParse(COFFEICENT_STRING, out number);
                if (success)
                {
                    COEFFICIENT = number;
                }
            }
        }

        private byte[] _IMAGESOURCE;
        public byte[] IMAGESOURCE { get => _IMAGESOURCE; set { _IMAGESOURCE = value; OnPropertyChanged(); } }

        private BitmapImage _IMAGE_SOURCE;
        public BitmapImage IMAGE_SOURCE { get => _IMAGE_SOURCE; set { _IMAGE_SOURCE = value; OnPropertyChanged(); } }

        private Brush _BRUSH;
        public Brush BRUSH { get => _BRUSH; set { _BRUSH = value; OnPropertyChanged(); } }

        private int _BUTTONTHICKNESS;
        public int BUTTONTHICKNESS { get => _BUTTONTHICKNESS; set { _BUTTONTHICKNESS = value; OnPropertyChanged(); } }
        #endregion

        #region Constructor
        //Constructor mặc định
        public AccountingViewModel(int ID)
        {
            LoadCommandList(ID);
        }
        //Constructor để hiển thị chi tiết lương
        public AccountingViewModel(SalaryInformationData data, int ID)
        {
            SelectedItem = data;
            LoadCommandDetail(ID);
        }
        //Constructor để hiển thị chi tiết lương khi nhấn vào record
        public AccountingViewModel(int idSelect, int idUser, int month, int year)
        {
            SelectedItem = LoadSelectedData(idSelect, month, year);
            LoadCommandDetail(idUser);
        }
        #endregion

        //Các command trong User Control hiển thị chi tiết lương
        private void LoadCommandDetail(int ID)
        {
            LoadUser(ID);
            //Chức năng của EditCommand
            EditCommand = new RelayCommand<object>(p => IsEditSalaryData(), p => EditSalaryData());

            //Chức năng của BackCommand
            BackCommand = new RelayCommand<ContentControl>(p => { return true; },
                p => { p.Content = new uConListEmployeeAccounting(ID); });

            //Chức năng add ảnh
            AddImageCommand = new RelayCommand<object>(p => IsAddImageData(), p => AddImageData());
        }

        //Tìm thông tin của Selected trong record thay đổi để binding tới chi tiết lương
        private SalaryInformationData LoadSelectedData(int ID, int month, int year)
        {
            SalaryInformationData data = new SalaryInformationData();
            hrmsEntities DB = new hrmsEntities();
            var item = (from emp in DB.EMPLOYEEs
                        join tk in DB.TIMEKEEPINGs on emp.EMPLOYEE_ID equals tk.EMPLOYEE_ID into temp1
                        from tk in temp1.DefaultIfEmpty()
                        join sl in DB.SALARies on emp.EMPLOYEE_ID equals sl.EMPLOYEE_ID into temp2
                        from sl in temp2.DefaultIfEmpty()
                        where sl.DATE_START.Value.Month == month && sl.DATE_START.Value.Year == year &&
                             tk.DATE_START.Value.Month == month && tk.DATE_START.Value.Year == year &&
                             emp.EMPLOYEE_ID == ID
                        select new
                        {
                            id = emp.EMPLOYEE_ID,
                            month_start = sl.DATE_START.Value.Month,
                            year_start = sl.DATE_START.Value.Year,
                            EMPLOYEE = emp,
                            TIMEKEEPING = tk,
                            SALARY = sl
                        }).SingleOrDefault();

            data.EMPLOYEE_ID = item.id;
            data.NAME = item.EMPLOYEE.NAME;
            data.DEPARTMENT = item.EMPLOYEE.DEPARTMENT.DEPT_NAME;
            data.ROLE = item.EMPLOYEE.ROLE.ROLE_NAME;
            data.BASICWAGE = (long)item.SALARY.BASIC_WAGE;
            data.WORKDAY = (int)item.TIMEKEEPING.NUMBER_OF_WORK_DAY;
            data.OVERTIMEDAY = (int)item.TIMEKEEPING.NUMBER_OF_OVERTIME_DAY;
            data.OVERTIMESALARY = (long)item.SALARY.OVERTIME_SALARY;
            data.TOTALSALARY = (long)item.SALARY.TOTAL_SALARY;
            data.SOCIALINSURANCE = (long)item.SALARY.SOCIAL_INSURANCE;
            data.HEALTHINSURANCE = (long)item.SALARY.HEALTH_INSURANCE;
            data.TAX = (long)item.SALARY.TAX;
            data.WELFARE = (long)item.SALARY.WELFARE;
            data.BONUS = (long)item.SALARY.BONUS;
            data.COEFFICIENT = (double)item.SALARY.COEFFICIENT;
            data.DATESTART = (DateTime)item.SALARY.DATE_START;
            data.DATEEND = (DateTime)item.SALARY.DATE_END;
            data.NOTE = item.SALARY.NOTE;

            return data;
        }

        //Các command trong User Control hiển thị trong list lương
        private void LoadCommandList(int ID)
        {
            #region Load Data khi mỗi khi truy cập tới view
            LoadComboboxTypeDeptList();
            LoadMonth();
            LoadComboboxTypeList();
            LoadUser(ID);
            #endregion

            //Chức năng của showEmployeeCommand
            showEmployeeCommand = new RelayCommand<ContentControl>(p => {
                if (SelectedItem != null)
                { return true; }
                else
                { return false; }
            },
                p =>
                { p.Content = new uConEmployeeSalary(SelectedItem, ID); });

            //Chức năng export to pdf
            ExportPDFCommand = new RelayCommand<DataGrid>(p => IsExportCommand(), p => ExportPDF(p, USER_NAME));

            //Chức năng export to excel
            ExportExcelCommand = new RelayCommand<DataGrid>(p => IsExportCommand(), p => ExportExcel(p));
        }

        //Load user
        private void LoadUser(int ID)
        {
            var emp = HRMSEntities.Ins.DB.EMPLOYEEs.Where(x => x.EMPLOYEE_ID == ID).SingleOrDefault();
            USER_ID = ID;
            USER_NAME = emp.NAME;
            USER_ROLE = (int)emp.ROLE.PERMISSION;
            USER_DEPT = (int)emp.DEPT_ID;
        }

        //Load data từ database vào datagrid trong EmployeeList
        private void LoadSalaryData()
        {
            //Kiểm tra selected comboBox chọn tháng có khác null không 
            if (SELECTMONTHTYPE == null)
            {
                return;
            }
            //Kiểm tra selected của comboBox chọn phòng ban có khác null không
            if (SELECTEDDEPTTYPE == null)
            {
                return;
            }

            //Tạo list chứa dữ liệu có lọc theo tháng
            hrmsEntities DB = new hrmsEntities();

            //Tạo ra list lưu dữ liệu các nhân viên được lọc theo phòng ban (hiện thị tất cả thì trả về true)
            var list_filter_dept = from emp in DB.EMPLOYEEs where SELECTEDDEPTTYPE.DEPT_ID > 0 ? emp.DEPT_ID == SELECTEDDEPTTYPE.DEPT_ID : true select emp;

            //Tạo ra list temp lưu dữ liệu các nhân viên nào chưa bị xóa nằm trong phòng ban đã lọc (nếu tháng được chọn, lúc đó nhân viên chưa bị xóa thì vẫn hiện)
            var list_temp = (from emp in list_filter_dept
                             join del in DB.DELETEs on emp.EMPLOYEE_ID equals del.EMPLOYEE_ID
                             where (del.ISDELETED == false) || (del.ISDELETED == true &&
                             (del.MONTH.Value.Year > SELECTMONTHTYPE.YEAR || (del.MONTH.Value.Month > SELECTMONTHTYPE.MONTH && del.MONTH.Value.Year == SELECTMONTHTYPE.YEAR)))
                             select emp).Distinct();

            //Tạo ra list lưu dữ liệu lương dựa vào nhân viên đã được lọc theo các điều kiện trên (lọc thoe phòng ban và hiển thị thông tin của nhân viên chưa bị xóa)
            var list = (from emp in list_temp
                        join tk in DB.TIMEKEEPINGs on emp.EMPLOYEE_ID equals tk.EMPLOYEE_ID into temp1
                        from tk in temp1.DefaultIfEmpty()
                        join sl in DB.SALARies on emp.EMPLOYEE_ID equals sl.EMPLOYEE_ID into temp2
                        from sl in temp2.DefaultIfEmpty()
                        where sl.MONTH.Value.Month == SELECTMONTHTYPE.MONTH && sl.MONTH.Value.Year == SELECTMONTHTYPE.YEAR &&
                            tk.MONTH.Value.Month == SELECTMONTHTYPE.MONTH && tk.MONTH.Value.Year == SELECTMONTHTYPE.YEAR
                        select new
                        {
                            id = emp.EMPLOYEE_ID,
                            month_start = sl.DATE_START.Value.Month,
                            year_start = sl.DATE_START.Value.Year,
                            EMPLOYEE = emp,
                            TIMEKEEPING = tk,
                            SALARY = sl
                        }).Distinct();

            //Kiểm tra xem list có null không (trong điều kiện qua tháng mới thì chưa có dữ liệu cần phải tạo tháng mới)
            if (list != null && list.Count() == 0)
            {
                //Kiểm tra xem tháng hiện tại có nhân viên nào bị xóa không
                list_temp = from emp in list_filter_dept
                            join del in DB.DELETEs on emp.EMPLOYEE_ID equals del.EMPLOYEE_ID
                            where ((del.ISDELETED == false) || (del.ISDELETED == true &&
                            (del.MONTH.Value.Year > SELECTMONTHTYPE.YEAR || (del.MONTH.Value.Month > SELECTMONTHTYPE.MONTH && del.MONTH.Value.Year == SELECTMONTHTYPE.YEAR))))
                            select emp;

                //Tạo ra 2 list (1 để binding tới datagrid và 1 cái bản sao)
                SalaryList = new ObservableCollection<SalaryInformationData>();
                SalaryTest = new ObservableCollection<SalaryInformationData>();

                //Tạo ra bảng lương tháng mới lấy basic_wage, overtime_salary, coefficient của tháng cũ trong trường hợp nhân viên cũ
                foreach (var item in list_temp)
                {
                    SalaryInformationData salaryData = new SalaryInformationData();

                    //Tìm tháng trước đó để lấy dữ liệu nói trên
                    DateTime date = AccountingClass.GetDateBefore();

                    //Tìm bảng lương tháng trước của nhân viên đó 
                    SALARY old_salary = DB.SALARies.Where(x => x.EMPLOYEE_ID == item.EMPLOYEE_ID &&
                    x.MONTH.Value.Month == date.Month && x.MONTH.Value.Year == date.Year).SingleOrDefault();

                    //Kiểm tra điều kiện nếu nhân viên đó là nhân viên mới
                    if (old_salary == null)
                    {
                        //Tìm bảng lương hiện tại (không cần thiết lắm vì đó là điều kiện lọc
                        SALARY new_salary = DB.SALARies.Where(x => x.EMPLOYEE_ID == item.EMPLOYEE_ID &&
                             x.MONTH.Value.Month == DateTime.Now.Month && x.MONTH.Value.Year == DateTime.Now.Year).SingleOrDefault();
                        salaryData.BASICWAGE = (long)new_salary.BASIC_WAGE;
                        salaryData.OVERTIMESALARY = (long)new_salary.OVERTIME_SALARY;
                        salaryData.COEFFICIENT = (double)new_salary.COEFFICIENT;
                    }
                    else
                    {
                        salaryData.BASICWAGE = (long)old_salary.BASIC_WAGE;
                        salaryData.OVERTIMESALARY = (long)old_salary.OVERTIME_SALARY;
                        salaryData.COEFFICIENT = (double)old_salary.COEFFICIENT;
                    }
                    //Binding dữ liệu
                    salaryData.EMPLOYEE_ID = item.EMPLOYEE_ID;
                    salaryData.NAME = item.NAME;
                    salaryData.DEPARTMENT = item.DEPARTMENT.DEPT_NAME;
                    salaryData.ROLE = item.ROLE.ROLE_NAME;
                    salaryData.WORKDAY = 0;
                    salaryData.OVERTIMEDAY = 0;
                    salaryData.TOTALSALARY = 0;
                    salaryData.SOCIALINSURANCE = 0;
                    salaryData.HEALTHINSURANCE = 0;
                    salaryData.TAX = 0;
                    salaryData.WELFARE = 0;
                    salaryData.BONUS = 0;
                    salaryData.NOTE = "";
                    salaryData.DATESTART = new DateTime(SELECTMONTHTYPE.YEAR, SELECTMONTHTYPE.MONTH, 1);
                    salaryData.DATEEND = new DateTime(SELECTMONTHTYPE.YEAR, SELECTMONTHTYPE.MONTH, AccountingClass.GetDaybyMonth(SELECTMONTHTYPE.MONTH, SELECTMONTHTYPE.YEAR));
                    salaryData.MONTH = new DateTime(SELECTMONTHTYPE.YEAR, SELECTMONTHTYPE.MONTH, 1);

                    //Add dữ liệu vào list
                    SalaryList.Add(salaryData);
                    SalaryTest.Add(salaryData);

                    //Tạo ra 1 bảng tương tự để add dữ liệu vào database
                    SALARY salary = new SALARY();
                    salary.EMPLOYEE_ID = item.EMPLOYEE_ID;
                    salary.BASIC_WAGE = (long)old_salary.BASIC_WAGE;
                    salary.COEFFICIENT = (double)old_salary.COEFFICIENT;
                    salary.OVERTIME_SALARY = (long)old_salary.OVERTIME_SALARY;
                    salary.DATE_START = new DateTime(SELECTMONTHTYPE.YEAR, SELECTMONTHTYPE.MONTH, 1);
                    salary.DATE_END = new DateTime(SELECTMONTHTYPE.YEAR, SELECTMONTHTYPE.MONTH, AccountingClass.GetDaybyMonth(SELECTMONTHTYPE.MONTH, SELECTMONTHTYPE.YEAR));
                    salary.MONTH = new DateTime(SELECTMONTHTYPE.YEAR, SELECTMONTHTYPE.MONTH, 1);
                    salary.BONUS = 0;
                    salary.HEALTH_INSURANCE = 0;
                    salary.SOCIAL_INSURANCE = 0;
                    salary.TAX = 0;
                    salary.WELFARE = 0;
                    salary.TOTAL_SALARY = 0;
                    salary.NOTE = "";
                    DB.SALARies.Add(salary);
                }

            }
            else
            {
                //lưu dữ liệu từ list vào 2 cái biến vừa khởi tạo
                foreach (var item in list)
                {
                    item.SALARY.TOTAL_SALARY = AccountingClass.CalculateSalary(item.SALARY, item.TIMEKEEPING);
                }
                HRMSEntities.Ins.DB.SaveChanges();

                //Khởi tạo 2 biến lưu dữ liệu từ list ở trên (1 cái binding tới datagrid và 1 cái bản sao)
                SalaryList = new ObservableCollection<SalaryInformationData>();
                SalaryTest = new ObservableCollection<SalaryInformationData>();

                //Lưu dữ liệu từ list đã lọc ở trên vào 2 list vừa khởi tạo 
                foreach (var item in list)
                {
                    SalaryInformationData salaryData = new SalaryInformationData();

                    salaryData.EMPLOYEE_ID = item.id;
                    salaryData.NAME = item.EMPLOYEE.NAME;
                    salaryData.DEPARTMENT = item.EMPLOYEE.DEPARTMENT.DEPT_NAME;
                    salaryData.ROLE = item.EMPLOYEE.ROLE.ROLE_NAME;
                    salaryData.BASICWAGE = (long)item.SALARY.BASIC_WAGE;
                    salaryData.WORKDAY = (int)item.TIMEKEEPING.NUMBER_OF_WORK_DAY;
                    salaryData.OVERTIMEDAY = (int)item.TIMEKEEPING.NUMBER_OF_OVERTIME_DAY;
                    salaryData.OVERTIMESALARY = (long)item.SALARY.OVERTIME_SALARY;
                    salaryData.TOTALSALARY = (long)item.SALARY.TOTAL_SALARY;
                    salaryData.SOCIALINSURANCE = (long)item.SALARY.SOCIAL_INSURANCE;
                    salaryData.HEALTHINSURANCE = (long)item.SALARY.HEALTH_INSURANCE;
                    salaryData.TAX = (long)item.SALARY.TAX;
                    salaryData.WELFARE = (long)item.SALARY.WELFARE;
                    salaryData.BONUS = (long)item.SALARY.BONUS;
                    salaryData.COEFFICIENT = (double)item.SALARY.COEFFICIENT;
                    salaryData.DATESTART = (DateTime)item.SALARY.DATE_START;
                    salaryData.MONTH = (DateTime)item.SALARY.MONTH;
                    salaryData.DATEEND = (DateTime)item.SALARY.DATE_END;
                    salaryData.NOTE = item.SALARY.NOTE;

                    SalaryList.Add(salaryData);
                    SalaryTest.Add(salaryData);
                }
            }
            DB.SaveChanges();
        }

        //Load dữ liệu chọn loại vào comboBox chọn loại để lọc (có thể thêm chọn loại mới vào đây)        
        private void LoadComboboxTypeList()
        {
            ListType = new ObservableCollection<ComboboxModel>();
            ListType.Add(new ComboboxModel("ID", true));
            ListType.Add(new ComboboxModel("NAME", false));
            ListType.Add(new ComboboxModel("DEPARTMENT", false));
            ListType.Add(new ComboboxModel("ROLE", false));
            SELECTEDTYPE = ListType.Where(x => x.ISSELECTED == true).FirstOrDefault();
        }

        //Load dữ liệu chọn loại vào comboBox chọn dept để lọc (có thể thêm chọn loại mới vào đây)        
        private void LoadComboboxTypeDeptList()
        {
            hrmsEntities DB = new hrmsEntities();
            var list = from dept in DB.DEPARTMENTs select dept;
            ListDeptType = new ObservableCollection<ComboboxModel>();

            //ALL để chọn lọc theo tất cả phòng ban
            ListDeptType.Add(new ComboboxModel("ALL", 0, true));

            foreach (var item in list)
            {
                ListDeptType.Add(new ComboboxModel(item.DEPT_NAME, item.DEPT_ID, false));
            }
            SELECTEDDEPTTYPE = ListDeptType.Where(x => x.ISSELECTED == true).FirstOrDefault();
        }

        //Load dữ liệu tháng vào comboBox Month
        private void LoadMonth()
        {
            //Chọn tháng từ database KHÔNG TRÙNG LẶP (chọn DATE_START và DATE_END để kiểm tra tháng bắt đầu và tháng kết thúc có hợp lệ không (nếu cách nhau không quá 31 ngày hợp lệ)
            var listmonth = (from month in HRMSEntities.Ins.DB.SALARies
                             orderby month.MONTH descending
                             select new { Month = month.MONTH }).Distinct();

            //Khởi tạo biến MONTHLIST để chứa tháng
            MONTHLIST = new ObservableCollection<ComboboxModel>();

            bool isMonthNow = false;
            //Đưa dữ liệu từ listmonth vào MONTHLIST
            foreach (var item in listmonth)
            {
                DateTime date = (DateTime)item.Month;
                if (date.Month == DateTime.Now.Month && date.Year == DateTime.Now.Year)
                    isMonthNow = true;
                //Nếu điều kiện hợp lệ thì lưu dữ liệu vào ComboBox Month thông qua MONTHLIST
                MONTHLIST.Add(new ComboboxModel(date.Month, date.Year, (date.Month == DateTime.Now.Month && date.Year == DateTime.Now.Year) ? true : false));
            }
            if (isMonthNow == false)
            {
                MONTHLIST.Add(new ComboboxModel(DateTime.Now.Month, DateTime.Now.Year, true));
            }
            SELECTMONTHTYPE = MONTHLIST.Where(x => x.ISSELECTED == true).FirstOrDefault();
            if (SELECTMONTHTYPE == null)
            {
                SELECTMONTHTYPE = MONTHLIST.FirstOrDefault();
            }
        }

        //Thực hiện lệnh trong Edit Command
        private void EditSalaryData()
        {
            hrmsEntities DB = new hrmsEntities();

            //Lưu những thay đổi vào database 
            double number;
            bool success = double.TryParse(COFFEICENT_STRING, out number);
            if (success)
            {
                COEFFICIENT = number;
            }
            else
            {
                MessageBox.Show("Wrong coffecient", "MESSAGE", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var Employee = DB.EMPLOYEEs.Where(x => x.EMPLOYEE_ID == SelectedItem.EMPLOYEE_ID).SingleOrDefault();
            var Salary = DB.SALARies.Where(x => x.EMPLOYEE_ID == SelectedItem.EMPLOYEE_ID && x.MONTH.Value.Month == SelectedItem.DATESTART.Month).SingleOrDefault();
            var Timekeeping = DB.TIMEKEEPINGs.Where(x => x.EMPLOYEE_ID == SelectedItem.EMPLOYEE_ID && x.DATE_START.Value.Month == SelectedItem.DATESTART.Month).SingleOrDefault();
            Salary.HEALTH_INSURANCE = long.Parse(HEALTH_INSURANCE.ToString());
            Salary.SOCIAL_INSURANCE = long.Parse(SOCIAL_INSURANCE.ToString());
            Salary.WELFARE = long.Parse(WELFARE.ToString());
            Salary.BONUS = long.Parse(BONUS.ToString());
            Salary.TAX = long.Parse(TAX.ToString());
            Salary.BASIC_WAGE = long.Parse(BASIC_WAGE.ToString());
            Salary.OVERTIME_SALARY = long.Parse(OVERTIME_SALARY.ToString());
            Salary.COEFFICIENT = double.Parse(COEFFICIENT.ToString());
            Salary.TOTAL_SALARY = AccountingClass.CalculateSalary(Salary, Timekeeping);
            TOTAL_SALARY = (long)Salary.TOTAL_SALARY;
            if (IMAGESOURCE != null)
            {
                Employee.IMAGE = IMAGESOURCE;
            }

            //change để lưu những thay đổi, countchange để kiểm tra có thay đổi ko
            String change = "";
            int countchange = 0;
            if (SOCIAL_INSURANCE != SelectedItem.SOCIALINSURANCE)
            {
                change = change + string.Format("SOCIAL INSUARANCE ({0} -> {1})     ", SelectedItem.SOCIALINSURANCE, SOCIAL_INSURANCE);
                countchange++;
            }
            if (HEALTH_INSURANCE != SelectedItem.HEALTHINSURANCE)
            {
                change = change + string.Format("HEALTH INSUARANCE ({0} -> {1})     ,", SelectedItem.HEALTHINSURANCE, HEALTH_INSURANCE);
                countchange++;
            }
            if (BONUS != SelectedItem.BONUS)
            {
                change = change + string.Format("BONUS ({0} -> {1})     ,", SelectedItem.BONUS, BONUS);
                countchange++;
            }
            if (TAX != SelectedItem.TAX)
            {
                change = change + string.Format("TAX({0} -> {1})     ,", SelectedItem.TAX, TAX);
                countchange++;
            }
            if (WELFARE != SelectedItem.WELFARE)
            {
                change = change + string.Format("WELFARE ({0} -> {1})     ,", SelectedItem.WELFARE, WELFARE);
                countchange++;
            }
            if (BASIC_WAGE != SelectedItem.BASICWAGE)
            {
                change = change + string.Format("BASIC WAGE ({0} -> {1})     ,", SelectedItem.BASICWAGE, BASIC_WAGE);
                countchange++;
            }
            if (OVERTIME_SALARY != SelectedItem.OVERTIMESALARY)
            {
                change = change + string.Format("OVERTIME_SALARY ({0} -> {1})     ,", SelectedItem.OVERTIMESALARY, OVERTIME_SALARY);
                countchange++;
            }
            if (COEFFICIENT != SelectedItem.COEFFICIENT)
            {
                change = change + string.Format("COEFFICIENT ({0} -> {1})     ,", SelectedItem.COEFFICIENT, COEFFICIENT);
                countchange++;
            }
            if (TOTAL_SALARY != SelectedItem.TOTALSALARY)
            {
                change = change + string.Format("TOTAL SALARY ({0} -> {1})     ,", SelectedItem.TOTALSALARY, TOTAL_SALARY);
                countchange++;
            }
            if (!NOTE.Equals(SelectedItem.NOTE))
            {
                Salary.NOTE = NOTE;
                change = change + string.Format("NOTE ('{0}' -> '{1}')     ,", SelectedItem.NOTE, NOTE);
                countchange++;
            }

            //Nếu có thay đổi thì lưu những thay đổi vào bảng record trong database
            if (countchange != 0)
            {
                RECORD record = new RECORD();
                record.EMPLOYEE_ID = USER_ID;
                record.EMPLOYEE_CHANGE_NAME = EMPLOYEE_NAME;
                record.EMPLOYEE_CHANGE_ID = EMPLOYEE_ID;
                record.DATE_CHANGE = DateTime.Now;
                record.DEPT_ID = USER_DEPT;
                record.CHANGE = change;
                record.MONTH_CHANGE = new DateTime(SelectedItem.DATESTART.Year, SelectedItem.DATESTART.Month, 1);
                DB.RECORDs.Add(record);
            }
            DB.SaveChanges();

            //Thông báo lưu thành công
            String message = "Save successful";
            MessageBox.Show(message, "MESSAGE", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //Điều kiện thực hiện command
        private bool IsEditSalaryData()
        {
            if (SelectedItem != null)
            {
                if (USER_ROLE > 0)
                    return true;
                //Chỉ được sửa tháng hiện tại ko cho sửa tháng trước
                if (SelectedItem.DATESTART.Month >= DateTime.Now.Month - 1)
                    return true;
                else return false;

                var salaryList = HRMSEntities.Ins.DB.SALARies.Where(x => x.EMPLOYEE_ID == SelectedItem.EMPLOYEE_ID && x.DATE_START.Value.Month == SelectedItem.DATESTART.Month);
                if (salaryList != null && salaryList.Count() != 0)
                    return true;
                return false;
            }
            return false;
        }

        //Điều kiện add ảnh
        private bool IsAddImageData()
        {
            if (IMAGESOURCE == null)
                return true;
            return false;
        }

        //Command add ảnh
        private void AddImageData()
        {
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png", ValidateNames = true, Multiselect = false };

            if (ofd.ShowDialog() == true)
            {
                string FILENAME = ofd.FileName;
                BitmapImage image = new BitmapImage(new Uri(FILENAME));
                IMAGESOURCE = File.ReadAllBytes(FILENAME);
                IMAGE_SOURCE = image;
                BUTTONTHICKNESS = 0;
                BRUSH = Brushes.Transparent;
            }
        }

        private bool IsExportCommand()
        {
            if ((SELECTMONTHTYPE.MONTH < DateTime.Now.Month && SELECTMONTHTYPE.YEAR == DateTime.Now.Year) || (SELECTMONTHTYPE.YEAR < DateTime.Now.Year))
                return true;
            return false;
        }

        //Lưu datagrid thàng pdf (name: UserName)
        private void ExportPDF(DataGrid dtgrid, string name)
        {
            var d = dtgrid.ItemsSource.Cast<SalaryInformationData>();
            var data = AccountingClass.ToDataTable(d.ToList());

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
                Font fntHead = new Font(bfntHead, 40, 1, BaseColor.GRAY);
                Paragraph prgHeading = new Paragraph();
                prgHeading.Alignment = Element.ALIGN_CENTER;
                prgHeading.Add(new Chunk(String.Format("SALARY REPORT {0}/{1}", SELECTMONTHTYPE.MONTH, SELECTMONTHTYPE.YEAR), fntHead));
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
            var d = dtgrid.ItemsSource.Cast<SalaryInformationData>();
            var data = AccountingClass.ToDataTable(d.ToList());
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

    }
}
