using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    public int clubCount;
    protected bool TestEnvironment { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        //
        // Populate Radio Button List rblClubs with the Club Name and ID of the ones in the ClubSettings Database Table
        //
        //        rblClubs.Items.Clear();
        pnlLive.Visible = true;
        pnlTestEnvironment.Visible = false;
        if (ConfigurationManager.AppSettings["Environment"] == null)
        {
            this.TestEnvironment = false;
        }
        else
        {
            this.TestEnvironment = (ConfigurationManager.AppSettings["Environment"].ToString().Trim().ToLower() == "test");
        }
        if (this.TestEnvironment)
        {
            pnlTestEnvironment.Visible = true;
            pnlLive.Visible = false;
            if (!IsPostBack)
            {
                clubCount = 0;
                rblClubs.DataSource = CreateClubSource();
                rblClubs.DataTextField = "ClubName";
                rblClubs.DataValueField = "ClubID";
                rblClubs.DataBind();
            }
            if (rblClubs.Items.Count == 0)
            {
                rblClubs.Visible = false;
                lblNoClubs.Text = "No Clubs Available to Select.  Inform your Administrator of the problem.";
                lblNoClubs.Visible = true;
                btnShowSchedule.Visible = false;
                btnShowSchedule.Enabled = false;
            }
        }
        else
        {
            pnlLive.Visible = true;
        }

    }
    protected void btnShowSchedule_Click(object sender, EventArgs e)
    {
        // check that a Club is selected
        //
        if (rblClubs.SelectedIndex > -1)
        {
            string clubID = rblClubs.SelectedValue;
            Settings clubSettings = new Settings();
            clubSettings.ClubID = clubID;
            string x = clubSettings.ClubID;
            clubSettings.ClubInfo = ClubManager.GetSetting(clubID);
            string pageToLoad = "schedule.aspx?CLUB=" + clubID;
            Session["Settings"] = clubSettings;
            Response.Redirect(pageToLoad);
        }
        lblNoSelect.Text = "No CLUB Selected.  Please try again.";
//        btnAdmin.Enabled = false;
//        btnShowSchedule.Enabled = false;

        rblClubs.ClearSelection();
    }
    protected void rblClubs_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnShowSchedule.Visible = true;
        btnShowSchedule.Enabled = true;
        btnAdmin.Visible = false;
        btnAdmin.Enabled = false;
        int x = rblClubs.SelectedIndex;
//        rblClubs.Items[x].Selected = true;
    }

    public ICollection CreateClubSource()
    {
        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn("ClubName", typeof(string)));
        dt.Columns.Add(new DataColumn("ClubID", typeof(string)));
        ClubManager target = ClubManager.GetClubs();
        if (target.ClubCollection.Count > 0)
        {
            for (int i = 0; i < target.ClubCollection.Count; i++)
            {
                dr = dt.NewRow();
                dr[0] = target.ClubCollection[i].ClubName;
                dr[1] = target.ClubCollection[i].ClubID;
                dt.Rows.Add(dr);
            }
        }
        else
        {
        }
        DataView dv = new DataView(dt);
        return dv;
    }

    protected void btnAdmin_Click(object sender, EventArgs e)
    {
        // check that a Club is selected
        //
        if (rblClubs.SelectedIndex > -1)
        {
            string clubID = rblClubs.SelectedValue;
            Settings clubSettings = new Settings();
            clubSettings.ClubID = clubID;
            string x = clubSettings.ClubID;
            clubSettings.ClubInfo = ClubManager.GetSetting(clubID);
            string pageToLoad = "Admin/Default.aspx?CLUB=" + clubID;
            Session["Settings"] = clubSettings;
            Response.Redirect(pageToLoad);
        }
        lblNoSelect.Text = "No CLUB Selected.  Please try again.";

        rblClubs.ClearSelection();

    }
}