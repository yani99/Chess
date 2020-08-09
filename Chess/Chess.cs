using Chess.Components;
using Chess.Figures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Chess
{
    public class Chess
    {
        private const int boardSize = 8;
        private List<Figure> AllFigures { get; }
        public Tuple<PictureBox[,], Figure[,]> Board { get; private set; }
        public List<Figure> WhiteFigures { get; private set; }
        public List<Figure> BlackFigures { get; private set; }

        public Chess()
        {
            Board = new Tuple<PictureBox[,], Figure[,]>(new PictureBox[boardSize, boardSize], new Figure[boardSize, boardSize]);
            AllFigures = new List<Figure>();
            WhiteFigures = new List<Figure>();
            BlackFigures = new List<Figure>(); 
        }

        public void LoadBoard(Form1 form)
        {
            DrawBoard(form);
            LoadFigures();          
        }

        private void DrawBoard(Form form)
        {
            const int TileSize = 70;
            Color Color1 = Color.White;
            Color Color2 = Color.Gray;

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    PictureBox PictureBox = new PictureBox
                    {
                        Size = new Size(TileSize, TileSize),
                        Location = new Point(TileSize * j, TileSize * i)
                    };

                    if (i % 2 == 0)
                    {
                        if (j % 2 != 0)
                        {
                            PictureBox.BackColor = Color2;
                        }
                        else
                        {
                            PictureBox.BackColor = Color1;
                        }
                    }
                    else
                    {
                        if (j % 2 != 0)
                        {
                            PictureBox.BackColor = Color1;
                        }
                        else
                        {
                            PictureBox.BackColor = Color2;
                        }
                    }
                    form.Controls.Add(PictureBox);
                    Board.Item1[i, j] = PictureBox;
                }
            }
        }

        private void LoadFigures()
        {
            LoadPawns();
            LoadRooks();
            LoadHorses();
            LoadOfficers();
            LoadQueens();
            LoadKings();
        }

        private void LoadPawns()
        {
            for (int i = 0; i < 8; i++)
            {
                Pawn WhitePawn = new Pawn(Figure.FigureColor.Black, new Tuple<int, int>(1, i));
                DrawFigure(WhitePawn);
                AddFigure(WhitePawn);

                Pawn BlackPawn = new Pawn(Figure.FigureColor.White, new Tuple<int, int>(6, i));
                DrawFigure(BlackPawn);
                AddFigure(BlackPawn);
            }
        }

        private void LoadRooks()
        {
            int PossitionY = 7;

            for (int i = 0; i < 2; i++)
            {
                Rook BlackRook = new Rook( Figure.FigureColor.Black, new Tuple<int, int>(0, i * PossitionY));
                DrawFigure(BlackRook);
                AddFigure(BlackRook);

                Rook WhiteRook = new Rook(Figure.FigureColor.White, new Tuple<int, int>(7, i * PossitionY));
                DrawFigure(WhiteRook);
                AddFigure(WhiteRook);
            }
        }

        private void LoadHorses()
        {
            int PossitionY = 1;

            for (int i = 0; i < 2; i++)
            {
                if (i == 1)
                {
                    PossitionY = 6;
                }

                Knight BlackHorse = new Knight(Figure.FigureColor.Black, new Tuple<int, int>(0, PossitionY));
                DrawFigure(BlackHorse);
                AddFigure(BlackHorse);

                Knight WhiteHorse = new Knight(Figure.FigureColor.White, new Tuple<int, int>(7, PossitionY));
                DrawFigure(WhiteHorse);
                AddFigure(WhiteHorse);
            }
        }

        private void LoadOfficers()
        {
            int PossitionY = 2;

            for (int i = 0; i < 2; i++)
            {
                if (i == 1)
                {
                    PossitionY = 5;
                }             
                Bishop BlackOfficer = new Bishop(Figure.FigureColor.Black, new Tuple<int, int>(0, PossitionY));
                DrawFigure(BlackOfficer);
                AddFigure(BlackOfficer);

                Bishop WhiteOfficer = new Bishop(Figure.FigureColor.White, new Tuple<int, int>(7, PossitionY));
                DrawFigure(WhiteOfficer);
                AddFigure(WhiteOfficer);
            }
        }

        private void LoadQueens()
        {
            Queen BlackQueen = new Queen(Figure.FigureColor.Black, new Tuple<int, int>(0, 3));
            DrawFigure(BlackQueen);
            AddFigure(BlackQueen);

            Queen WhiteQueen = new Queen(Figure.FigureColor.White, new Tuple<int, int>(7, 3));
            DrawFigure(WhiteQueen);
            AddFigure(WhiteQueen);
        }

        private void LoadKings()
        {
            King BlackKing = new King(Figure.FigureColor.Black, new Tuple<int, int>(0, 4));
            DrawFigure(BlackKing);
            AddFigure(BlackKing);

            King WhiteKing = new King(Figure.FigureColor.White, new Tuple<int, int>(7, 4));
            DrawFigure(WhiteKing);
            AddFigure(WhiteKing);
        }

        private void DrawFigure(Figure figure)
        {
            int PossitionX = figure.CurrentPossition.Item1;
            int PossitionY = figure.CurrentPossition.Item2;

            Board.Item1[PossitionX, PossitionY].BackgroundImage = figure.Image;
            Board.Item1[PossitionX, PossitionY].BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void AddFigure(Figure figure)
        {
            Board.Item2[figure.CurrentPossition.Item1, figure.CurrentPossition.Item2] = figure;
            if (figure.Color == Figure.FigureColor.Black)
            {
                BlackFigures.Add(figure);
            }
            else
            {
                WhiteFigures.Add(figure);
            }
            AllFigures.Add(figure);
        }

        public void RemoveFigure(Figure figure)
        {
            int PossitionX = figure.CurrentPossition.Item1;
            int PossitionY = figure.CurrentPossition.Item2;

            Board.Item1[PossitionX, PossitionY].BackgroundImage = null;
            Board.Item2[PossitionX, PossitionY] = null;

            if (figure.Color == Figure.FigureColor.Black)
            {
                BlackFigures.Remove(figure);
            }
            else
            {
                WhiteFigures.Remove(figure);
            }
        }

        public void UpdateFigurePossition(Figure figure, Tuple<int, int> movement)
        {
            int previousPossitionX = figure.CurrentPossition.Item1;
            int previousPossitionY = figure.CurrentPossition.Item2;
            int newPossitionX = movement.Item1;
            int newPossitionY = movement.Item2;

            Board.Item1[newPossitionX, newPossitionY].BackgroundImage = figure.Image;
            Board.Item1[newPossitionX, newPossitionY].BackgroundImageLayout = ImageLayout.Stretch;
            Board.Item1[previousPossitionX, previousPossitionY].BackgroundImage = null;

            Board.Item2[newPossitionX, newPossitionY] = figure;
            Board.Item2[previousPossitionX, previousPossitionY] = null;
            figure.WasMoved = true;
            figure.UpdatePossition(movement);
        }
    }
}
