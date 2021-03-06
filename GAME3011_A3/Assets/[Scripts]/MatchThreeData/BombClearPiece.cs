using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bomb class inheriting from the base clear piece class
public class BombClearPiece : ClearablePiece
{
    public override void ClearPiece()
    {
        base.ClearPiece();

        Debug.Log("Bomb");
        piece.GridRef.ClearBomb(piece.XPos, piece.YPos);
    }
}
