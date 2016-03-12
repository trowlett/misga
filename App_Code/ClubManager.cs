using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

/// <summary>
/// Summary description for ClubManager



public class ClubManager
{

    private Collection<ClubInfo> clubCollection = new Collection<ClubInfo>();
    public Collection<ClubInfo> ClubCollection
    {
        get { return this.clubCollection; }
    }


    public static ClubInfo ci { get; set; }

    public static ClubInfo GetSetting(string clubid)
    {
        string sdbc = ConfigurationManager.ConnectionStrings["MRMISGADBConnect"].ToString();
        MRMISGADB csdb = new MRMISGADB(sdbc);

        var cs = csdb.ClubSettings.FirstOrDefault(c => c.ClubID == clubid);

        string ClubsConnect = ConfigurationManager.ConnectionStrings["ClubsConnect"].ToString();
        MISGACLUBS db = new MISGACLUBS(ClubsConnect);

        var q = db.Clubs.FirstOrDefault(p => p.ClubID == clubid);
        if (q != null)
        {

            ci = new ClubInfo
            {
                ClubID = clubid,
                ClubName = q.ClubName,
                MISGAURL = q.MISGAURL,
                ProName = q.ProName,
                ProEmail = q.ProEmail,
                ProPhone = q.ProPhone,
                ProFax = q.ProFax,
                RepName = q.RepName,
                RepEmail = q.RepEmail,
                RepPhone = q.RepPhone,
                PayOpt = q.PayOpt,
                Refresh = q.Refresh,
                SRule = q.SRule,
                OtherRule = q.OtherRule,
                Misc = q.Misc,
                slope = q.slope,
                MS_Yards = q.MS_Yards,
                MS_Rating = q.MS_Rating,
                MS_Par = q.MS_Par,
                Active = cs.Active,
                OrgName = cs.OrgName,
                OrgURL = cs.OrgURL,
                WebSiteName = cs.WebSiteName,
                WebSite = cs.Website,
                WebMaster = cs.WebMaster,
                WebMasterEmail = cs.WebMasterEmail,
                Signups = cs.Signups,
                AccessControl = cs.AccessControl,
                ControlCode = cs.ControlCode
            };
            if (ci.slope.Trim() == "???") ci.slope = "N/A";
            if (ci.MS_Yards == null) ci.MS_Yards = "N/A";
            if (ci.MS_Rating == null) ci.MS_Rating = "N/A";
            if (ci.MS_Par == null) ci.MS_Par = "NA";
        }
        else
        {
            ci = new ClubInfo
            {
                ClubID = clubid,
                ClubName = string.Format("No Club on file with the ID: {0}",clubid)
            };
        }
        return ci;
    }
    public static ClubManager GetClubs()
    {
        ClubManager target = new ClubManager();
        string sdbc = ConfigurationManager.ConnectionStrings["MRMISGADBConnect"].ToString();
        MRMISGADB csdb = new MRMISGADB(sdbc);

        var cs =
            from c in csdb.ClubSettings
            where c.Active == "YES"
            orderby c.OrgName
            select new { c.ClubID };
        if (cs != null)
        {
            foreach (var clb in cs)
            {
                ClubInfo ci = ClubManager.GetSetting(clb.ClubID);
                target.clubCollection.Add(ci);
            }
        }
        return target;
    }

	public ClubManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}