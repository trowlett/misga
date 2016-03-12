using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;
using System.Data.SqlTypes;

[Table (Name="Guest")]
public class Guest
{
    [Column(IsPrimaryKey = true, Name="ClubID", DbType="CHAR(3) NOT NULL", CanBeNull=false)]
    public string ClubID;
    [Column (IsPrimaryKey=true, Name="GuestID", DbType="INT NOT NULL", CanBeNull=false)]
    public int GuestID;
    [Column(Name="GuestName", DbType="VARCHAR(50) NOT NULL", CanBeNull=false)]
    public string GuestName;
    [Column(Name = "gLname", DbType = "VARCHAR(50) NOT NULL", CanBeNull = false)]
    public string gLname;
    [Column(Name = "gFname", DbType = "VARCHAR(50) NOT NULL", CanBeNull = false)]
    public string gFname;
    [Column(Name = "gHcp", DbType = "VARCHAR(50) NOT NULL", CanBeNull = false)]
    public string gHcp;
    [Column(Name = "gSex", DbType = "INT NOT NULL", CanBeNull = false)]
    public int gSex;
    [Column(Name = "Played", DbType = "INT NOT NULL", CanBeNull = false)]
    public int Played;
    [Column(Name = "DatePlayed", DbType = "DATETIME NOT NULL", CanBeNull = false)]
    public DateTime DatePlayed;

}