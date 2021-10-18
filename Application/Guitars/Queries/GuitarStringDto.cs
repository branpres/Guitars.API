namespace Application.Guitars.Queries
{
    public class GuitarStringDto
    {
        public int Id {  get; set; }

        public int Number { get; set; }

        public string Gauge { get; set; }

        public string Tuning { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }
    }
}