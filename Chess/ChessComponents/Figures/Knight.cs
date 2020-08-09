using Chess.Components;
using System;
using System.Drawing;

namespace Chess.Figures
{
    public class Knight : Figure
    {
        public Knight(FigureColor figureColor, Tuple<int, int> figureStartPossition) : base(figureColor, figureStartPossition)
        {
            Type = FigureType.Knight;
            if (figureColor == FigureColor.Black)
            {
                Image = Image.FromFile(@"D:Images\BlackKnight.png");
            }
            else
            {
                Image = Image.FromFile(@"D:Images\WhiteKnight.png");
            }
        }
    }
}
