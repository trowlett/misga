using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

/// <summary>
/// Summary description for Access Control Security
/// </summary>
public static class AccessControl
{
    private const string controlCode = "4972";

    public static bool IsValidMember(Settings club, string LastName, string FirstName, string AccessCode)
    {
        if (club.ClubInfo.ControlCode != null)
        {
            return AccessCode.Trim() == club.ClubInfo.ControlCode.Trim() ? true : false;
        }
        return false;
    }
}