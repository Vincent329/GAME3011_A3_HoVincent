using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGame : MonoBehaviour
{
    bool playerIsIn;
    [SerializeField]
    GameObject playerInstructionsText;
    private void Start()
    {
        playerIsIn = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() != null)
        {
            playerIsIn = true;
            playerInstructionsText.SetActive(false);
            GameManager.Instance.ToggleDifficultyPanel(true);
        }

    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (playerIsIn)
    //    {
    //        if (GameManager.Instance.inGame)
    //        {
    //            GameManager.Instance.ToggleDifficultyPanel(false);
    //        }
    //        else
    //        {
    //            GameManager.Instance.ToggleDifficultyPanel(true);

    //        }
    //    }
    //}
    private void OnTriggerExit(Collider other)
    {
        playerIsIn = false;
        playerInstructionsText.SetActive(true);

        GameManager.Instance.ToggleDifficultyPanel(false);
    }

    private void TurnOffPanel()
    {
        GameManager.Instance.ToggleDifficultyPanel(false);
    }
}
