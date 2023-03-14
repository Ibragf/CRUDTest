using CRUDTest.Models;

namespace CRUDTest
{
    public class DrillBlock
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<Hole> Holes { get; set; }
        public List<DrillBlockPoint> DrillBlockPoints { get; set; }
    }
}
