using System.Collections.Generic;
using System.Linq;

namespace Quantum.DataFlow;

public class ParallelDataFlowJob : IDataFlowJob
{
    private IList<IsADataFlowTask> Tasks { get; }

    private ParallelDataFlowJob(IList<IsADataFlowTask> tasks)
        => Tasks = tasks;

    public class Builder
    {
        private readonly IList<IsADataFlowTask> _tasks = new List<IsADataFlowTask>();

        public static Builder Instace(IsADataFlowTask task) 
            => new Builder(task);

        public Builder(IsADataFlowTask task)
        {
            _tasks.Add(task);
        }

        public Builder AddTask(IsADataFlowTask task)
        {
            _tasks.Add(task);
            return this;
        }

        public ParallelDataFlowJob Build()
        {
            return new ParallelDataFlowJob(_tasks);
        }
    }

    public override async Task<(string key, object value)[]> Execute(DataFlowBag input)
    {
        var valueTuples = await Task.WhenAll(Tasks.Select(t => t.Execute(input)));

        return valueTuples;
    }
}