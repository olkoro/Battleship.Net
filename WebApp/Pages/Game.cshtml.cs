using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class Game : PageModel
    {
        public GameBoard GameBoard;
        public GameBoard Map;
        public Player Player;
        public string Status = null;
        public async Task OnGetAsync(string where)
        {
            Player = WebUI.Current;
            if (Player.AI)
            {
                Status = WebUI.ShootAI();
            }
            if (where == "go" || where == null|| where =="save")
            {
                //Status = WebUI.ShootAI();
                if (where == "save")
                {
                    if (SaveSystem.GameStates.Count != 0)
                    {
                        if (SaveSystem.GameStates.Last().Status == null)
                        {
                            SaveSystem.GameStates.Last().Status = "[" + SaveSystem.GameStates.Last().P1.ToString() +
                                                                  ": " + SaveSystem.GameStates.Last().P1.Board.Ships.Count +
                                                                  " Ships, " +
                                                                  SaveSystem.GameStates.Last().P2.ToString() +
                                                                  ": " + SaveSystem.GameStates.Last().P2.Board.Ships.Count +
                                                                  " Ships]";
                        }

                        SaveSystem.SavesList.Add(new List<State>(SaveSystem.GameStates));
                
                    }
                    SaveSystem.GameStates = new List<State>();
                    DAL.AppDbContext.SaveToDb();
                }
            }
            else
            {
                Status = WebUI.Shoot(where);
            }
            SaveSystem.GameStates.Add(new State(WebUI.Player1,WebUI.Player2,Rules.CanTouch,WebUI.P2Turn));
            if (WebUI.GetWinner() == null){}
            else
            {
                Status = WebUI.GetWinner().Name;
            }
            GameBoard = Player.Board;
            Map = Player.Map;
            
        }
    }
}