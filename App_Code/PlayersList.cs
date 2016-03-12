using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;

/// <summary>
/// Summary description for PlayersList
/// </summary>
[Table(Name = "PlayersList")]
public class PlayersList
{
    [Column(IsPrimaryKey = true, Name="ClubID", DbType="CHAR(3) NOT NULL", CanBeNull=false)]
    public string ClubID;
    [Column(IsPrimaryKey=true, Name="EventID", DbType="CHAR(14) NOT NULL", CanBeNull=false)]
    public String EventID;
    [Column(IsPrimaryKey=true, Name="PlayerID", DbType="INT NOT NULL", CanBeNull= false)]
    public int PlayerID;
    [Column(Name="TransDate",DbType="DATETIME NOT NULL", CanBeNull=false)]
    public DateTime TransDate;
    [Column(Name="Action", DbType="CHAR(10) NOT NULL",CanBeNull=false)]
    public string Action;
    [Column(Name="Carpool", DbType="CHAR(10) NOY NULL", CanBeNull=false)]
    public string Carpool;
    [Column(Name="Marked", DbType="INT NOT NULL", CanBeNull=false)]
    public int Marked;
    [Column(Name="SpecialRule", DbType="CHAR(10) NOT NULL", CanBeNull=false)]
    public string SpecialRule;
    [Column(Name="GuestID", DbType="INT NOT NULL", CanBeNull=false)]
    public int GuestID;
}