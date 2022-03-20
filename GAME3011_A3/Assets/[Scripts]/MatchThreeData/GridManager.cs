using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    [System.Serializable]
    public struct PiecePrefab
    {
        public PieceTypeEnum type;
        public GameObject prefab;
    }

    // Grid Dimensions
    [SerializeField] private int gridX;
    [SerializeField] private int gridY;
    [SerializeField] private float fillTime;

    // Pieces configuration
    [SerializeField] private List<PiecePrefab> piecePrefabs;
    [SerializeField] private GameObject backgroundSprite;
    private float spriteDimension;

    private BasePiece[,] gamePieces;
    private bool inverse = false; // diagonal falling check, will see if the block falls one way or the other 
                                  // potential that more than 1 piece can fill an empty space
                                  // swap the direction tiles can fall down

    // Mouse events, when we click, we hold a reference to the piece
    private BasePiece selectedPiece;
    private BasePiece lastEnteredPiece;

    private Dictionary<PieceTypeEnum, GameObject> piecePrefabDictionary;

    private void PlaceBlocks()
    {
        Destroy(gamePieces[4, 4].gameObject);
        SpawnPiece(4, 4, PieceTypeEnum.BLOCK); 

        Destroy(gamePieces[8, 4].gameObject);
        SpawnPiece(8, 4, PieceTypeEnum.BLOCK); 

        Destroy(gamePieces[5, 5].gameObject);
        SpawnPiece(5, 5, PieceTypeEnum.BLOCK); 

        Destroy(gamePieces[0, 0].gameObject);
        SpawnPiece(0, 0, PieceTypeEnum.BLOCK);

        Destroy(gamePieces[2, 9].gameObject);
        SpawnPiece(2, 9, PieceTypeEnum.BLOCK);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject gridTile = null;
        piecePrefabDictionary = new Dictionary<PieceTypeEnum, GameObject>();

        // instantiate the game pieces
        for (int i = 0; i < piecePrefabs.Count; i++)
        {
            if (!piecePrefabDictionary.ContainsKey(piecePrefabs[i].type))
            {
                piecePrefabDictionary.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }

        spriteDimension = backgroundSprite.GetComponent<RectTransform>().rect.width;

        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridY; j++)
            {
                gridTile = Instantiate(backgroundSprite, GetGridPosition(i, j), Quaternion.identity);
                gridTile.transform.SetParent(this.transform);
            }
        }

        gamePieces = new BasePiece[gridX, gridY];

        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridY; j++)
            {
                SpawnPiece(i, j, PieceTypeEnum.EMPTY);
            }
        }

        // placing obbstacles
        PlaceBlocks();

        StartCoroutine(Fill());
    }

    public BasePiece SpawnPiece(int x, int y, PieceTypeEnum type)
    {
        GameObject pieceToSpawn = Instantiate(piecePrefabDictionary[type], GetGridPosition(x, y), Quaternion.identity);
        pieceToSpawn.transform.SetParent(this.transform);

        gamePieces[x, y] = pieceToSpawn.GetComponent<BasePiece>();
        gamePieces[x, y].Initialize(x, y, this, type);

        return gamePieces[x, y];
    
    }

    public IEnumerator Fill()
    {
        while (FillStep())
        {
            inverse = !inverse;
            yield return new WaitForSeconds(fillTime);
        }
    }
    public bool FillStep()
    {
        bool movedPiece = false;
        // bottom to top check of the grid
        for (int y = 1; y < gridY; y++)
        {
            for (int loopX = 0; loopX < gridX; loopX++)
            {
                // by default we'll check from 0 to the grid's maximum limit
                int x = loopX;

                if (inverse)
                {
                    x = gridX - 1 - loopX; // if inverse, we check our position from the opposite side
                }
                // check the piece at the current iteration
                BasePiece piece = gamePieces[x, y];

                // if it's movable, we check the piece under it
                if (piece.isMovable())
                {
                    BasePiece pieceBelow = gamePieces[x, y - 1];
                    if (pieceBelow.Type == PieceTypeEnum.EMPTY)
                    {
                        Destroy(pieceBelow.gameObject);
                        piece.MovablePiece.MovePiece(x, y - 1, fillTime);
                        gamePieces[x, y - 1] = piece;
                        SpawnPiece(x, y, PieceTypeEnum.EMPTY);
                        movedPiece = true;
                    }
                    else
                    {
                        // Diagonal checks
                        for (int diag = -1; diag <= 1; diag++)
                        {
                            // if diag is 0, it's the piece directly below
                            if (diag != 0)
                            {
                                int diagX = x + diag; // traverse to the right;
                                if (inverse)
                                {
                                    diagX = x - diag; // traverse to the left
                                }    
                                 // if we're within the bounds of the grid, it's a valid position
                                if (diagX >= 0 && diagX < gridX)
                                {
                                    BasePiece pieceAtDiagonal = gamePieces[diagX, y - 1];

                                    if (pieceAtDiagonal.Type == PieceTypeEnum.EMPTY)
                                    {
                                        bool hasPieceAbove = true;
                                        for (int aboveY = y; aboveY < gridY; aboveY++)
                                        {
                                            BasePiece pieceAbove = gamePieces[diagX, aboveY];
                                            if (pieceAbove.isMovable()) // break the loop if the piece above is movable
                                                break;
                                            else if (!pieceAbove.isMovable() && pieceAbove.Type != PieceTypeEnum.EMPTY) // if this is a block type
                                            { 
                                                hasPieceAbove = false;
                                                break;
                                            } 
                                        
                                        }
                                        if (!hasPieceAbove)
                                        {
                                            // Similar fill code from the top down, only to account for a diagonal transition
                                            Destroy(pieceAtDiagonal.gameObject);
                                            piece.MovablePiece.MovePiece(diagX, y - 1, fillTime);
                                            gamePieces[diagX, y - 1] = piece;
                                            SpawnPiece(x, y, PieceTypeEnum.EMPTY);
                                            movedPiece = true;
                                            break;
                                        }

                                    }
                                }
                            }
                        }

                    }
                }
            }
        }

        // check the first row
        for (int x = 0; x < gridX; x++)
        {
            BasePiece pieceBelow = gamePieces[x, gridY - 1];
            if (pieceBelow.Type == PieceTypeEnum.EMPTY)
            {
                Destroy(pieceBelow.gameObject);
                GameObject newPiece = Instantiate(piecePrefabDictionary[PieceTypeEnum.NORMAL], GetGridPosition(x, gridY), Quaternion.identity);
                newPiece.transform.SetParent(transform);

                gamePieces[x, gridY - 1] = newPiece.GetComponent<BasePiece>();
                gamePieces[x, gridY - 1].Initialize(x, gridY, this, PieceTypeEnum.NORMAL);
                gamePieces[x, gridY - 1].MovablePiece.MovePiece(x, gridY - 1, fillTime);
                gamePieces[x, gridY - 1].PieceSprite.SetType((PieceSprites.DiamondType)Random.Range(0, gamePieces[x, gridY - 1].PieceSprite.NumTypes));
                movedPiece = true;
            }
        }

        return movedPiece;
    }

    /// <summary>
    /// Gets a position on the grid
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Vector2 GetGridPosition(int x, int y)
    {
        Vector2 GridPos = new Vector2(transform.position.x - ((gridX / 2) * spriteDimension) + x * spriteDimension,
                                      transform.position.y - ((gridY / 2) * spriteDimension) + y * spriteDimension);
        return GridPos;
    }

    public bool adjacentPiece(BasePiece p1, BasePiece p2)
    {
        return (p1.XPos == p2.XPos && (int)Mathf.Abs(p1.YPos - p2.YPos) == 1// check if y coordinates are within 1 space of each other
            || (p1.YPos == p1.YPos && (int)Mathf.Abs(p1.XPos - p2.YPos) == 1)); // check if x coordinates are 1 space away within each other 
    }

    public void SwapPieces(BasePiece p1, BasePiece p2)
    {
        if (p1.isMovable() && p2.isMovable())
        {
            gamePieces[p1.XPos, p1.YPos] = p2;
            gamePieces[p2.XPos, p2.YPos] = p1;

            int p1XPos = p1.XPos;
            int p1YPos = p1.YPos;

            p1.MovablePiece.MovePiece(p2.XPos, p2.YPos, fillTime);
            p2.MovablePiece.MovePiece(p1XPos, p1YPos, fillTime);
        }
    }

    // Mouse Interaction Events
    public void PressPiece(BasePiece pieceSelect)
    {
        selectedPiece = pieceSelect;
    }

    public void HoverPiece(BasePiece enteredPiece)
    {
        lastEnteredPiece = enteredPiece;
    }

    public void ReleasePiece()
    {
        if (adjacentPiece(selectedPiece, lastEnteredPiece))
        {
            SwapPieces(selectedPiece, lastEnteredPiece);
        }

    }
}
