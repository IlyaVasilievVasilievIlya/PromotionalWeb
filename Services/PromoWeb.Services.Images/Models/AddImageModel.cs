namespace PromoWeb.Services.Images;

using AutoMapper;
using PromoWeb.Context.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PromoWeb.Context;

public class AddImageModel
{
    public string ImageName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int AppInfoId { get; set; }
    public string ImagePath { get; set; }
}

public class AddImageModelValidator : AbstractValidator<AddImageModel>
{
    private readonly IDbContextFactory<MainDbContext> contextFactory;

    public AddImageModelValidator(IDbContextFactory<MainDbContext> contextFactory)
    {
        this.contextFactory = contextFactory;

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(100).WithMessage("Description is long.");

        RuleFor(x => x.ImageName)
            .NotEmpty().WithMessage("Image name is required.")
            .MaximumLength(100).WithMessage("Image name is long.");

        RuleFor(x => x.ImagePath)
            .NotEmpty().WithMessage("Image path is required");

        RuleFor(x => x.ImageName).MustAsync(async (value, c) => await UniqueImageName(value))
                .WithMessage("Image name must be unique.");
    }

    public async Task<bool> UniqueImageName(string newImageName)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var image = await context.Images.FirstOrDefaultAsync(x => x.ImageName.ToUpper() == newImageName.ToUpper());

        if (image == null)
        {
            return true;
        }
        return false;
    }
}

public class AddImageModelProfile : Profile
{
    public AddImageModelProfile()
    {
        CreateMap<AddImageModel, Image>();
    }
}