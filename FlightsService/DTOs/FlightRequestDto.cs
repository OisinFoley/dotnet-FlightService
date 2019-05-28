namespace FlightsService.DTOs
{
    public class FlightRequestDto
    {
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }
        public string Date { get; set; }
        public int BasePrice { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsSpecialOffer { get; set; }
    }
}
