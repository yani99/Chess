using System;
using System.Collections.Generic;
using Chess.Figures;

namespace Chess.Components
{
    public class ChessAnalyzer
    {
        private List<Figure> WhiteFigures;
        private List<Figure> BlackFigures;

        public ChessAnalyzer(List<Figure> whiteFigures, List<Figure> blackFigures)
        {
            WhiteFigures = whiteFigures;
            BlackFigures = blackFigures;
        }

        public List<Tuple<int, int>> AvailableMoves(Figure figure)
        {
            List<Tuple<int, int>> AllMoves = GetAllMoves(figure);
            List<Tuple<int, int>> AvailableMoves = new List<Tuple<int, int>>();
            foreach (var move in AllMoves)
            {
                if (!WillExpoceTheKing(figure, move))
                {
                    AvailableMoves.Add(move);
                }
            }
            if (figure.Type == Figure.FigureType.King)
            {
                if (SmallRodake(figure).Count != 0)
                {
                    AvailableMoves.Add(SmallRodake(figure)[0].Item2);
                }
                if (LongRokade(figure).Count != 0)
                {
                    AvailableMoves.Add(LongRokade(figure)[0].Item2);
                }              
            }
            return AvailableMoves;
        }

        private List<Tuple<int, int>> GetAllMoves(Figure figure)
        {
            List<Tuple<int, int>> AvailableMoves = new List<Tuple<int, int>>();
            if (figure.Type == Figure.FigureType.Pawn)
            {
                AvailableMoves.AddRange(GetAvailableMovesOfPawn(figure));
            }
            else if (figure.Type == Figure.FigureType.Rook)
            {
                AvailableMoves.AddRange(GetAvailableMovesOfRook(figure));
            }
            else if (figure.Type == Figure.FigureType.Bishop)
            {
                AvailableMoves.AddRange(GetAvailableMovesOfBishop(figure));
            }
            else if (figure.Type == Figure.FigureType.Knight)
            {
                AvailableMoves.AddRange(GetAvailableMovesOfKnight(figure));
            }
            else if (figure.Type == Figure.FigureType.Queen)
            {
                AvailableMoves.AddRange(GetAvailableMovesOfQueen(figure));
            }
            else
            {
                AvailableMoves.AddRange(GetAvailableMovesOfKing(figure));
            }
            return AvailableMoves;
        }

        private List<Tuple<int, int>> GetAvailableMovesOfPawn(Figure figure)
        {
            List<Tuple<int, int>> AvailableMoves = new List<Tuple<int, int>>();

            int WayToMove;
            if (figure.Color == Figure.FigureColor.Black)
            {
                WayToMove = 1;
            }
            else
            {
                WayToMove = -1;
            }

            Tuple<int, int> MoveForward = new Tuple<int, int>(figure.CurrentPossition.Item1 + WayToMove, figure.CurrentPossition.Item2);
            if (!WillImpactWithAlly(figure, MoveForward).Item1 &&
                !WillImpactWithEnemy(figure, MoveForward).Item1 &&
                !IsOutOfBoard(MoveForward))
            {
                AvailableMoves.Add(MoveForward);

                if (!figure.WasMoved)
                {
                    Tuple<int, int> MoveForwardFromTheStart = new Tuple<int, int>(figure.CurrentPossition.Item1 + WayToMove * 2, figure.CurrentPossition.Item2);
                    if (!WillImpactWithAlly(figure, MoveForwardFromTheStart).Item1 &&
                        !WillImpactWithEnemy(figure, MoveForwardFromTheStart).Item1 &&
                        !IsOutOfBoard(MoveForwardFromTheStart))
                    {
                        AvailableMoves.Add(MoveForwardFromTheStart);
                    }
                }
            }
            List<Tuple<int, int>> MoveDiagonal = new List<Tuple<int, int>>
                {
                new Tuple<int, int>(figure.CurrentPossition.Item1 + WayToMove,figure.CurrentPossition.Item2 + WayToMove),
                new Tuple<int, int>(figure.CurrentPossition.Item1 + WayToMove,figure.CurrentPossition.Item2 - WayToMove)
                };
            for (int i = 0; i < MoveDiagonal.Count; i++)
            {
                if (!WillImpactWithAlly(figure, MoveDiagonal[i]).Item1 &&
                    WillImpactWithEnemy(figure, MoveDiagonal[i]).Item1 &&
                    !IsOutOfBoard(MoveDiagonal[i]))
                {
                    AvailableMoves.Add(MoveDiagonal[i]);
                }
            }
            return AvailableMoves;
        }

        private List<Tuple<int, int>> GetAvailableMovesOfRook(Figure figure)
        {
            List<Tuple<int, int>> AvailableMoves = new List<Tuple<int, int>>();

            int WayToMove;
            if (figure.Color == Figure.FigureColor.Black)
            {
                WayToMove = 1;
            }
            else
            {
                WayToMove = -1;
            }

            List<Tuple<int, int>> MoveVertical = new List<Tuple<int, int>>
            {
               new Tuple<int, int>(figure.CurrentPossition.Item1 + WayToMove, figure.CurrentPossition.Item2),
               new Tuple<int, int>(figure.CurrentPossition.Item1 - WayToMove, figure.CurrentPossition.Item2)
            };
            for (int i = 0; i < MoveVertical.Count; i++)
            {
                int move = WayToMove;
                while (!WillImpactWithAlly(figure, MoveVertical[i]).Item1 &&
                       !WillImpactWithEnemy(figure, MoveVertical[i]).Item1 &&
                       !IsOutOfBoard(MoveVertical[i]))
                {
                    AvailableMoves.Add(MoveVertical[i]);

                    move += WayToMove;
                    if (i == 0)
                    {
                        MoveVertical[i] = new Tuple<int, int>(figure.CurrentPossition.Item1 + move, figure.CurrentPossition.Item2);
                    }
                    else
                    {
                        MoveVertical[i] = new Tuple<int, int>(figure.CurrentPossition.Item1 - move, figure.CurrentPossition.Item2);
                    }
                }
                if (!WillImpactWithAlly(figure, MoveVertical[i]).Item1 &&
                     WillImpactWithEnemy(figure, MoveVertical[i]).Item1 &&
                    !IsOutOfBoard(MoveVertical[i]))
                {
                    AvailableMoves.Add(MoveVertical[i]);
                }
            }

            List<Tuple<int, int>> MoveHorizontal = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(figure.CurrentPossition.Item1, figure.CurrentPossition.Item2 - WayToMove),
                new Tuple<int, int>(figure.CurrentPossition.Item1, figure.CurrentPossition.Item2 + WayToMove)
            };
            for (int i = 0; i < MoveHorizontal.Count; i++)
            {
                int move = WayToMove;
                while (!WillImpactWithAlly(figure, MoveHorizontal[i]).Item1 &&
                       !WillImpactWithEnemy(figure, MoveHorizontal[i]).Item1 &&
                       !IsOutOfBoard(MoveHorizontal[i]))
                {
                    AvailableMoves.Add(MoveHorizontal[i]);

                    move += WayToMove;
                    if (i == 0)
                    {
                        MoveHorizontal[i] = new Tuple<int, int>(figure.CurrentPossition.Item1, figure.CurrentPossition.Item2 - move);
                    }
                    else
                    {
                        MoveHorizontal[i] = new Tuple<int, int>(figure.CurrentPossition.Item1, figure.CurrentPossition.Item2 + move);
                    }
                }
                if (!WillImpactWithAlly(figure, MoveHorizontal[i]).Item1 &&
                     WillImpactWithEnemy(figure, MoveHorizontal[i]).Item1 &&
                    !IsOutOfBoard(MoveHorizontal[i]))
                {
                    AvailableMoves.Add(MoveHorizontal[i]);
                }
            }
            return AvailableMoves;
        }

        private List<Tuple<int, int>> GetAvailableMovesOfKnight(Figure figure)
        {
            List<Tuple<int, int>> AvailableMoves = new List<Tuple<int, int>>();

            int WayToMove;
            if (figure.Color == Figure.FigureColor.Black)
            {
                WayToMove = 1;
            }
            else
            {
                WayToMove = -1;
            }

            List<Tuple<int, int>> MoveHorizontal = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(figure.CurrentPossition.Item1 + WayToMove, figure.CurrentPossition.Item2 - 2),
                new Tuple<int, int>(figure.CurrentPossition.Item1 - WayToMove , figure.CurrentPossition.Item2 - 2),
                new Tuple<int, int>(figure.CurrentPossition.Item1 + WayToMove, figure.CurrentPossition.Item2 + 2),
                new Tuple<int, int>(figure.CurrentPossition.Item1 - WayToMove, figure.CurrentPossition.Item2 + 2)
            };
            for (int i = 0; i < MoveHorizontal.Count; i++)
            {
                if (!WillImpactWithAlly(figure, MoveHorizontal[i]).Item1 &&
                    !WillImpactWithEnemy(figure, MoveHorizontal[i]).Item1 &&
                    !IsOutOfBoard(MoveHorizontal[i]))
                {
                    AvailableMoves.Add(MoveHorizontal[i]);
                }
                if (!WillImpactWithAlly(figure, MoveHorizontal[i]).Item1 &&
                     WillImpactWithEnemy(figure, MoveHorizontal[i]).Item1 &&
                    !IsOutOfBoard(MoveHorizontal[i]))
                {
                    AvailableMoves.Add(MoveHorizontal[i]);
                }
            }

            List<Tuple<int, int>> MoveVertical = new List<Tuple<int, int>>
            {
               new Tuple<int, int>(figure.CurrentPossition.Item1 + (WayToMove * 2), figure.CurrentPossition.Item2 - 1),
               new Tuple<int, int>(figure.CurrentPossition.Item1 + (WayToMove * 2), figure.CurrentPossition.Item2 + 1),
               new Tuple<int, int>(figure.CurrentPossition.Item1 - (WayToMove * 2), figure.CurrentPossition.Item2 - 1),
               new Tuple<int, int>(figure.CurrentPossition.Item1 - (WayToMove * 2), figure.CurrentPossition.Item2 + 1),
            };
            for (int i = 0; i < MoveVertical.Count; i++)
            {
                if (!WillImpactWithAlly(figure, MoveVertical[i]).Item1 &&
                    !WillImpactWithEnemy(figure, MoveVertical[i]).Item1 &&
                    !IsOutOfBoard(MoveVertical[i]))
                {
                    AvailableMoves.Add(MoveVertical[i]);
                }
                if (!WillImpactWithAlly(figure, MoveVertical[i]).Item1 &&
                     WillImpactWithEnemy(figure, MoveVertical[i]).Item1 &&
                    !IsOutOfBoard(MoveVertical[i]))
                {
                    AvailableMoves.Add(MoveVertical[i]);
                }
            }
            return AvailableMoves;
        }

        private List<Tuple<int, int>> GetAvailableMovesOfBishop(Figure figure)
        {
            List<Tuple<int, int>> AvailableMoves = new List<Tuple<int, int>>();

            int WayToMove;
            if (figure.Color == Figure.FigureColor.Black)
            {
                WayToMove = 1;
            }
            else
            {
                WayToMove = -1;
            }

            List<Tuple<int, int>> MoveForwardDiagonals = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(figure.CurrentPossition.Item1 + WayToMove, figure.CurrentPossition.Item2 + WayToMove),
                new Tuple<int, int>(figure.CurrentPossition.Item1 + WayToMove,figure.CurrentPossition.Item2 - WayToMove)
            };
            for (int i = 0; i < MoveForwardDiagonals.Count; i++)
            {
                int move = WayToMove;
                while (!WillImpactWithAlly(figure, MoveForwardDiagonals[i]).Item1 &&
                       !WillImpactWithEnemy(figure, MoveForwardDiagonals[i]).Item1 &&
                       !IsOutOfBoard(MoveForwardDiagonals[i]))
                {
                    AvailableMoves.Add(MoveForwardDiagonals[i]);

                    move += WayToMove;
                    if (i == 0)
                    {
                        MoveForwardDiagonals[i] = new Tuple<int, int>(figure.CurrentPossition.Item1 + move, figure.CurrentPossition.Item2 + move);
                    }
                    else
                    {
                        MoveForwardDiagonals[i] = new Tuple<int, int>(figure.CurrentPossition.Item1 + move, figure.CurrentPossition.Item2 - move);
                    }
                }
                if (!WillImpactWithAlly(figure, MoveForwardDiagonals[i]).Item1 &&
                     WillImpactWithEnemy(figure, MoveForwardDiagonals[i]).Item1 &&
                    !IsOutOfBoard(MoveForwardDiagonals[i]))
                {
                    AvailableMoves.Add(MoveForwardDiagonals[i]);
                }
            }

            List<Tuple<int, int>> MoveBackDiagonals = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(figure.CurrentPossition.Item1 - WayToMove, figure.CurrentPossition.Item2 - WayToMove),
                new Tuple<int, int>(figure.CurrentPossition.Item1 - WayToMove,figure.CurrentPossition.Item2 + WayToMove)
            };
            for (int i = 0; i < MoveBackDiagonals.Count; i++)
            {
                int move = WayToMove;
                while (!WillImpactWithAlly(figure, MoveBackDiagonals[i]).Item1 &&
                       !WillImpactWithEnemy(figure, MoveBackDiagonals[i]).Item1 &&
                       !IsOutOfBoard(MoveBackDiagonals[i]))
                {
                    AvailableMoves.Add(MoveBackDiagonals[i]);

                    move += WayToMove;
                    if (i == 0)
                    {
                        MoveBackDiagonals[i] = new Tuple<int, int>(figure.CurrentPossition.Item1 - move, figure.CurrentPossition.Item2 - move);
                    }
                    else
                    {
                        MoveBackDiagonals[i] = new Tuple<int, int>(figure.CurrentPossition.Item1 - move, figure.CurrentPossition.Item2 + move);
                    }
                }
                if (!WillImpactWithAlly(figure, MoveBackDiagonals[i]).Item1 &&
                     WillImpactWithEnemy(figure, MoveBackDiagonals[i]).Item1 &&
                    !IsOutOfBoard(MoveBackDiagonals[i]))
                {
                    AvailableMoves.Add(MoveBackDiagonals[i]);
                }
            }
            return AvailableMoves;
        }

        private List<Tuple<int, int>> GetAvailableMovesOfQueen(Figure figure)
        {
            List<Tuple<int, int>> AvailableMoves = new List<Tuple<int, int>>();

            List<Tuple<int, int>> AvailableDiagonalMoves = GetAvailableMovesOfBishop(figure);
            AvailableMoves.AddRange(AvailableDiagonalMoves);

            List<Tuple<int, int>> AvailableHorizontalAndVerticalMoves = GetAvailableMovesOfRook(figure);
            AvailableMoves.AddRange(AvailableHorizontalAndVerticalMoves);

            return AvailableMoves;
        }

        private List<Tuple<int, int>> GetAvailableMovesOfKing(Figure figure)
        {
            List<Tuple<int, int>> AvailableMoves = new List<Tuple<int, int>>();

            int WayToMove;
            if (figure.Color == Figure.FigureColor.Black)
            {
                WayToMove = 1;
            }
            else
            {
                WayToMove = -1;
            }

            List<Tuple<int, int>> MoveHorizontal = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(figure.CurrentPossition.Item1,figure.CurrentPossition.Item2 + WayToMove),
                new Tuple<int, int>(figure.CurrentPossition.Item1,figure.CurrentPossition.Item2 - WayToMove)
            };
            for (int i = 0; i < MoveHorizontal.Count; i++)
            {
                if (!WillImpactWithAlly(figure, MoveHorizontal[i]).Item1 &&
                    !WillImpactWithEnemy(figure, MoveHorizontal[i]).Item1 &&
                    !IsOutOfBoard(MoveHorizontal[i]))
                {
                    AvailableMoves.Add(MoveHorizontal[i]);
                }
                if (!WillImpactWithAlly(figure, MoveHorizontal[i]).Item1 &&
                     WillImpactWithEnemy(figure, MoveHorizontal[i]).Item1 &&
                    !IsOutOfBoard(MoveHorizontal[i]))
                {
                    AvailableMoves.Add(MoveHorizontal[i]);
                }
            }

            List<Tuple<int, int>> MoveVertical = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(figure.CurrentPossition.Item1 + WayToMove,figure.CurrentPossition.Item2),
                new Tuple<int, int>(figure.CurrentPossition.Item1 - WayToMove,figure.CurrentPossition.Item2)
            };
            for (int i = 0; i < MoveVertical.Count; i++)
            {
                if (!WillImpactWithAlly(figure, MoveVertical[i]).Item1 &&
                    !WillImpactWithEnemy(figure, MoveVertical[i]).Item1 &&
                    !IsOutOfBoard(MoveVertical[i]))
                {
                    AvailableMoves.Add(MoveVertical[i]);
                }
                if (!WillImpactWithAlly(figure, MoveVertical[i]).Item1 &&
                     WillImpactWithEnemy(figure, MoveVertical[i]).Item1 &&
                    !IsOutOfBoard(MoveVertical[i]))
                {
                    AvailableMoves.Add(MoveVertical[i]);
                }
            }

            List<Tuple<int, int>> MoveDiagonal = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(figure.CurrentPossition.Item1 + WayToMove,figure.CurrentPossition.Item2 + WayToMove),
                new Tuple<int, int>(figure.CurrentPossition.Item1 + WayToMove,figure.CurrentPossition.Item2 - WayToMove),
                new Tuple<int, int>(figure.CurrentPossition.Item1 - WayToMove,figure.CurrentPossition.Item2 + WayToMove),
                new Tuple<int, int>(figure.CurrentPossition.Item1 - WayToMove,figure.CurrentPossition.Item2 - WayToMove)
            };
            for (int i = 0; i < MoveDiagonal.Count; i++)
            {
                if (!WillImpactWithAlly(figure, MoveDiagonal[i]).Item1 &&
                    !WillImpactWithEnemy(figure, MoveDiagonal[i]).Item1 &&
                    !IsOutOfBoard(MoveDiagonal[i]))
                {
                    AvailableMoves.Add(MoveDiagonal[i]);
                }
                if (!WillImpactWithAlly(figure, MoveDiagonal[i]).Item1 &&
                     WillImpactWithEnemy(figure, MoveDiagonal[i]).Item1 &&
                    !IsOutOfBoard(MoveDiagonal[i]))
                {
                    AvailableMoves.Add(MoveDiagonal[i]);
                }
            }
            return AvailableMoves;
        }

        public List<Tuple<Figure, Tuple<int, int>>> SmallRodake(Figure figure) 
        {
            List<Tuple<Figure,Tuple<int,int>>> smallRokade = new List<Tuple<Figure, Tuple<int, int>>>();

            int moveLeft = 1;
            Tuple<int, int> Movement = new Tuple<int, int>(figure.CurrentPossition.Item1, figure.CurrentPossition.Item2 + moveLeft);
            if (!figure.WasMoved)
            {
                int move = moveLeft;
                while (!IsOutOfBoard(Movement) &&
                    !WillImpactWithAlly(figure, Movement).Item1)
                {
                    move += moveLeft;
                    Movement = new Tuple<int, int>(figure.CurrentPossition.Item1, figure.CurrentPossition.Item2 + move);
                }
                if (!IsOutOfBoard(Movement) &&
                   WillImpactWithAlly(figure,Movement).Item1)
                {
                    
                    Figure temp = WillImpactWithAlly(figure, Movement).Item2;
                    if (temp.Type == Figure.FigureType.Rook && temp.WasMoved == false)
                    {
                        Tuple<int, int> RookpreviousPossition = temp.CurrentPossition;
                        Tuple<int, int> KingNewPossition = new Tuple<int, int>(figure.CurrentPossition.Item1, figure.CurrentPossition.Item2 + 2);
                        Tuple<int, int> RookNewPossition = new Tuple<int, int>(temp.CurrentPossition.Item1, temp.CurrentPossition.Item2 - 2);
                        temp.UpdatePossition(RookNewPossition);
                        if (!WillExpoceTheKing(figure,KingNewPossition))
                        {
                            temp.UpdatePossition(RookpreviousPossition);
                            smallRokade.Add(new Tuple<Figure, Tuple<int, int>>(figure,KingNewPossition));
                            smallRokade.Add(new Tuple<Figure, Tuple<int, int>>(temp, RookNewPossition));
                        }
                        else
                        {
                          temp.UpdatePossition(RookpreviousPossition);
                        }
                    }
                }
            }
            return smallRokade;
        }   

        public List<Tuple<Figure, Tuple<int, int>>> LongRokade(Figure figure)
        {
            List<Tuple<Figure, Tuple<int, int>>> longRokade = new List<Tuple<Figure, Tuple<int, int>>>();

            int moveRight = -1;
            Tuple<int, int> Movement = new Tuple<int, int>(figure.CurrentPossition.Item1, figure.CurrentPossition.Item2 + moveRight);

            if (!figure.WasMoved)
            {
                int move = moveRight;
                while (!IsOutOfBoard(Movement) &&
                    !WillImpactWithAlly(figure, Movement).Item1)
                {
                    move += moveRight;
                    Movement = new Tuple<int, int>(figure.CurrentPossition.Item1, figure.CurrentPossition.Item2 + move);
                }
                if (!IsOutOfBoard(Movement) &&
                   WillImpactWithAlly(figure, Movement).Item1)
                {
                    Figure temp = WillImpactWithAlly(figure, Movement).Item2;
                    if (temp.Type == Figure.FigureType.Rook && temp.WasMoved == false)
                    {
                        Tuple<int, int> RookpreviousPossition = temp.CurrentPossition;
                        Tuple<int, int> KingNewPossition = new Tuple<int, int>(figure.CurrentPossition.Item1, figure.CurrentPossition.Item2 - 2);
                        Tuple<int, int> RookNewPossition = new Tuple<int, int>(temp.CurrentPossition.Item1, temp.CurrentPossition.Item2 + 3);
                        temp.UpdatePossition(RookNewPossition);
                        if (!WillExpoceTheKing(figure, KingNewPossition))
                        {
                            temp.UpdatePossition(RookpreviousPossition);
                            longRokade.Add(new Tuple<Figure, Tuple<int, int>>(figure, KingNewPossition));
                            longRokade.Add(new Tuple<Figure, Tuple<int, int>>(temp, RookNewPossition));
                        }
                        else
                        {
                            temp.UpdatePossition(RookpreviousPossition);
                        }
                    }
                }
            }
            return longRokade;
        }
 
        public Tuple<bool,Figure> WillImpactWithAlly(Figure figure, Tuple<int, int> movement)
        {
            List<Figure> AllyFigures;
            if (figure.Color == Figure.FigureColor.Black)
            {
                AllyFigures = BlackFigures;        
            }
            else
            {
                AllyFigures = WhiteFigures;
            }

            foreach (Figure fig in AllyFigures)
            {
                if (fig.CurrentPossition.Item1 == movement.Item1 &&
                    fig.CurrentPossition.Item2 == movement.Item2)
                {
                    return new Tuple<bool, Figure>(true,fig);
                }
            }
            return new Tuple<bool, Figure>(false,null);
        }

        public Tuple<bool,Figure> WillImpactWithEnemy(Figure figure, Tuple<int, int> movement)
        {
            List<Figure> EnemyFigures;
            if (figure.Color == Figure.FigureColor.Black)
            {
                EnemyFigures = WhiteFigures;    
            }
            else
            {
                EnemyFigures = BlackFigures;
            }

            foreach (Figure fig in EnemyFigures)
            {
                if (fig.CurrentPossition.Item1 == movement.Item1 &&
                    fig.CurrentPossition.Item2 == movement.Item2)
                {
                    return new Tuple<bool, Figure>(true,fig);
                }
            }
            return new Tuple<bool, Figure>(false,null);
        }

        private bool WillExpoceTheKing(Figure figure, Tuple<int, int> movement)
         {
            Figure allyKing;
            Tuple<int, int> PrefiousPossition = figure.CurrentPossition;
            if (figure.Color == Figure.FigureColor.Black)
            {
                allyKing = BlackFigures.Find(x => x.Type == Figure.FigureType.King);
            }
            else
            {
                allyKing = WhiteFigures.Find(x => x.Type.Equals(Figure.FigureType.King));
            }

            Tuple<bool,Figure> WasFigureRemoved = WillImpactWithEnemy(figure, movement);

            if (WasFigureRemoved.Item1)
            {
                if (figure.Color == Figure.FigureColor.Black)
                {
                    WhiteFigures.Remove(WillImpactWithEnemy(figure, movement).Item2);
                }
                else
                {
                    BlackFigures.Remove(WillImpactWithEnemy(figure, movement).Item2);
                }
            }
            figure.UpdatePossition(movement);

            if (figure.Color == Figure.FigureColor.Black)
            {
                foreach (Figure fig in WhiteFigures)
                {
                    List<Tuple<int, int>> allMoves = GetAllMoves(fig);
                    if (allMoves.Contains(allyKing.CurrentPossition))
                    {
                        if (!Equals(figure.CurrentPossition,fig.CurrentPossition))
                        {
                            if (WasFigureRemoved.Item1)
                            {
                                BlackFigures.Add(WasFigureRemoved.Item2);   
                            }
                            figure.UpdatePossition(PrefiousPossition);
                            return true;
                        }                           
                    }
                }
            }
            else
            {
                foreach (Figure fig in BlackFigures)
                {
                    List<Tuple<int, int>> availableMoves = GetAllMoves(fig);
                    if (availableMoves.Contains(allyKing.CurrentPossition))
                    {
                        if (!Equals(figure.CurrentPossition, fig.CurrentPossition))
                        {
                            if (WasFigureRemoved.Item1)
                            {
                                BlackFigures.Add(WasFigureRemoved.Item2);
                            }
                            figure.UpdatePossition(PrefiousPossition);
                            return true;
                        }
                    }
                 }
            }
            if (WasFigureRemoved.Item1)
            {
                if (WasFigureRemoved.Item2.Color == Figure.FigureColor.Black)
                {
                    BlackFigures.Add(WasFigureRemoved.Item2);
                }
                else
                {
                    WhiteFigures.Add(WasFigureRemoved.Item2);
                }
            }
            figure.UpdatePossition(PrefiousPossition);
            return false;
        }

        private List<Figure> BreakReference(List<Figure> input)
        {
            List<Figure> OutPut = new List<Figure>();
            Figure.FigureColor color = input[0].Color;

            foreach (Figure fig in input)
            {
                Tuple<int, int> possition = fig.CurrentPossition;
                if (fig.Type == Figure.FigureType.Bishop)
                {
                    Bishop temp = new Bishop(color, possition);
                    OutPut.Add(temp);
                }
                else if (fig.Type == Figure.FigureType.King)
                {
                    King temp = new King(color, possition);
                    OutPut.Add(temp);
                }
                else if (fig.Type == Figure.FigureType.Knight)
                {
                    Knight temp = new Knight(color, possition);
                    OutPut.Add(temp);
                }
                else if (fig.Type == Figure.FigureType.Pawn)
                {
                    Pawn temp = new Pawn(color, possition);
                    OutPut.Add(temp);
                }
                else if (fig.Type == Figure.FigureType.Queen)
                {
                    Queen temp = new Queen(color, possition);
                    OutPut.Add(temp);
                }
                else
                {
                    Rook temp = new Rook(color, possition);
                    OutPut.Add(temp);
                }
            }
            return OutPut;
        }

        public bool IsOutOfBoard(Tuple<int, int> figureMovement)
        {
            if (figureMovement.Item1 < 0 || figureMovement.Item2 < 0 ||
                figureMovement.Item1 > 7 || figureMovement.Item2 > 7)
            {
                return true;
            }
            return false;
        }          
    }
}
