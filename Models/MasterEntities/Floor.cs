using RentManagementApp.Models.RelationshipEntities;
using RentManagementApp.Models.TransactionalEntities;

namespace RentManagementApp.Models.MasterEntities
{
    public class Floor
    {
        public int Id { get; set; }

        public int HouseId { get; set; }

        public int FloorNumber { get; set; }

        public string Name { get; set; } = null!;

        public House House { get; set; } = null!;

        public List<Room> Rooms { get; set; } = new();
    }
}