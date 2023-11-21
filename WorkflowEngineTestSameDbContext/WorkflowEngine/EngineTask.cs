namespace WorkflowEngine
{
    public class EngineTask
    {
        protected EngineTask()
        {
            Status = TaskStatus.Pending;
        }
        
        public int Id { get; internal set; }
        public TaskStatus? Status { get; set; }

        public static EngineTask CreatePendingTask()
        {
            return new EngineTask();
        }
    }
}