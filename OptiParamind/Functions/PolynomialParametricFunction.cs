namespace OptiParamind.Functions;

public class PolynomialParametricFunction : IParametricFunction
{
    public IFunction Bind(IVector parameters)
    {
        return new PolynomialFunction(parameters);
    }

    private class PolynomialFunction(IVector parameters) : IFunction
    {
        public double Value(IVector point)
        {
            var x = point[0];

            return parameters
                .Select((p, index) => p * double.Pow(x, index))
                .Sum();
        }
    }
}