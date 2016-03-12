using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;

/// <summary>
/// Summary description for Schedule
/// </summary>

[Table(Name="Events")]
public class Events
{
    [Column(IsPrimaryKey = true, Name="ClubID", DbType="CHAR(3) NOT NULL",CanBeNull=false)]
    public string ClubID;
    [Column(IsPrimaryKey = true,Name="EventID", DbType="CHAR(14) NOT NULL", CanBeNull=false)]
    public string EventID;
    [Column(Name="Date",DbType="DATETIME NOT NULL", CanBeNull=false)]
    public DateTime Date;
    [Column(Name="HostID", DbType="CHAR(3) NOT NULL", CanBeNull=false)]
    public string HostID;
    [Column(Name="Type", DbType="CHAR(10) NOT NULL",CanBeNull=false)]
    public string Type;
    [Column(Name="Title", DbType="VARCHAR(400) NOT NULL",CanBeNull=false)]
    public string Title;
    [Column(Name="Cost", DbType="CHAR(10) NOT NULL", CanBeNull=false)]
    public string Cost;
    [Column(Name="PlayerLimit", DbType="INT NOT NULL", CanBeNull=false)]
    public int PlayerLimit;
    [Column(Name="Deadline", DbType="DATETIME NOT NULL", CanBeNull=false)]
    public DateTime Deadline;
    [Column(Name = "PostDate", DbType = "DATETIME NOT NULL", CanBeNull = false)]
    public DateTime PostDate;
    [Column(Name="HostPhone", DbType="VARCHAR(50) NOT NULL", CanBeNull=false)]
    public string HostPhone;
    [Column(Name = "SpecialRule", DbType = "VARCHAR(50) NOT NULL", CanBeNull = false)]
    public string SpecialRule;
    [Column(Name = "Guest", DbType = "CHAR(10) NOT NULL", CanBeNull = false)]
    public string Guest;
    [Column(Name = "CreationDate", DbType = "DATETIME NOT NULL", CanBeNull = false)]
    public DateTime CreationDate;
}