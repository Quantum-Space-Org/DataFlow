namespace Quantum.DataFlow;

public abstract class IDataFlowJob
{
    public abstract Task<(string key, object value)[]> Execute(DataFlowBag input);
}