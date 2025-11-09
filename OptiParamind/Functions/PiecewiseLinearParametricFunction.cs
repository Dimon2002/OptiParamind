using OptiParamind.Extensions;

namespace OptiParamind.Functions;

public class PiecewiseLinearParametricFunction : IParametricFunction
{
    public IFunction Bind(IVector parameters)
    {
        if (parameters.Count < 4 || parameters.Count % 2 != 0)
        {
            throw new ArgumentException("Expected even number of parameters >= 4: (x0, y0, x1, y1, ...)");
        }

        var knotCount = parameters.Count / 2;
        var knots = new Vector(knotCount);
        var values = new Vector(knotCount);

        for (var i = 0; i < knotCount; i++)
        {
            knots.Add(parameters[2 * i]);
            values.Add(parameters[2 * i + 1]);

            if (knots[i + 1] < knots[i])
            {
                throw new ArgumentException("Abscissas do not satisfy the condition: x0 < x1 < ... < xN-1");
            }
        }

        return new PiecewiseLinearFunction(knots, values);
    }

    private class PiecewiseLinearFunction(IVector knots, IVector values) : IDifferentiableFunction
    {
        private readonly IVector _slopes = CalculateSlopes(knots, values);

        private static IVector CalculateSlopes(IVector knots, IVector values)
        {
            var slopes = new Vector(knots.Count - 1);
            for (var i = 0; i < knots.Count - 1; i++)
            {
                var deltaX = knots[i + 1] - knots[i];
                slopes.Add((values[i + 1] - values[i]) / deltaX);
            }
            return slopes;
        }

        public double Value(IVector point)
        {
            var x = point[0];

            if (x < knots.First() || x > knots.Last())
            {
                throw new ArgumentOutOfRangeException(nameof(point), "Point is out of domain [x0, xN]");
            }

            if (x.IsEqualsPrecision(knots.Last()))
            {
                return values.Last();
            }
            var segmentIndex = knots.Find(x);
            
            return values[segmentIndex] + _slopes[segmentIndex] * (x - knots[segmentIndex]);
        }

        public IVector Gradient(IVector point)
        {
            var x = point[0];

            if (x <= knots.First() || x >= knots.Last())
            {
                throw new ArgumentOutOfRangeException(nameof(point), "Gradient is not defined outside (x0, xN)");
            }

            var segmentIndex = knots.Find(x);
            if (x.IsEqualsPrecision(knots[segmentIndex]) ||
                x.IsEqualsPrecision(knots[segmentIndex + 1]))
            {
                throw new InvalidOperationException("Gradient is not defined in knots.");
            }

            return new Vector([_slopes[segmentIndex]]);
        }
    }
}
