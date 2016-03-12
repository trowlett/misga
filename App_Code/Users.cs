using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;

/// <summary>
/// Summary description for Users
/// </summary>
/// 
[Table(Name="Users")]
public class Users
{
    [Column(IsPrimaryKey = true, Name="UserID", DbType="VARCHAR(25) NOT NULL", CanBeNull=false)]
    public string UserID;
    [Column(Name = "Password", DbType = "VARCHAR(25) NOT NULL", CanBeNull=false)]
    public string Password;
    [Column(Name = "Name", DbType = "VARCHAR(50) NOT NULL", CanBeNull=false)]
    public string Name;
    [Column(Name = "ClubID", DbType = "CHAR(3) NOT NULL", CanBeNull= false)]
    public string ClubID;
    [Column(Name = "Email", DbType = "VARCHAR(100) NOT NULL",CanBeNull=false)]
    public string Email;
    [Column(Name="Phone", DbType="VARCHAR(15) NOT NULL", CanBeNull= false)]
    public string Phone;
    [Column(Name = "Role", DbType = "CHAR(10) NOT NULL", CanBeNull= false)]
    public string role;
    [Column(Name = "LoginCount", DbType = "INT NOT NULL", CanBeNull=false)]
    public int LoginCount;
    [Column(Name = "RegisteredDate", DbType = "DATETIME", CanBeNull=true)]
    public DateTime RegisteredDate;
    [Column(Name = "ChangeDate", DbType = "DATETIME", CanBeNull=true)]
    public DateTime ChangeDate;
    [Column(Name = "LastLogin", DbType = "DATETIME", CanBeNull= true)]
    public DateTime LastLogin;


	public Users()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}