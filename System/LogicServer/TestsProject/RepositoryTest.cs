using System;
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
}
