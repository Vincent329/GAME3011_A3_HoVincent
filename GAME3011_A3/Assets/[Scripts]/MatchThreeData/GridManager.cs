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
                GameObject basePiece = Instantiate(piecePrefabDictionary[PieceTypeEnum.NORMAL], Vector3.zero, Quaternion.identity);
                basePiece.name = "Piece (" + i + "," + j + ")";
                basePiece.transform.SetParent(this.transform);

                gamePieces[i, j] = basePiece.GetComponent<BasePiece>();
                gamePieces[i, j].Initialize(i, j, this, PieceTypeEnum.NORMAL);

                if (gamePieces[i, j].isMovable())
                {
                    gamePieces[i, j].MovablePiece.MovePiece(i, j);
                }
                if (gamePieces[i, j].IsColored())
                {
                    gamePieces[i, j].PieceSprite.SetType((PieceSprites.DiamondType)Random.Range(0, gamePieces[i,j].PieceSprite.NumTypes));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
