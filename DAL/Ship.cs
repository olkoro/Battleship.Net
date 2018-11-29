using System;
using System.Collections.Generic;
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
        
        public List<ShipsLocation> ShipsLocations { get; set; } = new List<ShipsLocation>();
        
        //FK
        public int GameBoardId { get; set; }
        public GameBoard GameBoard { get; set; }

        public Domain.Ship GetDomainShip()
        {
            var domainship = new Domain.Ship(Length,Health);
            foreach (var location in this.ShipsLocations)
            {
                domainship.Locations.Add(new int[]{location.y,location.x});
            }
            return domainship;
        }


    }
}