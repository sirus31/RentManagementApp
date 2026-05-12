using RentManagementApp.Models.RelationshipEntities;
using RentManagementApp.Models.TransactionalEntities;

namespace RentManagementApp.Models.MasterEntities
{
    public class Floor
    {
        public int Id { get; set; }

        public int HouseId { get; set; }

        public string Name { get; set; }

        public House House { get; set; }

        public List<Room> Rooms { get; set; }
    }
}