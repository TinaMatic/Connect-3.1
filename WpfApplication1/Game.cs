//name: Matina Matic

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class Game
    {
        public const int ROWS = 6;
        public const int COLS = 7;
        private int numFilledCells;
        private char [ , ] GameBoard;

        private char turn { get; set; }

        public Game(char t = 'X')
        {
            GameBoard = new char[ROWS, COLS];

            turn = t;
            numFilledCells = 0;

            //initialize each cell to empty
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    GameBoard[i, j] = ' ';
                }
            }
        }

        //Copy constructor to make a deep Game copy
        public Game(Game G)
        {
            GameBoard = new char[ROWS, COLS];

            turn = G.turn;
            numFilledCells = G.numFilledCells;

            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    GameBoard[i, j] = G.GameBoard[i, j];
                }
            }
        }

        public char[,] getGameBoard()
        {
            return GameBoard;
        }

        public char getTurn()
        {
            return turn;
        }

        public bool CellIsEmpty(int row, int col){
            return (GameBoard[row, col] == ' ');
        }

        public bool IsValid(int row, int col)
        {
            if (CellIsEmpty(row, col))
            {
                if (row == ROWS - 1)
                {
                    return true;
                }
                else if (!CellIsEmpty(row + 1, col))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public void MarkGameBoard(int row, int col){

            if (!CellIsEmpty(row, col))
            {
                return;
            }
            GameBoard[row, col] = turn;
            if (turn == 'X')
            {
                turn = 'O';
            }
            else
            {
                turn = 'X';
            }
            numFilledCells++;
        }

        public bool BoardIsFull()
        {
            return numFilledCells >= ROWS * COLS;
        }

        public Tuple<char, List<Tuple<int, int>>> checkWinner()
        {
            List<Tuple<int, int>> winningSpots = new List<Tuple<int, int>>();

            //check 
            for (int i = 0; i < ROWS - 1; i++)
            {
                for (int j = 0; j < COLS - 2; j++)
                    {
                        if (GameBoard[i, j] != ' ' && GameBoard[i, j] == GameBoard[i, j + 1] && GameBoard[i, j] == GameBoard[i, j + 2] && GameBoard[i, j] == GameBoard[i + 1, j])
                        {
                            //return GameBoard[i, 0];
                            winningSpots.Add(Tuple.Create(i, j));
                            winningSpots.Add(Tuple.Create(i, j+1));
                            winningSpots.Add(Tuple.Create(i, j+2));
                            winningSpots.Add(Tuple.Create(i + 1, j));
                            return Tuple.Create(GameBoard[i, j], winningSpots);
                        }
                        else if (GameBoard[i, j] != ' ' && GameBoard[i, j] == GameBoard[i, j + 1] && GameBoard[i, j] == GameBoard[i, j + 2] && GameBoard[i, j] == GameBoard[i + 1, j+2])
                        {
                            winningSpots.Add(Tuple.Create(i, j));
                            winningSpots.Add(Tuple.Create(i, j + 1));
                            winningSpots.Add(Tuple.Create(i, j + 2));
                            winningSpots.Add(Tuple.Create(i + 1, j+2));
                            return Tuple.Create(GameBoard[i, j], winningSpots);
                        }
                    }
                }

            for (int i = 1; i < ROWS; i++)
            {
                for (int j = 0; j < COLS - 2; j++)
                {
                    if (GameBoard[i, j] != ' ' && GameBoard[i, j] == GameBoard[i, j + 1] && GameBoard[i, j] == GameBoard[i, j + 2] && GameBoard[i, j] == GameBoard[i - 1, j])
                    {
                        //return GameBoard[i, 0];
                        winningSpots.Add(Tuple.Create(i, j));
                        winningSpots.Add(Tuple.Create(i, j + 1));
                        winningSpots.Add(Tuple.Create(i, j + 2));
                        winningSpots.Add(Tuple.Create(i - 1, j));
                        return Tuple.Create(GameBoard[i, j], winningSpots);
                    }
                    else if (GameBoard[i, j] != ' ' && GameBoard[i, j] == GameBoard[i, j + 1] && GameBoard[i, j] == GameBoard[i, j + 2] && GameBoard[i, j] == GameBoard[i - 1, j + 2])
                    {
                        winningSpots.Add(Tuple.Create(i, j));
                        winningSpots.Add(Tuple.Create(i, j + 1));
                        winningSpots.Add(Tuple.Create(i, j + 2));
                        winningSpots.Add(Tuple.Create(i - 1, j + 2));
                        return Tuple.Create(GameBoard[i, j], winningSpots);
                    }
                }
            }

            for (int i = 0; i < ROWS - 2; i++)
            {
                for (int j = 0; j < COLS - 1; j++)
                {
                    if (GameBoard[i, j] != ' ' && GameBoard[i, j] == GameBoard[i + 1, j] && GameBoard[i, j] == GameBoard[i + 2, j] && GameBoard[i, j] == GameBoard[i + 2, j + 1])
                    {
                        //return GameBoard[i, 0];
                        winningSpots.Add(Tuple.Create(i, j));
                        winningSpots.Add(Tuple.Create(i + 1, j));
                        winningSpots.Add(Tuple.Create(i + 2, j));
                        winningSpots.Add(Tuple.Create(i + 2, j + 1));
                        return Tuple.Create(GameBoard[i, j], winningSpots);
                    }
                    else if (GameBoard[i, j] != ' ' && GameBoard[i, j] == GameBoard[i + 1, j] && GameBoard[i, j] == GameBoard[i + 2, j] && GameBoard[i, j] == GameBoard[i, j + 1])
                    {
                        winningSpots.Add(Tuple.Create(i, j));
                        winningSpots.Add(Tuple.Create(i + 1, j));
                        winningSpots.Add(Tuple.Create(i + 2, j));
                        winningSpots.Add(Tuple.Create(i, j + 1));
                        return Tuple.Create(GameBoard[i, j], winningSpots);
                    }
                }
            }

            for (int i = 0; i < ROWS - 2; i++)
            {
                for (int j = 1; j < COLS; j++)
                {
                    if (GameBoard[i, j] != ' ' && GameBoard[i, j] == GameBoard[i + 1, j] && GameBoard[i, j] == GameBoard[i + 2, j] && GameBoard[i, j] == GameBoard[i + 2, j - 1])
                    {
                        //return GameBoard[i, 0];
                        winningSpots.Add(Tuple.Create(i, j));
                        winningSpots.Add(Tuple.Create(i + 1, j));
                        winningSpots.Add(Tuple.Create(i + 2, j));
                        winningSpots.Add(Tuple.Create(i + 2, j - 1));
                        return Tuple.Create(GameBoard[i, j], winningSpots);
                    }
                    else if (GameBoard[i, j] != ' ' && GameBoard[i, j] == GameBoard[i + 1, j] && GameBoard[i, j] == GameBoard[i + 2, j] && GameBoard[i, j] == GameBoard[i, j  - 1])
                    {
                        winningSpots.Add(Tuple.Create(i, j));
                        winningSpots.Add(Tuple.Create(i + 1, j));
                        winningSpots.Add(Tuple.Create(i + 2, j));
                        winningSpots.Add(Tuple.Create(i, j - 1));
                        return Tuple.Create(GameBoard[i, j], winningSpots);
                    }
                }
            }

            return Tuple.Create(' ', winningSpots);
        }
    }
}
