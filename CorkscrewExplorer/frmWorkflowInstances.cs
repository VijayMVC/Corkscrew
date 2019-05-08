using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    public partial class frmWorkflowInstances : Form
    {

        private CSWorkflowInstanceCollection _instanceCollection = null;

        public CSFarm Farm
        {
            get;
            set;
        }


        public frmWorkflowInstances()
        {
            InitializeComponent();
        }

        private void frmWorkflowInstances_Shown(object sender, EventArgs e)
        {
            LoadWorkflowInstances();
        }

        private void btnTerminateInstance_Click(object sender, EventArgs e)
        {
            //
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadWorkflowInstances();
        }

        private void cbShowAllInstances_CheckedChanged(object sender, EventArgs e)
        {
            LoadWorkflowInstances();
        }

        private void LoadWorkflowInstances()
        {
            lvWorkflowInstances.Items.Clear();

            CSWorkflowDefinitionCollection definitions = Farm.AllWorkflowDefinitions;
            foreach (CSWorkflowDefinition def in definitions)
            {
                _instanceCollection = (cbShowAllInstances.Checked ? def.AllInstances : def.AllRunnableInstances);
                foreach(CSWorkflowInstance instance in _instanceCollection)
                {
                    ListViewItem item = new ListViewItem(Enum.GetName(typeof(CSWorkflowEventTypesEnum), instance.CurrentState));
                    item.SubItems.Add(instance.Id.ToString("d"));
                    item.SubItems.Add(instance.Association.WorkflowDefinition.Name);
                    item.SubItems.Add(instance.Association.WorkflowDefinition.Id.ToString("d"));
                    item.SubItems.Add(instance.Association.Name);
                    item.SubItems.Add(instance.Association.Id.ToString("d"));

                    lvWorkflowInstances.Items.Add(item);
                }
            }

            btnTerminateInstance.Enabled = false;
        }

        private void lvWorkflowInstances_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnTerminateInstance.Enabled = (lvWorkflowInstances.SelectedItems.Count > 0);
        }

        private void lvWorkflowInstances_ItemActivate(object sender, EventArgs e)
        {
            if (lvWorkflowInstances.SelectedItems.Count > 1)
            {
                lvWorkflowInstances.SelectedItems.Clear();
            }

            Guid instanceId = Utility.SafeConvertToGuid(lvWorkflowInstances.SelectedItems[0].SubItems[1].Text);
            foreach (CSWorkflowInstance instance in _instanceCollection)
            {
                if (instance.Id.Equals(instanceId))
                {
                    CSWorkflowHistoryChain history = instance.GetHistory();

                    DataTable dtLog = new DataTable();
                    dtLog.Columns.Add("State", typeof(string));
                    dtLog.Columns.Add("Completion Reason", typeof(string));
                    dtLog.Columns.Add("Error Message", typeof(string));
                    dtLog.Columns.Add("Timestamp", typeof(DateTime));
                    dtLog.Columns.Add("Persisted State", typeof(string));

                    LinkedListNode<CSWorkflowHistory> node = history.First;
                    while (node != null)
                    {
                        CSWorkflowHistory entry = node.Value;

                        DataRow row = dtLog.NewRow();

                        row["State"] = Enum.GetName(typeof(CSWorkflowEventTypesEnum), entry.State);
                        row["Completion Reason"] = Enum.GetName(typeof(CSWorkflowEventCompletionTypesEnum), entry.CompletedReason);
                        row["Error Message"] = entry.ErrorMessage;
                        row["Timestamp"] = entry.Created;
                        row["Persisted State"] = entry.AssociationData;

                        dtLog.Rows.Add(row);

                        node = node.Next;
                    }

                    if (dtLog.Rows.Count > 0)
                    {
                        using (frmViewLog frm = new frmViewLog())
                        {
                            frm.LogData = dtLog;
                            frm.ShowDialog(this);
                        }
                    }

                    break;
                }
            }
        }

        private void btnFormClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblLogoText_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessFormMove(this.Handle, e);
        }
    }
}
