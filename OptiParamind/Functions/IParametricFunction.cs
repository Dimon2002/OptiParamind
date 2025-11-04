namespace OptiParamind.Functions;

public interface IParametricFunction
{
    IFunction Bind(IVector parameters);
}