using Microsoft.AspNetCore.Identity;

namespace HeuroLogicV3.Database.Models;

public sealed class Decision
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string UserId { get; set; } = string.Empty;
    public IdentityUser User { get; set; } = null!;
    public string? AlgorithmVersion { get; set; }
    public double? CriteriaConsistencyRatio { get; set; }
    public string? InputSnapshotJson { get; set; }

    public ICollection<Criteria> Criterias { get; set; } = new List<Criteria>();
    public ICollection<Option> Options { get; set; } = new List<Option>();
}