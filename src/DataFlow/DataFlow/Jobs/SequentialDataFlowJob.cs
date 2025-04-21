using System.Collections.Generic;

namespace Quantum.DataFlow;

public class SequentialDataFlowJob : IDataFlowJob
{
    private Queue<IsADataFlowTask> Tasks { get; }

    private SequentialDataFlowJob(Queue<IsADataFlowTask> tasks)
        => Tasks = tasks;

    public class Builder
    {
        private readonly Queue<IsADataFlowTask> _tasks = new();

        public static Builder Instace(IsADataFlowTask task) => new(task);

        private Builder(IsADataFlowTask task)
            => Enqueue(task);

        public Builder FollowedBy(IsADataFlowTask task)
        {
            Enqueue(task);
            return this;
        }

        private void Enqueue(IsADataFlowTask task)
        {
            _tasks.Enqueue(task);
        }

        public SequentialDataFlowJob Build()
        {
            return new SequentialDataFlowJob(_tasks);
        }
    }

    public override async Task<(string key, object value)[]> Execute(DataFlowBag input)
    {
        (string key, object value) result = default;

        var bag= input.Clone();

        while (Tasks.TryDequeue(out var task))
        {
            result = await task.Execute(input);
            bag[result.key] = result.value;
        }

        return new[] { result };
    }
}