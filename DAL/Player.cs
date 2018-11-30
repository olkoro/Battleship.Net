using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class Player
    {
        
        public int PlayerId { get; set; } 
        public string Name { get; set; }

        [Column(TypeName = "bit")]
        public bool AI { get; set; }
        
        //FK
        
        public Player()
        {
            Name = "unnamed";
        }

        public Domain.Player GetDomainPlayer(Domain.GameBoard board, Domain.GameBoard map)
        {
            var domainplayer = new Domain.Player(Name,board,map);
            return domainplayer;
        }
    }
}