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

    private Dictionary<PieceTypeEnum, GameObject> piecePrefabDictionary;

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
            yield return new WaitForSeconds(fillTime);
        }
    }
    public bool FillStep()
    {
        bool movedPiece = false;
        // bottom to top check of the grid
        for (int y = 1; y < gridY; y++)
        {
            for (int x = 0; x < gridX; x++)
            {
                // check the piece at the current iteration
                BasePiece piece = gamePieces[x, y];

                // if it's movable, we check the piece under it
                if (piece.isMovable())
                {
                    BasePiece pieceBelow = gamePieces[x, y - 1];
                    if (pieceBelow.Type == PieceTypeEnum.EMPTY)
                    {
                        piece.MovablePiece.MovePiece(x, y - 1);
                        gamePieces[x, y - 1] = piece;
                        SpawnPiece(x, y, PieceTypeEnum.EMPTY);
                        movedPiece = true;
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
                GameObject newPiece = Instantiate(piecePrefabDictionary[PieceTypeEnum.NORMAL], GetGridPosition(x, gridY), Quaternion.identity);
                newPiece.transform.SetParent(transform);

                gamePieces[x, gridY - 1] = newPiece.GetComponent<BasePiece>();
                gamePieces[x, gridY - 1].Initialize(x, gridY, this, PieceTypeEnum.NORMAL);
                gamePieces[x, gridY - 1].MovablePiece.MovePiece(x, gridY - 1);
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
}
