namespace HeuroLogicV3.Core.Models;

public sealed record AhpMatrixResult(IReadOnlyList<double> Weights, double LambdaMax, double ConsistencyIndex, double ConsistencyRatio)
{
    public bool IsConsistent => ConsistencyRatio <= 0.10 + 1e-12;
}

public sealed record RankedOption(Guid OptionId, string Name, double Score);

public sealed class AhpDecisionResult
{
    public AhpMatrixResult CriteriaAnalysis { get; init; } = null!;
    public IReadOnlyDictionary<Guid, AhpMatrixResult> AlternativeAnalyses { get; init; } = null!;
    public IReadOnlyList<RankedOption> RankedOptions { get; init; } = null!;
    public bool IsConsistent => CriteriaAnalysis.IsConsistent && AlternativeAnalyses.Values.All(result => result.IsConsistent);
}