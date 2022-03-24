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

    // USE THIS CLEAR PIECE TO ADD TO THE GAME MANAGER
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
            addScoreToGameManager();
            yield return new WaitForSeconds(clearAnim.length);
            Destroy(gameObject);
        }
    }

    private void addScoreToGameManager()
    {
        if (piece.PieceSprite.Type == PieceSprites.DiamondType.DIAMOND)
        {
            GameManager.Instance.DiamondAmount++;
        } else if (piece.PieceSprite.Type == PieceSprites.DiamondType.RUBY)
        {
            GameManager.Instance.RubyAmount++;

        }
        if (piece.PieceSprite.Type == PieceSprites.DiamondType.EMERALD)
        {
            GameManager.Instance.EmeraldAmount++;

        }
        else if (piece.PieceSprite.Type == PieceSprites.DiamondType.AMETHYST)
        {
            GameManager.Instance.AmethystAmount++;

        }
        else if (piece.PieceSprite.Type == PieceSprites.DiamondType.GEM)
        {
            GameManager.Instance.GemAmount++;
        }
        GameManager.Instance.UpdateText();
    }
}
