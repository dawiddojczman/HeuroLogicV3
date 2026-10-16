namespace HeuroLogicV3.Core.Models;

public sealed class Criteria
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
}