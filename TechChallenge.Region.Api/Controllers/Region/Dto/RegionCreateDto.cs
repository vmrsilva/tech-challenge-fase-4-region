namespace TechChallenge.Region.Api.Controllers.Region.Dto
{
    public record RegionCreateDto
    {
        public required string Name { get; init; }
        public required string Ddd { get; init; }
    }
}
