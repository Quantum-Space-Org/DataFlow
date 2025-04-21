using Quantum.Specification;

namespace Quantum.DataFlow;


public abstract class IsADataFlowTask
{
    public abstract Task<(string key, object value)> Execute(DataFlowBag input);
}