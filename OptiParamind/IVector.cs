namespace OptiParamind;

public interface IVector : IList<double>;

public class Vector : List<double>, IVector
{
    public Vector()
    {
    }

    public Vector(IEnumerable<double> collection) : base(collection)
    {
    }
    
    public Vector(int dimension) : base(dimension)
    {
    }
}