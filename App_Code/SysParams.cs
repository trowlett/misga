using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;

/// <summary>
/// Summary description for SysParams
/// </summary>
[Table(Name = "Params")]
public class SysParams
{
    [Column(IsPrimaryKey = true, Name="ClubID", DbType="CHAR(3) NOT NULL", CanBeNull=false)]
    public string ClubID;
    [Column(IsPrimaryKey = true, Name="Key", DbType="VARCHAR(50) NOT NULL", CanBeNull=false)]
    public string Key { get; set; }

    [Column(Name="Value", DbType="VARCHAR(50) NOT NULL", CanBeNull=false)]
    public string Value { get; set; }

    [Column(Name="ChangeDate", DbType="DATETIME NOT NULL",CanBeNull=false)]
    public DateTime ChangeDate { get; set; }
}