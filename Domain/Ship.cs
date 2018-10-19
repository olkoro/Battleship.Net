using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Ship
    {
        public ShipType Type; 
        public int Length { get; set; }
        public List<int[]> Locations { get; set; } = new List<int[]>();
        public int Health { get; set; }

        public Ship(int length)
        {
            Length = length;
            Health = length;
        }
        
        public void SetHealth(int health)
        {
            Health = health;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Length; i++)
            {
                sb.Append("+ ");
            }
            return sb.ToString();
        }
    }
}