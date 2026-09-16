using ClimaPulse.Models;

namespace ClimaPulse.Services;

/// <summary>Advisory rule — extend with Condition and Message properties when implementing.</summary>
public class AdvisoryRule
{
    // TODO: define Condition (Predicate<AnomalyResult>) and Message (string)
}

public class AdvisoryService
{
    private readonly List<AdvisoryRule> _ruleSet;

    public AdvisoryService(List<AdvisoryRule> ruleSet)
    {
        _ruleSet = ruleSet;
    }

    public List<string> GetAdvisory(AnomalyResult result)
    {
        throw new NotImplementedException();
    }
}
