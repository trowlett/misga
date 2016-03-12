using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

/// <summary>
/// Summary description for ClubInfo
/// </summary>
public class ClubInfo
{
    public string ClubID { get; set; }
    public string ClubName { get; set; }
    public string MISGAURL { get; set; }
    public string ProName { get; set; }
    public string ProEmail { get; set; }
    public string ProPhone { get; set; }
    public string ProFax { get; set; }
    public string RepName { get; set; }
    public string RepEmail { get; set; }
    public string RepPhone { get; set; }
    public string PayOpt { get; set; }
    public string Refresh { get; set; }
    public string SRule { get; set; }
    public string OtherRule { get; set; }
    public string Misc { get; set; }
    public string slope { get; set; }
    public string MS_Yards { get; set; }
    public string MS_Rating { get; set; }
    public string MS_Par { get; set; }
    public string OrgName { get; set; }
    public string OrgURL { get; set; }
    public string WebSiteName { get; set; }
    public string WebSite { get; set; }
    public string WebMaster { get; set; }
    public string WebMasterEmail {get;set;}
    public string Signups {get;set;}
    public string AccessControl { get; set; }
    public string Active { get; set; }
    public string ControlCode { get; set; }



    public ClubInfo()
	{
		//
		// TODO: Add constructor logic here
		//
	}

}