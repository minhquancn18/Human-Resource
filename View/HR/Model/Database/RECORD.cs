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
    
    public partial class RECORD
    {
        public int ID { get; set; }
        public Nullable<int> EMPLOYEE_ID { get; set; }
        public Nullable<int> DEPT_ID { get; set; }
        public Nullable<int> EMPLOYEE_CHANGE_ID { get; set; }
        public string EMPLOYEE_CHANGE_NAME { get; set; }
        public string CHANGE { get; set; }
        public Nullable<System.DateTime> DATE_CHANGE { get; set; }
        public Nullable<System.DateTime> MONTH_CHANGE { get; set; }
    
        public virtual DEPARTMENT DEPARTMENT { get; set; }
        public virtual EMPLOYEE EMPLOYEE { get; set; }
    }
}
