using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [System.NonSerialized]public string objectID;
    public int player = 1;
    public GameObject[] textActivate; 
    public GameObject[] textNonActivate;
    public GameObject[] discard;
    public Button start;
    private static GameManager Instance;
    public static GameManager instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        Instance = this;
        objectID = name + transform.position.ToString() + transform.eulerAngles.ToString();
    }
    private void Start()
    {
        GameManager[] gameManagers = Object.FindObjectsOfType<GameManager>();
        for (int i = 0; i < gameManagers.Length; i++)
        {
            if (gameManagers[i] != this)
            {
                if (gameManagers[i].objectID == objectID)
                {
                    Destroy(gameManagers[i].gameObject);
                }
            }
        }
        DontDestroyOnLoad(gameObject);


    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "NumberPlayers")
        {
            if (player >= 2)
            {
                try
                {
                    start.interactable = true;
                }
                catch
                {
                    Debug.Log("Testing");
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && SceneManager.GetActiveScene().name == "NumberPlayers" && player < 4)
            {
                player++;
                int index = player - 2;

                if (index >= 0 && index < textActivate.Length)
                {
                    textNonActivate[index].SetActive(false);
                    textActivate[index].SetActive(true);
                }


            }
            if (player == 3)
            {
                discard[0].SetActive(true);
            }
            else
                discard[0].SetActive(false);

            if (player == 4)
            {
                discard[2].SetActive(true);
            }
        }

    }

    public void Discard(int Player)
    {
        player--;
        textActivate[Player].SetActive(false);
        textNonActivate[Player].SetActive(true);
        discard[Player].SetActive(false);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
