﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SJBCS.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AmsModel : DbContext
    {
        public AmsModel()
            : base("name=AmsModel")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Biometric> Biometrics { get; set; }
        public virtual DbSet<Calendar> Calendars { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<DistributionList> DistributionLists { get; set; }
        public virtual DbSet<Level> Levels { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<RelBiometric> RelBiometrics { get; set; }
        public virtual DbSet<RelDistributionList> RelDistributionLists { get; set; }
        public virtual DbSet<RelOrganization> RelOrganizations { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
