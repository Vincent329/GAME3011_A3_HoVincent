using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePiece : MonoBehaviour
{

    private BasePiece piece;
    private IEnumerator movingCoroutine; // coroutine within the movable piece
    private void Awake()
    {
        piece = GetComponent<BasePiece>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MovePiece(int targetX, int targetY, float movementTime)
    {

        if (movingCoroutine != null)
        {
            StopCoroutine(movingCoroutine);
        }

        movingCoroutine = MoveCoroutine(targetX, targetY, movementTime);
        StartCoroutine(movingCoroutine);
        
    }

    private IEnumerator MoveCoroutine(int targetX, int targetY, float movementTime)
    {
        piece.XPos = targetX;
        piece.YPos = targetY;

        Vector3 startPos = transform.position;
        Vector3 endPos = piece.GridRef.GetGridPosition(targetX, targetY);

        for (float t = 0; t <= 1 * movementTime; t += Time.deltaTime)
        {
            piece.transform.position = Vector3.Lerp(startPos, endPos, t / movementTime);
            yield return 0; 
        }

        piece.transform.position = endPos;
        

    }



}
