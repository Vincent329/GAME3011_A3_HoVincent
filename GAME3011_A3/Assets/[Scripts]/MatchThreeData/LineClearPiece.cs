using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherit from the clearable piece so that we can extend some functionality to clear lines
/// </summary>
public class LineClearPiece : ClearablePiece
{

    public bool isRowType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ClearPiece()
    {
        base.ClearPiece();

        if (isRowType)
        {
            Debug.Log("Clear Row");
            Debug.Log(piece);
            piece.GridRef.ClearRow(piece.YPos);
        } else
        {
            Debug.Log("Column Clear");
            Debug.Log(piece);
            piece.GridRef.ClearColumn(piece.XPos);
        }
    }
}
