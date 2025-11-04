using OptiParamind.Functions;

namespace OptiParamind.Functionals;

public interface ILeastSquaresFunctional : IFunctional
{
    IVector Residual(IFunction function);

    IMatrix Jacobian(IFunction function);
}