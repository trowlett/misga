using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;

/// <summary>
/// Summary description for Clubs
/// </summary>
[Table(Name="Clubs")]
public class Clubs
{
	[Column(IsPrimaryKey = true, Name="ClubID",DbType="CHAR(10) NOT NULL",CanBeNull=false)]
	public string ClubID;
    [Column(Name="ClubName", DbType="VARCHAR(75) NOT NULL", CanBeNull=false)]
    public string ClubName;
    [Column(Name="MISGAURL", DbType="VARCHAR(255) NOT NULL", CanBeNull=false)]
    public string MISGAURL;
    [Column(Name="ProName",DbType="VARCHAR(50) NOT NULL", CanBeNull=false)]
    public string ProName;
    [Column(Name="ProEmail",DbType="VARCHAR(50) NOT NULL", CanBeNull=false)]
    public string ProEmail;
    [Column(Name="ProPhone", DbType="VARCHAR(50) NOT NULL",CanBeNull=false)]
    public string ProPhone;
    [Column(Name="ProFax",DbType="VARCHAR(50) NOT NULL",CanBeNull=false)]
    public string ProFax;
    [Column(Name="RepName",DbType="VARCHAR(50) NOT NULL",CanBeNull=false)]
    public string RepName;
    [Column(Name = "RepEmail", DbType = "VARCHAR(50) NOT NULL", CanBeNull = false)]
    public string RepEmail;
    [Column(Name = "RepPhone", DbType = "VARCHAR(50) NOT NULL", CanBeNull = false)]
    public string RepPhone;
    [Column(Name = "PayOpt", DbType = "VARCHAR(50) NOT NULL", CanBeNull = false)]
    public string PayOpt;
    [Column(Name = "Refresh", DbType = "VARCHAR(400) NOT NULL", CanBeNull = false)]
    public string Refresh;
    [Column(Name = "SRule", DbType = "VARCHAR(400) NOT NULL", CanBeNull = false)]
    public string SRule;
    [Column(Name = "OtherRule", DbType = "VARCHAR(400) NOT NULL", CanBeNull = false)]
    public string OtherRule;
    [Column(Name = "Misc", DbType = "VARCHAR(400) NOT NULL", CanBeNull = false)]
    public string Misc;
    [Column(Name = "Slope", DbType = "VARCHAR(3) NOT NULL", CanBeNull = false)]
    public string slope;
    [Column(Name = "MS_Yards", DbType = "CHAR(10)", CanBeNull=true)] 
    public string MS_Yards;
    [Column(Name = "MS_Rating", DbType= "CHAR(10)", CanBeNull = true)]
    public string MS_Rating;
    [Column(Name = "MS_Par", DbType = "CHAR(10)", CanBeNull = true)]
    public string MS_Par;
    [Column(Name = "TChoice", DbType = "VARCHAR(1200)", CanBeNull = true)]
    public string TChoice;

	public Clubs()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}