namespace PersonCRUD.Domain.Models
{
    public class Individual
    {
        public decimal ID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
