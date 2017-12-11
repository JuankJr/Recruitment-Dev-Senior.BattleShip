using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codifico.Senior.Core
{
    public class Player
    {
        public string _ConnectionID { get; set; }

        public string _PlayerEmail{ get; set; }

        public string _PlayerName{ get; set; }

        public GameBoard _GameBoard { get; set; }

        public Player(string Email,string Name,string connectionID)
        {
            this._PlayerEmail = Email;
            this._PlayerName = Name;
            this._ConnectionID = connectionID;
            NewBoard();
        }
        public void NewBoard() {
            this._GameBoard = new GameBoard(8, 8);

            _GameBoard.LoadShips(Ship.Acorsado);
            _GameBoard.LoadShips(Ship.Crucero);
            _GameBoard.LoadShips(Ship.Desctructor);
            _GameBoard.LoadShips(Ship.Desctructor);
        }

        public string Jugar(int column, int row)
        {
            return _GameBoard.SetMove(column, row, 'D').ToString();
        }
        public bool IsAliveShips() {
            return _GameBoard.isAliveBoard();

        }
    }
}
