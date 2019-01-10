using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class Spot
    {
        public int row;
        public int col;
        public int score;
        public Spot()
        {
            row = -1;
            col = -1;
            score = 0;
        }
    }
    class AI_PLayer
    {
        public Spot GetBestMove(Game G, int difficulty)
        {
            return Minimax(G, 0, difficulty, G.getTurn());
        }

        private Spot Minimax(Game G, int level, int maxLevel, char turn)
        {
            Spot BestSpot = new Spot();

            //if current AI player
            if (level % 2 == 0)
            {
                BestSpot.score = -1000000;
            }
            //if AI opponent
            else
            {
                BestSpot.score = 1000000;
            }

            //if there is a winner or we are as deep as we are allowed to go
            if (G.checkWinner().Item1 != ' ' || G.BoardIsFull() || level >= maxLevel)
            {
                BestSpot.score = scoreGame(G, level + 1, turn);
                //MessageBox.Show(string.Format("Score is {0}, level is {1}", BestSpot.score, level));
            }
            else
            {
                //note that consts in C# are static, meaning that to access 
                //them you should use the class name
                for (int i = 0; i < Game.ROWS; i++)
                {
                    for (int j = 0; j < Game.COLS; j++)
                    {
                        if (G.IsValid(i, j))
                        {
                            Game TmpGame = new Game(G);

                            TmpGame.MarkGameBoard(i, j);

                            Spot TmpSpot = new Spot();
                            TmpSpot.row = i;
                            TmpSpot.col = j;

                            //if (TmpGame.checkWinner().Item1 != ' ')
                            //{
                            TmpSpot.score = Minimax(TmpGame, level + 1, maxLevel, turn).score;

                            //MessageBox.Show(string.Format("Score is {0}, level is {1}", TmpSpot.score, level));
                            //if current AI player
                            if (level % 2 == 0)
                            {
                                if (TmpSpot.score > BestSpot.score)
                                {
                                    BestSpot = TmpSpot;
                                }
                            }
                            //if AI opponent
                            else
                            {
                                if (TmpSpot.score < BestSpot.score)
                                {
                                    BestSpot = TmpSpot;
                                }
                            }
                            //}
                        }
                    }
                }
            }
            //MessageBox.Show(string.Format("Score is {0}, level is {1}", BestSpot.score, level));
            return BestSpot;
        }
        private int scoreGame(Game G, int level, char turn)
        {
            int score = 0;

            score += ScoreFourInRow(G, turn) / (level * level);
            score += ScoreThreeInRowOpen(G, turn) / (level * level);
            score += ScoreTwoInRow(G, turn) / (level * level);
            score += ScoreTwoInRowOpen(G, turn) / (level * level);

            return score;
        }
        private int ScoreFourInRow(Game G, char turn)
        {
            int score = 0;
            char checkWinner = G.checkWinner().Item1;
            if (checkWinner != ' ')
            {
                if (checkWinner == turn)
                {
                    score += 100000;
                }
                else
                {
                    score -= 100000;
                }
            }
            return score;
        }
        private int ScoreThreeInRowOpen(Game G, char turn)
        {
            int score = 0;
            
            for (int i = 0; i < Game.ROWS - 1; i++)
            {
                for (int j = 0; j < Game.COLS - 2; j++)
                {
                    if (( G.getGameBoard()[i, j] == G.getGameBoard()[i, j + 1] && G.getGameBoard()[i, j] == G.getGameBoard()[i, j + 2] && G.getGameBoard()[i + 1, j] == ' ')
                        || (G.getGameBoard()[i, j + 1] == G.getGameBoard()[i, j + 2] && G.getGameBoard()[i, j + 1] == G.getGameBoard()[i + 1, j] && G.getGameBoard()[i, j] == ' ')
                        || (G.getGameBoard()[i, j + 2] == G.getGameBoard()[i, j] && G.getGameBoard()[i, j + 2] == G.getGameBoard()[i + 1, j] && G.getGameBoard()[i, j + 1] == ' ')
                        || (G.getGameBoard()[i + 1, j] == G.getGameBoard()[i, j + 1] && G.getGameBoard()[i + 1, j] == G.getGameBoard()[i, j] && G.getGameBoard()[i, j + 2] == ' ' ))
                    {
                        if (G.getGameBoard()[i, j] == turn || G.getGameBoard()[i, j + 1] == turn || G.getGameBoard()[i, j + 2] == turn)
                        {
                            score += 10000;
                        }
                        else
                        {
                            score -= 10000;
                        }

                    }
                    else if ((G.getGameBoard()[i, j] == G.getGameBoard()[i, j + 1] && G.getGameBoard()[i, j] == G.getGameBoard()[i, j + 2] && G.getGameBoard()[i + 1, j + 2] == ' ')
                        || (G.getGameBoard()[i, j + 1] == G.getGameBoard()[i, j + 2] && G.getGameBoard()[i, j + 1] == G.getGameBoard()[i + 1, j +2] && G.getGameBoard()[i, j] == ' ')
                        || (G.getGameBoard()[i, j + 2] == G.getGameBoard()[i, j] && G.getGameBoard()[i, j + 2] == G.getGameBoard()[i + 1, j+2] && G.getGameBoard()[i, j + 1] == ' ')
                        || (G.getGameBoard()[i + 1, j+2] == G.getGameBoard()[i, j + 1] && G.getGameBoard()[i + 1, j+2] == G.getGameBoard()[i, j] && G.getGameBoard()[i, j + 2] == ' '))
                    {
                        if (G.getGameBoard()[i, j] == turn || G.getGameBoard()[i, j + 1] == turn || G.getGameBoard()[i, j + 2] == turn)
                        {
                            score += 1000;
                        }
                        else
                        {
                            score -= 1000;
                        }

                    }
                }
            }

            for (int i = 1; i < Game.ROWS; i++)
            {
                for (int j = 0; j < Game.COLS - 2; j++)
                {
                    if ((G.getGameBoard()[i, j] == G.getGameBoard()[i, j + 1] && G.getGameBoard()[i,j] == G.getGameBoard()[i, j + 2] && G.getGameBoard()[i - 1, j] == ' ')
                        ||(G.getGameBoard()[i, j+1] == G.getGameBoard()[i, j+2] && G.getGameBoard()[i, j+1] == G.getGameBoard()[i-1, j] && G.getGameBoard()[i,j] == ' ')
                        ||(G.getGameBoard()[i, j+2] == G.getGameBoard()[i, j] && G.getGameBoard()[i, j+2] == G.getGameBoard()[i-1, j] && G.getGameBoard()[i, j+1] == ' ')
                        ||(G.getGameBoard()[i-1, j]  == G.getGameBoard()[i, j] && G.getGameBoard()[i-1, j] == G.getGameBoard()[i, j+1] && G.getGameBoard()[i, j+2] == ' '))
                    {
                        if (G.getGameBoard()[i, j] == turn || G.getGameBoard()[i, j + 1] == turn || G.getGameBoard()[i, j + 2] == turn)
                        {
                            score += 1000;
                        }
                        else
                        {
                            score -= 1000;
                        }
                    }
                    else if ((G.getGameBoard()[i, j] == G.getGameBoard()[i, j + 1] && G.getGameBoard()[i, j] == G.getGameBoard()[i, j + 2] && G.getGameBoard()[i - 1, j+2] == ' ')
                        || (G.getGameBoard()[i, j + 1] == G.getGameBoard()[i, j + 2] && G.getGameBoard()[i, j + 1] == G.getGameBoard()[i - 1, j+2] && G.getGameBoard()[i, j] == ' ')
                        || (G.getGameBoard()[i, j + 2] == G.getGameBoard()[i, j] && G.getGameBoard()[i, j + 2] == G.getGameBoard()[i - 1, j+2] && G.getGameBoard()[i, j + 1] == ' ')
                        || (G.getGameBoard()[i - 1, j+2] == G.getGameBoard()[i, j] && G.getGameBoard()[i - 1, j+2] == G.getGameBoard()[i, j + 1] && G.getGameBoard()[i, j + 2] == ' '))
                    {
                        if (G.getGameBoard()[i, j] == turn || G.getGameBoard()[i, j + 1] == turn || G.getGameBoard()[i, j + 2] == turn )
                        {
                            score += 1000;
                        }
                        else
                        {
                            score -= 1000;
                        }
                    }
                }
            }

            for (int i = 0; i < Game.ROWS - 2; i++)
            {
                for (int j = 0; j < Game.COLS - 1; j++)
                {
                    if ((G.getGameBoard()[i, j]  == G.getGameBoard()[i + 1, j] && G.getGameBoard()[i, j] == G.getGameBoard()[i + 2, j] && G.getGameBoard()[i + 2, j + 1] == ' ')
                        ||(G.getGameBoard()[i+1, j] == G.getGameBoard()[i+2,j] && G.getGameBoard()[i+1, j] == G.getGameBoard()[i+2, j+1] && G.getGameBoard()[i,j] == ' ')
                        ||(G.getGameBoard()[i+2, j] == G.getGameBoard()[i,j] && G.getGameBoard()[i+2, j] == G.getGameBoard()[i+2, j+1] && G.getGameBoard()[i+1, j] == ' ')
                        || (G.getGameBoard()[i+2, j+1] == G.getGameBoard()[i,j] && G.getGameBoard()[i+2, j+1] == G.getGameBoard()[i+1, j] && G.getGameBoard()[i+2, j] == ' '))
                    {
                        if (G.getGameBoard()[i, j] == turn || G.getGameBoard()[i + 1, j] == turn || G.getGameBoard()[i + 2, j] == turn)
                        {
                            score += 1000;
                        }
                        else
                        {
                            score -= 1000;
                        }
                    }
                    else if ((G.getGameBoard()[i, j] == G.getGameBoard()[i + 1, j] && G.getGameBoard()[i, j] == G.getGameBoard()[i + 2, j] && G.getGameBoard()[i, j + 1] == ' ')
                        || (G.getGameBoard()[i + 1, j] == G.getGameBoard()[i + 2, j] && G.getGameBoard()[i + 1, j] == G.getGameBoard()[i, j + 1] && G.getGameBoard()[i, j] == ' ')
                        || (G.getGameBoard()[i + 2, j] == G.getGameBoard()[i, j] && G.getGameBoard()[i + 2, j] == G.getGameBoard()[i, j + 1] && G.getGameBoard()[i + 1, j] == ' ')
                        || (G.getGameBoard()[i, j + 1] == G.getGameBoard()[i, j] && G.getGameBoard()[i, j + 1] == G.getGameBoard()[i + 1, j] && G.getGameBoard()[i + 2, j] == ' '))
                    {
                        if (G.getGameBoard()[i, j] == turn || G.getGameBoard()[i + 1, j] == turn || G.getGameBoard()[i + 2, j] == turn)
                        {
                            score += 1000;
                        }
                        else
                        {
                            score -= 1000;
                        }
                    }
                }
            }

            for (int i = 0; i < Game.ROWS - 2; i++)
            {
                for (int j = 1; j < Game.COLS; j++)
                {
                    if ((G.getGameBoard()[i, j] == G.getGameBoard()[i + 1, j] && G.getGameBoard()[i, j] == G.getGameBoard()[i + 2, j] && G.getGameBoard()[i + 2, j - 1] == ' ')
                        || (G.getGameBoard()[i + 1, j] == G.getGameBoard()[i + 2, j] && G.getGameBoard()[i + 1, j] == G.getGameBoard()[i + 2, j - 1] && G.getGameBoard()[i, j] == ' ')
                        || (G.getGameBoard()[i + 2, j] == G.getGameBoard()[i, j] && G.getGameBoard()[i + 2, j] == G.getGameBoard()[i + 2, j - 1] && G.getGameBoard()[i + 1, j] == ' ')
                        || (G.getGameBoard()[i + 2, j - 1] == G.getGameBoard()[i, j] && G.getGameBoard()[i + 2, j - 1] == G.getGameBoard()[i + 1, j] && G.getGameBoard()[i + 2, j] == ' '))
                    {
                        if (G.getGameBoard()[i, j] == turn || G.getGameBoard()[i + 1, j] == turn || G.getGameBoard()[i + 2, j] == turn)
                        {
                            score += 1000;
                        }
                        else
                        {
                            score -= 1000;
                        }
                    }
                    else if ((G.getGameBoard()[i, j] == G.getGameBoard()[i + 1, j] && G.getGameBoard()[i, j] == G.getGameBoard()[i + 2, j] && G.getGameBoard()[i, j - 1] == ' ')
                        || (G.getGameBoard()[i + 1, j] == G.getGameBoard()[i + 2, j] && G.getGameBoard()[i + 1, j] == G.getGameBoard()[i, j - 1] && G.getGameBoard()[i, j] == ' ')
                        || (G.getGameBoard()[i + 2, j] == G.getGameBoard()[i, j] && G.getGameBoard()[i + 2, j] == G.getGameBoard()[i, j - 1] && G.getGameBoard()[i + 1, j] == ' ')
                        || (G.getGameBoard()[i, j - 1] == G.getGameBoard()[i, j] && G.getGameBoard()[i, j - 1] == G.getGameBoard()[i + 1, j] && G.getGameBoard()[i + 2, j] == ' '))
                    {
                        if (G.getGameBoard()[i, j] == turn || G.getGameBoard()[i + 1, j] == turn || G.getGameBoard()[i + 2, j] == turn )
                        {
                            score += 1000;
                        }
                        else
                        {
                            score -= 1000;
                        }
                    }
                }
            }
            
            return score;
        }

        private int ScoreTwoInRow(Game G, char turn)
        {
            int score = 0;

            for (int i = 0; i < Game.ROWS - 1; i++)
            {
                for (int j = 0; j < Game.COLS - 2; j++)
                {
                    if ((G.getGameBoard()[i, j] == G.getGameBoard()[i, j + 1] && G.getGameBoard()[i, j + 2] == ' ' && G.getGameBoard()[i + 1, j] == ' ')
                        || (G.getGameBoard()[i, j + 1] == G.getGameBoard()[i, j + 2] && G.getGameBoard()[i + 1, j] == ' ' && G.getGameBoard()[i, j] == ' ')
                        || (G.getGameBoard()[i, j + 2] == G.getGameBoard()[i, j] && G.getGameBoard()[i + 1, j] == ' ' && G.getGameBoard()[i, j + 1] == ' ')
                        || (G.getGameBoard()[i + 1, j] == G.getGameBoard()[i, j + 1] && G.getGameBoard()[i, j] == ' ' && G.getGameBoard()[i, j + 2] == ' ')
                        || (G.getGameBoard()[i,j] == G.getGameBoard()[i+1, j] && G.getGameBoard()[i, j + 1] == ' ' && G.getGameBoard()[i, j + 2] == ' ')
                        || (G.getGameBoard()[i, j + 2] == G.getGameBoard()[i + 1, j] && G.getGameBoard()[i, j] == ' ' && G.getGameBoard()[i, j + 1] == ' '))
                    {
                        if (G.getGameBoard()[i, j] == turn || G.getGameBoard()[i, j + 1] == turn)
                        {
                            score += 1000;
                        }
                        else
                        {
                            score -= 1000;
                        }

                    }
                    else if ((G.getGameBoard()[i, j] == G.getGameBoard()[i, j + 1] && G.getGameBoard()[i, j + 2] == ' ' && G.getGameBoard()[i + 1, j+2] == ' ')
                        || (G.getGameBoard()[i, j + 1] == G.getGameBoard()[i, j + 2] && G.getGameBoard()[i + 1, j+2] == ' ' && G.getGameBoard()[i, j] == ' ')
                        || (G.getGameBoard()[i, j + 2] == G.getGameBoard()[i, j] && G.getGameBoard()[i + 1, j+2] == ' ' && G.getGameBoard()[i, j + 1] == ' ')
                        || (G.getGameBoard()[i + 1, j+2] == G.getGameBoard()[i, j + 1] && G.getGameBoard()[i, j] == ' ' && G.getGameBoard()[i, j + 2] == ' ')
                        || (G.getGameBoard()[i, j] == G.getGameBoard()[i + 1, j+2] && G.getGameBoard()[i, j + 1] == ' ' && G.getGameBoard()[i, j + 2] == ' ')
                        || (G.getGameBoard()[i, j + 2] == G.getGameBoard()[i + 1, j+2] && G.getGameBoard()[i, j] == ' ' && G.getGameBoard()[i, j + 1] == ' '))
                    {
                        if (G.getGameBoard()[i, j] == turn || G.getGameBoard()[i, j + 1] == turn)
                        {
                            score += 1000;
                        }
                        else
                        {
                            score -= 1000;
                        }

                    }
                }
            }

            for (int i = 1; i < Game.ROWS; i++)
            {
                for (int j = 0; j < Game.COLS - 2; j++)
                {
                    if ((G.getGameBoard()[i, j] == G.getGameBoard()[i, j + 1] && G.getGameBoard()[i, j + 2] == ' ' && G.getGameBoard()[i - 1, j] == ' ')
                        || (G.getGameBoard()[i, j + 1] == G.getGameBoard()[i, j + 2] && G.getGameBoard()[i - 1, j] == ' ' && G.getGameBoard()[i, j] == ' ')
                        || (G.getGameBoard()[i, j + 2] == G.getGameBoard()[i, j] && G.getGameBoard()[i - 1, j] == ' ' && G.getGameBoard()[i, j + 1] == ' ')
                        || (G.getGameBoard()[i - 1, j] == G.getGameBoard()[i, j + 1] && G.getGameBoard()[i, j] == ' ' && G.getGameBoard()[i, j + 2] == ' ')
                        || (G.getGameBoard()[i, j] == G.getGameBoard()[i - 1, j] && G.getGameBoard()[i, j + 1] == ' ' && G.getGameBoard()[i, j + 2] == ' ')
                        || (G.getGameBoard()[i, j + 2] == G.getGameBoard()[i - 1, j] && G.getGameBoard()[i, j] == ' ' && G.getGameBoard()[i, j + 1] == ' '))
                    {
                        if (G.getGameBoard()[i, j] == turn || G.getGameBoard()[i, j + 1] == turn)
                        {
                            score += 1000;
                        }
                        else
                        {
                            score -= 1000;
                        }
                    }
                    else if ((G.getGameBoard()[i, j] == G.getGameBoard()[i, j + 1] && G.getGameBoard()[i, j + 2] == ' ' && G.getGameBoard()[i - 1, j + 2] == ' ')
                        || (G.getGameBoard()[i, j + 1] == G.getGameBoard()[i, j + 2] && G.getGameBoard()[i - 1, j + 2] == ' ' && G.getGameBoard()[i, j] == ' ')
                        || (G.getGameBoard()[i, j + 2] == G.getGameBoard()[i, j] && G.getGameBoard()[i - 1, j + 2] == ' ' && G.getGameBoard()[i, j + 1] == ' ')
                        || (G.getGameBoard()[i - 1, j + 2] == G.getGameBoard()[i, j + 1] && G.getGameBoard()[i, j] == ' ' && G.getGameBoard()[i, j + 2] == ' ')
                        || (G.getGameBoard()[i, j] == G.getGameBoard()[i - 1, j+2] && G.getGameBoard()[i, j + 1] == ' ' && G.getGameBoard()[i, j + 2] == ' ')
                        || (G.getGameBoard()[i, j + 2] == G.getGameBoard()[i - 1, j+2] && G.getGameBoard()[i, j] == ' ' && G.getGameBoard()[i, j + 1] == ' '))
                    {
                        if (G.getGameBoard()[i, j] == turn || G.getGameBoard()[i, j + 1] == turn)
                        {
                            score += 1000;
                        }
                        else
                        {
                            score -= 1000;
                        }
                    }
                }
            }

            for (int i = 0; i < Game.ROWS - 2; i++)
            {
                for (int j = 0; j < Game.COLS - 1; j++)
                {
                    if ((G.getGameBoard()[i, j] == G.getGameBoard()[i + 1, j] && G.getGameBoard()[i + 2, j] == ' ' && G.getGameBoard()[i + 2, j + 1] == ' ')
                        || (G.getGameBoard()[i + 1, j] == G.getGameBoard()[i + 2, j] && G.getGameBoard()[i + 2, j + 1] == ' ' && G.getGameBoard()[i, j] == ' ')
                        || (G.getGameBoard()[i + 2, j] == G.getGameBoard()[i, j] && G.getGameBoard()[i + 2, j + 1] == ' ' && G.getGameBoard()[i + 1, j] == ' ')
                        || (G.getGameBoard()[i + 2, j + 1] == G.getGameBoard()[i, j] && G.getGameBoard()[i + 1, j] == ' ' && G.getGameBoard()[i + 2, j] == ' ')
                        || (G.getGameBoard()[i + 1, j] == G.getGameBoard()[i + 2, j + 1] && G.getGameBoard()[i, j] == ' ' && G.getGameBoard()[i + 2, j] == ' ')
                        || (G.getGameBoard()[i + 2, j] == G.getGameBoard()[i + 2, j + 1] && G.getGameBoard()[i, j] == ' ' && G.getGameBoard()[i + 1, j] == ' '))
                    {
                        if (G.getGameBoard()[i, j] == turn || G.getGameBoard()[i + 1, j] == turn)
                        {
                            score += 1000;
                        }
                        else
                        {
                            score -= 1000;
                        }
                    }
                    else if ((G.getGameBoard()[i, j] == G.getGameBoard()[i + 1, j] && G.getGameBoard()[i + 2, j] == ' ' && G.getGameBoard()[i, j + 1] == ' ')
                        || (G.getGameBoard()[i + 1, j] == G.getGameBoard()[i + 2, j] && G.getGameBoard()[i, j + 1] == ' ' && G.getGameBoard()[i, j] == ' ')
                        || (G.getGameBoard()[i + 2, j] == G.getGameBoard()[i, j] && G.getGameBoard()[i, j + 1] == ' ' && G.getGameBoard()[i + 1, j] == ' ')
                        || (G.getGameBoard()[i, j + 1] == G.getGameBoard()[i, j] && G.getGameBoard()[i + 1, j] == ' ' && G.getGameBoard()[i + 2, j] == ' ')
                        || (G.getGameBoard()[i + 1, j] == G.getGameBoard()[i, j + 1] && G.getGameBoard()[i, j] == ' ' && G.getGameBoard()[i + 2, j] == ' ')
                        || (G.getGameBoard()[i + 2, j] == G.getGameBoard()[i, j + 1] && G.getGameBoard()[i, j] == ' ' && G.getGameBoard()[i + 1, j] == ' '))
                    {
                        if (G.getGameBoard()[i, j] == turn || G.getGameBoard()[i + 1, j] == turn )
                        {
                            score += 1000;
                        }
                        else
                        {
                            score -= 1000;
                        }
                    }
                }
            }

            for (int i = 0; i < Game.ROWS - 2; i++)
            {
                for (int j = 1; j < Game.COLS; j++)
                {
                    if ((G.getGameBoard()[i, j] == G.getGameBoard()[i + 1, j] && G.getGameBoard()[i + 2, j] == ' ' && G.getGameBoard()[i + 2, j - 1] == ' ')
                        || (G.getGameBoard()[i + 1, j] == G.getGameBoard()[i + 2, j] && G.getGameBoard()[i + 2, j - 1] == ' ' && G.getGameBoard()[i, j] == ' ')
                        || (G.getGameBoard()[i + 2, j] == G.getGameBoard()[i, j] && G.getGameBoard()[i + 2, j - 1] == ' ' && G.getGameBoard()[i + 1, j] == ' ')
                        || (G.getGameBoard()[i + 2, j - 1] == G.getGameBoard()[i, j] && G.getGameBoard()[i + 1, j] == ' ' && G.getGameBoard()[i + 2, j] == ' ')
                        || (G.getGameBoard()[i + 1, j] == G.getGameBoard()[i + 2, j - 1] && G.getGameBoard()[i, j] == ' ' && G.getGameBoard()[i + 2, j] == ' ')
                        || (G.getGameBoard()[i + 2, j] == G.getGameBoard()[i + 2, j - 1] && G.getGameBoard()[i, j] == ' ' && G.getGameBoard()[i + 1, j] == ' '))
                    {
                        if (G.getGameBoard()[i, j] == turn || G.getGameBoard()[i + 1, j] == turn)
                        {
                            score += 1000;
                        }
                        else
                        {
                            score -= 1000;
                        }
                    }
                    else if ((G.getGameBoard()[i, j] == G.getGameBoard()[i + 1, j] && G.getGameBoard()[i + 2, j] == ' ' && G.getGameBoard()[i, j - 1] == ' ')
                        || (G.getGameBoard()[i + 1, j] == G.getGameBoard()[i + 2, j] && G.getGameBoard()[i, j - 1] == ' ' && G.getGameBoard()[i, j] == ' ')
                        || (G.getGameBoard()[i + 2, j] == G.getGameBoard()[i, j] && G.getGameBoard()[i, j - 1] == ' ' && G.getGameBoard()[i + 1, j] == ' ')
                        || (G.getGameBoard()[i, j - 1] == G.getGameBoard()[i, j] && G.getGameBoard()[i + 1, j] == ' ' && G.getGameBoard()[i + 2, j] == ' ')
                        || (G.getGameBoard()[i + 1, j] == G.getGameBoard()[i, j - 1] && G.getGameBoard()[i, j] == ' ' && G.getGameBoard()[i + 2, j] == ' ')
                        || (G.getGameBoard()[i + 2, j] == G.getGameBoard()[i, j - 1] && G.getGameBoard()[i, j] == ' ' && G.getGameBoard()[i + 1, j] == ' '))
                    {
                        if (G.getGameBoard()[i, j] == turn || G.getGameBoard()[i + 1, j] == turn )
                        {
                            score += 1000;
                        }
                        else
                        {
                            score -= 1000;
                        }
                    }
                }
            }

            return score;
        }

        private int ScoreTwoInRowOpen(Game G, char turn)
        {
            int score = 0;
            //check horizontal
            for (int i = 0; i < Game.ROWS; i++)
            {
                for (int j = 0; j < Game.COLS -2; j++){
                    if ((G.getGameBoard()[i, j] == G.getGameBoard()[i, j +1] && G.getGameBoard()[i, j+2] == ' ') 
                    || (G.getGameBoard()[i, j+1] == G.getGameBoard()[i, j+2] && G.getGameBoard()[i, j] == ' ')
                    || (G.getGameBoard()[i, j] == G.getGameBoard()[i, j+2] && G.getGameBoard()[i, j+1] == ' '))
                    {
                        if (G.getGameBoard()[i, j] == turn || G.getGameBoard()[i, j+1] == turn)
                        {
                            score += 100;
                        }
                        else{
                            score -= 100;
                        }
                    }   
                }
            }

            //check vertical
            for (int i = 0; i < Game.ROWS -2; i++){
                for (int j = 0; j < Game.COLS; j++)
                {
                    if ((G.getGameBoard()[i, j] == G.getGameBoard()[i+1, j] && G.getGameBoard()[i+2, j] == ' ') 
                    || (G.getGameBoard()[i+1, j] == G.getGameBoard()[i+2, j] && G.getGameBoard()[i, j] == ' ')
                    || (G.getGameBoard()[i, j] == G.getGameBoard()[i+2, j] && G.getGameBoard()[i+1, j] == ' '))
                    {

                        if (G.getGameBoard()[i, j] == turn || G.getGameBoard()[i+1, j] == turn)
                        {
                            score += 100;
                        }
                        else
                        {
                            score -= 100;
                         }
                    }
                }
            }

           return score;
         }
    }
}
