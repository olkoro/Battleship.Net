using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class Ship
    {
        public int ShipId { get; set; }
        public string Name { get; set; }
        
        [Column(TypeName = "int")]
        public int Length { get; set; }
        
        [Column(TypeName = "int")]
        public int Health { get; set; }
        
        
    }
}