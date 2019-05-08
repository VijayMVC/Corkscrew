using Corkscrew.SDK.objects;
using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Corkscrew.ControlCenter.farm
{
    public partial class ContentTypes : System.Web.UI.Page
    {

        private CSFarm farm = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            farm = CSFarm.Open(WebHelpers.GetSessionUser(HttpContext.Current));

            lvDataView.ItemDataBound += lvDataView_ItemDataBound;
            lvDataView.ItemInserting += lvDataView_ItemInserting;
            lvDataView.ItemEditing += lvDataView_ItemEditing;
            lvDataView.ItemUpdating += lvDataView_ItemUpdating;
            lvDataView.ItemDeleting += lvDataView_ItemDeleting;
            lvDataView.ItemCanceling += lvDataView_ItemCanceling;

            if (!IsPostBack)
            {
                BindView();
            }
        }

        void lvDataView_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            BindView(-1);
        }

        void lvDataView_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            HiddenField hiddenItemFileExtension = (HiddenField)lvDataView.Items[e.ItemIndex].FindControl("hiddenItemFileExtension");

            CSMIMEType ct = farm.AllContentTypes.Find(hiddenItemFileExtension.Value);
            if (ct != null)
            {
                ct.Delete();
            }

            RefreshPage(true);
        }

        void lvDataView_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            TextBox tbExtension = (TextBox)lvDataView.InsertItem.FindControl("InsertFilenameExtension"),
                    tbTypeName = (TextBox)lvDataView.InsertItem.FindControl("InsertMIMETypeName");

            if (!tbExtension.Text.StartsWith("."))
            {
                tbExtension.Text = string.Format(".{0}", tbExtension.Text);
            }

            if (tbTypeName.Text.IndexOf("/") < 0)
            {
                tbTypeName.Text = string.Format("application/{0}", tbTypeName.Text);
            }

            farm.AllContentTypes.Add(tbExtension.Text, tbTypeName.Text);
            RefreshPage(true);
        }

        void lvDataView_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            TextBox tbExtension = (TextBox)lvDataView.Items[e.ItemIndex].FindControl("EditFilenameExtension"),
                    tbTypeName = (TextBox)lvDataView.Items[e.ItemIndex].FindControl("EditMIMETypeName");

            if (tbTypeName.Text.IndexOf("/") < 0)
            {
                tbTypeName.Text = string.Format("application/{0}", tbTypeName.Text);
            }

            CSMIMEType ct = farm.AllContentTypes.Find(tbExtension.Text);
            if (ct != null)
            {
                ct.KnownMimeType = tbTypeName.Text;
                ct.Save();
            }

            RefreshPage(true);
        }

        void lvDataView_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            BindView(e.NewEditIndex);
            TextBox tbName = (TextBox)lvDataView.Items[e.NewEditIndex].FindControl("EditFilenameExtension");
            TextBox tbMime = (TextBox)lvDataView.Items[e.NewEditIndex].FindControl("EditMIMETypeName");
            tbName.Focus();
        }

        void lvDataView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (lvDataView.EditIndex != -1)
            {
                if ((e.Item.DisplayIndex != lvDataView.EditIndex) || (e.Item.ItemType == ListViewItemType.InsertItem) || (e.Item.ItemType == ListViewItemType.EmptyItem))
                {
                    e.Item.Visible = false;
                }
            }
        }

        private void BindView(int editIndex = -2)
        {
            if (editIndex >= -1)
            {
                lvDataView.EditIndex = editIndex;
            }

            lvDataView.DataSource = farm.AllContentTypes.OrderBy(ct => ct.FileExtension);
            lvDataView.DataBind();
        }

        private void RefreshPage(bool reload = false)
        {
            Response.Redirect((reload ? "ContentTypes.aspx" : Request.Url.ToString()));
        }
    }
}