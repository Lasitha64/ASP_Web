namespace BookingFrontend.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string PersonName { get; set; }
        public int CentreId { get; set; }

    }
}
