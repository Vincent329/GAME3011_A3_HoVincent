using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePiece : MonoBehaviour
{

    [SerializeField] private int xPos;
    [SerializeField] private int yPos;

    public int XPos
    {
        get
        {
            return xPos;
        }
        set
        {
            if (isMovable())
            yPos = value;
        }
    }
    public int YPos
    {
        get
        {
            return yPos;
        }
        set
        {
            if (isMovable())
            yPos = value;
        }
    }

    private PieceTypeEnum type;

    public PieceTypeEnum Type => type;

    private GridManager gridRef;
    public GridManager GridRef => gridRef;

    private MovablePiece movablePiece;
    public MovablePiece MovablePiece => movablePiece;

    private PieceSprites pieceSprite;
    public PieceSprites PieceSprite => pieceSprite;
    private void Awake()
    {
        movablePiece = GetComponent<MovablePiece>();
        pieceSprite = GetComponent<PieceSprites>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(int x, int y, GridManager _gridRef, PieceTypeEnum pieceType)
    {
        xPos = x;
        yPos = y;
        gridRef = _gridRef;
        type = pieceType;
    }

    public bool isMovable()
    {
        return movablePiece != null;
    }

    public bool IsColored()
    {
        return pieceSprite != null;
    }
}
