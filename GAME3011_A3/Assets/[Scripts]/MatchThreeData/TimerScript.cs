using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textDisplay;
    public float timer;
    private bool stopTimer;
    [SerializeField]
    private bool init = false;

    // Start is called before the first frame update
    void Awake()
    {
        textDisplay = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        stopTimer = false;
        timer = GameManager.Instance.initialStartTimer;

        init = true;
        GameManager.Instance.Win += StopTimer;
    }
    private void OnEnable()
    {
        if (init)
        {
            stopTimer = false;
            timer = GameManager.Instance.initialStartTimer;
            GameManager.Instance.Win += StopTimer;

        }
    }

    private void OnDisable()
    {
        GameManager.Instance.Win -= StopTimer;

    }
    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }


    private void UpdateTimer()
    {
        timer -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer - minutes * 60f);
        string textTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (timer <= 0)
        {
            stopTimer = true;
            timer = 0;
            GameManager.Instance.InvokeLoseGame();
        }
        if (!stopTimer)
        {
            textDisplay.SetText(textTime);
        }
    }
    private void StopTimer()
    {
        stopTimer = true;
    }
}
