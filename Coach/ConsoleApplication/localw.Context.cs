﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConsoleApplication
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class localdbEntities : DbContext
    {
        public localdbEntities()
            : base("name=localdbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<allowance_summary> allowance_summary { get; set; }
        public virtual DbSet<mail_configuration> mail_configuration { get; set; }
        public virtual DbSet<payroll_mapping> payroll_mapping { get; set; }
        public virtual DbSet<personnel_info> personnel_info { get; set; }
        public virtual DbSet<salary_change> salary_change { get; set; }
    }
}