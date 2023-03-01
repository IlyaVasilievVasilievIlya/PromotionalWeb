namespace PromoWeb.Services.Images;

using AutoMapper;
using PromoWeb.Context.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PromoWeb.Context;

public class UpdateImageModel
{
    public string ImageName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public Stream ImageStream { get; set; }

    public int AppInfoId { get; set; }
    public string ImagePath { get; set; }
}

public class UpdateImageModelValidator : AbstractValidator<UpdateImageModel>
{
    private readonly IDbContextFactory<MainDbContext> contextFactory;

    public UpdateImageModelValidator(IDbContextFactory<MainDbContext> contextFactory)
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(100).WithMessage("Description is long.");

        RuleFor(x => x.ImageName)
            .NotEmpty().WithMessage("Image name is required.")
            .MaximumLength(100).WithMessage("Image name is long.");

        RuleFor(x => x.ImagePath)
            .NotEmpty().WithMessage("Image path is required");
    }

    public async Task<bool> UniqueImageName(string newImageName) //в методе check неасинхронная валидация
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var image = await context.Images.FirstOrDefaultAsync(x => x.ImageName.Equals(newImageName)); //postgre чувствителен к регистру, windows - нет, и здесь тоже (проблемаа)

        if (image == null)
        {
            return true;
        }
        return false;
    }
}

public class UpdateImageModelProfile : Profile
{
    public UpdateImageModelProfile()
    {
        CreateMap<UpdateImageModel, Image>();
    }
}