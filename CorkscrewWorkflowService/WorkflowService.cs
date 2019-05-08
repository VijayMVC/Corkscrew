
using System.ServiceProcess;

namespace CorkscrewWorkflowService
{
    public partial class WorkflowService : ServiceBase
    {
        private CSWorkflowManager _wfManager = null;


        public WorkflowService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // thats all we need to do
            _wfManager = new CSWorkflowManager();
        }

        protected override void OnStop()
        {
            // thats all we need to do
            _wfManager.Dispose();
        }
    }
}
