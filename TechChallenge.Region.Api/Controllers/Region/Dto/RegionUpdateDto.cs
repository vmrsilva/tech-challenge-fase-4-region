namespace TechChallenge.Region.Api.Controllers.Region.Dto
{
    public record RegionUpdateDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Ddd { get; init; }
    }
}
