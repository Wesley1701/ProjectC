using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
  public enum Gender { Male, Female, Other, Unknown }
  public class CompanyContext : DbContext
  {
    public DbSet<Employee> Employee { get; set; }
    public DbSet<Project> Project { get; set; }
    public DbSet<WorksOn> WorksOn { get; set; }
    public DbSet<Hours> Hours { get; set; }
    public DbSet<Service> Service { get; set; }
    public DbSet<HourType> HourType { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseNpgsql("User ID=postgres;Password=a;Host=localhost;Port=5432;Database=test;Pooling=true;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Employee>()
      .HasKey(e => e.SSN);

      modelBuilder.Entity<Project>()
      .HasKey(p => p.NumberID);

      modelBuilder.Entity<Service>()
      .HasKey(s => s.ID);

      modelBuilder.Entity<HourType>()
      .HasKey(ht => ht.ID);

      modelBuilder.Entity<WorksOn>()
      .HasKey(wo => wo.Hours);

      modelBuilder.Entity<Hours>()
      .HasKey(h => h.ID);

    }
  }

  public class Employee
  {
    [Column(TypeName = "varchar(5)")]
    public string Initials { get; set; }
    [Column(TypeName = "varchar(50)")]
    public string Name { get; set; }
    [Column(TypeName = "char(25)")]
    public string SSN { get; set; }
    public Gender Gender { get; set; }
    [Column(TypeName = "char(6)")]
    public IEnumerable<WorksOn> Schedule { get; set; }

  }

  public class Project
  {
    [Column(TypeName = "varchar(25)")]
    public string Name { get; set; }
    [Column(TypeName = "char(25)")]
    public string NumberID { get; set; }
    [Column(TypeName = "varchar(3)")]
    public int WorkingOn { get; set; }
    [Column(TypeName = "char(6)")]
    public IEnumerable<WorksOn> Developers { get; set; }
  }
  public class Service
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string ProjectID { get; set; }
    }

    public class HourType
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string TypeID { get; set; }
        public string ServiceID { get; set; }
    }

  public class WorksOn
  {
    [Column(TypeName = "char(30)")]
    public string EmployeeSSN { get; set; }
    [Column(TypeName = "char(30)")]
    public string ProjectName { get; set; }
    public int Hours { get; set; }
  }

   public class Hours
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key, Column(Order = 0)]
    public int ID { get; set; }
    [Column(TypeName = "char(30)")]
    public string EmployeeID { get; set; }
    [Column(TypeName = "char(25)")]
    public string ProjectID { get; set; }
    public string ServiceID { get; set; }
    public string HoursTypeID { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string Note { get; set; }
        //userid, projectid, starttime, endtime, hoursWorked, 
  }
}