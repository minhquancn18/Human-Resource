//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRMS.HR.Model.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class SALARY
    {
        public int SALARY_ID { get; set; }
        public Nullable<int> EMPLOYEE_ID { get; set; }
        public Nullable<long> OVERTIME_SALARY { get; set; }
        public Nullable<double> COEFFICIENT { get; set; }
        public Nullable<long> BONUS { get; set; }
        public Nullable<long> BASIC_WAGE { get; set; }
        public Nullable<long> WELFARE { get; set; }
        public Nullable<long> TAX { get; set; }
        public Nullable<long> SOCIAL_INSURANCE { get; set; }
        public Nullable<long> HEALTH_INSURANCE { get; set; }
        public Nullable<System.DateTime> DATE_START { get; set; }
        public Nullable<System.DateTime> DATE_END { get; set; }
        public Nullable<long> TOTAL_SALARY { get; set; }
        public string NOTE { get; set; }
        public Nullable<System.DateTime> MONTH { get; set; }
    
        public virtual EMPLOYEE EMPLOYEE { get; set; }
    }
}
