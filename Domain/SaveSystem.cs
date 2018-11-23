using System.Collections.Generic;
//using DbSaveSystem;

namespace Domain
{
    public class SaveSystem
    {
        public static List<State> GameStates = new List<State>();
        public static List<List<State>> SavesList = new List<List<State>>();

        public static void SaveToDb()
        {
            foreach (var save in SavesList)
            {
                
                foreach (var state in save)
                {
                    
                }
            }
        }
    }
}