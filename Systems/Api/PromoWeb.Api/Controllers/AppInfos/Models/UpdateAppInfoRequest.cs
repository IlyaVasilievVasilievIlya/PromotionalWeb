using AutoMapper;
using FluentValidation;
using PromoWeb.Context.Entities;
using PromoWeb.Services.AppInfos;

namespace PromoWeb.Api.AppInfos
{
    public class UpdateAppInfoRequest
    {
		/// <summary>
		/// AppInfo title
		/// </summary>
		public string TextTitle { get; set; } = string.Empty;

		/// <summary>
		/// AppInfo text
		/// </summary>
		public string Text { get; set; } = string.Empty;

		/// <summary>
		/// Section that the appinfo will belong to
		/// </summary>
		public int SectionId { get; set; }
    }

    public class UpdateAppInfoRequestValidator : AbstractValidator<UpdateAppInfoRequest>
    {
        public UpdateAppInfoRequestValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Text is required.")
                .MaximumLength(5000).WithMessage("Text is too long.");

            RuleFor(x => x.TextTitle)
                .NotEmpty().WithMessage("Text Title is required.")
                .MaximumLength(100).WithMessage("Text Title is too long.");

			RuleFor(x => x.SectionId)
	            .NotEmpty().WithMessage("Section id is required.");
		}
    }

    public class UpdateAppInfoRequestProfile : Profile
    {
        public UpdateAppInfoRequestProfile()
        {
            CreateMap<UpdateAppInfoRequest, UpdateAppInfoModel>();
        }
    }
}
