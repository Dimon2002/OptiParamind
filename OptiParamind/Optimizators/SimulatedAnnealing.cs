using OptiParamind.Functionals;
using OptiParamind.Functions;

namespace OptiParamind.Optimizators;

public class SimulatedAnnealing : IOptimizator
{
    private readonly Random _random;
    private readonly double _initialTemperature;
    private readonly double _minTemperature;
    private readonly double _coolingRate;

    public SimulatedAnnealing(double initialTemperature = 1.0, double minTemperature = 0.01, double coolingRate = 0.99)
    {
        _random = new Random();
        _initialTemperature = initialTemperature;
        _minTemperature = minTemperature;
        _coolingRate = coolingRate;
    }

    public IVector Minimize(
        IFunctional objective, 
        IParametricFunction function, 
        IVector initialParameters,
        IVector? minimumParameters = null, 
        IVector? maximumParameters = null)
    {
        var current = new Vector(initialParameters);
        var best = new Vector(initialParameters);
        var bestValue = objective.Value(function.Bind(best));

        var temperature = _initialTemperature;

        while (temperature > _minTemperature)
        {
            var candidate = GenerateNextParameters(current, temperature, minimumParameters, maximumParameters);

            var candidateValue = objective.Value(function.Bind(candidate));
            var delta = candidateValue - bestValue;

            if (delta < 0 || Math.Exp(-delta / temperature) > _random.NextDouble())
            {
                current = candidate;

                if (candidateValue < bestValue)
                {
                    best = candidate;
                    bestValue = candidateValue;
                }
            }

            temperature *= _coolingRate;
        }

        return best;
    }

    private Vector GenerateNextParameters(
        IVector point, 
        double temperature, 
        IVector? minimumParameters = null,
        IVector? maximumParameters = null)
    {
        var nextPoint = new Vector(point.Count);

        if (minimumParameters != null && maximumParameters != null)
        {
            for (var i = 0; i < point.Count; i++)
            {
                var alpha = _random.NextDouble();
                var z = Math.Sign(alpha - 0.5) * temperature *
                        (Math.Pow(1 + 1 / temperature, Math.Abs(2 * alpha - 1)) - 1);

                nextPoint.Add(point[i] + (maximumParameters[i] - minimumParameters[i]) * z);
            }

            return nextPoint;
        }

        for (var i = 0; i < point.Count; i++)
        {
            var step = temperature * Math.Tan(Math.PI * (_random.NextDouble() - 0.5));
            nextPoint.Add(point[i] + step);
        }

        return nextPoint;
    }
}