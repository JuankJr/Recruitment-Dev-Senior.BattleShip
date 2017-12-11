using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Codifico.Senior.Core;

namespace Codifico.Senior.Web.Hubs
{
    public class GameHub : Hub
    {
        private static Dictionary<string, Player> OnlineUsers = new Dictionary<string, Player>();
        private static string PlayerTurn = string.Empty;
        private static bool winGame = false;


        public void login(string name, string email)
        {
            if (!OnlineUsers.ContainsKey(email) && OnlineUsers.Count < 2)
            {
                if (PlayerTurn.Equals(string.Empty))
                {
                    PlayerTurn = email;
                }
                Player player = new Player(email, name, Context.ConnectionId);
                OnlineUsers.Add(email, player);
                Clients.Client(Context.ConnectionId).LoadBoard(player._GameBoard);
            }
            else
            {
                if (OnlineUsers.ContainsKey(email))
                {
                    Player player = OnlineUsers[email];
                    player = new Player(email, name, player._ConnectionID);
                    OnlineUsers[email] = player;
                    Clients.Client(player._ConnectionID).LoadBoard(player._GameBoard);
                }
            }
        }
        public void Jugar(string email, string column, string row)
        {
            if (OnlineUsers.Count >= 2)
            {
                if (winGame == false)
                {
                    Player player = OnlineUsers[email];
                    Player Opponent = OnlineUsers.Where(c => c.Key != email).Select(c => c.Value).FirstOrDefault();
                    if (PlayerTurn == player._PlayerEmail)
                    {

                        if (Opponent != null)
                        {
                            string response = Opponent.Jugar(int.Parse(column), int.Parse(row));
                            var crespon = new
                            {
                                column = column,
                                row = row,
                                response = response
                            };
                            Clients.Client(player._ConnectionID).userMove(crespon);

                            Clients.Client(Opponent._ConnectionID).opponentShoot(crespon);
                            if (!(Opponent.IsAliveShips()))
                            {
                                Clients.All.GanoElJugador("Gano el jugador:" + PlayerTurn);
                                winGame = true;
                                return;
                            }
                            PlayerTurn = Opponent._PlayerEmail;
                        }
                    }
                    else
                    {
                        Clients.Client(player._ConnectionID).showMessageGame("Es el turno de: " + Opponent._PlayerName);
                    }
                }
                else
                    Clients.All.GanoElJugador("Gano el jugador:" + PlayerTurn);
            }
            else
            {
                Clients.All.showMessageGame("Falta oponente para Jugar.");
            }
        }
        public void restartGame(string email)
        {
            winGame = false;

            foreach (var item in OnlineUsers)
            {
                if (OnlineUsers.ContainsKey(item.Key))
                {
                    Player player = item.Value;
                    player.NewBoard();
                    Clients.Client(player._ConnectionID).LoadAllBoard(player._GameBoard);
                }
            }




        }


    }

}

