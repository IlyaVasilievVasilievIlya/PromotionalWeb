namespace PromoWeb.Services.Sections;

using AutoMapper;
using PromoWeb.Common.Exceptions;
using PromoWeb.Common.Validator;
using PromoWeb.Context;
using PromoWeb.Context.Entities;
using Microsoft.EntityFrameworkCore;

public class SectionService : ISectionService
{
    private readonly IDbContextFactory<MainDbContext> contextFactory;
    private readonly IMapper mapper;
    private readonly IModelValidator<AddSectionModel> addSectionModelValidator;
    private readonly IModelValidator<UpdateSectionModel> updateSectionModelValidator;

    public SectionService(
        IDbContextFactory<MainDbContext> contextFactory,
        IMapper mapper,
        IModelValidator<AddSectionModel> addSectionModelValidator,
        IModelValidator<UpdateSectionModel> updateSectionModelValidator
        )
    {
        this.contextFactory = contextFactory;
        this.mapper = mapper;
        this.addSectionModelValidator = addSectionModelValidator;
        this.updateSectionModelValidator = updateSectionModelValidator;
    }

    public async Task<IEnumerable<SectionModel>> GetSections(int offset = 0, int limit = 10)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var sections = context
            .Sections
            .AsQueryable();

        sections = sections
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)));

        var data = (await sections.ToListAsync()).Select(section => mapper.Map<SectionModel>(section));

        return data;
    }

    public async Task<SectionModel> GetSection(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var section = await context.Sections.FirstOrDefaultAsync(x => x.Id.Equals(id));

        var data = mapper.Map<SectionModel>(section);

        return data;
    }

    public async Task<SectionModel> AddSection(AddSectionModel model)
    {
        addSectionModelValidator.Check(model);

        using var context = await contextFactory.CreateDbContextAsync();

        var section = mapper.Map<Section>(model);
        await context.Sections.AddAsync(section);
        context.SaveChanges();

        return mapper.Map<SectionModel>(section);
    }

    public async Task UpdateSection(int sectionId, UpdateSectionModel model)
    {
        updateSectionModelValidator.Check(model);

        using var context = await contextFactory.CreateDbContextAsync();

        var section = await context.Sections.FirstOrDefaultAsync(x => x.Id.Equals(sectionId));

        ProcessException.ThrowIf(() => section is null, $"The section (id: {sectionId}) was not found");

        section = mapper.Map(model, section);

        context.Sections.Update(section);
        context.SaveChanges();
    }

    public async Task DeleteSection(int sectionId)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var section = await context.Sections.FirstOrDefaultAsync(x => x.Id.Equals(sectionId))
            ?? throw new ProcessException($"The section (id: {sectionId}) was not found");

        context.Remove(section);
        context.SaveChanges();
    }
}
