<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Corkscrew.ControlCenter.Default" %>
<%@ Import Namespace="Corkscrew.SDK.workflow" %>
<%@ Import Namespace="System.Linq" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Dashboard
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Dashboard
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <table>
        <tr>
            <th>Sites</th>
            <th>Users</th>
            <th>Workflow Definitions</th>
        </tr>
        <tr>
            <td style="text-align: center;"><a href="/sites/All.aspx"><% =CurrentFarm.AllSites.Count %></a></td>
            <td style="text-align: center;"><a href="/users/All.aspx"><% =CurrentFarm.AllUsers.Count %></a></td>
            <td style="text-align: center;"><% =(CurrentFarm.IsWorkflowEnabled ? "<a href=\"/workflows/WorkflowDefinitions.aspx\">" + CurrentFarm.AllWorkflowDefinitions.Count.ToString() + "</a>" : "DISABLED") %></td>
        </tr>
    </table>
    <p>&nbsp;</p>
    <% if (CurrentFarm.IsWorkflowEnabled)
        {
            long inProgress = 0, errored = 0;

            foreach(CSWorkflowDefinition def in CurrentFarm.AllWorkflowDefinitions)
            {
                CSWorkflowInstanceCollection.GetInstances(def, false).ToList().ForEach(i => {
                    if (i.CanChangeState)
                    {
                        inProgress++;
                    }
                    else if (i.CurrentState == CSWorkflowEventTypesEnum.Errored)
                    {
                        errored++;
                    }
                });
            }

            %>
    <table>
        <tr>
            <th colspan="2">Workflow Status</th>
        </tr>
        <tr>
            <th>In Progress</th>
            <th>Errored</th>
        </tr>
        <tr>
            <td><%= inProgress %></td>
            <td><%= errored %></td>
        </tr>
    </table>
    <% } %>
    <p>&nbsp;</p>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
