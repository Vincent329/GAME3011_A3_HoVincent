using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelectButton : MonoBehaviour
{
    [SerializeField] DifficultyEnum difficulttyEnum;
    Button buttonComponent;
    // Start is called before the first frame update
    void Start()
    {
        buttonComponent = GetComponent<Button>();
        buttonComponent.onClick.AddListener(ButtonSelect);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ButtonSelect()
    {
        Debug.Log("Switching to Match 3");
        // CALL THESE TWO FUNCTIONS
        GameManager.Instance.DifficultyChange(difficulttyEnum);

    }
}
