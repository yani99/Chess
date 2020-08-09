using System;
using System.Windows.Forms;
using Chess.Components;
using System.Collections.Generic;
using System.Drawing;
using System.Linq; 

namespace Chess
{
    public partial class Form1 : Form
    {
        private Chess chess;
        private ChessAnalyzer chessAnalyzer;
        private Figure figure;
        private Figure.FigureColor figuresTurn = Figure.FigureColor.White;
        private List<Tuple<Color, Tuple<int, int>>> tileNormalColorBeforeTheChange;
        private List<Tuple<int, int>> availableMoves;

        public Form1()
        {            
            InitializeComponent();
            chess = new Chess();
            chess.LoadBoard(this);
            chessAnalyzer = new ChessAnalyzer(chess.WhiteFigures, chess.BlackFigures);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    chess.Board.Item1[i, j].MouseClick += Chess_MouseClick;
                }
            }
        }

        private void Chess_MouseClick(object sender, MouseEventArgs e)
        {
            if (HasAFigureOnThisPicturebox((PictureBox)sender).Item1)
            {
                Figure temp = HasAFigureOnThisPicturebox((PictureBox)sender).Item2;
                if (temp.Color == figuresTurn)
                {
                    if (availableMoves != null)
                    {
                        RemoveColorOnAvailableMoves(availableMoves);
                    }
                    figure = temp;
                    availableMoves = chessAnalyzer.AvailableMoves(figure);
                    tileNormalColorBeforeTheChange = AddColorOnAvailableMoves(availableMoves);
                }
            }
            if (availableMoves != null)
            {
                Tuple<int, int> movement = GetPossitionOfPictureBox((PictureBox)sender);
                if (availableMoves.Contains(movement))
                {
                    if (figure.Type == Figure.FigureType.Pawn)
                    {
                        if (figure.Color == Figure.FigureColor.Black)
                        {
                            if (movement.Item1 == 7)
                            {
                                ChangePawn(figure);
                            }
                        }   
                        else
                        {
                            if (movement.Item1 == 0)
                            {
                                ChangePawn(figure);
                            }  
                        }
                    }

                    if (figure.Type == Figure.FigureType.King)
                    {
                        Tuple<int, int> possition = GetPossitionOfPictureBox((PictureBox)sender);
                        if (figure.WasMoved == false &&
                            availableMoves.Contains(possition))
                        {
                            if (possition.Item1 == figure.CurrentPossition.Item1 &&
                                possition.Item2 + 2 == figure.CurrentPossition.Item2 ||
                                possition.Item2 - 2 == figure.CurrentPossition.Item2)
                            {
                                List<Tuple<Figure, Tuple<int, int>>> smallRokade = chessAnalyzer.SmallRodake(figure);
                                List<Tuple<Figure, Tuple<int, int>>> longRokade = chessAnalyzer.LongRokade(figure);
                                if (smallRokade.Count != 0 && smallRokade[0].Item2.Equals(possition))
                                {
                                    chess.UpdateFigurePossition(smallRokade[1].Item1, smallRokade[1].Item2);
                                }
                                if (longRokade.Count != 0 && longRokade[0].Item2 == possition)
                                {
                                    chess.UpdateFigurePossition(longRokade[1].Item1, longRokade[1].Item2);
                                }
                            }
                        }
                    }

                    Tuple<bool, Figure> willImpactWithEnemy = chessAnalyzer.WillImpactWithEnemy(figure, movement);
                    if (willImpactWithEnemy.Item1)
                    {
                        chess.RemoveFigure(willImpactWithEnemy.Item2);
                        chess.UpdateFigurePossition(figure, movement);

                    }                   
                    else
                    {
                        chess.UpdateFigurePossition(figure, movement);
                    }          

                    if (figuresTurn == Figure.FigureColor.Black)
                    {
                        figuresTurn = Figure.FigureColor.White;
                    }
                    else
                    {
                        figuresTurn = Figure.FigureColor.Black;
                    }
                    availableMoves = null;
                    RemoveColorOnAvailableMoves(availableMoves);
                }
            }
            WinTeam();
        }

        private Tuple<bool, Figure> HasAFigureOnThisPicturebox(PictureBox pictureBox)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (pictureBox.Location == chess.Board.Item1[i, j].Location)
                    {
                        if (chess.Board.Item2[i, j] != null)
                        {
                            return new Tuple<bool, Figure>(true, chess.Board.Item2[i, j]);
                        }
                        else
                        {
                            return new Tuple<bool, Figure>(false, null);
                        }
                    }
                }
            }
            return new Tuple<bool, Figure>(false, null);
        }

        private Tuple<int, int> GetPossitionOfPictureBox(PictureBox pictureBox)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (pictureBox.Location == chess.Board.Item1[i, j].Location)
                    {
                        return new Tuple<int, int>(i, j);
                    }
                }
            }
            return null;
        }

        private List<Tuple<Color, Tuple<int, int>>> AddColorOnAvailableMoves(List<Tuple<int, int>> availableMoves)
        {
            List<Tuple<Color, Tuple<int, int>>> tileNormalColorBeforeChange = new List<Tuple<Color, Tuple<int, int>>>();
            for (int i = 0; i < availableMoves.Count; i++)
            {
                int PossitionX = availableMoves[i].Item1;
                int PossitionY = availableMoves[i].Item2;

                tileNormalColorBeforeChange.Add(new Tuple<Color, Tuple<int, int>>(chess.Board.Item1[PossitionX, PossitionY].BackColor, availableMoves[i]));
                chess.Board.Item1[PossitionX, PossitionY].BackColor = Color.LightBlue;
            }
            return tileNormalColorBeforeChange;
        }

        private void RemoveColorOnAvailableMoves(List<Tuple<int, int>> availableMoves)
        {
            for (int i = 0; i < tileNormalColorBeforeTheChange.Count; i++)
            {
                chess.Board.Item1[tileNormalColorBeforeTheChange[i].Item2.Item1, tileNormalColorBeforeTheChange[i].Item2.Item2].BackColor = tileNormalColorBeforeTheChange[i].Item1;
            }
        }

        private void ChangePawn(Figure figure,Tuple<int,int> movement)
        {
            
        }

        private Figure ChangePawn(Figure figure)
        {
            return null;
        }

        private void WinTeam()
        {
            List<Tuple<int, int>> blackFiguresMoves = new List<Tuple<int, int>>();
            for (int i = 0; i < chess.BlackFigures.Count; i++)
            {
                blackFiguresMoves.AddRange(chessAnalyzer.AvailableMoves(chess.BlackFigures[i]));
            }

            List<Tuple<int, int>> WhiteFiguresMoves = new List<Tuple<int, int>>();
            for (int i = 0; i < chess.WhiteFigures.Count; i++)
            {
                WhiteFiguresMoves.AddRange(chessAnalyzer.AvailableMoves(chess.WhiteFigures[i]));
            }

            if (blackFiguresMoves.Count == 0)
            {
                MessageBox.Show("White Team Win");
            }
            if (WhiteFiguresMoves.Count == 0)
            {
                MessageBox.Show("Black Team Win");
            }
        }

        private void Restart()
        {
            figuresTurn = Figure.FigureColor.White;
            chess = new Chess();
            chess.LoadBoard(this);
            chessAnalyzer = new ChessAnalyzer(chess.WhiteFigures, chess.BlackFigures);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    chess.Board.Item1[i, j].MouseClick += Chess_MouseClick;
                }
            }
        }
    }
}
