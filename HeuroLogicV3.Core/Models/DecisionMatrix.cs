namespace HeuroLogicV3.Core.Models;

public sealed class DecisionMatrix
{
    public List<Criteria> Criterias { get; init; } = new();
    public List<Option> Options { get; init; } = new();
    public double[,] CriteriaPairwiseMatrix { get; init; } = new double[0, 0];
    public Dictionary<Guid, double[,]> OptionPairwiseMatrices { get; init; } = new();
}