using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

/// <summary>
/// Summary description for PrSchedule
/// </summary>
public class PrSchedule
{
	private Collection<PrEvent> pevents = new Collection<PrEvent>();

	public Collection<PrEvent> PEvents
	{
		get
		{
			return this.pevents;
		}
	}

	public int FutureCount { get {return futureCount; } set {futureCount = value;} }
	public int PrevCount { get { return prevCount; } set { prevCount = value; } }
	private static int futureCount;
	private static int prevCount;

	public static PrSchedule GetPlayerSchedule(string club, int playerID, string type)
	{

		PrSchedule target = new PrSchedule();
		string MRMISGADBConn = ConfigurationManager.ConnectionStrings["MRMISGADBConnect"].ToString();
		MRMISGADB db = new MRMISGADB(MRMISGADBConn);
		MrTimeZone mtz = new MrTimeZone();
		DateTime etzNow = mtz.eastTimeNow();

		var ps =
			from pl in db.PlayersList
			where ((pl.ClubID == club) && (pl.PlayerID == playerID) && (pl.Marked == 0))
			join ev in db.Events
			on pl.EventID equals ev.EventID
			orderby ev.EventID
			select new { ev.ClubID, ev.EventID, ev.Date, ev.Type, ev.Title, ev.Cost, ev.Deadline, ev.PostDate, ev.HostPhone, pl.Carpool };
		futureCount = 0;
		prevCount = 0;
		
		if (ps != null)
		{
			foreach (var item in ps)
			{
					PrEvent pe = new PrEvent
					{
                        PrClubID = item.ClubID,
						PrEvID = item.EventID,
						PrDate = item.Date,
						PrType = item.Type,
						PrTitle = item.Title,
						PrCost = item.Cost,
                        PrTime = item.Date.ToString("h:mm t").ToLower(),
						PrDeadline = item.Deadline,
						PrHostPhone = item.HostPhone,
						PrCarpool = item.Carpool
					};
				if (item.Date >= etzNow)
				{
					futureCount++;
					pe.PrCompleted = false;
					target.PEvents.Add(pe);
				}
				else
				{
					if (type == "0")
					{
						prevCount++;
						pe.PrCompleted = true;
						target.PEvents.Add(pe);
					}
				}

			}
		}

		return target;
	}

	public PrSchedule()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}