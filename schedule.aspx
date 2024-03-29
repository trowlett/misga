﻿<%@ Page Title="MISGA Schedule Page" Language="C#" Debug="true" MasterPageFile="~/Schedule.master" AutoEventWireup="true" CodeFile="schedule.aspx.cs" Inherits="Signup_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
    #pathinfo 
    {
        font-size: x-small;
    }
</style>
		<link href="Styles/schedule.css" rel="Stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    	<div id="print-hdr">
	<h1><%: ClubName %> MISGA Schedule for <%: PhysicalYear %></h1></div>

    <asp:Panel ID="pnlShowSchedVer" runat="server">
    <div id="scheduledate">
        <p style="font-size: xx-small;">
            Schedule as of:  &nbsp;&nbsp;<%: SystemParameters.Get(clubSettings.ClubID, SystemParameters.ScheduleDate) %></p> 
        </div>
        <div id="scheduleShowAll">
            <span class="noPrint">
            <asp:Label ID="lblShowAll" runat="server" Text="Remaining Schedule Shown:&nbsp;&nbsp;"></asp:Label>
            <asp:CheckBox ID="cbShowAll" runat="server" AutoPostBack="True" 
                Text=" check to show the Complete Schedule" 
                OnCheckedChanged="cbShowAll_CheckedChanged" TextAlign="Right" />
            
            </span>

        </div>
        <div id="reset_hdr" style="clear: both;"></div> 


    </asp:Panel>
<asp:Panel ID="pnlShowPath" runat="server" Visible="False">
<div id="pathinfo">
<p>Original Application path: <%: Request.ApplicationPath %></p>
<p>Current virtual path: <%:Request.CurrentExecutionFilePath %></p>
<p>Current virtual path: <%: Request.MapPath(Request.CurrentExecutionFilePath) %></p>
</div>
</asp:Panel>
    <asp:Panel ID="pnlNextYear" runat="server" Visible="False">

    <h2 style="margin: 5px 0px 5px 0px">
    <%: PhysicalYear %> Event Schedule</h2>
    <p>The <%: PhysicalYear %> Schedule is being developed. If you have any questions or comments, please contact <%: ScheduleContactName %> 
         at <a href="mailto:<%: ScheduleContactEmail %>?subject=Schedule Contact from Website"><%: ScheduleContactEmail %></a>. &nbsp;Confirmed 
    events are displayed below.</p>
    </asp:Panel>

    <asp:Panel ID="pnlSchedule" runat="server">
	<div class="sched">
        <div id="preface">
        <div id="mylist">
            <asp:LinkButton ID="lbtnMyList" runat="server" PostBackUrl="~/mylist.aspx">Show My Events</asp:LinkButton> 
        </div>
    	<h2 style="margin: 5px 0px 5px 0px">
		 <%: PhysicalYear %> Event Schedule and Sign Up 
		 <span style="font-size: medium;">(for 
			 <asp:Literal ID="litOrg3" runat="server" Text="[Org]"></asp:Literal> Members ONLY)
       		 </span></h2>
        <asp:Label ID="lblBeginActive" runat="server" Visible="false"><h4 class="alert">Sign ups can be made after <%: BeginActive.ToLongDateString() %>.</h4>
            </asp:Label>
	<p class="noPrint">
		To sign up for an event, click on the line item that has a [sign up] in the left column. 
            The Sign Up page for that event will then be displayed.&nbsp; If the event you want 
            is unmarked, sign ups for it cannot be made at this time.&nbsp; Check back after the 
            Posting Date to sign up.&nbsp; If the event is marked with [closed], click on it to 
            find out how you might be able to sign up for it.</p>
            
		<p style="font-style: italic; font-weight: bold; text-align: center;">
			MISGA fees include: greens fee, cart, range balls, prizes, refreshments (usually coffee and doughnuts), and lunch.
	</p>
    </div>
		<asp:Repeater runat="server" ID="ScheduleRepeater">
		<ItemTemplate>
		<table>
			<tr>
				<th class="selectcol">[sign up]</th>
				<th class="datecol">Date</th>
				<th class="hacol">H/A</th>
				<th class="titlecol">Club</th>
				<th class="costcol">Event Fee*</th>
				<th class='timecol'>Tee Time</th>
				<th class="playerlimit">Player Limit</th>
				<th class="deadlinecol">Deadline</th>
                <th class="postdatecol">Posting Date**</th>
			</tr>

			<asp:Repeater runat="server" DataSource='<%# Eval("Events") %>' Visible="<%# ShowPartOfSchedule %>">
			<ItemTemplate>
				<tr class='<%# ((SysEvent)(Container.DataItem)).EType.ToLower() %>' runat="server" visible='<%# ((SysEvent)(Container.DataItem)).CanSignUp(((Signup_Default)Page).displayDate,((Signup_Default)Page).signupOffset) %>'>
					<td class="selectcol"><a href="Signup.aspx?CLUB=<%# Eval("EClubID") %>&ID=<%# Eval("Id") %>"><%# (((Signup_Default)Page).SignupsAllowed) ? MrSignup.IsClosed(((SysEvent)(Container.DataItem)).EDeadline) ? "[closed]" : "[sign up]" : "Disabled" %></a></td>
					<td class='datecol'><a href='Signup.aspx?CLUB=<%# Eval("EClubID") %>&ID=<%# Eval("Id") %>'><%# ((SysEvent)Container.DataItem).EDate.ToString("ddd, MMM d") %></a></td>
					<td class='hacol'><a href='Signup.aspx?CLUB=<%# Eval("EClubID") %>&ID=<%# Eval("Id") %>'><%# ((SysEvent)Container.DataItem).EType %></a></td>
					<td class='titlecol'><a href='Signup.aspx?CLUB=<%# Eval("EClubID") %>&ID=<%# Eval("Id") %>'><%# ((SysEvent)Container.DataItem).ETitle %></a></td>
					<td class='costcol'><a href='Signup.aspx?CLUB=<%# Eval("EClubID") %>&ID=<%# Eval("Id") %>'><%# ((SysEvent)Container.DataItem).ECost %></a></td>
					<td class='timecol'><a href='Signup.aspx?CLUB=<%# Eval("EClubID") %>&ID=<%# Eval("Id") %>'><%# ((SysEvent)Container.DataItem).ETime %></a></td>
					<td class='playerlimit'><a href='Signup.aspx?CLUB=<%# Eval("EClubID") %>&ID=<%# Eval("Id") %>'><%# ((SysEvent)Container.DataItem).EPlayerLimit %></a></td>                   
					<td class='deadlinecol'><a href='Signup.aspx?CLUB=<%# Eval("EClubID") %>&ID=<%# Eval("Id") %>'><%# ((SysEvent)Container.DataItem).EDeadline.ToString("MMM d") %></a></td>
                    <td class="postdatecol"><a href='Signup.aspx?CLUB=<%# Eval("EClubID") %>&ID=<%# Eval("id") %>'><%# ((SysEvent)Container.DataItem).EPostDate.ToString("MMM d") %></a></td>
				</tr>
				<tr id="Tr1" class='<%# ((SysEvent)(Container.DataItem)).EType.ToLower() %>' runat="server" visible='<%# !((SysEvent)(Container.DataItem)).CanSignUp(((Signup_Default)Page).displayDate,((Signup_Default)Page).signupOffset) %>'>
					<td class='selectcol'></td>
					<td class='datecol'><%# ((SysEvent)Container.DataItem).EDate.ToString("ddd, MMM d") %></td>
					<td class='hacol'><%# ((SysEvent)Container.DataItem).EType %></td>
					<td class='titlecol'><%# ((SysEvent)Container.DataItem).ETitle %></td>
					<td class="center"><%# ((SysEvent)Container.DataItem).ECost %></td>
					<td class='timecol'><%# ((SysEvent)Container.DataItem).ETime %></td>
					<td class='playerlimit'><%# ((SysEvent)Container.DataItem).EPlayerLimit %></td>
					<td class='deadlinecol'><%# ((SysEvent)Container.DataItem).EDeadline.ToString("MMM d") %></td>
                    <td class='postdatecol'><%# ((SysEvent)Container.DataItem).EPostDate.ToString("MMM d") %></td>
				</tr>
			</ItemTemplate>
			</asp:Repeater>
        <asp:Repeater runat="server" DataSource='<%# Eval("Events") %>' Visible="<%# ShowAll %>">
            <ItemTemplate>
                <tr id="Tr3" class='<%# ((SysEvent)(Container.DataItem)).EType.ToLower() %>' runat="server">
					<td class='selectcol'></td>
					<td class='datecol'><%# ((SysEvent)Container.DataItem).EDate.ToString("ddd, MMM d") %></td>
					<td class='hacol'><%# ((SysEvent)Container.DataItem).EType %></td>
					<td class='titlecol'><%# ((SysEvent)Container.DataItem).ETitle %></td>
					<td class="center"><%# ((SysEvent)Container.DataItem).ECost %></td>
					<td class='timecol'><%# ((SysEvent)Container.DataItem).ETime %></td>
					<td class='playerlimit'><%# ((SysEvent)Container.DataItem).EPlayerLimit %></td>
					<td class='deadlinecol'><%# ((SysEvent)Container.DataItem).EDeadline.ToString("MMM d") %></td>
                    <td class='postdatecol'><%# ((SysEvent)Container.DataItem).EPostDate.ToString("MMM d") %></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
		</table>
            <p style="margin-left: 4em;">
                  * Payment options are on the SIGN UP PAGE.
                    <br />
		          ** Posting Date is when SIGN UPS are enabled for this mixer.
                </p>
            <div id="scheduleEnd">
                <p>
                END OF SCHEDULE</p>
            </div>
		</ItemTemplate>
		</asp:Repeater>
	</div>  <!-- sched -->
        </asp:Panel> 
    <asp:Panel ID="pnlNoSchedule" runat="server" Visible="False">
        <h1 style="text-align: center; color: #b22;">No Events Currently Scheduled.</h1>
        
    </asp:Panel>

</asp:Content>

