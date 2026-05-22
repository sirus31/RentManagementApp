namespace RentManagementApp.DTOs.Responses
{
    public class FloorResponseDto
    {
        public int Id { get; set; }

        public int HouseId { get; set; }

        public int FloorNumber { get; set; }

        public string Name { get; set; } = null!;
    }
}