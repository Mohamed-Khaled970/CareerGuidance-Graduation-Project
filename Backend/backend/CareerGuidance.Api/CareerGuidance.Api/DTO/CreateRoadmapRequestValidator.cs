namespace CareerGuidance.Api.DTO
{
    public class CreateRoadmapRequestValidator : AbstractValidator<CreateRoadmapRequest>
    {
        public CreateRoadmapRequestValidator()
        {
            RuleFor(x => x.RoadmapName)
                .NotEmpty()
                .WithMessage("Roadmap Name Can't be Empty");
            RuleFor(x => x.Category)
                .NotEmpty()
                .WithMessage("Roadmap Category Can't be Empty");


            RuleFor(x => x.Discription)
                .NotEmpty()
                .WithMessage("Roadmap Discription Can't be Empty");

            RuleFor(x => x.ImageUrl)
                .NotEmpty()
                .WithMessage("Image Can't be Empty");

        }
    }
}
