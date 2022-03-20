using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BasePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerClickHandler
{

    [SerializeField] private int xPos;
    [SerializeField] private int yPos;
    [SerializeField] private bool isHovered;

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

    public void Initialize(int x, int y, GridManager _gridRef, PieceTypeEnum pieceType)
    {
        xPos = x;
        yPos = y;
        gridRef = _gridRef;
        type = pieceType;
    }

    public void OnPointerEnter(PointerEventData evt)
    {
        Debug.Log("Enter " + XPos + "," + YPos);
        gridRef.HoverPiece(this);
        isHovered = true;
    }
    public void OnPointerExit(PointerEventData evt)
    {
        isHovered = false;
    }

    public void OnPointerClick(PointerEventData evt)
    {
        Debug.Log("Click");
    }

    public void OnPointerDown(PointerEventData evt)
    {
        Debug.Log("Clicked on Piece: " + XPos + "," + YPos);
        gridRef.PressPiece(this);
    }

    public void OnPointerUp(PointerEventData evt)
    {
        gridRef.ReleasePiece();
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
