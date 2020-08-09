using Chess.Components;
using System;
using System.Drawing;

namespace Chess.Figures
{
    public class King : Figure
    {
        public King(FigureColor figureColor, Tuple<int, int> figureStartPossition) : base(figureColor, figureStartPossition)
        {
            Type = FigureType.King;
            if (figureColor == FigureColor.Black)
            {
                Image = Image.FromFile(@"D:Images\BlackKing.png");
            }
            else
            {
                Image = Image.FromFile(@"D:Images\WhiteKing.png");
            }
        }
    }
}
