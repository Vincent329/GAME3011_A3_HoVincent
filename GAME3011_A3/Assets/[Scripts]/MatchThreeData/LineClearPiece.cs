using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherit from the clearable piece so that we can extend some functionality to clear lines
/// </summary>
public class LineClearPiece : ClearablePiece
{

    public bool isRowType;

    public override void ClearPiece()
    {
        base.ClearPiece();

        if (isRowType)
        {
            
            piece.GridRef.ClearRow(piece.YPos);
        } else
        {
            
            piece.GridRef.ClearColumn(piece.XPos);
        }
    }
}
