using Entities;

namespace RepositoryContracts;

public interface ILearningStepRepository
{
    // Note: We get steps BY COURSE ID, not all steps in the database
    Task<IEnumerable<LearningStep>> GetForCourseAsync(int courseId);
    Task<LearningStep> GetSingleAsync(int courseId, int stepOrder);
    Task<LearningStep> AddAsync(LearningStep entity);
    Task UpdateAsync(LearningStep entity);
    Task DeleteAsync(int courseId, int stepOrder);
}