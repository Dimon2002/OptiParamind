// See https://aka.ms/new-console-template for more information

using OptiParamind;
using OptiParamind.Functions;

Console.WriteLine("Hello, World!");

var function = new LinearParametricFunction()
    .Bind(new Vector([1, 2, 3]));

if (function is not IDifferentiableFunction differentiableFunction)
{
    throw new ArgumentException("Function is not a differentiable function");
}

IVector point = new Vector([7, 2]);

var value = differentiableFunction.Value(point);
var gradient = differentiableFunction.Gradient(point);

Console.WriteLine(value);
Console.WriteLine(string.Join(",", gradient));

function = new PolynomialParametricFunction().Bind(
    new Vector([1, 2, 3]));

value = function.Value(point);

Console.WriteLine(value);