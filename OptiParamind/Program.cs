// See https://aka.ms/new-console-template for more information

using OptiParamind;
using OptiParamind.Functions;

Console.WriteLine("Hello, World!");

var parameters = new Vector([1, 2, 3]);
Console.WriteLine("=== Linear Parametric Function Test ===");
var function = new LinearParametricFunction()
    .Bind(parameters);

if (function is not IDifferentiableFunction differentiableFunction)
{
    throw new ArgumentException("Function is not a differentiable function");
}

IVector point = new Vector([7, 2]);

var value = differentiableFunction.Value(point);
var gradient = differentiableFunction.Gradient(point);

Console.WriteLine($"parameters: {string.Join(",", parameters)} | x: ({string.Join(";", point)}) | f(x): {value}");
Console.WriteLine($"parameters: {string.Join(",", parameters)} | x: ({string.Join(";", point)}) | grad = ({string.Join(";", gradient)})");

Console.WriteLine();
Console.WriteLine("=== Polynomial Parametric Function Test ===");

function = new PolynomialParametricFunction().Bind(parameters);
value = function.Value(point);

Console.WriteLine($"parameters: {string.Join(",", parameters)} | x: ({string.Join(";", point)}) | f(x): {value}");

Console.WriteLine();
Console.WriteLine("=== Quadratic Bezier Function Test ===");
parameters = [0, 1, 0];

function = new QuadraticBezierParametricFunction()
    .Bind(parameters);

double[] testPoints = [0, 0.5, 1];

foreach (var x in testPoints)
{
    value = function.Value(new Vector { x });
    Console.WriteLine($"control points: {string.Join(",", parameters)} | x = {x,4:F1} | f(x) = {value:F4}");
}