using FluentValidation;
using TechChallenge.Region.Api.Controllers.Region.Dto;

namespace TechChallenge.Region.Api.Validators.Region
{
    public class RegionUpdateValidator : AbstractValidator<RegionUpdateDto>
    {
        public RegionUpdateValidator()
        {
            RuleFor(c => c.Name)
           .NotEmpty()
           .WithMessage("O nome é obrigatório.")
           .MaximumLength(100)
           .WithMessage("O nome não pode exceder 50 caracteres.");

            RuleFor(c => c.Ddd)
              .NotEmpty()
              .WithMessage("O DDD é obrigatório.")
              .MaximumLength(3)
              .WithMessage("O DDD não pode exceder 3 caracteres.");

            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O Id é obrigatório.");
        }
    }
}
