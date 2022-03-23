using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager Instance
    {
        get => instance;
    }

    [SerializeField] private GameObject gamePanelCanvas;

    // ----------------- Scoring Types -------------------
    [SerializeField] private int diamondAmount;
    public int DiamondAmount
    {
        get => diamondAmount;
        set
        {
            diamondAmount = value;
        }
    }
    [SerializeField] private int rubyAmount;

    public int RubyAmount
    {
        get => rubyAmount;
        set
        {
            rubyAmount = value;
        }
    }

    [SerializeField] private int emeraldAmount;
    public int EmeraldAmount
    {
        get => emeraldAmount;
        set
        {
            emeraldAmount = value;
        }
    }
    [SerializeField] private int amethystAmount;

    public int AmethystAmount
    {
        get => amethystAmount;
        set
        {
            amethystAmount = value;
        }
    }
    [SerializeField] private int gemAmount;
    public int GemAmount
    {
        get => gemAmount;
        set
        {
            gemAmount = value;
        }
    }

    // ACTIVATES ON GAME STATE BEING TOGGLED
    public bool inGame;

    // EVENTS TO COMMUNICATE TO THE BOARD

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        //gamePanelCanvas.SetActive(false);
        Reset();
    }

    public void TogglePanel()
    {
        gamePanelCanvas.SetActive(gamePanelCanvas.activeInHierarchy ? false : true) ;
    }

    public void Reset()
    {
        diamondAmount = 0;
        rubyAmount = 0;
        emeraldAmount = 0;
        amethystAmount = 0;
        gemAmount = 0;
    }
}


