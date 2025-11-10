using OptiParamind.Functions;

namespace OptiParamind.Functionals;

public class L2Norm : IDifferentiableFunctional, ILeastSquaresFunctional
{
    private readonly IReadOnlyList<IVector> _nodes;
    private readonly IVector _y;

    public L2Norm(IReadOnlyList<IVector> nodes, IFunction originalFunction)
    {
        _nodes = nodes;
        _y = new Vector(nodes.Select(originalFunction.Value));
    }

    public double Value(IFunction function)
    {
        return _nodes
            .Select((node, index) => function.Value(node) - _y[index])
            .Sum(r => r * r);
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
            for (var j = 0; j < grad.Count; j++)
            {
                functionalGradient[j] += residual * grad[j];
            }
        }

        return functionalGradient;
    }

    public IVector Residual(IFunction function)
    {
        var res = new Vector();
        res.AddRange(_nodes.Select((node, index) => function.Value(node) - _y[index]));
        return res;
    }

    public IMatrix Jacobian(IFunction function)
    {
        if (function is not IDifferentiableFunction diffFunction)
        {
            throw new ArgumentException("Function must be differentiable for Jacobian computation");
        }

        var nRows = _nodes.Count;
        var nCols = diffFunction.Gradient(_nodes[0]).Count;
        var jacobian = new Matrix(nRows, nCols);

        for (var i = 0; i < nRows; i++)
        {
            var grad = diffFunction.Gradient(_nodes[i]);
            for (var j = 0; j < nCols; j++)
            {
                jacobian[i][j] = grad[j];
            }
        }

        return jacobian;
    }
}