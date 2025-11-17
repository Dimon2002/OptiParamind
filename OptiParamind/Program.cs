using OptiParamind.Functionals;
using OptiParamind.Functions;
using OptiParamind.Optimizators;

namespace OptiParamind.Examples;

public class Program
{
    private static void Main()
    {
        var optimizer = new SimulatedAnnealing();

        Console.WriteLine();
        Console.Write("Введите количество переменных: ");
        var numVariables = int.Parse(Console.ReadLine()!);
        var initial = new Vector(new double[numVariables + 1]);
        
        Console.WriteLine();
        Console.Write("Введите количество точек: ");
        var n = int.Parse(Console.ReadLine()!);

        var nodes = new List<IVector>();
        Console.WriteLine($"Введите {n} x точек ");

        for (var i = 0; i < n; i++)
        {
            Console.Write($"Точка {i + 1}: ");
            var str = Console.ReadLine()!.Split();
            var point = new Vector(str.Select(double.Parse).ToArray());
            nodes.Add(point);
        }

        var fun = new PolynomialParametricFunction();
        var functinal = new L2Norm(nodes, fun.Bind(new Vector([1, 2, 1])));

        var res = optimizer.Minimize(functinal, fun, initial);
        Console.WriteLine("Найденные коэффициенты: ");
        for (var i = 0; i < res.Count; i++)
        {
            Console.WriteLine($"w{i} = {res[i]:F3} ");
        }

        Console.WriteLine();
    }
}