using Corkscrew.SDK.objects;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Corkscrew.ControlCenter.farm
{
    public partial class Configuration : System.Web.UI.Page
    {

        private CSFarm farm = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            farm = CSFarm.Open(WebHelpers.GetSessionUser(HttpContext.Current));

            lvConfigurationSettings.ItemInserting += lvConfigurationSettings_ItemInserting;
            lvConfigurationSettings.ItemEditing += lvConfigurationSettings_ItemEditing;
            lvConfigurationSettings.ItemUpdating += lvConfigurationSettings_ItemUpdating;
            lvConfigurationSettings.ItemDeleting += lvConfigurationSettings_ItemDeleting;
            lvConfigurationSettings.ItemCanceling += lvConfigurationSettings_ItemCanceling;

            if (! IsPostBack)
            {
                BindView();
            }
        }

        private void lvConfigurationSettings_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            HiddenField hf = (HiddenField)lvConfigurationSettings.Items[e.ItemIndex].FindControl("hiddenRowKeyName");
            farm.AllConfiguration.Remove(hf.Value);

            BindView();
        }

        private void lvConfigurationSettings_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            TextBox key = (TextBox)lvConfigurationSettings.Items[e.ItemIndex].FindControl("EditItemKey")
                , value = (TextBox)lvConfigurationSettings.Items[e.ItemIndex].FindControl("EditItemValue");

            farm.AllConfiguration.Update(key.Text, value.Text);

            lvConfigurationSettings.EditIndex = -1;
            BindView();
        }

        private void lvConfigurationSettings_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            TextBox key = (TextBox)e.Item.FindControl("InsertItemKey")
                , value = (TextBox)e.Item.FindControl("InsertItemValue");

            farm.AllConfiguration.Add(key.Text, value.Text);
            BindView();
        }

        private void lvConfigurationSettings_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lvConfigurationSettings.EditIndex = -1;
            BindView();
        }

        private void lvConfigurationSettings_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lvConfigurationSettings.EditIndex = e.NewEditIndex;
            BindView();
        }

        private void BindView()
        {
            lvConfigurationSettings.InsertItemPosition = ((lvConfigurationSettings.EditIndex == -1) ? InsertItemPosition.FirstItem : InsertItemPosition.None);

            lvConfigurationSettings.DataSource = farm.AllConfiguration;
            lvConfigurationSettings.DataBind();
        }
    }
}