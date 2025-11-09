namespace OptiParamind.Extensions;

public static class DoubleExtension
{
    public static bool IsEqualsPrecision(this double self, double other, double precision = 1e-15)
    {
        return double.Abs(self - other) <= precision;
    }
}