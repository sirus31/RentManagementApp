using RentManagementApp.Models.RelationshipEntities;
using RentManagementApp.Models.TransactionalEntities;

namespace RentManagementApp.Models.MasterEntities
{
    public class Room
    {
        public int Id { get; set; }

        public int FloorId { get; set; }

        public string RoomNumber { get; set; }

        public bool IsOccupied { get; set; }

        public Floor Floor { get; set; }

        public List<TenantRoom> TenantRooms { get; set; }
    }
}