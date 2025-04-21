using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CamouflageManager : MonoBehaviour
{
    [Header("Core")]
    public List<CamouflagePlayer> players = new List<CamouflagePlayer>();
    public int targetCount;
    public float gameTime = 20f;
    private float currentTime;
    public Text timeText;

    [Header("Game Over")]
    public GameObject popUpGameOver;
    public Text winnerText;
    public bool gameIsOver = false;
    public bool endedByTime = false;

    public static CamouflageManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(StartTheGame());
    }
    IEnumerator StartTheGame()
    {
        currentTime = gameTime;

        yield return new WaitForSeconds(0.5f);

        CamouflagePlayer[] foundPlayers = FindObjectsOfType<CamouflagePlayer>();
        players.AddRange(foundPlayers);

        int hunterIndex = Random.Range(0, players.Count);
        for (int i = 0; i < players.Count; i++)
        {
            if (i == hunterIndex)
            {
                players[i].isHunter = true;
                players[i].playerMovement.normalSpeed += 5f;
                players[i].playerMovement.runningSpeed += 5f;
            }
            else
            {
                players[i].isHunter = false;
                targetCount++;
            }
            foundPlayers[i].Init();
        }
    }


        // Update is called once per frame
    void Update()
    {
        if (!gameIsOver)
        {
            currentTime -= Time.deltaTime;

            // Konversi detik menjadi menit dan detik
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);

            // Format waktu dalam format "m:ss"
            string formattedTime = string.Format("{0}:{1:00}", minutes, seconds);

            // Tampilkan waktu di teks atau tempat yang sesuai
            timeText.text = formattedTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                timeText.gameObject.SetActive(false);
                gameIsOver = true;
                endedByTime = true;
                GameOver(false);
            }
        }
        else
        {
            CharacterController[] lastSurvivors = FindObjectsOfType<CharacterController>();

            foreach (CharacterController survivor in lastSurvivors)
            {
                survivor.enabled = false;
            }

            PlayerMovement[] winners = FindObjectsOfType<PlayerMovement>();

            foreach (PlayerMovement winner in winners)
            {
                winner.anim.SetTrigger("Celebration");
            }
        }
    }

    static string FormatTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        // Menggunakan string.Format untuk memformat waktu dalam format menit:detik
        return string.Format("{0}:{1:00}", minutes, seconds);
    }

    public void HunterKillsTarget()
    {
        targetCount--;

        if (targetCount <= 0)
        {
            GameOver(true);
        }
        Debug.Log("test");
    }

    public void GameOver(bool hunterWin)
    {
        popUpGameOver.SetActive(true);

        if (hunterWin)
        {
            winnerText.text = "Hunter wins the game!";
        }
        else
        {
            winnerText.text = "Preys win the game!";
        }

        gameIsOver = true;
        Debug.Log("test");
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
