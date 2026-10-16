namespace HeuroLogicV3.Database.Models;

public sealed class Option
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid DecisionId { get; set; }
    public Decision Decision { get; set; } = null!;
    public double? FinalScore { get; set; }
}