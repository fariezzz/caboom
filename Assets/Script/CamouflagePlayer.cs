using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CamouflagePlayer : MonoBehaviour
{
    public GameObject[] models;
    public int maxNpcKills = 5;
    public int npcKills = 0;
    public bool isHunter = false;
    public PlayerMovement playerMovement;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (SceneManager.GetActiveScene().name == "HideAndSeek")
        {
            if (isHunter)
            {
                models[0].SetActive(true);
                models[1].SetActive(false);
                playerMovement.normalSpeed += 5;
                playerMovement.runningSpeed += 5;
            }
            else
            {
                models[0].SetActive(false);
                models[1].SetActive(true);
            }
        }       
    }
}
