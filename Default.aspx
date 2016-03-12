<%@ Page Title="MISGA-SignUp Home Page" Language="C#" MasterPageFile="~/misga_site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="pnlTestEnvironment" runat="server">


    <h2>
        Select your Club to view the Schedule:</h2>
    
    <asp:RadioButtonList ID="rblClubs" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" BorderStyle="Solid" BorderColor="#000099" CellPadding="10" Font-Bold="True" BackColor="#99CCFF" OnSelectedIndexChanged="rblClubs_SelectedIndexChanged" AutoPostBack="True">
        <asp:ListItem Value="646">Compass Pointe GC</asp:ListItem>
        <asp:ListItem Value="645">Cross Creek GC</asp:ListItem>
        <asp:ListItem Value="231">The Links at Challedon</asp:ListItem>
        <asp:ListItem Value="216">Holly Hills</asp:ListItem>
        <asp:ListItem Value="232">Musket Ridge GC</asp:ListItem>
        <asp:ListItem Value="229">Rattlewood GC</asp:ListItem>
    </asp:RadioButtonList>
    <asp:Label ID="lblNoClubs" runat="server" Text="Label" Visible="False"></asp:Label>
    <br />
    <table>
        <tr>
            <td style="width: 30%">
                <asp:Button ID="btnAdmin" runat="server" Text="Do Admin Functions" Width="150px" OnClick="btnAdmin_Click" Visible="False" />
                </td>
            <td style="width: 30%">
                <asp:Button ID="btnShowSchedule" runat="server" Text="Show Schedule" OnClick="btnShowSchedule_Click" Width="150px" Enabled="False" Visible="False" />

            </td>
        </tr>
    </table>

<!--    <asp:Table ID="tblClubs" runat="server">
        <asp:TableRow Width="625px">
            <asp:TableCell Wrap="True">
                <asp:HyperLink ID="hl646" runat="server" NavigateUrl="http://misga_signup.org/schedule?CLUB=646">Compass Pointe GC</asp:HyperLink>
</asp:TableCell>
            <asp:TableCell>
                <asp:HyperLink ID="hl645" runat="server" NavigateUrl="http://misga-signup.org/schedule?CLUB=645">Cross Creek GC</asp:HyperLink>
</asp:TableCell>
            <asp:TableCell>
                <asp:HyperLink ID="hl232" runat="server" NavigateUrl="http://misga-signup.org/schedule?CLUB=232">Musket Ridge GC</asp:HyperLink>
</asp:TableCell>
            <asp:TableCell>
                <asp:HyperLink ID="hl229" runat="server" NavigateUrl="http://misga-signup.org/schedule?CLUB=229">Rattlewood GC</asp:HyperLink>
</asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    -->
 
    <asp:Label ID="lblNoSelect" runat="server" ForeColor="Red"></asp:Label>
    </asp:Panel>

    <asp:Panel ID="pnlLive" runat="server">
    <h3 style="color:red;">
    You used an invalid URL to access MISGA-SignUp.org.  
        </h3> 
    <h3>Please contact your MISGA Club's SignUp System Point of Contact to find out the correct URL to use.
        </h3>
        <br />
    </asp:Panel> 
</asp:Content>

