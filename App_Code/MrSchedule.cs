using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

/// <summary>
/// Summary description for MrSchedule
/// </summary>
public class MrSchedule
{
    private static readonly char[] delimiterChars = { ';' };

    private Collection<SysEvent> events = new Collection<SysEvent>();

    public Collection<SysEvent> Events
    {
        get
        {
            return this.events;
        }
    }

    public string FileName { get; private set; }

    public DateTime CreateTime { get; private set; }

    public int ScheduleYear { get; set; }

    public static MrSchedule LoadFromDB(string club, bool showAll)
    {
        MrSchedule target = new MrSchedule();

        MrTimeZone mtz = new MrTimeZone();
        target.ScheduleYear = mtz.eastTimeNow().Year;
        DateTime etzNow = mtz.eastTimeNow();
        DateTime yesterday = etzNow.Subtract(TimeSpan.FromDays(1));
        if (showAll)
        {
            yesterday = new DateTime(target.ScheduleYear, 1, 1, 0, 1, 0);
        }
        target.CreateTime = DateTime.MinValue;
        
        string MRMISGADBConn = ConfigurationManager.ConnectionStrings["MRMISGADBConnect"].ToString();
        MRMISGADB db = new MRMISGADB(MRMISGADBConn);
        var qevents = 
            from ev in db.Events
//            where (ev.ClubID == club && ev.Date > yesterday)
            where (ev.ClubID == club && ev.Date >= yesterday)
            orderby ev.ClubID, ev.EventID
            select ev;

        foreach (Events item in qevents)
        {
            SysEvent e = new SysEvent()
            {
                EClubID= item.ClubID,
                Id = item.EventID.Trim(),
                EDate = item.Date,
                EType = item.Type.Trim(),
                ETitle = item.Title,
                ECost = item.Cost.Trim(),
                ETime = item.Date.ToString("h:mm t").ToLower(),
                EDeadline = item.Deadline,
                EPlayerLimit = item.PlayerLimit,
                EHostPhone = item.HostPhone,
                ESpecialRule = item.SpecialRule.Trim(),
                EGuest = item.Guest.Trim(),
                EHostID = item.HostID,
                EPostDate = item.PostDate,
                ECreationDate = item.CreationDate
            };
            if (item.PostDate == null) e.EPostDate = DateTime.MinValue;
            if (item.CreationDate == null) e.ECreationDate = target.CreateTime;
            if (e.ECreationDate > target.CreateTime) target.CreateTime = e.ECreationDate;
            target.ScheduleYear = e.EDate.Year;
            target.Events.Add(e);
        }

        return target;
    }
}