using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get => instance;
    }

    [SerializeField] private GameObject gamePanelCanvas;

    // ACTIVATES ON GAME STATE BEING TOGGLED
    public bool inGame;

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
    }

    public void TogglePanel()
    {
        gamePanelCanvas.SetActive(gamePanelCanvas.activeInHierarchy ? false : true) ;
    }
}


