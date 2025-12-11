using System;
using Entities;
using FluentAssertions;
using InMemoryRepositories;

namespace TestsProject;

public class RepositoryTest
{
    private class TestEntity : Entities.IIdentifiable<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    [Fact]
    public async Task FullInMemoryRepoLifecycle()
    {
        var repo = new InMemoryRepository<TestEntity, int>();

        // AddAsync

        var entity = new TestEntity { Id = 1, Name = "Test" };
        var addedEntity = await repo.AddAsync(entity);
        entity.Id.Should().Be(addedEntity.Id);
        addedEntity.Id.Should().Be(1);
        entity.Name.Should().Be(addedEntity.Name);
        addedEntity.Name.Should().Be("Test");

        // GetSingleAsync

        var fetchedEntity = await repo.GetSingleAsync(1);
        fetchedEntity.Id.Should().Be(1);
        fetchedEntity.Name.Should().Be("Test");

        // GetMany

        await repo.AddAsync(new TestEntity{ Id = 2, Name = "Test2" });
        await repo.AddAsync(new TestEntity{ Id = 3, Name = "Test3" });

        var allEntities = repo.GetMany().ToList();
        allEntities.Count.Should().Be(3);
        allEntities.Should().Contain(e => e.Id == 1);
        allEntities.Should().Contain(e => e.Id == 2);
        allEntities.Should().Contain(e => e.Id == 3);
        allEntities.Should().Contain(e => e.Name == "Test");
        allEntities.Should().Contain(e => e.Name == "Test2");
        allEntities.Should().Contain(e => e.Name == "Test3");

        // UpdateAsync

        entity = new TestEntity { Id = 1, Name = "Updated Test" };
        var updatedEntity = await repo.UpdateAsync(entity);
        updatedEntity.Name.Should().Be("Updated Test");        

        // DeleteAsync

        await repo.DeleteAsync(entity.Id);
        var entitiesAfterDelete = repo.GetMany().ToList();
        entitiesAfterDelete.Should().NotContain(e => e.Id == entity.Id);
        entitiesAfterDelete.Count.Should().Be(2);

        // ClearAsync

        await repo.ClearAsync();
        var entitiesAfterClear = repo.GetMany().ToList();
        entitiesAfterClear.Count.Should().Be(0);
        
    }

    [Fact]
    public async Task CourseLearningStepInMemoryRepositories()
    {
        var learningStepRepo = new InMemoryRepository<LearningStep, (int, int)>();
        var courseRepo = new InMemoryCourseRepository(learningStepRepo);

        // Add a course
        var course = await courseRepo.AddAsync(new CreateCourseDto { Title = "Test Course", Language = "ENG" });
        course.Id.Should().Be(1);

        // Add learning steps
        var step1 = await learningStepRepo.AddAsync(new LearningStep { CourseId = course.Id, StepOrder = 1, Type = "Text", Content = "Step 1 Content" });
        var step2 = await learningStepRepo.AddAsync(new LearningStep { CourseId = course.Id, StepOrder = 2, Type = "QuestionMC", Content = "Step 2 Content" });

        step1.CourseId.Should().Be(course.Id);
        step1.StepOrder.Should().Be(1);
        step2.CourseId.Should().Be(course.Id);
        step2.StepOrder.Should().Be(2);

        // Verify total steps in single course
        var fetchedCourse = await courseRepo.GetSingleAsync(course.Id);
        fetchedCourse.TotalSteps.Should().Be(2);

        // Verify total steps in all courses
        var allCourses = courseRepo.GetMany().ToList(); 
        allCourses.Count.Should().Be(1);
        allCourses[0].TotalSteps.Should().Be(2);
    }
}
