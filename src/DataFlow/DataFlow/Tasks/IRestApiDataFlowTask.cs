using System;
using System.Net.Http;

namespace Quantum.DataFlow;

public abstract class IRestApiDataFlowTask<T> : IsADataFlowTask
{
    private readonly string _key;

    protected IRestApiDataFlowTask(string key)
        => _key = key;

    protected abstract Uri GETUri();
    
    protected virtual Task<T> Map(DataFlowBag dataFlowBag, HttpResponseMessage response)
        => ReduceTo<T>(response);

    protected async Task<TReduce> ReduceTo<TReduce>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return Newtonsoft.Json.JsonConvert.DeserializeObject<TReduce>(content);
    }

    public override async Task<(string key, object value)> Execute(DataFlowBag input)
    {
        var httpResponseMessage = await new HttpClient().GetAsync(GETUri());
        var map = await Map(input, httpResponseMessage);

        return (_key, map);
    }
}