using OptiParamind.Functions;

namespace OptiParamind.Functionals;

public interface IDifferentiableFunctional : IFunctional
{
    IVector Gradient(IFunction function);
}