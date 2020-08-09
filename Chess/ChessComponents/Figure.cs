using System;
using System.Collections.Generic;
using System.Drawing;

namespace Chess.Components
{
    

 

    public abstract class Figure
    {
        public enum FigureType
        {
            Pawn,
            Rook,
            Knight,
            Bishop,
            Queen,
            King
        }

        public enum FigureColor
        {
            Black,
            White
        }

        public FigureColor Color { get; protected set; }
        public FigureType Type { get; protected set; }
        public Image Image { get; protected set; }
        public Tuple<int, int> CurrentPossition { get; private set; }
        public bool WasMoved { get; set; } = false;

        public Figure(FigureColor figureColor,Tuple<int,int>figureStartPossition) 
        {
            Color = figureColor;
            CurrentPossition = figureStartPossition;
        }               
      
        public void UpdatePossition(Tuple<int,int> figureMovement)
        {
            CurrentPossition = figureMovement;
        }
    }   
}
    