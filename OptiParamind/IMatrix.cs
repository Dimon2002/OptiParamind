namespace OptiParamind;

public interface IMatrix : IList<IList<double>>;

public class Matrix : List<IList<double>>, IMatrix
{
    public Matrix(int n) : this(n, n)
    {
    }

    public Matrix(int n, int m) : base(n)
    {
        for (var i = 0; i < n; i++)
        {
            this[i] = new List<double>(m);
        }
    }
}