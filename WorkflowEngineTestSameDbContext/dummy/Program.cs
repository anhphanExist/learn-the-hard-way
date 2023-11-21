using Microsoft.EntityFrameworkCore;
using WorkflowEngine;
using TaskStatus = WorkflowEngine.TaskStatus;

namespace dummy;

public static class Program
{
    private static void Main(string[] args)
    {
        using (var context = new TableSplittingContext())
        {
            context.Database.EnsureCreated();

            context.Database.BeginTransaction();

            // var engineTask = context.EngineTasks.FirstOrDefault(x => x.Id == 2);
            //
            // engineTask.Status = TaskStatus.Shipped;
            //
            // context.SaveChanges();
            //
            // var ecommTask = context.EcommTasks.FirstOrDefault(x => x.Id == 2);

            var engineTask = EngineTask.CreatePendingTask();
            
            context.Add(engineTask);
            
            context.SaveChanges();

            var ecommTask = context.EcommTasks.FirstOrDefault(x => x.Id == engineTask.Id);

            ecommTask.Status = TaskStatus.Shipped;
            
            
            context.SaveChanges();
            
            context.Database.CommitTransaction();
        }
    }
}