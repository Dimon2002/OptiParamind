using OptiParamind.Functions;

namespace OptiParamind.Functionals;

public class L1Norm : IDifferentiableFunctional
{
    private readonly IReadOnlyList<IVector> _nodes;
    private readonly IVector _y;
 
    public L1Norm(IReadOnlyList<IVector> nodes, IFunction originalFunction)
    {
        _nodes = nodes;
        _y = new Vector(nodes.Select(originalFunction.Value));
    }

    public double Value(IFunction function)
    {
        return _nodes
            .Select((node, index) => double.Abs(function.Value(node) - _y[index]))
            .Sum();
    }

    public IVector Gradient(IFunction function)
    {
        if (function is not IDifferentiableFunction diffFunction)
        {
            throw new ArgumentException("Function must be differentiable for gradient computation");
        }

        var functionalGradient = new Vector(new double[diffFunction.Gradient(_nodes[0]).Count]);
        for (var i = 0; i < _nodes.Count; i++)
        {
            var residual = diffFunction.Value(_nodes[i]) - _y[i];
            var grad = diffFunction.Gradient(_nodes[i]);
            double sign = double.Sign(residual);
            
            for (var j = 0; j < grad.Count; j++)
            {
                functionalGradient[j] += sign * grad[j];
            }
        }
        
        return functionalGradient;
    }
}