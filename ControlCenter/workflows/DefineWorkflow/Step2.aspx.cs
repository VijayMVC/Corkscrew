using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.Web;

namespace Corkscrew.ControlCenter.workflows.DefineWorkflow
{
    public partial class Step2 : System.Web.UI.Page
    {
        private Guid defId = Guid.Empty;
        private CSFarm farm = null;
        private CSWorkflowDefinition def = null;
        private CSWorkflowManifest defManifest = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            farm = CSFarm.Open(WebHelpers.GetSessionUser(HttpContext.Current));

            if (string.IsNullOrEmpty(Request.QueryString["DefinitionId"]))
            {
                Response.Redirect("/workflows/WorkflowDefinitions.aspx");
            }

            defId = Utility.SafeConvertToGuid(Request.QueryString["DefinitionId"]);
            if (defId == Guid.Empty)
            {
                Response.Redirect("/workflows/WorkflowDefinitions.aspx");
            }

            def = farm.AllWorkflowDefinitions.Find(defId);
            if (def == null)
            {
                Response.Redirect("/workflows/WorkflowDefinitions.aspx");
            }

            defManifest = def.GetManifest();
            if (! IsPostBack)
            {
                DefinitionId.Text = def.Id.ToString("d");

                if (defManifest != null)
                {
                    ExecutionEngine.Items.FindByValue(Enum.GetName(typeof(WorkflowEngineEnum), defManifest.WorkflowEngine)).Selected = true;
                    WorkflowAssemblyName.Text = defManifest.OutputAssemblyName;
                    WorkflowClassName.Text = defManifest.WorkflowClassName;
                    BuildProductTitle.Text = defManifest.BuildAssemblyTitle;
                    BuildProductName.Text = defManifest.BuildAssemblyProduct;
                    BuildProductDescription.Text = defManifest.BuildAssemblyDescription;
                    BuildCompany.Text = defManifest.BuildAssemblyCompany;
                    BuildCopyrightNotice.Text = defManifest.BuildAssemblyCopyright;
                    BuildTrademarkNotice.Text = defManifest.BuildAssemblyTrademark;

                    if (defManifest.AlwaysCompile)
                    {
                        BuildCacheOptions.Items.FindByValue("AlwaysCompile").Selected = true;
                    }
                    else
                    {
                        BuildCacheOptions.Items.FindByValue("AlwaysCache").Selected = true;
                    }

                    BuildVersionMajor.Text = defManifest.BuildAssemblyVersion.Major.ToString();
                    BuildVersionMinor.Text = defManifest.BuildAssemblyVersion.Minor.ToString();
                    BuildVersionBuild.Text = defManifest.BuildAssemblyVersion.Build.ToString();
                    BuildVersionRevision.Text = defManifest.BuildAssemblyVersion.Revision.ToString();

                    BuildFileVersionMajor.Text = defManifest.BuildAssemblyFileVersion.Major.ToString();
                    BuildFileVersionMinor.Text = defManifest.BuildAssemblyFileVersion.Minor.ToString();
                    BuildFileVersionBuild.Text = defManifest.BuildAssemblyFileVersion.Build.ToString();
                    BuildFileVersionRevision.Text = defManifest.BuildAssemblyFileVersion.Revision.ToString();
                }
            }
        }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("/workflows/DefineWorkflow/Step1.aspx?DefinitionId=" + def.Id.ToString("d"));
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            ErrorMessage.Text = "";

            if (ExecutionEngine.SelectedItem == null)
            {
                ErrorMessage.Text = "Please select an execution engine.";
                return;
            }

            WorkflowEngineEnum engine = (WorkflowEngineEnum)Enum.Parse(typeof(WorkflowEngineEnum), ExecutionEngine.SelectedItem.Value);
            if (defManifest == null)
            {
                defManifest = def.CreateManifest(engine, WorkflowAssemblyName.Text, WorkflowClassName.Text, BuildCacheOptions.Items.FindByValue("AlwaysCompile").Selected, BuildCacheOptions.Items.FindByValue("AlwaysCache").Selected);
            }
            else
            {
                defManifest.WorkflowEngine = engine;
                defManifest.OutputAssemblyName = WorkflowAssemblyName.Text;
                defManifest.WorkflowClassName = WorkflowClassName.Text;
            }

            defManifest.BuildAssemblyCompany = BuildCompany.Text;
            defManifest.BuildAssemblyCopyright = BuildCopyrightNotice.Text;
            defManifest.BuildAssemblyDescription = BuildProductDescription.Text;
            defManifest.BuildAssemblyFileVersion = new Version(string.Join(".", BuildFileVersionMajor.Text, BuildFileVersionMinor.Text, BuildFileVersionBuild.Text, BuildFileVersionRevision.Text));
            defManifest.BuildAssemblyProduct = BuildProductName.Text;
            defManifest.BuildAssemblyTitle = BuildProductTitle.Text;
            defManifest.BuildAssemblyTrademark = BuildTrademarkNotice.Text;
            defManifest.BuildAssemblyVersion = new Version(string.Join(".", BuildVersionMajor.Text, BuildVersionMinor.Text, BuildVersionBuild.Text, BuildVersionRevision.Text));
            defManifest.Save();

            if (defManifest.GetItems().Count > 0)
            {
                Response.Redirect("/workflows/DefineWorkflow/EditExistingManifestFiles.aspx?DefinitionId=" + def.Id.ToString("d"));
            }

            Response.Redirect("/workflows/DefineWorkflow/Step3.aspx?DefinitionId=" + def.Id.ToString("d"));
        }
    }
}