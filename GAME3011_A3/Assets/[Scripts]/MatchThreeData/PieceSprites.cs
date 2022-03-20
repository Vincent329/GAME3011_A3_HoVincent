using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceSprites : MonoBehaviour
{
    public enum DiamondType
    {
        DIAMOND,
        RUBY,
        EMERALD,
        AMETHYST,
        GEM
    }

    [System.Serializable]
    public struct DiamondSprite
    {
        public DiamondType type;
        public Sprite diamondSprite;
    }

    public List<DiamondSprite> diamondSprites;

    private DiamondType type;
    public DiamondType Type
    {
        get => type;
        set
        {
            SetType(value);
        }
    }

    // returns number of possible types
    public int NumTypes
    {
        get => diamondSprites.Count;
    }

    [SerializeField]
    private Image sprite;
    private Dictionary<DiamondType, Sprite> diamondSpriteDictionary;

    private void Awake()
    {
        sprite = transform.GetChild(0).GetComponent<Image>();

        diamondSpriteDictionary = new Dictionary<DiamondType, Sprite>();
        for (int i = 0; i < diamondSprites.Count; i++)
        {
            if (!diamondSpriteDictionary.ContainsKey(diamondSprites[i].type))
            {
                diamondSpriteDictionary.Add(diamondSprites[i].type, diamondSprites[i].diamondSprite);
            }
        }
    }

    public void SetType(DiamondType diamondType)
    {
        type = diamondType;
        if (diamondSpriteDictionary.ContainsKey(diamondType))
        {
            sprite.sprite = diamondSpriteDictionary[diamondType];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
