using Microsoft.EntityFrameworkCore;

namespace CRUDTest.Models
{
    [Index("Sequence", IsUnique = true)]
    [Index("X","Y","Z", IsUnique =true)]
    public class DrillBlockPoint
    {
        public int Id { get; set; }
        public int DrillBlockId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Sequence { get; set; }
    }
}
