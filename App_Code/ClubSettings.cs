using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;

/// <summary>
/// Summary description for ClubSettings
/// </summary>
/// 
[Table(Name = "ClubSettings")]
public class ClubSettings
{
    [Column(IsPrimaryKey = true, Name="ClubID", DbType="CHAR(3) NOT NULL", CanBeNull=false)]
    public string ClubID;
    [Column(Name="Active", DbType="CHAR(10)", CanBeNull=true)]
    public string Active;
    [Column(Name="OrgName", DbType="VARCHAR(75) NOT NULL", CanBeNull=false)]
    public string OrgName;
    [Column(Name="OrgURL",DbType="VARCHAR(100) NOT NULL", CanBeNull=false)]
    public string OrgURL;
    [Column(Name="WebSiteName", DbType="VARCHAR(50) NOT NULL", CanBeNull=false)]
    public string WebSiteName;
    [Column(Name = "Website", DbType = "VARCHAR(50) NOT NULL", CanBeNull = false)]
    public string Website;
    [Column(Name = "WebMaster", DbType = "VARCHAR(50) NOT NULL", CanBeNull = false)]
    public string WebMaster;
    [Column(Name = "WebMasterEmail", DbType = "VARCHAR(75) NOT NULL", CanBeNull = false)]
    public string WebMasterEmail;
    [Column(CanBeNull=true, Name="Signups", DbType="CHAR(20)")]
    public string Signups;
    [Column(Name="AccessControl", DbType="CHAR(10) NOT NULL", CanBeNull=false)]
    public string AccessControl;
    [Column(CanBeNull = true, Name="ControlCode", DbType="CHAR(10)")]
    public string ControlCode;
    [Column(Name="DeadlineSpan", DbType="INT NOT NULL", CanBeNull=false)]
    public int DeadlineSpan;
    [Column(CanBeNull=true, Name="PostSpan", DbType="INT")]
    public int PostSpan;

	public ClubSettings()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}