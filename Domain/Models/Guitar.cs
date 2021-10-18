using Domain.Common;
using Domain.Enums;

namespace Domain.Models
{
    public class Guitar : ModelBase
    {
        public Guitar(GuitarType guitarType, int maxNumberOfStrings, string make, string model)
        {
            GuitarType = guitarType;
            MaxNumberOfStrings = maxNumberOfStrings;
            Make = make;
            Model = model;

            GuitarStrings = new List<GuitarString>();
        }

        public GuitarType GuitarType { get; private set; }

        public int MaxNumberOfStrings { get; private set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public List<GuitarString> GuitarStrings { get; private set; }

        public void String(int number, string gauge, string tuning)
        {
            var guitarString = GuitarStrings.FirstOrDefault(x => x.Number == number);
            if (guitarString != null)
            {
                guitarString.ReString(gauge);
                guitarString.Tune(tuning);
            }
            else
            {
                if (GuitarStrings.Count < MaxNumberOfStrings)
                {
                    GuitarStrings.Add(new GuitarString(number, gauge, tuning));
                }
            }
        }

        public void Tune(int number, string tuning)
        {
            var guitarString = GuitarStrings.FirstOrDefault(x => x.Number == number);
            if (guitarString != null)
            {
                guitarString.Tune(tuning);
            }
        }
    }
}