namespace OptiParamind.Functions;

public class LinearParametricFunction : IParametricFunction
{
    public IFunction Bind(IVector parameters)
    {
        return new LinearFunction(parameters);
    }

    private class LinearFunction(IVector parameters) : IDifferentiableFunction
    {
        public double Value(IVector point)
        {
            if (parameters.Count != point.Count + 1)
            {
                throw new ArgumentException(
                    $"Incorrect number of parameters. \nParameters length: {parameters.Count}, point length: {point.Count}");
            }

            var result = parameters[0];
            for (var i = 0; i < point.Count; i++)
            {
                result += parameters[i + 1] * point[i];
            }

            return result;
        }

        public IVector Gradient(IVector point)
        {
            return new Vector(point.Prepend(1));
        }
    }
}