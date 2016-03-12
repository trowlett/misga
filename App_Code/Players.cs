using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;
using System.Data.SqlTypes;

[Table(Name = "Players")]
public class Players
{
    [Column(IsPrimaryKey = true, Name="ClubID", DbType="CHAR(3) NOT NULL", CanBeNull=false)]
    public string ClubID;
    [Column(IsPrimaryKey = true, Name="PlayerID", DbType="INT NOT NULL", CanBeNull=false)]
    public int PlayerID;
    [Column(Name="Name", DbType="VARCHAR(100) NOT NULL", CanBeNull=false)]
    public string Name;
    [Column(Name="LName", DbType="VARCHAR(100) NOT NULL", CanBeNull=false)]
    public string LName;
    [Column(Name = "FName", DbType = "VARCHAR(100) NOT NULL", CanBeNull = false)]
    public string FName;
    [Column(Name="Hcp", DbType="CHAR(10) NOT NULL", CanBeNull=false)]
    public string Hcp;
    [Column(Name = "MemberID", DbType = "CHAR(10) NOT NULL", CanBeNull = false)]
    public string MemberID;
    [Column(Name="Sex", DbType="INT NOT NULL", CanBeNull=false)]
    public int Sex;
    [Column(Name = "Title", DbType = "CHAR(10) NOT NULL", CanBeNull = false)]
    public string Title;
    [Column(Name="HDate", DbType="DATETIME NOT NULL", CanBeNull=false)]
    public DateTime HDate;
    [Column(Name = "Delete", DbType = "INT NOT NULL", CanBeNull = false)]
    public int Delete;
}