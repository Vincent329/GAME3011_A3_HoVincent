using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public enum PieceType
    {
        NORMAL,
        COUNT
    }


    [System.Serializable]
    public struct PiecePrefab
    {
        public PieceType type;
        public GameObject prefab;
    }

    // Grid Dimensions
    [SerializeField] private int gridX;
    [SerializeField] private int gridY;

    // Pieces configuration
    [SerializeField] private List<PiecePrefab> piecePrefabs;
    [SerializeField] private GameObject backgroundSprite;
    private float spriteDimension;

    private GameObject[,] gamePieces;

    private Dictionary<PieceType, GameObject> piecePrefabDictionary;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gridTile = null;
        piecePrefabDictionary = new Dictionary<PieceType, GameObject>();

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

        gamePieces = new GameObject[gridX, gridY];

        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridY; j++)
            {
                gamePieces[i, j] = Instantiate(piecePrefabDictionary[PieceType.NORMAL], GetGridPosition(i, j), Quaternion.identity);
                gamePieces[i, j].name = "Piece (" + i + "," + j + ")";
                gamePieces[i, j].transform.SetParent(this.transform);

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
    Vector2 GetGridPosition(int x, int y)
    {
        Vector2 GridPos = new Vector2(transform.position.x - ((gridX / 2) * spriteDimension) + x * spriteDimension,
                                      transform.position.y - ((gridY / 2) * spriteDimension) + y * spriteDimension);
        return GridPos;
    }
}
