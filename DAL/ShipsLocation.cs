using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class ShipsLocation
    {
        public int ShipsLocationId { get; set; }
        
        [Column(TypeName = "int")]
        public int x { get; set; }
        
        [Column(TypeName = "int")]
        public int y { get; set; }
        //FK
        
        public int ShipId { get; set; }
        public Ship Ship { get; set; }


        public ShipsLocation()
        {
        }
    }
}