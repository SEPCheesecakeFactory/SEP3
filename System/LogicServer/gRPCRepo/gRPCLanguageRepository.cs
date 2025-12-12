using Entities;
using RepositoryContracts;
using via.sep3.dataserver.grpc;
using Language = Entities.Language;
using BlazorApp.Entities;

namespace gRPCRepo;

public class gRPCLanguageRepository(string host, int port)
    : gRPCRepository<Language, CreateLanguageDto, Language, string>(host, port), IRepositoryID<Language, CreateLanguageDto, Language, string>
{
    public override async Task<Language> AddAsync(CreateLanguageDto entity)
    {
        var request = new CreateLanguageRequest
        {
            Code = entity.Code,
            Name = entity.Name
        };

        var response = await CourseServiceClient.CreateLanguageAsync(request);

        return new Language
        {
            Code = response.Language.Code,
            Name = response.Language.Name
        };
    }

    public override IQueryable<Language> GetMany()
    {
        var response = CourseServiceClient.GetLanguages(new GetLanguagesRequest());

        return response.Languages.Select(l => new Language
        {
            Code = l.Code,
            Name = l.Name
        }).AsQueryable();
    }

    public override Task<Language> GetSingleAsync(string id)
    {
        throw new NotImplementedException();
    }

    public override Task<Language> UpdateAsync(Language entity)
    {
        throw new NotImplementedException();
    }

    public override Task DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public override Task ClearAsync()
    {
        throw new NotImplementedException();
    }
}