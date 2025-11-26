using System;
using RepositoryContracts;

namespace RESTAPI.Controllers;

public class LearningStepsController(IRepositoryID<Entities.LearningStep, (int, int)> repository) : GenericController<Entities.LearningStep, (int, int)>(repository)
{
    protected override Func<string, (int, int)> IdParser { get; } = idStr =>
    {
        var parts = idStr.Split('_');
        if (parts.Length != 2)
        {
            throw new ArgumentException("Invalid ID format. Expected format: 'part1_part2'");
        }
        return (int.Parse(parts[0]), int.Parse(parts[1]));
    };
}
