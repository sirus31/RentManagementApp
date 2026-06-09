namespace RentManagementApp.DTOs.Responses
{
    public class RoomOverviewByFloorResponseDto
    {
        public int FloorId { get; set; }


        public string FloorName { get; set; } = null!;


        public int TotalRooms { get; set; }


        public int OccupiedRooms { get; set; }


        public int VacantRooms { get; set; }


        public List<RoomOverviewResponseDto> Rooms { get; set; }
            = new();
    }
}