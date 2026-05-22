namespace RentManagementApp.DTOs.Responses
{
    public class HouseResponseDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; } = null!;

        public string Address { get; set; } = null!;

        public decimal ElectricityRate { get; set; }

        public decimal GarbageFee { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}