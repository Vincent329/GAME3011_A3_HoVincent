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

    [SerializeField] private int gridX;
    [SerializeField] private int gridY;

    private Dictionary<PieceType, GameObject> piecePrefabDictionary;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
