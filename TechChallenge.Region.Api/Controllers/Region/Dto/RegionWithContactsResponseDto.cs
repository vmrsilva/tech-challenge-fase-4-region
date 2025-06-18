namespace TechChallenge.Region.Api.Controllers.Region.Dto
{
    public record RegionWithContactsResponseDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Ddd { get; init; }
        // public IEnumerable<ContactResponseDto> Contacts { get; init; }
    }
}
