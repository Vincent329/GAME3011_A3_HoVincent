using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get => instance;
    }

    // Match 3 panel
    [SerializeField] private GameObject gamePanelCanvas;
    [SerializeField] private GameObject difficultySelectPanel;

    // ----------------- Scoring Types -------------------
    [SerializeField] private int diamondAmount;
    public int DiamondAmount
    {
        get => diamondAmount;
        set{ diamondAmount = value;  }
    }
    [SerializeField] private int diamondLimit;

    [SerializeField] private int rubyAmount;
    public int RubyAmount
    {
        get => rubyAmount;
        set { rubyAmount = value; }
    }
    [SerializeField] private int rubyLimit;

    [SerializeField] private int emeraldAmount;
    public int EmeraldAmount
    {
        get => emeraldAmount;
        set { emeraldAmount = value; }
    }
    [SerializeField] private int emeraldLimit;

    [SerializeField] private int amethystAmount;
    public int AmethystAmount
    {
        get => amethystAmount;
        set{ amethystAmount = value;  }
    }
    [SerializeField] private int amethystLimit;

    [SerializeField] private int gemAmount;
    public int GemAmount
    {
        get => gemAmount;
        set { gemAmount = value; }
    }
    [SerializeField] private int gemLimit;

    // ---------- Text Displays -------------------
    [SerializeField] private TextMeshProUGUI diamondScoreText;
    [SerializeField] private TextMeshProUGUI rubyScoreText;
    [SerializeField] private TextMeshProUGUI emeraldScoreText;
    [SerializeField] private TextMeshProUGUI amethystScoreText;
    [SerializeField] private TextMeshProUGUI gemScoreText;
    [SerializeField] private GameObject amethystDisplay;
    [SerializeField] private GameObject gemDisplay;
    

    public float initialStartTimer;
    [SerializeField] private TextMeshProUGUI timerText;

    // ACTIVATES ON GAME STATE BEING TOGGLED
    public bool inGame;

    [SerializeField]
    private DifficultyEnum gameDifficulty;
    public DifficultyEnum GameDifficulty => gameDifficulty;

    // --------------EVENTS TO COMMUNICATE TO THE BOARD------------------
    public delegate void DifficultySet();
    public event DifficultySet StartAtDifficulty;

    public delegate void WinGame();
    public event WinGame Win;

    public delegate void LoseGame();
    public event LoseGame Lose;

    public delegate void ResetGame();
    public event ResetGame Reset;

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
        inGame = false;
        gamePanelCanvas.SetActive(false);
        difficultySelectPanel.SetActive(false);
        ResetScore();
    }

    public void TogglePanel()
    {
        gamePanelCanvas.SetActive(gamePanelCanvas.activeInHierarchy ? false : true) ;
    }

    public void ResetScore()
    {
        diamondAmount = 0;
        rubyAmount = 0;
        emeraldAmount = 0;
        amethystAmount = 0;
        gemAmount = 0;
        diamondScoreText.text = diamondAmount + "/" + diamondLimit; 
        rubyScoreText.text = rubyAmount + "/" + rubyLimit; 
        emeraldScoreText.text = emeraldAmount + "/" + emeraldLimit; 
        amethystScoreText.text = amethystAmount + "/" + amethystLimit; 
        gemScoreText.text = gemAmount + "/" + gemLimit; 
    }

    public void UpdateText()
    {
        diamondScoreText.text = diamondAmount + "/" + diamondLimit;
        rubyScoreText.text = rubyAmount + "/" + rubyLimit;
        emeraldScoreText.text = emeraldAmount + "/" + emeraldLimit;
        amethystScoreText.text = amethystAmount + "/" + amethystLimit;
        gemScoreText.text = gemAmount + "/" + gemLimit;
    }

    public void ToggleDifficultyPanel(bool isEntered)
    {
        inGame = isEntered;
        difficultySelectPanel.SetActive(inGame);
    }

    public void DifficultyChange(DifficultyEnum targetDifficulty)
    {
        inGame = true;
        gameDifficulty = targetDifficulty;
        difficultySelectPanel.SetActive(false);
        StartAtDifficulty();
    }
}


