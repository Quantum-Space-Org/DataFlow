using System.Collections.Generic;

namespace Quantum.DataFlow;

public class DataFlow
{
    private readonly DataFlowBag _bag;
    private Queue<IDataFlowJob> _jobs;

    private DataFlow((string key, object value) input)
    {
        _jobs = new Queue<IDataFlowJob>();
        _bag = DataFlowBag.Instance;

        _bag.SetInput(input);
    }

    private DataFlow(object inputValue)
    {
        _jobs = new Queue<IDataFlowJob>();
        _bag = DataFlowBag.Instance;

        _bag.SetInput(inputValue);
    }

    public static DataFlow Instance((string key, object value) input) => new(input);
    public static DataFlow Instance(object inputValue) => new(inputValue);

    public DataFlow AddJob(IDataFlowJob parallelDataFlowJob)
    {
        _jobs.Enqueue(parallelDataFlowJob);
        return this;
    }

    public async Task<DataFlowBag> Execute()
    {
        while (_jobs.TryDequeue(out var job))
        {
            var valueTuples = await job.Execute(_bag);

            foreach (var valueTuple in valueTuples)
            {
                _bag[valueTuple.key] = valueTuple.value;
            }
        }

        return _bag;
    }
}