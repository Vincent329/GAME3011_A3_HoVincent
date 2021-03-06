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

    // State check if the game is over, then don't interact with the grid

    // Mouse events, when we click, we hold a reference to the piece
    private BasePiece selectedPiece;
    private BasePiece lastEnteredPiece;

    // Dictionary for easier lookups of the piece prefabs
    private Dictionary<PieceTypeEnum, GameObject> piecePrefabDictionary;

    private void PlaceBlocks()
    {
        if (GameManager.Instance.GameDifficulty == DifficultyEnum.HARD)
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
            
            Destroy(gamePieces[8, 7].gameObject);
            SpawnPiece(8, 7, PieceTypeEnum.BLOCK);

            Destroy(gamePieces[8, 8].gameObject);
            SpawnPiece(8, 8, PieceTypeEnum.BLOCK);

            Destroy(gamePieces[7, 7].gameObject);
            SpawnPiece(7, 7, PieceTypeEnum.BLOCK);

            Destroy(gamePieces[7, 8].gameObject);
            SpawnPiece(7, 8, PieceTypeEnum.BLOCK);

            Destroy(gamePieces[3, 2].gameObject);
            SpawnPiece(3, 2, PieceTypeEnum.BLOCK);
        }
        else if (GameManager.Instance.GameDifficulty == DifficultyEnum.NORMAL)
        {
            Destroy(gamePieces[4, 4].gameObject);
            SpawnPiece(4, 4, PieceTypeEnum.BLOCK);

            Destroy(gamePieces[3, 4].gameObject);
            SpawnPiece(3, 4, PieceTypeEnum.BLOCK);
            Destroy(gamePieces[3, 2].gameObject);
            SpawnPiece(3, 2, PieceTypeEnum.BLOCK);

            Destroy(gamePieces[3, 3].gameObject);
            SpawnPiece(3, 3, PieceTypeEnum.BLOCK);

            Destroy(gamePieces[8, 7].gameObject);
            SpawnPiece(8, 7, PieceTypeEnum.BLOCK);

            Destroy(gamePieces[7, 7].gameObject);
            SpawnPiece(7, 7, PieceTypeEnum.BLOCK);


        }
    }
    private void InitializeBoard()
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

        // placing obstacles... change upon difficulty
        PlaceBlocks();

        StartCoroutine(Fill());
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Lose += ClearBoard;
    }

    private void OnEnable()
    {
        InitializeBoard();

    }

    private void OnDisable()
    {
        GameManager.Instance.Lose -= ClearBoard;
        ClearBoard();

    }

    private void ClearBoard()
    {
        StopAllCoroutines();

        for (int i = 0; i < transform.childCount; i++)
        {

            Destroy(transform.GetChild(i).gameObject);
        }
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
        bool needsRefilling = true;

        while (needsRefilling)
        {
            yield return new WaitForSeconds(fillTime);
            while (FillStep())
            {
                inverse = !inverse;
                yield return new WaitForSeconds(fillTime);
            }
            needsRefilling = ClearPotentialMatches();
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
                        piece.MovementPiece.MovePiece(x, y - 1, fillTime);
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
                                            piece.MovementPiece.MovePiece(diagX, y - 1, fillTime);
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
                gamePieces[x, gridY - 1].MovementPiece.MovePiece(x, gridY - 1, fillTime);
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
            || (p1.YPos == p2.YPos && (int)Mathf.Abs(p1.XPos - p2.XPos) == 1)); // check if x coordinates are 1 space away within each other 
    }

    public void SwapPieces(BasePiece p1, BasePiece p2)
    {
        if (GameManager.Instance.didGameFinish)
        {
            Debug.Log("Game's over, go home");
            return;
        }

        if (p1.isMovable() && p2.isMovable())
        {
            gamePieces[p1.XPos, p1.YPos] = p2;
            gamePieces[p2.XPos, p2.YPos] = p1;

            if (CheckMatch(p1, p2.XPos, p2.YPos) != null || CheckMatch(p2, p1.XPos, p1.YPos) != null)
            {

                int p1XPos = p1.XPos;
                int p1YPos = p1.YPos;

                p1.MovementPiece.MovePiece(p2.XPos, p2.YPos, fillTime);
                p2.MovementPiece.MovePiece(p1XPos, p1YPos, fillTime);

                // once a valid swap has been made, we clear the potential matches
                ClearPotentialMatches();

                if (p1.Type == PieceTypeEnum.ROW_CLEAR || p1.Type == PieceTypeEnum.COLUMN_CLEAR)
                {
                    ClearPiece(p1.XPos, p1.YPos);
                }
                if (p2.Type == PieceTypeEnum.ROW_CLEAR || p2.Type == PieceTypeEnum.COLUMN_CLEAR)
                {
                    ClearPiece(p2.XPos, p2.YPos);
                }

                selectedPiece = null;
                lastEnteredPiece = null;

                StartCoroutine(Fill());
            }
            else
            {
                gamePieces[p1.XPos, p1.YPos] = p1;
                gamePieces[p2.XPos, p2.YPos] = p2;
            }
        }
    }

    // Mouse Interaction Events
    // TODO: DON'T ACTIVATE THESE FUNCTIONS IF THE GAME IS OVER
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

    // ------------- MATCHING AND CLEARING EVENTS -------------------
    public List<BasePiece> CheckMatch(BasePiece pieceToCheck, int newX, int newY)
    {
        if (pieceToCheck.IsDiamond())
        {

            // take the enumerator of the piece to check and store it as a local variable
            PieceSprites.DiamondType diamond = pieceToCheck.PieceSprite.Type;
            List<BasePiece> horizontalPieces = new List<BasePiece>(); // 
            List<BasePiece> verticalPieces = new List<BasePiece>();
            List<BasePiece> matchingPieces = new List<BasePiece>();

            // --------- HORIZONTAL CHECK ----------
            // for every adjacent piece that's the same color, will be added to the appropriate list;
            // list will check for adjacent pieces checking up and doown
            horizontalPieces.Add(pieceToCheck); // we know this piece will be in any potential match

            // using the for loop to check which direction we're going
            for (int horizontalDirection = 0; horizontalDirection <= 1; horizontalDirection++)
            {
                // checking how far away the adjacent piece is from central piece
                for (int xOffset = 1; xOffset < gridX; xOffset++)
                {
                    int x;

                    if (horizontalDirection == 0) // check left direction
                    {
                        x = newX - xOffset;
                    }
                    else // check right direction
                    {
                        x = newX + xOffset;
                    }

                    if (x < 0 || x >= gridX)
                    {
                        break; // break out of the horizontal check and continue
                    }
                    if (gamePieces[x, newY].IsDiamond() && gamePieces[x, newY].PieceSprite.Type == diamond)
                    {
                        // add to the list of horizontal pieces if we've found a match
                        horizontalPieces.Add(gamePieces[x, newY]);
                    }
                    else
                    {
                        break;
                    }

                }
            }
            // should we reach the minimum amount of checks for a match
            if (horizontalPieces.Count >= 3)
            {
                for (int i = 0; i < horizontalPieces.Count; i++)
                {
                    matchingPieces.Add(horizontalPieces[i]);
                }
            }

            // L and T shape check (FOR HORIZONTAL, CHECKING UPWARD VERTICALLY FOR EVERY PIECE IN THE HORIZONTAL PIECES LIST
            if (horizontalPieces.Count >= 3)
            {
                // for every node in the horizontal match set in horizontal
                // EXPERIMENTAL, ITERATING VIA FOREACH LOOP
                // IF IT DOESN'T WORK, USE STANDARD FOR LOOP 
                foreach (BasePiece horizontalMatch in horizontalPieces)
                {
                    for (int verticalDir = 0; verticalDir <= 1; verticalDir++)
                    {
                        for (int yOffset = 1; yOffset < gridY; yOffset++)
                        {
                            int y;

                            if (verticalDir == 0)
                            {
                                y = newY - yOffset; // check downward;
                            } else
                            {
                                y = newY + yOffset;
                            }

                            if (y < 0 || y >= gridY)
                            {
                                break;
                            }

                            if (gamePieces[horizontalMatch.XPos, y].IsDiamond() && gamePieces[horizontalMatch.XPos, y].PieceSprite.Type == diamond)
                            {
                                verticalPieces.Add(gamePieces[horizontalMatch.XPos,y]);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (verticalPieces.Count < 2)
                    {
                        verticalPieces.Clear();
                    } else
                    {
                        Debug.Log("T or L found (HORIZONTAL)");

                        //EXPERIMENTAL: FOREACH LOOP ITERATION
                        foreach (BasePiece verticalPiece in verticalPieces)
                        {
                            matchingPieces.Add(verticalPiece);
                        }
                        break;
                    }
                }
            }

            if (matchingPieces.Count >= 3)
            {
                return matchingPieces;
            }

            // --------- VERTICAL CHECK ----------

            // for every adjacent piece that's the same color, will be added to the appropriate list;
            // list will check for adjacent pieces checking up and down
            horizontalPieces.Clear();
            verticalPieces.Clear();

            verticalPieces.Add(pieceToCheck); // we know this piece will be in any potential match

            // using the for loop to check which direction we're going
            for (int verticalDirection = 0; verticalDirection <= 1; verticalDirection++)
            {
                // checking how far away the adjacent piece is from central piece
                for (int yOffset = 1; yOffset < gridY; yOffset++)
                {
                    int y;

                    if (verticalDirection == 0) // check going down
                    {
                        y = newY - yOffset;
                    }
                    else // check going up
                    {
                        y = newY + yOffset;
                    }

                    if (y < 0 || y >= gridY)
                    {
                        break; // break out of the horizontal check and continue
                    }

                    if (gamePieces[newX, y].IsDiamond() && gamePieces[newX, y].PieceSprite.Type == diamond)
                    {
                        // add to the list of horizontal pieces if we've found a match
                        verticalPieces.Add(gamePieces[newX, y]);

                    }
                    else
                    {
                        break;
                    }

                }
            }

            // should we reach the minimum amount of checks for a match
            if (verticalPieces.Count >= 3)
            {
                for (int i = 0; i < verticalPieces.Count; i++)
                {
                    matchingPieces.Add(verticalPieces[i]);
                }
            }
            // L and T shape check (Vertical)
            if (verticalPieces.Count >= 3)
            {
                // for every node in the horizontal match set in horizontal
                // EXPERIMENTAL, ITERATING VIA FOREACH LOOP
                // IF IT DOESN'T WORK, USE STANDARD FOR LOOP 
                foreach (BasePiece verticalMatch in verticalPieces)
                {
                    for (int horizontalDir = 0; horizontalDir <= 1; horizontalDir++)
                    {
                        for (int xOffset = 1; xOffset < gridX; xOffset++)
                        {
                            int x;

                            if (horizontalDir == 0)
                            {
                                x = newX - xOffset; // check left;
                            }
                            else
                            {
                                x = newX + xOffset; // check right;
                            }

                            if (x < 0 || x >= gridX)
                            {
                                break;
                            }

                            if (gamePieces[x, verticalMatch.YPos].IsDiamond() && gamePieces[x, verticalMatch.YPos].PieceSprite.Type == diamond)
                            {
                                horizontalPieces.Add(gamePieces[x, verticalMatch.YPos]);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (horizontalPieces.Count < 2)
                    {
                        horizontalPieces.Clear();
                    }
                    else
                    {
                        Debug.Log("T or L found (VERTICAL)");

                        //EXPERIMENTAL: FOREACH LOOP ITERATION
                        foreach (BasePiece horizontalPiece in horizontalPieces)
                        {
                            matchingPieces.Add(horizontalPiece);
                        }
                        break;
                    }
                }
            }
            if (matchingPieces.Count >= 3)
            {
                return matchingPieces;
            }
        }
        return null;
    }

    public bool ClearPotentialMatches()
    {
        bool filling = false;
        for (int y = gridY-1; y >= 0; y--) {
            for (int x = 0; x < gridX; x++)
            {
                List<BasePiece> match = CheckMatch(gamePieces[x, y], x, y);

                if (match != null)
                {
                    GameManager.Instance.PlaySFX();
                    PieceTypeEnum specialPieceType = PieceTypeEnum.COUNT; // the type of piece that we should spawn
                    BasePiece randomPiece = match[Random.Range(0, match.Count)];
                    int specialPieceLocationX = randomPiece.XPos;
                    int specialPieceLocationY = randomPiece.YPos;
                    
                    // spawn a special piece if the match count  = 4
                    if (match.Count == 4)
                    {
                        if (selectedPiece == null || lastEnteredPiece == null)
                        {
                            specialPieceType = (PieceTypeEnum)Random.Range((int)PieceTypeEnum.ROW_CLEAR, (int)PieceTypeEnum.COLUMN_CLEAR);
                        } else if (selectedPiece.YPos == lastEnteredPiece.YPos)
                        {
                            specialPieceType = PieceTypeEnum.ROW_CLEAR;
                        } else 
                        {
                            specialPieceType = PieceTypeEnum.COLUMN_CLEAR;
                        }
                    }

                    // spawn a bomb special piece if the match count >= 5
                    if (match.Count >= 5)
                    {
                       specialPieceType = PieceTypeEnum.BOMB_CLEAR;                       
                    }

                    for (int i = 0; i < match.Count; i++)
                    {
                        if (ClearPiece(match[i].XPos, match[i].YPos))
                        {
                            filling = true;

                            if (match[i] == selectedPiece || match[i] == lastEnteredPiece)
                            {
                                specialPieceLocationX = match[i].XPos;
                                specialPieceLocationY = match[i].YPos;
                            }
                        }
                    }
                    
                    if (specialPieceType != PieceTypeEnum.COUNT)
                    {
                        Destroy(gamePieces[specialPieceLocationX, specialPieceLocationY].gameObject);
                        BasePiece newPiece = SpawnPiece(specialPieceLocationX, specialPieceLocationY, specialPieceType);

                        if ((specialPieceType == PieceTypeEnum.ROW_CLEAR 
                            || specialPieceType == PieceTypeEnum.COLUMN_CLEAR
                            || specialPieceType == PieceTypeEnum.BOMB_CLEAR)
                            && newPiece.IsDiamond() && match[0].IsDiamond())
                        {
                            newPiece.PieceSprite.SetType(match[0].PieceSprite.Type);
                        }


                    }

                }
            }        
        }
        return filling;
    }

    public bool ClearPiece(int targetX, int targetY)
    {
        if (gamePieces[targetX, targetY].isClearable() && !gamePieces[targetX, targetY].ClearPieceComponent.IsClearing)
        {
            gamePieces[targetX, targetY].ClearPieceComponent.ClearPiece();
            SpawnPiece(targetX, targetY, PieceTypeEnum.EMPTY);

            return true;
        }
        return false;
    }

    //---------- LINE CLEAR FUNCTIONS ------------
    // if the piece we clear is of a line clear type, we update the board as such
    public void ClearRow(int row)
    {
        for (int x = 0; x < gridX; x++)
        {
            ClearPiece(x, row);
        }
    }
    public void ClearColumn(int column)
    {
        for (int y = 0; y < gridY; y++)
        {
            ClearPiece(column, y);
        }
    }

    public void ClearBomb(int bombX, int bombY)
    {
        for (int i = -2; i <= 2; i++)
        {
            for (int j = -2; j <= 2; j++)
            {
            if ((bombX + i >= 0 && bombX + i < gridX)
                && (bombY + j >= 0 && bombY + j < gridY))
                {
                    ClearPiece(bombX + i, bombY + j);
                }
            }
        }
    }
}
