namespace HeuroLogicV3.Database.Models;

public sealed class Criteria
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Weight { get; set; }
    public Guid DecisionId { get; set; }
    public Decision Decision { get; set; } = null!;
}