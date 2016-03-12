using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class Signup_Default : System.Web.UI.Page
{
	public MrSchedule Schedule { get; set; }
    public DateTime displayDate;
    public int signupOffset { get; set; }
    private MrTimeZone ETZ;
    private DateTime sysToday;
    public string PhysicalYear {get; set;}
    public DateTime BeginActive { get; set; }
    public DateTime EndActive { get; set; }
    private DateTime nullDate = new DateTime(2010, 1, 1, 0, 0, 1);
    public DateTime scheduleDate { get; set; }
    public string clubID;
    public Settings clubSettings {get; set;}
    public string ScheduleContactName { get; set; }
    public string ScheduleContactEmail { get; set; }
    public bool SignupsAllowed { get; set; }
    public string ClubName { get; set; }
    public bool ShowCompleteSchedule { get; set; }
    public bool ShowAll { get; set; }
    protected bool ShowPartOfSchedule 
    { 
        get 
        { 
            return !ShowAll; 
        } 
    }

	protected void load_schedule()
	{

		this.Schedule = MrSchedule.LoadFromDB(clubSettings.ClubID,ShowCompleteSchedule);         //  Get the remaining events from the database
        int eventsRemaining = this.Schedule.Events.Count;
        this.scheduleDate = this.Schedule.CreateTime;
        if (this.scheduleDate > DateTime.MinValue)
        {
            SystemParameters.Update(clubSettings.ClubID, SystemParameters.ScheduleDate, scheduleDate.ToLongDateString());
        }
        else
        {
            SystemParameters.Update(clubSettings.ClubID, SystemParameters.ScheduleDate, "Nothing");
        }
        if (eventsRemaining > 0)
        {
            pnlSchedule.Visible = true;
            pnlNoSchedule.Visible = false;
            this.ScheduleRepeater.DataSource = new MrSchedule[] { this.Schedule };
            this.ScheduleRepeater.DataBind();
        }
        else
        {
            pnlSchedule.Visible = false;
            pnlNoSchedule.Visible = true;
        }
	}

    protected DateTime GetParamDateTimeValue(String key)
    {
        string MRMISGADBConn = ConfigurationManager.ConnectionStrings["MRMISGADBConnect"].ToString();
        MRMISGADB db = new MRMISGADB(MRMISGADBConn);

        var prm = db.MRParams.FirstOrDefault(p => p.ClubID == clubSettings.ClubID && p.Key == key);
        if (prm == null) return nullDate;
        DateTime pd = Convert.ToDateTime(prm.Value);
        return pd;
    }


	protected void Page_Load(object sender, EventArgs e)
	{
        if (Request.QueryString["Club"] == null)
        {
            Response.Redirect("BadClub.aspx");
        }
        clubID = Request.QueryString["CLUB"].Trim();
        clubSettings = new Settings();
        clubSettings.ClubID = clubID;
        string x = clubSettings.ClubID;
        clubSettings.ClubInfo = ClubManager.GetSetting(clubSettings.ClubID);
        Session["Settings"] = clubSettings;
        lbtnMyList.PostBackUrl = "~/mylist.aspx?CLUB=" + clubID;

//        clubSettings = (Settings)Session["Settings"];
//        clubSettings.ClubID = clubID;
//        clubSettings.ClubInfo = ClubManager.GetSetting(clubID);
//        if (clubSettings.ClubInfo == null)
//        {
//            Response.Redirect("BadClub.aspx");
//        }
//        litOrg3.Text = ConfigurationManager.AppSettings["Org"];
//        Page.Title = ConfigurationManager.AppSettings["Org"] + " Schedule";
        ClubName = clubSettings.ClubInfo.ClubName;
        litOrg3.Text = clubSettings.ClubInfo.ClubName;
        Page.Title = clubSettings.ClubInfo.ClubName + " Schedule";
        string offset = ConfigurationManager.AppSettings["SignupOffset"];
        if (offset == null) offset = "0";
//        offset = "180";                       FOR TESTING PURPOSES
        ETZ = new MrTimeZone();
        sysToday = ETZ.eastTimeNow();
        int pyear = sysToday.Year;
        DateTime tempDate = new DateTime(pyear, 11, 15, 0, 0, 1);
        if (tempDate < sysToday) pyear++;
        DateTime Sstart = GetParamDateTimeValue("FirstDate");
        DateTime Esignup = GetParamDateTimeValue("LastDate");
        if (Sstart == nullDate)  {BeginActive = new DateTime(pyear, 2, 15, 0, 0, 1); }
        else {
            BeginActive = Sstart;
        }
        if (Esignup == nullDate) { EndActive = new DateTime(pyear, 11, 15, 0, 0, 1); }
        else
        {
            EndActive = Esignup;
        }
//        this.PhysicalYear = pyear.ToString();
        if (sysToday < BeginActive)
            lblBeginActive.Visible = true;
        signupOffset = 0;
        if (offset != null)
            if (offset.Trim() != "")
                signupOffset = Convert.ToInt32(offset);

        ScheduleContactName = clubSettings.ClubInfo.RepName;
        ScheduleContactEmail = clubSettings.ClubInfo.RepEmail;
        if (clubSettings.ClubInfo.Signups == null)
        {
            SignupsAllowed = true;
        }
        else
        {
            SignupsAllowed = (clubSettings.ClubInfo.Signups.Trim().ToLower() == "enabled");
        }
        SignupDates sd = new SignupDates();
        this.displayDate = sd.getDisplayDate(clubID);
        this.load_schedule();
        this.PhysicalYear = this.Schedule.ScheduleYear.ToString();

	}
    protected void cbShowAll_CheckedChanged(object sender, EventArgs e)
    {
        ShowCompleteSchedule = cbShowAll.Checked;
        ShowAll = ShowCompleteSchedule;
        if (ShowAll)
        {
            lblShowAll.Text = "Complete Schedule Shown:&nbsp;&nbsp;";
            cbShowAll.Text = " uncheck to show the Remaining Schedule";
        }
        else
        {
            lblShowAll.Text = "Remaining Schedule Shown:&nbsp;&nbsp;";
            cbShowAll.Text = " check to show the Complete Schedule";
        }
        this.load_schedule();
        this.PhysicalYear = this.Schedule.ScheduleYear.ToString();

    }
}