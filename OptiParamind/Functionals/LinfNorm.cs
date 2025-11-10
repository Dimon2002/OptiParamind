using OptiParamind.Functions;

namespace OptiParamind.Functionals;

public class LInfNorm : IFunctional
{
    private readonly IReadOnlyList<IVector> _nodes;
    private readonly IVector _y;
    
    public LInfNorm(IReadOnlyList<IVector> nodes, IFunction originalFunction)
    {
        _nodes = nodes;
        _y = new Vector(nodes.Select(originalFunction.Value));
    }
    
    public double Value(IFunction function)
    {
        return _nodes
            .Select((t, index) => function.Value(t) - _y[index])
            .Prepend(0)
            .Max(double.Abs);
    }
}