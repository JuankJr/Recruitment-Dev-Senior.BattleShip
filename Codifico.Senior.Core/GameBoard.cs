using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codifico.Senior.Core
{
    public class GameBoard
    {
        private int _columns;
        private int _rows;
        public char[,] _board { get; set; }
        private const char _emptySpace = 'O';
        private const char _failShoot = 'D';
        private const char _ship = 'B';
        private const char _Xploit = 'X';

        public GameBoard(int Columns, int Rows)
        {
            this._columns = Columns;
            this._rows = Rows;
            _board = new char[_columns, _rows];
            GenerateGBoard();
        }
        private void GenerateGBoard()
        {
            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    _board[i, j] = _emptySpace;
                }
            }
        }
        public void PrintBoard()
        {
            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    Console.Write(_board[i, j].ToString() + "\t");
                }
                Console.WriteLine();
            }
        }

        public char SetMove(int column, int row, char character)
        {
            char actualChar = _board[row, column];
            switch (actualChar)
            {
                case _failShoot:
                    _board[row , column] = character;
                    break;
                case _emptySpace:
                    _board[row, column] = character;
                    break;
                case _ship:
                    _board[row, column] = _Xploit;
                    break;
            }
            return _board[row, column];
        }
        public void setShip(Tuple<int, int> posX, Tuple<int, int> posY)
        {
            if (posX.Item1 == posY.Item1)
            { //vertical
                for (int i = 0; i < (posY.Item2 - posX.Item2); i++)
                {
                    _board[posX.Item1, i] = 'B';
                }
            }
            else//Horizontal
            {
                for (int i = 0; i < (posY.Item1 - posX.Item1); i++)
                {
                    _board[i, posY.Item1] = 'B';
                }
            }
        }
        public bool IsValidShip(Tuple<int, int> posX, Tuple<int, int> posY)
        {
            if (posX.Item1 == posY.Item1)
            { //vertical
                for (int i = 0; i < (posY.Item2 - posX.Item2); i++)
                {
                    if (_board[posX.Item1, i] != _emptySpace)
                        return false;
                }
            }
            else//Horizontal
            {
                for (int i = 0; i < (posY.Item1 - posX.Item1); i++)
                {
                    if (_board[i, posY.Item1] != _emptySpace)
                        return false;
                }
            }
            return true;
        }

        public void LoadShips(Ship Navy)
        {

            CreateShip ship = null;
            bool isValid = true;
            do
            {
                ship = new CreateShip(Navy, this._columns);
                isValid = IsValidShip(ship.getInitialPosition(), ship.getFinalPosition());
            } while (!isValid);

            setShip(ship.getInitialPosition(), ship.getFinalPosition());
        }

        public bool isAliveBoard ()
        {
            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    if(_board[i, j]==_ship)
                        return true;
                }
            }
            return false;
        }
    }
}
