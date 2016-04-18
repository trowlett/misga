using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

	public partial class Schedule_Signup : System.Web.UI.Page
	{
		public SignupList sl { get; set; }

        private string clubID;
		private string eventID;
		private string path;
		private const string keyPlayers = "Players";
        private const string keySignups = "Signups";
		private string[] contact = new string[3];
    public string ClubName { get; set; }
		public string webMaster;
		public string webMasterEmail;
		public string website;
		private bool GuestOption = false;
		public string ruleGuest = "";
		public string rule89 = "";
		public const string Guest = "Guest";
		private DateTime deadline;
		private int playerLimit;
        private DateTime postDate;
		private string sRule = "";
        private string[] sRuleChoices;
        private static readonly char[] delimiterChars = { ',' };
        private string spcRule;
        private int prevSRSelection;
        private string prevSRItem;
        private int errorCode = 0;
		private int maxLimit = 60;
		private const string keySignupLimit = "SignupLimit";
        private const string noChoicesSpecialCharacter = "?";
        private string hostClubPhone = "Hamburger I 812";
		private string userLastName = "";
		private string userFirstName = "";
		private string userGender = "";
		private string userLastVisit = "";
		private string ln = "";
		private string fn = "";
		private string carpool = "";
		private string tgender = "";
        private bool enableSignups = false;
        private string offset;
        private int signupOffset;
        private int srCount;
        private bool IsAccessControlOn;
        private Settings clubSettings;

		protected void LoadMixerInfo(string[] mixerLines)
		{
			litInfo.Text = "";
			int i = 0;
			foreach (string line in mixerLines)
			{
				i = i + 1;
				litInfo.Text = litInfo.Text + line;
			}
		}
/*        protected bool IsSignupAllowed(MRParams p)
        {
            Settings temp = clubSettings;
            if (p == null) return true;                     // Allowed if nothing in MRParams file
            if (p.Value.ToUpper() == "YES") return true;    // Allowed if YES
            if ((p.Value.Length > 5) && (p.Value.Substring(0,6).ToUpper() == "ENABLE")) return true;    
            return false;                                   // disallow if anything else
        }
        */
        protected bool IsSignupAllowed(Settings cs, MRParams p)
        {
            // The value in ClubInfo,Signups takes precedence over the value in MRParams
            //
            bool ans = true;
            if (cs.ClubInfo.Signups != null)
            {
                ans = false;
                string sup = cs.ClubInfo.Signups.Trim().ToUpper();
                if ((sup == "YES") || ((sup.Length > 5) && (sup.Substring(0, 6) == "ENABLE"))) 
                {
                    ans = true;
                    if (p !=null)
                    {
                        string psup = p.Value.Trim().ToUpper();
                        if ((psup == "YES") || ((psup.Length > 5) && (psup.Substring(0, 6) == "ENABLE")))
                        {
                            ans = true;
                        }
                        else
                        {
                            ans = false;
                        }
                    }
                }
            }
            return ans;
        }
		protected void Page_Load(object sender, EventArgs e)
		{
			eventID = Request.QueryString["ID"].Trim();
//            clubID = Request.QueryString["CLUB"];
            clubID = eventID.Substring(0, 3);
            clubSettings = new Settings();
            clubSettings.ClubID = clubID;
            clubSettings.ClubInfo = ClubManager.GetSetting(clubID);
        //            clubSettings = (Settings)Session["Settings"];
        //			litOrg1.Text = ConfigurationManager.AppSettings["Org"];
        //			litOrg2.Text = ConfigurationManager.AppSettings["Org"];
        ClubName = clubSettings.ClubInfo.ClubName;
        Page.Title = clubSettings.ClubInfo.ClubName + " SignUp Page";
        litOrg1.Text = clubSettings.ClubInfo.OrgName;
            litOrg2.Text = clubSettings.ClubInfo.OrgName;
            offset = ConfigurationManager.AppSettings["SignupOffset"];
            IsAccessControlOn = false;
            if ((clubSettings.ClubInfo.AccessControl != null) &&
                (clubSettings.ClubInfo.AccessControl.ToLower().Trim() == "on"))
                    IsAccessControlOn = true;
            if (!IsAccessControlOn)
            {
                lblAccessCode.Visible = false;
                tbAccessCode.Visible = false;
            }
            signupOffset = 0;
            if (offset != null)
                if (offset.Trim() != "")
                    signupOffset = Convert.ToInt32(offset);


            MrTimeZone tz = new MrTimeZone();
            deadline = tz.eastTimeNow();
            postDate = tz.eastTimeNow();
            playerLimit = 1;
            hostClubPhone = "";
            linkbuttonSR.Visible = false;
            

            enableSignups = false;
            SignupsDisabledPanel.Visible = false;
            string MRMISGADBConn = ConfigurationManager.ConnectionStrings["MRMISGADBConnect"].ToString();
            MRMISGADB db = new MRMISGADB(MRMISGADBConn);
            MRParams param = db.MRParams.FirstOrDefault(p => p.ClubID == clubSettings.ClubID && p.Key == keySignups);
//            if (IsSignupAllowed(param)) enableSignups = true;
            enableSignups = IsSignupAllowed(clubSettings, param);
            if (!IsPostBack)
			{
				NormalPanel.Visible = true;
                SignupPanel.Visible = true;
                ClosedPanel.Visible = false;
				GuestListPanel.Visible = false;
				SpecialRulePanel.Visible = false;
				GuestOption = false;
//				cbSpecialRule.Checked = false;
                SRCBLPanel.Visible = false;
                SRCBKL.Visible = false;
                SRCBKL.Items.Clear();
                Select_STD.Visible = false;
                cbGuestRule.Checked = false;
				tbGFN.Text = "";
				tbGLN.Text = "";
				tbGHcp.Text = "";
                GetCookieUserInfo();
            }
            var mixer = db.Events.FirstOrDefault(ev => ev.EventID == eventID);

            if (mixer != null)
            {
                if (mixer.Type.Trim() == "Home")
                {
                    Page.Title = string.Format("{0} {1} Sign-up", mixer.Date.ToString("MMM d"), ClubName);
                }
                else
                {
                    Page.Title = string.Format("{0} {1} Sign-up",mixer.Date.ToString("MMM d"), mixer.Title);
                }
            deadline = mixer.Deadline;
                playerLimit = mixer.PlayerLimit;
                hostClubPhone = GetHostPhone(mixer.HostID);
                MISGA_Std_Tee.Text = GetHostMISGAStdTee(mixer.HostID);
                postDate = mixer.PostDate;
                if (playerLimit >= maxLimit)
                {
                    lblPlayerLimit.Text = "(Player Limit: Unlimited)";
                }
                else
                {
                    lblPlayerLimit.Text = "(Player Limit: " + playerLimit.ToString("##0") + "*)";
                }
                if (mixer.SpecialRule != null)
                {
                    sRule = mixer.SpecialRule.Trim();
                    sRuleChoices = sRule.Split(delimiterChars);
                }
                if ((mixer.Guest != null) && (mixer.Guest.Trim()) == Guest)
                {
                    GuestOption = true; ;
                }
            }
            postDate = tz.eastTimeNow().AddDays(-signupOffset);  // temporary for testing
            enableSignups = (tz.eastTimeNow() >= postDate) && enableSignups;

			//
			// load mixer details  from text file
			//
			path = Server.MapPath("");
			MrLoadMixerInfo mixerInfo = new MrLoadMixerInfo(path, clubSettings, eventID);
			LoadMixerInfo(mixerInfo.MixerLines());
            btnSubmit.Enabled = mixerInfo.PageFileExists;
			lblEventID.Text = eventID;
			//
			// Get the mixer from the events db
			//

			MRParams entry = db.MRParams.FirstOrDefault(p => p.ClubID == clubSettings.ClubID && p.Key == keySignupLimit);
			if (entry != null)
			{
				maxLimit = Convert.ToInt32((string)entry.Value);
			}

			getPlayersList(eventID);
			//
			// Check for deadline already passed
			//
            if (MrSignup.IsClosed(deadline))
            {
                // Deadline is in the past			}
                lblClosed.Text = @"<span class=""closed"">Contact ";
                //                lblSignupForm.Text += "the<br />Host Club at<br />";
                //				  lblSignupForm.Text += hostClubPhone;
//                lblClosed.Text += "your <br />MISGA Rep, <br >";
                string contact = GetParamValue("Contact");
                string cp = GetParamValue("ContactPhone");
                if (contact == "") contact = "the Host Club";
                lblClosed.Text += contact + " at<br />";
                if (cp == "")
                    cp = hostClubPhone;

                lblClosed.Text += cp;

                lblClosed.Text += "<br />to see if you can be added to the list.</span>";
/*                rblAction.Items[0].Selected = false;
                rblAction.Items[1].Selected = true;
                rblAction.Items[0].Enabled = false;
                rblAction.Items[1].Enabled = true;
 * */
                SignupPanel.Visible = false; 
                ClosedPanel.Visible = true;
                ClosedMessagePanel.Visible = true;
                tblGenderPool.Visible = false;
                rblAction.Visible = false;
                cbCarpool.Visible = false;
                lblgender.Visible = false;
                rblgender.Visible = false;
                PlayerPanel.Visible = false;
//                lblFN.Visible = false;
//                lblLN.Visible = false;
//                FirstNameTextBox.Visible = false;
//                LastNameTextBox.Visible = false;
//                cbCarpool.Visible = false;
//                LinkButton1.Visible = false;
                SpecialRulePanel.Visible = false;
                SRCBLPanel.Visible = false;
                SRCBKL.Visible = false;
                SRCBKL.Items.Clear();
                Select_STD.Visible = false;
                linkbuttonSR.Visible = false;
                cbGuestRule.Visible = false;
//                cbSpecialRule.Visible = false;
                GuestPanel.Visible = false;
                if (GuestOption)
                {
                    GuestListPanel.Visible = true;
                    NormalPanel.Visible = false;
                }
                enableSignups = false;
            }
            else
            {
                // enable signup page to accomodate special rule
                if (!IsPostBack)
                {
                    if ((sRuleChoices.Length > 0) && (sRuleChoices[0] != ""))
                    {
                        if (sRuleChoices[0] != noChoicesSpecialCharacter)
                        {
//                            ListItem std = new ListItem("STD", "STD", true);
//                            SRCBKL.Items.Add(std);
                            
                            foreach (string choice in sRuleChoices)
                            {
                                if (choice != "")
                                {
                                    SRCBKL.Items.Add(choice);
                                }
                            }
                            srCount = 0;
                            if (SRCBKL.Items.Count > 0)
                            {
                                Select_STD.Visible = false;
                                SpecialRulePanel.Visible = true;
                                SRCBKL.Visible = true;
                                SRCBLPanel.Visible = true;
                                prevSRSelection = -1;
                                prevSRItem = "";
                            }
                        }
                    }
                }
/*                if (sRule != "")
                                {
                                    cbSpecialRule.Text = sRule;
                                    SpecialRulePanel.Visible = true;
                                    cbSpecialRule.Visible = true;
                                    linkbuttonSR.Visible = true;

                                }   */
                if (GuestOption)
                {
                    SpecialRulePanel.Visible = true;
                    cbGuestRule.Text = "Partner";
                    cbGuestRule.Visible = true;

                    GuestPanel.Visible = true;

                    GuestListPanel.Visible = true;
                    NormalPanel.Visible = false;
                }
            }
            btnSubmit.Enabled = (mixerInfo.PageFileExists && enableSignups);
            SubmitPanel.Visible = (mixerInfo.PageFileExists && enableSignups);
//            btnSubmit.Enabled = (mixerInfo.PageFileExists);
            SignupsDisabledPanel.Visible = false;
            if (btnSubmit.Enabled == false)
            {
                SignupsDisabledPanel.Visible = true;
                PlayerPanel.Visible = false;
                SignupPanel.Visible = false;
            }
		}

		protected void getPlayersList(string eid)
		{
			this.sl = SignupList.LoadFromPlayersListDB(eid);   // load Signup entries from PlayersLists for the selected Event ID
			this.PlayersListRepeater.DataSource = new SignupList[] { this.sl };
			this.PlayersListRepeater.DataBind();
			this.GuestListRepeater.DataSource = new SignupList[] { this.sl };
			this.GuestListRepeater.DataBind();

		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
            clubSettings = (Settings)Session["Settings"];
			ln = LastNameTextBox.Text.Trim();
			fn = FirstNameTextBox.Text.Trim();
			string action = rblAction.SelectedValue;       // Action  = Signup or Cancel
			carpool = cbCarpool.Checked?"Yes":"No";        // Carpool = No or Yes
			tgender = rblgender.SelectedValue;

			if (carpool.Trim() == "No")
			{
				carpool = "";
			}

/*            if (invalidMember(ln, fn))
            {
                errorCode = 5;
            }
*/
            if (IsAccessControlOn)
            {
                if (!AccessControl.IsValidMember(clubSettings, ln, fn, tbAccessCode.Text))
                {
                    errorCode = 6;
                }
            }
			SaveUserInfo(fn, ln, tgender, carpool);
			int gender = rblgender.SelectedIndex + 1;

			lblAction.Text = "";
            lblAction.Visible = false;

            linkbuttonSR.Visible = false;
            spcRule = "";
            int n = 0;
            if (SRCBKL.Items.Count > 0)
            {
                for (int i = 0; i < SRCBKL.Items.Count; i++)
                {
                    if (SRCBKL.Items[i].Selected)
                    {
                        spcRule += SRCBKL.Items[i].Value;
                        n++;
                    }
                }
            }
            if (n > 2) errorCode = 4;

			string eid = eventID;
			string gln = "";
			string gfn = "";
			int ggender = 0;
			string ghcp = tbGHcp.Text;
			int guestID = 0;
			int PlayerID = MrSignup.GetPlayerID(clubSettings.ClubID, ln, fn, gender);

			
			if (cbGuestRule.Checked) 
			{
				gln = tbGLN.Text.Trim();
				gfn = tbGFN.Text.Trim();
				ggender = rblGgender.SelectedIndex + 1;
				guestID = MrSignup.GetGuestID(clubSettings.ClubID, gln, gfn, ghcp, ggender);
				errorCode = MrSignup.ValidateGuestPlaying(clubSettings.ClubID, eid, guestID, PlayerID);
			}
			if (errorCode == 0)
			{
				string errmsg = MrSignup.AddToList(clubSettings.ClubID, eid, PlayerID, action, carpool, gender, spcRule, guestID);
//				lblAction.Text = string.Format("Your {0} for this event was successful.",action);
                lblAction.Text = errmsg;
//				lblAction.ForeColor = System.Drawing.Color.DarkGreen;
                lblAction.ForeColor = System.Drawing.Color.Firebrick;
                lblAction.Font.Bold = true;
                lblAction.Visible = true;
//                LinkButton1.Visible = false;
			}
			if (errorCode == 1)
			{
				lblAction.Text = "Partner already playing.  Signup is unsuccessful.";
				lblAction.ForeColor = System.Drawing.Color.Firebrick;
                lblAction.Visible = true;
//                LinkButton1.Visible = false;
            }
			if (errorCode == 2)
			{
				lblAction.Text = "Partner has played once.  Signup unsuccessful.";
				lblAction.ForeColor = System.Drawing.Color.Firebrick;
                lblAction.Visible = true;
//                LinkButton1.Visible = false;
            }
            if (errorCode == 4)
            {
                lblAction.Text = "Select no more than one Special Rule.  Signup unsuccessful.";
                lblAction.ForeColor = System.Drawing.Color.Firebrick;
                lblAction.Visible = true;

            }
            if (errorCode == 6)
            {
                lblAction.Text = "Invalid Access Code.  Please try again";
                lblAction.ForeColor = System.Drawing.Color.Firebrick;
                lblAction.Visible = true;
            }

			getPlayersList(eventID);

			resetSignupForm();
            if (errorCode == 0)
            {
//                Server.Transfer("Default.aspx");
            }
		}
        protected string GetHostPhone(string HostID)
        {
            string phone = "Unavailable";
            string ClubsConnect = ConfigurationManager.ConnectionStrings["ClubsConnect"].ToString();
            MISGACLUBS cdb = new MISGACLUBS(ClubsConnect);

            Clubs item = cdb.Clubs.SingleOrDefault(c => c.ClubID.Trim() == HostID.Trim());
            if (item != null)
                phone = item.ProPhone;

            return phone;
        }
        protected string GetHostMISGAStdTee(string hostID)
        {
            string stdTee = "MISGA Standard Tee Information is UNAVAILABLE.";
            string yards = "N/A";
            string rating = "N/A";
            string slope = "N/A";
            string par = "NA";
            string ClubsConnect = ConfigurationManager.ConnectionStrings["ClubsConnect"].ToString();
            MISGACLUBS cdb = new MISGACLUBS(ClubsConnect);

            Clubs item = cdb.Clubs.SingleOrDefault(c => c.ClubID.Trim() == hostID.Trim());
            if (item != null)
            {
                if (item.MS_Yards != null) yards = item.MS_Yards.Trim();
                if (item.MS_Rating != null) rating = item.MS_Rating.Trim();
                if (item.MS_Par != null) par = item.MS_Par.Trim();
                if (item.slope != "") slope = item.slope.Trim();
                stdTee = string.Format("MISGA Standard Tee is <b>{0}</b> yards; slope/rating is <b>{1}/{2}</b>; Par is <b>{3}</b>.", yards, slope, rating, par);
            }

            return stdTee;
        }

		protected void resetSignupForm()
		{
//			FirstNameTextBox.Text = userFirstName;
//			LastNameTextBox.Text = userLastName;
			rblAction.SelectedIndex = 0;
//			rblgender.SelectedValue = userGender;
			lblgender.Visible = true;
			cbGuestRule.Checked = false;
//            SRCBKL.Items.Clear();
//            if (sRule != "")

//            SRCBLPanel.Visible = false;
//            cbSpecialRule.Checked = false;
            cbCarpool.Checked = false;
            rbClosedCancel.Checked = false;
			tbGHcp.Text = "";
			tbGFN.Text = "";
			tbGLN.Text = "";
            int j = 0;
            while (j < SRCBKL.Items.Count)
            {
                SRCBKL.Items[j].Selected = false;
                j++;
            }
		}


		protected void rbSpecialRule_CheckedChanged(object sender, EventArgs e)
		{

		}
        protected void rbClosedCancel_CheckedChanged(object sender, EventArgs e)
        {
            ClosedMessagePanel.Visible = false;
            PlayerPanel.Visible = true;
            btnSubmit.Enabled = true;
            rblAction.SelectedItem.Text = "Cancel";
        }
        protected void SRCBKL_SelectedIndexChanged(object sender, EventArgs e)
        {
            srCount++;
            int sndx = SRCBKL.SelectedIndex;
            string sitem = SRCBKL.SelectedValue;
            spcRule = sitem;
            if (SRCBKL.Items.Count > 0)
            {
                for (int i = 0; i < SRCBKL.Items.Count; i++)
                {
                    if (SRCBKL.Items[i].Selected)
                    {
                        if (SRCBKL.Items[i].Value == prevSRItem)
                        {
                            SRCBKL.Items[i].Selected = false;
                            prevSRItem = "";
                            prevSRSelection = -1;
                        }
                    }
                }
            }
            prevSRItem = SRCBKL.SelectedValue;
            prevSRSelection = SRCBKL.SelectedIndex;
        }
        protected void GetCookieUserInfo()
        {
            if (Request.Cookies["userInfo"] != null)
            {
                userLastName =
                    Server.HtmlEncode(Request.Cookies["userInfo"]["userLastName"]);
                userFirstName =
                    Server.HtmlEncode(Request.Cookies["userInfo"]["userFirstName"]);
                userGender =
                    Server.HtmlEncode(Request.Cookies["userInfo"]["userGender"]);

                userLastVisit =
                    Server.HtmlEncode(Request.Cookies["userInfo"]["lastVisit"]);

                if (userGender == "") userGender = "Male";
                if (FirstNameTextBox.Text == "") FirstNameTextBox.Text = userFirstName;
                if (LastNameTextBox.Text == "") LastNameTextBox.Text = userLastName;

                if (rblgender.SelectedValue != userGender) rblgender.SelectedValue = userGender;
            }
        }

        protected void SaveUserInfo(string fn, string ln, string gender, string carpool)
        {
            HttpCookie aCookie = new HttpCookie("userInfo");
            aCookie.Values["userFirstName"] = fn;
            aCookie.Values["userLastName"] = ln;
            aCookie.Values["userGender"] = gender;
            aCookie.Values["lastVisit"] = DateTime.Now.ToString();
            aCookie.Expires = DateTime.Now.AddMinutes(30);
            Response.Cookies.Add(aCookie);
        }
        protected void ResetUserInfo()
        {
            HttpCookie aCookie = new HttpCookie("userInfo");
            aCookie.Values["userFirstName"] = "";
            aCookie.Values["userLastName"] = "";
            aCookie.Values["userGender"] = "";
            aCookie.Values["lastVisit"] = DateTime.Now.ToString();
            aCookie.Expires = DateTime.Now.AddMinutes(120);
            Response.Cookies.Add(aCookie);
        }
        protected void btnSubmit_Click1(object sender, EventArgs e)
        {

        }
        private string GetParamValue(String key)
        {
            string MRMISGADBConn = ConfigurationManager.ConnectionStrings["MRMISGADBConnect"].ToString();
            MRMISGADB db = new MRMISGADB(MRMISGADBConn);

            var prm = db.MRParams.FirstOrDefault(p => (p.ClubID == clubSettings.ClubID) && (p.Key == key));
            if (prm == null) return "";
            return prm.Value;
        }

}

