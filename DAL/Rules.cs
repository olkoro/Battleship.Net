using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class Rules
    {
        public int RulesId { get; set; }
        
        [Column(TypeName = "bit")]
        public bool CanTouch { get; set; }
        
        [Column(TypeName = "int")]
        public int Columns { get; set; }
        
        [Column(TypeName = "int")]
        public int Rows { get; set; }
    }
}