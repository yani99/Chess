using Chess.Components;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace Chess.Figures
{
    public class Rook : Figure
    {
        public Rook(FigureColor figureColor, Tuple<int, int> figureStartPossition) : base(figureColor, figureStartPossition)
        {
            Type = FigureType.Rook;
            if (figureColor == FigureColor.Black)
            {
                Image = Image.FromFile(@"D:Images\BlackRook.png");
            }
            else
            {
                Image = Image.FromFile(@"D:Images\WhiteRook.png");
            }
        }
    }
}
