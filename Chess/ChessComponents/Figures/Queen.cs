using Chess.Components;
using System;
using System.Drawing;

namespace Chess.Figures
{
    public class Queen : Figure
    {
        public Queen(FigureColor figureColor, Tuple<int, int> figureStartPossition) : base(figureColor, figureStartPossition)
        {
            Type = FigureType.Queen;
            if (figureColor == FigureColor.Black)
            {
                Image = Image.FromFile(@"D:Images\BlackQueen.png");
            }
            else
            {
                Image = Image.FromFile(@"D:Images\WhiteQueen.png");
            }
        }
    }
}

