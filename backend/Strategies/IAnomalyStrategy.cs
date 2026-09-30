using ClimaPulse.Models;

namespace ClimaPulse.Strategies;

public interface IAnomalyStrategy
{
    AnomalyResult Evaluate(double observed, double baseline);
}
