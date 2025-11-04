using OptiParamind.Functions;

namespace OptiParamind.Functionals;

public interface IFunctional
{
    double Value(IFunction function);
}