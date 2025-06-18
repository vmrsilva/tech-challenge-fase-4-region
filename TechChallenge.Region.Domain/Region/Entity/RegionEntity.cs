using System.ComponentModel.DataAnnotations;
using TechChallenge.Region.Domain.Base.Entity;

namespace TechChallenge.Region.Domain.Region.Entity
{
    public class RegionEntity : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(3)]
        public string Ddd { get; set; }
        //public IList<ContactEntity>? Contacts { get; set; }

        public RegionEntity(string name, string ddd)
        {
            Name = name;
            Ddd = ddd;
        }
    }
}
