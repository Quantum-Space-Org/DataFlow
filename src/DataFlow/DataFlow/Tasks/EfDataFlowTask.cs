using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Quantum.DataFlow;

public abstract class EfDataFlowTask<T, T1> : IsADataFlowTask where T : class
{
    private readonly DbContext _context;
    private readonly string _key;

    protected EfDataFlowTask(DbContext context, string key)
    {
        _context = context;
        _key = key;
    }

    protected virtual Expression<Func<T, bool>> Specification(DataFlowBag dataFlowBag)
        => _ => true;

    protected abstract T1 Map(T value);

    public override async Task<(string key, object value)> Execute(DataFlowBag input)
    {
        var result = _context.Set<T>().Where(Specification(input)).AsEnumerable().Select(Map).ToList();

        return (_key, result);
    }
}

public abstract class EfDataFlowTask<T, T1, T2> : IsADataFlowTask where T : class
{
    private readonly DbContext _context;
    private readonly string _key;

    protected EfDataFlowTask(DbContext context, string key)
    {
        _context = context;
        _key = key;
    }

    protected virtual Expression<Func<T, bool>> Specification(DataFlowBag dataFlowBag)
        => _ => true;

    protected abstract T1 Map(T value);

    protected abstract T2 Reduce(T1 value);

    public override async Task<(string key, object value)> Execute(DataFlowBag input)
    {
        var listAsync = _context.Set<T>().Where(Specification(input)).AsEnumerable()
            .Select(Map).ToList();

        var result = listAsync.Select(Reduce).ToList();

        return (_key, result);
    }
}

public abstract class EfDataFlowTask<T> : IsADataFlowTask where T : class
{
    private readonly DbContext _context;
    private readonly string _key;

    protected EfDataFlowTask(DbContext context, string key)
    {
        _context = context;
        _key = key;
    }

    protected virtual Expression<Func<T, bool>> Specification(DataFlowBag input)
        => _ => true;

    public override async Task<(string key, object value)> Execute(DataFlowBag input)
    {
        var result = await _context.Set<T>().Where(Specification(input)).ToListAsync();

        return (_key, result);
    }
}