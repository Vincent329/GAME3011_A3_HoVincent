using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BasePiece : MonoBehaviour , IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
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
            xPos = value;
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

    [SerializeField]
    private GridManager gridRef;
    public GridManager GridRef => gridRef;

    // --------- PIECE COMPONENTS -----------

    private MovablePiece movementPiece;
    public MovablePiece MovementPiece => movementPiece;

    private PieceSprites pieceSprite;
    public PieceSprites PieceSprite => pieceSprite;

    private ClearablePiece clearPieceComponent;
    public ClearablePiece ClearPieceComponent => clearPieceComponent;
    
    private void Awake()
    {
        movementPiece = GetComponent<MovablePiece>();
        pieceSprite = GetComponent<PieceSprites>();
        clearPieceComponent = GetComponent<ClearablePiece>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(int x, int y, GridManager _gridRef, PieceTypeEnum pieceType)
    {
        xPos = x;
        yPos = y;
        gridRef = _gridRef;
        type = pieceType;
    }

    public void OnPointerEnter(PointerEventData evt)
    {
        gridRef.HoverPiece(this);
    }

    public void OnPointerDown(PointerEventData evt)
    {
        gridRef.PressPiece(this);
    }

    public void OnPointerUp(PointerEventData evt)
    {
        gridRef.ReleasePiece();
    }

    public bool isMovable()
    {
        return movementPiece != null;
    }

    public bool IsDiamond()
    {
        return pieceSprite != null;
    }

    public bool isClearable()
    {
        return clearPieceComponent != null;
    }
}
