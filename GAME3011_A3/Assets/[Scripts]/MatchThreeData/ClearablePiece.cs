using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearablePiece : MonoBehaviour
{
    public AnimationClip clearAnim;

    private bool isClearing = false;
    public bool IsClearing
    {
        get => isClearing;
    }

    protected BasePiece piece;

    // Start is called before the first frame update
    void Awake()
    {
        piece = GetComponent<BasePiece>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void ClearPiece()
    {
        isClearing = true;
        StartCoroutine(ClearCoroutine());
    }

    IEnumerator ClearCoroutine()
    {
        Animator animController = GetComponent<Animator>();

        if (animController)
        {
            animController.Play(clearAnim.name);
            yield return new WaitForSeconds(clearAnim.length);
            Destroy(gameObject);
        }
    }
}
