using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePiece : MonoBehaviour
{

    private BasePiece piece;

    private void Awake()
    {
        piece = GetComponent<BasePiece>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MovePiece(int x, int y)
    {

        Debug.Log("Moving Piece to " + x + "," + y);
       
        piece.XPos = x;
        piece.YPos = y;

        piece.GetComponent<RectTransform>().position = piece.GridRef.GetGridPosition(x, y);
    }
}
