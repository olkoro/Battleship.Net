using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class Test
    {
        public int TestId { get; set; }
        public string value { get; set; }
        
        [Column(TypeName = "int")]
        public int Number { get; set; }
        
        public string HitOrMiss { get; set; }
    }
}