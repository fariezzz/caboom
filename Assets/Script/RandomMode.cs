using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomMode : MonoBehaviour
{
    public List<Button> gameModeButtons;

    public void ChooseRandomGameMode()
    {
        if (gameModeButtons.Count > 0)
        {
            int randomIndex = Random.Range(0, gameModeButtons.Count); 
            Button chosenButton = gameModeButtons[randomIndex]; 
            chosenButton.onClick.Invoke();
        }
    }
}
