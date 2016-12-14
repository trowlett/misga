using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

/// <summary>
/// Summary description for SignupList
/// </summary>
public class SignupList
{

	private Collection<SignupEntry> entries = new Collection<SignupEntry>();

	public Collection<SignupEntry> Entries
	{
		get
		{
			return this.entries;
		}
	}

	public static int EntriesPurged { get { return _entriesPurged; } set { _entriesPurged = value; } }
	private static int _entriesPurged = 0;
	public static int CountOfEntires { get { return _countOfEntries; } set { _countOfEntries = value; } }
	private static int _countOfEntries = 0;
    public int Females { get; private set; }

	public static SignupList LoadFromPlayersListDB(string EventID)
	{
        
		string MRMISGADBConn = ConfigurationManager.ConnectionStrings["MRMISGADBConnect"].ToString();
		MRMISGADB db = new MRMISGADB(MRMISGADBConn);

		SignupList target = new SignupList();
        target.Females = 0;
//		var slist =
//			from pl in db.PlayersList
//			join pn in db.Players on pl.PlayerID equals pn.PlayerID
//			where pl.ClubID == Settings.ClubID && pl.EventID == EventID && pl.Marked == 0
//			orderby pl.TransDate
//			select new { pl.TransDate, pl.ClubID, pl.EventID, pn.Name, pn.Sex, pn.Hcp, pl.Action, pl.Carpool, pl.Marked, pl.SpecialRule, pl.GuestID };
        var xlist = 
            from x in db.PlayersList
//                where x.ClubID == Settings.ClubID && x.EventID == EventID && x.Marked == 0
                where x.EventID == EventID && x.Marked == 0
            orderby x.TransDate
                select new { x.TransDate, x.ClubID, x.EventID, x.PlayerID, x.Action, x.Carpool, x.Marked, x.SpecialRule, x.GuestID }; ;

		_countOfEntries = 0;
		foreach (var item in xlist)
		{
            int pid = item.PlayerID;
            var player = db.Players.Single(p => p.ClubID == item.ClubID && p.PlayerID == item.PlayerID);

			_countOfEntries++;
			SignupEntry entry = new SignupEntry(){
                
                ClubID = item.ClubID,
				SeqNo = _countOfEntries,
				STDate = item.TransDate,
				SeventId = item.EventID,
				Splayer = player.Name,
				Shcp = player.Hcp,
				Saction= item.Action,
				Scarpool = item.Carpool,
				Smarked= item.Marked,
				SspecialRule = item.SpecialRule,
				SGuest = item.GuestID
			};
			entry.Ssex = "Male";
            if ((int)player.Sex == 2)
            {
                entry.Ssex = "Female";
                target.Females++;
            }
			if (entry.Scarpool.Trim() == "No") entry.Scarpool = "";
			if (item.GuestID == 0)
			{
				entry.SGuest = 0;
			}
			else
			{
				//				MRParams param = db.MRParams.FirstOrDefault(p => p.Key == keyPlayers);
				Guest guest = db.Guest.FirstOrDefault(g => g.ClubID == item.ClubID && g.GuestID == item.GuestID);
				entry.SGuestName = guest.GuestName;
				entry.SgHcp = guest.gHcp;
				entry.SgSex = "Male";
				if (guest.gSex == 2) entry.SgSex = "Female";
			}
			target.entries.Add(entry);
		}
		SignupList reverseTarget = new SignupList();
		for (int i = target.entries.Count-1; i >= 0; i--)
		{
			reverseTarget.entries.Add(target.entries[i]);
		}
		return reverseTarget;
	}
	public static int CountPlayersActiveSignupEntries(string club, int PlayerID)
	{
		int entryCount = 0;
		string MRMISGADBConn = ConfigurationManager.ConnectionStrings["MRMISGADBConnect"].ToString();
		MRMISGADB db = new MRMISGADB(MRMISGADBConn);

		var plist = 
			from p in db.PlayersList
			where ((p.ClubID == club)  && (p.PlayerID == PlayerID))
			select new {p.PlayerID, p.EventID, p.Marked, p.TransDate};
		if (plist != null)
		{
			foreach (var entry in plist)
			{
				if (entry.Marked == 0) entryCount++;
			}
		}

		return entryCount;
	}

	public static void PurgeMarkedEntries(string club)
	{
		string MRMISGADBConn = ConfigurationManager.ConnectionStrings["MRMISGADBConnect"].ToString();
		MRMISGADB db = new MRMISGADB(MRMISGADBConn);
		var markedEntries =
			from SignupEntries in db.PlayersList
			where SignupEntries.ClubID == club && SignupEntries.Marked > 0
			select SignupEntries;
		_entriesPurged = 0;
		if (markedEntries != null)
		{

			foreach (var entry in markedEntries)
			{
				db.PlayersList.DeleteOnSubmit(entry);
				_entriesPurged++;
			}
			db.SubmitChanges();
		}

	}

	public static int MarkedEntryCount(string club)
	{
		int mc = 0;
		string MRMISGADBConn = ConfigurationManager.ConnectionStrings["MRMISGADBConnect"].ToString();
		MRMISGADB db = new MRMISGADB(MRMISGADBConn);
		var plist =
			from p in db.PlayersList
			where ((p.ClubID == club) && (p.Marked > 0))
			select p;
		if (plist != null)
		{
			foreach (var entry in plist)
			{
				if (entry.Marked > 0) mc++;
			}
		}
		return mc;
	}

	public SignupList()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}