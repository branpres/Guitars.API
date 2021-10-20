using Domain.Common;

namespace Domain.Models
{
    public class GuitarString : ModelBase
    {
        public GuitarString(int number, string gauge, string tuning)
        {
            Number = number;
            Gauge = gauge;
            Tuning = tuning;
        }

        public int Number { get; private set; }

        public string Gauge { get; private set; }

        public string Tuning { get; private set; }

        public void ReString(string gauge, string tuning)
        {
            Gauge = gauge;
            Tuning = tuning;
        }

        public void Tune(string tuning)
        {
            Tuning = tuning;
        }
    }
}