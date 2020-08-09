using Chess.Components;
using System;
using System.Drawing;

namespace Chess.Figures
{
    public class Pawn : Figure
    {
        public Pawn(FigureColor figureColor, Tuple<int, int> figureStartPossition) : base(figureColor, figureStartPossition)
        {
            Type = FigureType.Pawn;
            if (figureColor == FigureColor.Black)
            {
                Image = Image.FromFile(@"D:Images\BlackPawn.png");
            }
            else
            {
                Image = Image.FromFile(@"D:Images\WhitePawn.png");
            }
        }

        public void ReplacePawn(Tuple<int,int> pawnPossition)
        {

        }
    }
}

