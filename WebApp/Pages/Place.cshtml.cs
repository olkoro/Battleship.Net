using System;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class Place : PageModel
    {
        public Player Player;
        public void OnGet(string cmd)
        {
            if (cmd.Contains("load-"))
            {
                WebUI.LoadSave(Int32.Parse(cmd.Substring(5)));
            }
            if (cmd == "random")
            {
                WebUI.PlaceRandomly();
            }

            if (cmd == "switch")
            {
                WebUI.Switch();
            }
            Player = WebUI.Current;
            if (cmd == "pvp")
            {
                WebUI.Player2.AI = false;
                WebUI.Player2.Name = "Player 2";
                WebUI.Run();
            }
            if (cmd == "sp")
            {
                WebUI.Run();
                WebUI.AIPlace();
                WebUI.Player2.AI = true;
                WebUI.Player2.Name = "AI";
            }

            if (cmd.Contains(","))
            {
                if (WebUI.ShipInHand != 0 && AI.CheckPlace(Player.Board, WebUI.ParseCoords(cmd), WebUI.ShipInHand, WebUI.RotationInHand))
                {
                    AI.SetPlace(Player.Board, WebUI.ParseCoords(cmd), WebUI.ShipInHand, WebUI.RotationInHand);
                    foreach (var ship in WebUI.Current.Ships)
                    {
                        if (ship.Length == WebUI.ShipInHand)
                        {
                            WebUI.Current.Ships.Remove(ship);
                            WebUI.ShipInHand = 0;
                            break;
                        }
                    }
                }
            }

            if (cmd.Contains("choose-"))
            {
                WebUI.ShipInHand = Int32.Parse(cmd.Substring(7));
            }

            if (cmd == "rotate")
            {
                if (WebUI.RotationInHand)
                {
                    WebUI.RotationInHand = false;
                }
                else
                {
                    WebUI.RotationInHand = true;
                }
                
            }
        }
    }
}