namespace DAL
{
    public class SaveState
    {
        public int SaveStateId { get; set; }
        
        //FK
        public int SaveId { get; set; }
        public Save Save { get; set; }
        
        public int StateId { get; set; }
        public State State { get; set; }
    }
}