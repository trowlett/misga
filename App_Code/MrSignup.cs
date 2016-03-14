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

/// <summary>
/// Summary description for MrSignup
/// </summary>
public class MrSignup
{
	public const string keyPlayers = "Players";
	private static DateTime minDate = new DateTime(2011,12,31);

	public static string Proper(string s)
	{
		string txt = "";
		for (int i = 0; i < s.Length; i++)
		{
			if (i == 0)
			{
				txt = "" + (char)s[i];
				txt = txt.ToUpper();
			}
			else
			{
				string tmp = "" + (char)s[i];
				txt = txt + tmp.ToLower();
			}
		}
        txt = txt.ToUpper();
		return txt;
	}


	private static string keyName;
	private const string txtCancel = "Cancel";

	public static int GetPlayerID(string club, string lName, string fName, int sex)
	{
		keyName = lName.Trim().ToUpper() + ", " + fName.Trim().ToUpper();
		MrTimeZone tz = new MrTimeZone();

		
		int pid = 0;
		string MRMISGADBConn = ConfigurationManager.ConnectionStrings["MRMISGADBConnect"].ToString();
		MRMISGADB db = new MRMISGADB(MRMISGADBConn);
		var query =
			from p in db.Players
			where ((p.ClubID == club) && (p.Name == keyName))
			select p;

		Players player = query.FirstOrDefault();     // Lookup Player in Players Datbase
		if (player != null)
		{
			pid = player.PlayerID;
		}
		else{
				Players pl = new Players()
				{   
                    ClubID = club,
					Name = keyName,
					LName = lName.Trim().ToUpper(),
					FName = fName.Trim().ToUpper(),
					Hcp = "NONE",
					MemberID = "",
					Sex = sex,
					Title = "",
					HDate = tz.eastTimeNow(),
                    Delete = 0
					};
				MRParams param = db.MRParams.FirstOrDefault(p => ((p.ClubID == club) && (p.Key == keyPlayers)));
				if (param != null)
				{
					pid = Convert.ToInt32(param.Value);
					pid += 1;
					pl.PlayerID = pid;
					db.Players.InsertOnSubmit(pl);
					param.Value = pid.ToString();
					param.ChangeDate = tz.eastTimeNow();
				}
				else
				{
					MRParams par = new MRParams();
                    par.ClubID = club;
					par.Key = keyPlayers;
					par.Value = "1";
					par.ChangeDate = tz.eastTimeNow();
					db.MRParams.InsertOnSubmit(par);
					pid = 1;
					pl.PlayerID = pid;
					db.Players.InsertOnSubmit(pl);
			}
		}
		db.SubmitChanges();

		return pid;
	}

	public static string AddToList(string club, string EventID, int pID, string Action, string Carpool, int Sex, string sr, int guestID)
	{
		///<Remarks>
		///
		/// Action settings
		///    Signup
		///    Cancel
		///    
		/// Carpool Settings
		///    No  to Carpool
		///    Yes to Carpool
		///     
		////*  Add Player to Players List for the mixer or event specified in'EventID'
		/// *  First task is to find player in the table of 'Players' and if the player cannot be found,
		/// *  to give the player the next PlayerID available then add the player to the 'Players' Table.
		/// *  After completing that then create a players list entry for this player and event.  Then add that entry to the 'PlayersList'
		/// *  Table.
		/// *  
		/// * Then return control to caller with a success indicator as the result.  
		/// * Result of integer Zero indicates successful completion
		/// *           integer One  indicates an unsuccessful completion
		/// *
		/// * */
		/// 

        string result = "";
		//
		string MRMISGADBConn = ConfigurationManager.ConnectionStrings["MRMISGADBConnect"].ToString();
		MRMISGADB db = new MRMISGADB(MRMISGADBConn);
		//
		//
		MrTimeZone tz = new MrTimeZone();
        Events evnt = db.Events.FirstOrDefault(e => ((e.ClubID == club) && (e.EventID == EventID)));
        if (evnt != null)
        {
            Players player = db.Players.FirstOrDefault(x => ((x.ClubID == club) && (x.PlayerID == pID)));
            if (player != null)
            {
                PlayersList delEnt = db.PlayersList.FirstOrDefault(p => p.ClubID == club && p.EventID == EventID && p.PlayerID == pID && p.Marked > 0);
                if (delEnt != null)
                {  // Players List entry os marked for deletion.
                    db.PlayersList.DeleteOnSubmit(delEnt);      // delete old db entry before processing the new one
                    db.SubmitChanges();
                }
                PlayersList entry = db.PlayersList.FirstOrDefault(p => p.ClubID == club && p.EventID == EventID && p.PlayerID == pID && p.Marked == 0);
                if (entry != null)
                {
                    if (Action == txtCancel)
                    {
                        entry.Marked = 1;       // set marked for Delete
                        if (evnt.Date < tz.eastTimeNow().AddHours(24))
                        {
                            result = "Cannot cancel online within 24 hours of Event.<br />Call the Golf Shop.";
                        }
                        else
                        {
                            if (tz.eastTimeNow() > evnt.Deadline)
                            {
                                result = "Cancel recorded.<br />Please notify the Host Golf Shop that you cancelled.";
                            }
                            else
                            {
                                db.PlayersList.DeleteOnSubmit(entry);
                                result = "Cancel successful.";
                            }
                        }
                    }
                    else
                    {                           // modify existing entry in players database
                        entry.Carpool = Carpool;
                        entry.SpecialRule = sr;
                        entry.GuestID = guestID;
                        result = "Modification of entry was sucessful.";
                    }
                }
                else
                {                               // no entry in Playerslist DB for this player and event
                    PlayersList ple = new PlayersList()
                    {                                   // create new entry in players list database
                        ClubID = club,
                        EventID = EventID,
                        PlayerID = pID,
                        TransDate = tz.eastTimeNow(),
                        Marked = 0,                     // set entrymark to zero
                        Action = Action,
                        Carpool = Carpool,
                        SpecialRule = sr,
                        GuestID = guestID
                    };
                    string did = IsSignUpForAnotherEvent(ple);
                    if (Action != txtCancel)
                    {                                       // cannot do a cancel on new entry
                        if (did == "")
                        {
                            db.PlayersList.InsertOnSubmit(ple);
                            result = "Sign-up was successful.";
                        }
                        else
                        {
                            string yy = did.Substring(3, 2);
                            string mm = did.Substring(5, 2);
                            string dd = did.Substring(7, 2);
                            result = string.Format("Sign-up unsuccessful. You are signed up<br />for another Event on Date {1}/{2}/{3}",did,mm,dd,yy);
                        }
                    }
                    else
                    {
                        ple.Marked = 1;        // set to not display canceled entries
                        result = string.Format("Cancel unsuccessful because {0} {1} not Signed Up.", player.FName, player.LName);
                    }
                }
                db.SubmitChanges();
            }
            else
            {
                result = "Player not in database.";
            }
        }
        else
        {
            result = "Event not in database.";
        }
        return result;
	}
    public static string IsSignUpForAnotherEvent(PlayersList pl)
    {
        string signedUpForAnother = "";
        string id = pl.EventID;
        int player = pl.PlayerID;
        string club = pl.ClubID;
        string host = id.Substring(11, 3);          // Host Club ID is last three characters of Event ID/
        string eDate = id.Substring(3, 6);
        int hr = (Convert.ToInt32(id.Substring(9, 2)) < 12) ? 0 : 1;       // get first digit of hour only
        string skey = id.Substring(0, 9);
        string MRMISGADBConn = ConfigurationManager.ConnectionStrings["MRMISGADBConnect"].ToString();
        MRMISGADB db = new MRMISGADB(MRMISGADBConn);

        var dups = from p in db.PlayersList
                   where ((p.ClubID == club) && (p.EventID.Substring(0, 9) == skey) && (p.PlayerID == player) && (p.Marked == 0))
                   select new { p.ClubID, p.PlayerID, p.EventID };
        if (dups != null)
        {
//            bool dup = false;
            signedUpForAnother = "";
            foreach (var item in dups)
            {
                int tmp = (Convert.ToInt32(item.EventID.Substring(9,2)) < 12) ? 0 : 1;
                if (hr == tmp)
                {
                    signedUpForAnother = item.EventID;
//                    dup = true;
                }
            }
        }
        return signedUpForAnother;
    }

	public static int ValidateGuestPlaying(string club, string EventID, int gID, int pID)
	{
		int errorCode = 0;
		string MRMISGADBConn = ConfigurationManager.ConnectionStrings["MRMISGADBConnect"].ToString();
		MRMISGADB db = new MRMISGADB(MRMISGADBConn);

		PlayersList entry = db.PlayersList.FirstOrDefault(p => p.ClubID == club && p.EventID == EventID && p.GuestID == gID && p.Marked == 0);
		if (entry != null)
		{
			if (entry.PlayerID != pID)
			{
				errorCode = 1;       // set error that guest is already playing with another member
			}
		}

		Guest gentry = db.Guest.FirstOrDefault(g => ((g.ClubID == club) && (g.GuestID == gID)));
		if (gentry != null)
		{
			if (gentry.Played > 0)
			{
				errorCode = 2;          // set error that guest has already played as a guest once this year.
			}
		}

		return errorCode;
	}

	public static int GetGuestID(string club, string ln, string fn, string hcp, int gender)
	{
		int id = 0;
		string keyGuest = ln.Trim().ToUpper() + ", " + fn.Trim().ToUpper();
		
		string MRMISGADBConn = ConfigurationManager.ConnectionStrings["MRMISGADBConnect"].ToString();
		MRMISGADB db = new MRMISGADB(MRMISGADBConn);
		int gcount = db.Guest.Count(c => c.ClubID == club);
		Guest ginfo = db.Guest.FirstOrDefault(g => ((g.ClubID == club) && (g.GuestName == keyGuest)));
		if (ginfo != null)
		{
			id = ginfo.GuestID;
			if (hcp.Trim() != "")
			{
				if (ginfo.gHcp.Trim() != hcp.Trim())
				{
					ginfo.gHcp = hcp.Trim();
					db.SubmitChanges();
				}
			}
		}
		else
		{
			id = gcount + 1;
			Guest gin = new Guest
			{
                ClubID = club,
				GuestID = id,
				GuestName = keyGuest,
				gLname = ln.Trim().ToUpper(),
				gFname = fn.Trim().ToUpper(),
				gSex = gender,
				gHcp = hcp,
				Played = 0,
				DatePlayed = minDate
			};
			db.Guest.InsertOnSubmit(gin);
			db.SubmitChanges();
		}
		return id;
	}
    public static bool IsClosed(DateTime deadline)
    {
        MrTimeZone etz = new MrTimeZone();
        return (etz.eastTimeNow() >= deadline);
    }

	public MrSignup()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}