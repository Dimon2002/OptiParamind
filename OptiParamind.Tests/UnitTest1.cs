using OptiParamind.Functionals;
using OptiParamind.Functions;
using OptiParamind.Optimizators;

namespace OptiParamind.Tests;

[TestFixture]
public class SimulatedAnnealingParameterizedTests
{
    private static IVector V(params double[] a) => new Vector(a);
    private static IVector P(params double[] a) => new Vector(a);

    public class OptimizationCase
    {
        public string Description;
        public IParametricFunction Function;
        public IFunctional Functional;
        public IVector Start;
        public IVector Expected;
        public double Tolerance;
    }

    public static IEnumerable<OptimizationCase> Cases
    {
        get
        {
            // ----------- L1Norm + Linear -----------
            yield return new OptimizationCase
            {
                Description = "L1Norm + LinearParametric (1 variable)",
                Function = new LinearParametricFunction(),
                Functional = new L1Norm(
                    new List<IVector> { P(0), P(1), P(2) },
                    new LinearParametricFunction().Bind(V(1, 2))
                ),
                Start = V(0, 0),
                Expected = V(1, 2),
                Tolerance = 0.3
            };

            // ----------- L2Norm + Linear -----------
            yield return new OptimizationCase
            {
                Description = "L2Norm + LinearParametric (1 variable)",
                Function = new LinearParametricFunction(),
                Functional = new L2Norm(
                    new List<IVector> { P(0), P(1), P(2) },
                    new LinearParametricFunction().Bind(V(1, 2))
                ),
                Start = V(0, 0),
                Expected = V(1, 2),
                Tolerance = 0.2
            };

            // ----------- LInfNorm + Linear -----------
            yield return new OptimizationCase
            {
                Description = "LInfNorm + LinearParametric (1 variable)",
                Function = new LinearParametricFunction(),
                Functional = new LInfNorm(
                    new List<IVector> { P(0), P(1), P(2) },
                    new LinearParametricFunction().Bind(V(1, 2))
                ),
                Start = V(0, 0),
                Expected = V(1, 2),
                Tolerance = 0.3
            };

            // ----------- Polynomial degree 2 -----------
            yield return new OptimizationCase
            {
                Description = "Polynomial degree 2: expected [1,2,3]",
                Function = new PolynomialParametricFunction(),
                Functional = new L2Norm(
                    new List<IVector> { P(0), P(1), P(2), P(3) },
                    new PolynomialParametricFunction().Bind(V(1, 2, 3))
                ),
                Start = V(0, 0, 0),
                Expected = V(1, 2, 3),
                Tolerance = 0.5
            };

            // ----------- Quadratic Bezier -----------
            yield return new OptimizationCase
            {
                Description = "QuadraticBezier: [1,2,3]",
                Function = new QuadraticBezierParametricFunction(0, 1),
                Functional = new L2Norm(
                    new List<IVector> { P(0), P(0.5), P(1) },
                    new QuadraticBezierParametricFunction(0, 1).Bind(V(1, 2, 3))
                ),
                Start = V(0, 0, 0),
                Expected = V(1, 2, 3),
                Tolerance = 0.5
            };
        }
    }

    [Test]
    [TestCaseSource(nameof(Cases))]
    public void RunOptimizationTests(OptimizationCase c)
    {
        var optimizer = new SimulatedAnnealing();
        var result = optimizer.Minimize(c.Functional, c.Function, c.Start);

        for (var i = 0; i < c.Expected.Count; i++)
            Assert.That(result[i], Is.EqualTo(c.Expected[i]).Within(c.Tolerance), c.Description);
    }
}