namespace TruYum.Api.Models
{
    public class Beverage : MenuItem
    {
        // Example specific fields 
        public bool IsIced { get; set; }
        public double VolumeMl { get; set; } = 350; // default 350ml 
    }
}
