using System.Collections.Concurrent;

namespace Quantum.DataFlow;

public class DataFlowBag
{
    private readonly ConcurrentDictionary<string, object> _data = new();
    private string _inputKey = "InputKey";
    private DataFlowBag(ConcurrentDictionary<string, object> data)
        => _data = data;

    public static DataFlowBag Instance => new(new ConcurrentDictionary<string, object>());

    public object this[string key]
    {
        get => _data[key];
        internal set => _data[key] = value;
    }

    public T Get<T>(string key)
        => (T)_data[key];

    public void Set<T>(string key, T value)
    {
        _data[key] = value;
    }

    public T GetInput<T>()
        => (T)_data[_inputKey];

    public object GetInput()
        => _data[_inputKey];

    internal void SetInput((string key, object value) input)
    {
        _inputKey = input.key;
        SetInput(input.value);
    }

    internal void SetInput(object inputValue) => _data[_inputKey] = inputValue;

    public DataFlowBag Clone() => new(_data);
}