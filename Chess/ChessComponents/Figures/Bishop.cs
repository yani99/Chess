using Chess.Components;
using System;
using System.Drawing;

namespace Chess.Figures
{
    public class Bishop : Figure
    {
        public Bishop(FigureColor figureColor, Tuple<int, int> figureStartPossition) : base(figureColor, figureStartPossition)
        {
           
            Type = FigureType.Bishop;
            if (figureColor == FigureColor.Black)
            {
                Image = Image.FromFile(@"D:Images\BlackBishop.png");
            }
            else
            {
                Image = Image.FromFile(@"D:Images\WhiteBishop.png");
            }
        }
    }
}
