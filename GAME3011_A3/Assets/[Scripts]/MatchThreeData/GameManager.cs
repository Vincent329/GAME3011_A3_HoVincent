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

    // Match 3 and Difficulty Select panel
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
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI loseText;
    
    public float initialStartTimer;

    // ACTIVATES ON GAME STATE BEING TOGGLED
    public bool inGameTrigger;
    public bool didGameFinish;

    [SerializeField]
    private DifficultyEnum gameDifficulty;
    public DifficultyEnum GameDifficulty => gameDifficulty;

    // Audio Source for SFX
    private AudioSource audioSource;

    // --------------EVENTS TO COMMUNICATE TO THE BOARD------------------
    public delegate void DifficultySet();
    public event DifficultySet StartAtDifficulty;

    public delegate void WinGame();
    public event WinGame Win;

    public delegate void LoseGame();
    public event LoseGame Lose;

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
        audioSource = GetComponent<AudioSource>();
        inGameTrigger = false;
        didGameFinish = false;
        gamePanelCanvas.SetActive(false);
        difficultySelectPanel.SetActive(false);
        ResetGameState();
    }

    public void TogglePanel()
    {
        gamePanelCanvas.SetActive(gamePanelCanvas.activeInHierarchy ? false : true) ;
    }
    
    public void InvokeLoseGame()
    {
        DisplayLoseText();
        Lose();
    }

    public void ResetGameState()
    {
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
        didGameFinish = false;
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
        inGameTrigger = isEntered;
        difficultySelectPanel.SetActive(inGameTrigger);
    }

    public void DifficultyChange(DifficultyEnum targetDifficulty)
    {
        inGameTrigger = true;
        gameDifficulty = targetDifficulty;
        difficultySelectPanel.SetActive(false);

        if (gameDifficulty == DifficultyEnum.EASY)
        {
            diamondScoreText.gameObject.SetActive(true);      
            rubyScoreText.gameObject.SetActive(true);
            emeraldScoreText.gameObject.SetActive(true);
            amethystScoreText.gameObject.SetActive(false);
            gemScoreText.gameObject.SetActive(false);
            amethystDisplay.SetActive(false);
            gemDisplay.SetActive(false);
            diamondLimit = 40;
            rubyLimit = 40;
            emeraldLimit = 40;
            initialStartTimer = 40;
        }
        else if (gameDifficulty == DifficultyEnum.NORMAL)
        {
            diamondScoreText.gameObject.SetActive(true);
            rubyScoreText.gameObject.SetActive(true);
            emeraldScoreText.gameObject.SetActive(true);
            amethystScoreText.gameObject.SetActive(true);
            gemScoreText.gameObject.SetActive(false);
            amethystDisplay.SetActive(true);
            gemDisplay.SetActive(false);
            diamondLimit = 50;
            rubyLimit = 50;
            emeraldLimit = 50;
            amethystLimit = 50;
            initialStartTimer = 60;
        } 
        else if (gameDifficulty == DifficultyEnum.HARD)
        {
            diamondScoreText.gameObject.SetActive(true);
            rubyScoreText.gameObject.SetActive(true);
            emeraldScoreText.gameObject.SetActive(true);
            amethystScoreText.gameObject.SetActive(true);
            gemScoreText.gameObject.SetActive(true);
            amethystDisplay.SetActive(true);
            gemDisplay.SetActive(true);
            diamondLimit = 60;
            rubyLimit = 60;
            emeraldLimit = 60;
            amethystLimit = 60;
            gemLimit = 60;

            initialStartTimer = 75;
        }
        ResetGameState();
        StartAtDifficulty();
    }

    public void CheckWinCondition()
    {
        if (gameDifficulty == DifficultyEnum.EASY)
        {
            if (diamondAmount >= diamondLimit && rubyAmount >= rubyLimit && emeraldAmount >= emeraldLimit)
            {
                Debug.Log("Win");
                didGameFinish = true;
                DisplayWinText();
                Win();
            }
        }
        else
        if (gameDifficulty == DifficultyEnum.NORMAL)
        {
            if (diamondAmount >= diamondLimit && rubyAmount >= rubyLimit && emeraldAmount >= emeraldLimit && amethystAmount >= amethystLimit)
            {
                Debug.Log("Win");
                didGameFinish = true;
                DisplayWinText();
                Win();
            }
        }
        else
        if (gameDifficulty == DifficultyEnum.HARD)
        {
            if (diamondAmount >= diamondLimit && rubyAmount >= rubyLimit && emeraldAmount >= emeraldLimit && amethystAmount >= amethystLimit && gemAmount >= gemLimit)
            {
                Debug.Log("Win");
                didGameFinish = true;
                DisplayWinText();
                Win();
            }
        }
    }

    public void PlaySFX()
    {
        audioSource.Play();
    }

    public void DisplayWinText()
    {
        winText.gameObject.SetActive(true);

    }

    public void DisplayLoseText()
    {
        loseText.gameObject.SetActive(true);

    }
}


