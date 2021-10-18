namespace Application.Guitars.Queries
{
    public class GuitarDto
    {
        public int Id {  get; set; }

        public int GuitarType { get; set; }

        public int MaxNumberOfStrings { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public DateTime Created {  get; set; }

        public DateTime? Updated { get; set; }

        public List<GuitarStringDto> GuitarStrings { get; set; }        
    }
}