﻿using HRMS.Accouting.Model;
using HRMS.Accouting.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HRMS.Accounting.View
{
    /// <summary>
    /// Interaction logic for uConEmployeeSalary.xaml
    /// </summary>
    public partial class uConEmployeeSalary : UserControl
    {
        public uConEmployeeSalary(int ID)
        {
            InitializeComponent();
            DataContext = new AccountingViewModel(ID);
        }

        public uConEmployeeSalary(SalaryInformationData data, int ID)
        {
            DataContext = new AccountingViewModel(data, ID);
            InitializeComponent();
        }

        public uConEmployeeSalary(int IDSelect, int IDUser, int month, int year)
        {
            DataContext = new AccountingViewModel(IDSelect, IDUser, month, year);
            InitializeComponent();
        }
    }
}
