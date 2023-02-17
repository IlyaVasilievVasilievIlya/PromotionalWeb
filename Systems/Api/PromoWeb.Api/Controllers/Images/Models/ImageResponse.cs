﻿namespace PromoWeb.Api.Images;

using AutoMapper;
using PromoWeb.Context.Entities;
using PromoWeb.Services.Images;

public class ImageResponse
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public byte[] Bytes { get; set; }
    public string FileExtension { get; set; } = string.Empty;

    public int AppInfoId { get; set; }
    public string AppInfo { get; set; } = string.Empty;
}

public class ImageResponseProfile : Profile
{
    public ImageResponseProfile()
    {
        CreateMap<ImageModel, ImageResponse>();
    }
}
