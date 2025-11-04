namespace OptiParamind.Functions;

public interface IDifferentiableFunction : IFunction
{
    // По параметрам исходной IParametricFunction
    IVector Gradient(IVector point);
}