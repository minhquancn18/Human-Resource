//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRMS.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class EMPLOYEE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EMPLOYEE()
        {
            this.RECORDs = new HashSet<RECORD>();
            this.SALARies = new HashSet<SALARY>();
            this.TIMEKEEPINGs = new HashSet<TIMEKEEPING>();
            this.TIMEKEEPING_DETAIL = new HashSet<TIMEKEEPING_DETAIL>();
        }
    
        public int EMPLOYEE_ID { get; set; }
        public Nullable<int> ID_CARD { get; set; }
        public string NAME { get; set; }
        public Nullable<int> AGE { get; set; }
        public string GENDER { get; set; }
        public string PASSWORD { get; set; }
        public string ACADEMIC_LEVEL { get; set; }
        public Nullable<System.DateTime> BIRTH_DATE { get; set; }
        public string BIRTH_PLACE { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
        public string CITIZENSHIP { get; set; }
        public Nullable<int> DEPT_ID { get; set; }
        public Nullable<int> ROLE_ID { get; set; }
        public byte[] IMAGE { get; set; }
    
        public virtual DEPARTMENT DEPARTMENT { get; set; }
        public virtual ROLE ROLE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RECORD> RECORDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SALARY> SALARies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TIMEKEEPING> TIMEKEEPINGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TIMEKEEPING_DETAIL> TIMEKEEPING_DETAIL { get; set; }
    }
}
