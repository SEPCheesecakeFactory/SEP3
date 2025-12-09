using Entities;
using RepositoryContracts;
using via.sep3.dataserver.grpc;
using CourseCategory = Entities.CourseCategory;
using BlazorApp.Entities;

namespace gRPCRepo;

public class gRPCCourseCategoryRepository(string host, int port) : gRPCRepository<CourseCategory, CreateCourseCategoryDto, CourseCategory, int>(host, port), ICourseCategoryRepository
{
    public override async Task<CourseCategory> AddAsync(CreateCourseCategoryDto entity)
    {
        var request = new CreateCategoryRequest
        {
            Name = entity.Name,
            Description = entity.Descritpion
        };
        var response = await CourseServiceClient.CreateCategoryAsync(request);
        return new CourseCategory
        {
            Id = response.Category.Id,
            Name = response.Category.Name,
            Descritpion = response.Category.Description
        };
    }

    public override IQueryable<CourseCategory> GetMany()
    {
        var response = CourseServiceClient.GetCategories(new GetCategoriesRequest());
        return response.Categories.Select(c => new CourseCategory
        {
            Id = c.Id,
            Name = c.Name,
            Descritpion = c.Description
        }).AsQueryable();
    }

    public override Task<CourseCategory> GetSingleAsync(int id)
    {
        throw new NotImplementedException();
    }

    public override Task<CourseCategory> UpdateAsync(CourseCategory entity)
    {
        throw new NotImplementedException();
    }

    public override Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public override Task ClearAsync()
    {
        throw new NotImplementedException();
    }
}
