using System.ComponentModel.DataAnnotations;
using WorkflowEngine;
using TaskStatus = WorkflowEngine.TaskStatus;

namespace dummy
{
    public class EcommTask
    {
        public EcommTask()
        {
            
        }
        
        public int Id { get; private set; }
        public TaskStatus? Status { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }

        public EngineTask EngineTask { get; set; }
    }
}