namespace CRUDTest.Models
{
    public class Hole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DrillBlockId { get; set; }
        public int DEPTH { get; set; }
        public List<HolePoint> HolePoints { get; set; }
    }
}
