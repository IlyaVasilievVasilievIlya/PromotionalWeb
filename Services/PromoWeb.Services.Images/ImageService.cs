namespace PromoWeb.Services.Images;

using AutoMapper;
using PromoWeb.Common.Exceptions;
using PromoWeb.Common.Validator;
using PromoWeb.Context;
using PromoWeb.Context.Entities;
using Microsoft.EntityFrameworkCore;
using System.IO;

public class ImageService : IImageService
{
    private readonly IDbContextFactory<MainDbContext> contextFactory;
    private readonly IMapper mapper;
    private readonly IModelValidator<AddImageModel> addImageModelValidator;
    private readonly IModelValidator<UpdateImageModel> updateImageModelValidator;

    public ImageService(
        IDbContextFactory<MainDbContext> contextFactory,
        IMapper mapper,
        IModelValidator<AddImageModel> addImageModelValidator,
        IModelValidator<UpdateImageModel> updateImageModelValidator
        )
    {
        this.contextFactory = contextFactory;
        this.mapper = mapper;
        this.addImageModelValidator = addImageModelValidator;
        this.updateImageModelValidator = updateImageModelValidator;
    }

    public async Task<IEnumerable<ImageModel>> GetImages(int offset = 0, int limit = 10)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var images = context
            .Images
            .Include(x => x.AppInfo)
            .AsQueryable();

        images = images
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)));

        var data = (await images.ToListAsync()).Select(image => mapper.Map<ImageModel>(image));

        return data;
    }

    public async Task<ImageModel> GetImage(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var image = await context.Images.Include(x => x.AppInfo).FirstOrDefaultAsync(x => x.Id.Equals(id));

        var data = mapper.Map<ImageModel>(image);

        return data;
    }

    public async Task<ImageModel> AddImage(AddImageModel model)
    {
        addImageModelValidator.CheckAsync(model);

        using var context = await contextFactory.CreateDbContextAsync();

        var image = mapper.Map<Image>(model);
        await context.Images.AddAsync(image);
        context.SaveChanges();

        return mapper.Map<ImageModel>(image);
    }

    public async Task UpdateImage(int imageId, UpdateImageModel model)
    {
        updateImageModelValidator.Check(model);

        using var context = await contextFactory.CreateDbContextAsync();

        var image = await context.Images.FirstOrDefaultAsync(x => x.Id.Equals(imageId));            

        ProcessException.ThrowIf(() => image is null, $"The image (id: {imageId}) was not found");

        string imagePath = image!.ImagePath;
        image = mapper.Map(model, image);
        context.Images.Update(image);
        context.SaveChanges();

        if (model.ImagePath != imagePath)
        {
            using (FileStream stream = new FileStream(model.ImagePath, FileMode.Create))
            {
                await model.ImageStream.CopyToAsync(stream);
            }
            File.Delete(imagePath);
        }
    }

    public async Task DeleteImage(int imageId)
    {
        using var context = await contextFactory.CreateDbContextAsync();


        var image = await context.Images.FirstOrDefaultAsync(x => x.Id.Equals(imageId))
            ?? throw new ProcessException($"The image (id: {imageId}) was not found");
        context.Remove(image);
        context.SaveChanges();
        File.Delete(image.ImagePath);
    }
}
