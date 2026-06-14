namespace RentManagementApp.DTOs.Responses
{
    public class PaymentFilterResponseDto
    {
        public List<PaymentFilterHouseDto> Houses { get; set; }
            = new();


        public List<PaymentFilterTenantDto> Tenants { get; set; }
            = new();


        public List<PaymentFilterMonthDto> Months { get; set; }
            = new();


        public List<int> Years { get; set; }
            = new();
    }



    public class PaymentFilterMonthDto
    {
        public int Value { get; set; }


        public string Name { get; set; }
            = string.Empty;
    }


    public class PaymentFilterHouseDto
    {
        public int Id { get; set; }


        public string Name { get; set; }
            = string.Empty;
    }

    public class PaymentFilterTenantDto
    {
        public int Id { get; set; }


        public string Name { get; set; }
            = string.Empty;


        public List<int> HouseIds { get; set; }
            = new();
    }
}