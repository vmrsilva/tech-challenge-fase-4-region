namespace TechChallenge.Region.Domain.Region.Exception
{
    public class RegionAlreadyExistsException : System.Exception
    {
        public RegionAlreadyExistsException() : base(message: "Região já cadastrada.")
        {

        }
    }
}
