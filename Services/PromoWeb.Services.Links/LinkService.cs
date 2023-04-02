namespace PromoWeb.Services.Links;

using AutoMapper;
using PromoWeb.Common.Exceptions;
using PromoWeb.Common.Validator;
using PromoWeb.Context;
using PromoWeb.Context.Entities;
using Microsoft.EntityFrameworkCore;

public class LinkService : ILinkService
{
    private readonly IDbContextFactory<MainDbContext> contextFactory;
    private readonly IMapper mapper;
    private readonly IModelValidator<AddLinkModel> addLinkModelValidator;
    private readonly IModelValidator<UpdateLinkModel> updateLinkModelValidator;

    public LinkService(
        IDbContextFactory<MainDbContext> contextFactory,
        IMapper mapper,
        IModelValidator<AddLinkModel> addLinkModelValidator,
        IModelValidator<UpdateLinkModel> updateLinkModelValidator
        )
    {
        this.contextFactory = contextFactory;
        this.mapper = mapper;
        this.addLinkModelValidator = addLinkModelValidator;
        this.updateLinkModelValidator = updateLinkModelValidator;
    }

    public async Task<IEnumerable<LinkModel>> GetLinks(int offset = 0, int limit = 10)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var links = context
            .Links
            .Include(x => x.Section)
            .AsQueryable();

        links = links
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)));

        var data = (await links.ToListAsync()).Select(link => mapper.Map<LinkModel>(link));

        return data;
    }
    
    public async Task<IEnumerable<LinkModel>> GetLinksBySectionId(int sectionId)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var links = context
            .Links
            .Include(x => x.Section)
            .Where(x => x.SectionId.Equals(sectionId))
            .AsQueryable();

        var data = (await links.ToListAsync()).Select(link => mapper.Map<LinkModel>(link));

        return data;
    }

    public async Task<LinkModel> GetLink(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var link = await context.Links.Include(x => x.Section).FirstOrDefaultAsync(x => x.Id.Equals(id));

        var data = mapper.Map<LinkModel>(link);

        return data;
    }

    public async Task<LinkModel> AddLink(AddLinkModel model)
    {
        addLinkModelValidator.Check(model);

        using var context = await contextFactory.CreateDbContextAsync();

        var link = mapper.Map<Link>(model);
        await context.Links.AddAsync(link);
        context.SaveChanges();

        return mapper.Map<LinkModel>(link);
    }

    public async Task UpdateLink(int linkId, UpdateLinkModel model)
    {
        updateLinkModelValidator.Check(model);

        using var context = await contextFactory.CreateDbContextAsync();

        var link = await context.Links.FirstOrDefaultAsync(x => x.Id.Equals(linkId));

        ProcessException.ThrowIf(() => link is null, $"The link (id: {linkId}) was not found");

        link = mapper.Map(model, link);

        context.Links.Update(link);
        context.SaveChanges();
    }

    public async Task DeleteLink(int linkId)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var link = await context.Links.FirstOrDefaultAsync(x => x.Id.Equals(linkId))
            ?? throw new ProcessException($"The link (id: {linkId}) was not found");

        context.Remove(link);
        context.SaveChanges();
    }
}
