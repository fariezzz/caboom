using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BombManager : MonoBehaviour
{
    [Header("Sending Bomb Mechanic")]
    public List<SendingBomb> sendingBombs = new List<SendingBomb>();
    public GameObject bombSign;
    public int playerCount;

    [Header("Countdown")]
    public float countdownTime;
    private float currentTime;
    private bool isCountingDown = false;
    public Material bombSignMaterial;
    private bool isBlinking;
    public AudioSource countdown;
    public AudioSource bombExplosion;
    public bool gonnaExplode = false;

    [Header("Game Over")]
    public GameObject gameOverGO;
    public Text winnerText;
    public ParticleSystem particleSystemz;
    public Camera mainCamera;

    public static BombManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartTheGame());
    }

    private IEnumerator StartTheGame()
    {
        yield return new WaitForSeconds(1f);

        SendingBomb[] sendingBomb = FindObjectsOfType<SendingBomb>();
        sendingBombs.AddRange(sendingBomb);

        int randomIndex = Random.Range(0, sendingBombs.Count);
        sendingBombs[randomIndex].holdingBomb = true;
        PlayerMovement playerMovement = sendingBombs[randomIndex].GetComponent<PlayerMovement>();
        playerMovement.normalSpeed += 5f;
        playerMovement.runningSpeed += 5f;
        Instantiate(bombSign, sendingBombs[randomIndex].transform);
        bombSign.transform.localPosition = new Vector3(0f, 11f, 1f);

        playerCount = sendingBombs.Count;
        bombSignMaterial.color = Color.black;

        isBlinking = false;
        currentTime = countdownTime;
        isCountingDown = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCountingDown)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                isCountingDown = false;

                SendingBomb lastPlayer = sendingBombs.Find(player => !player.holdingBomb);

                StartCoroutine(DestroyPlayerWithBomb(lastPlayer));
            }
            else if (currentTime <= 5 && !isBlinking)
            {
                StartCoroutine(BlinkBombColor());
                isBlinking = true;
                countdown.Play();
            }
            else if (currentTime <= 1 && !gonnaExplode)
            {
                gonnaExplode = true;

                // Menghentikan suara countdown saat currentTime tersisa 1 detik
                countdown.Stop();

                SendingBomb lastPlayer = sendingBombs.Find(player => !player.holdingBomb);

                // Memainkan suara ledakan saat currentTime tersisa 1 detik
                bombExplosion.Play();
            }
        }
    }

    private IEnumerator ShakeCamera(float duration, float magnitude)
    {
        Vector3 originalPosition = mainCamera.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            mainCamera.transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = originalPosition;
    }

    public IEnumerator DelayExplode(ParticleSystem ps)
    {
        ps.Play();
        StartCoroutine(ShakeCamera(0.2f, 0.2f));
        yield return new WaitForSeconds(1f);
        ps.Stop();
    }

    private IEnumerator BlinkBombColor()
    {
        while (currentTime <= 5)
        { 
            bombSignMaterial.color = Color.red; 
            yield return new WaitForSeconds(0.5f);

            bombSignMaterial.color = Color.black;
            yield return new WaitForSeconds(0.5f);
        }
    }

    void ShowGameOver(SendingBomb winningPlayer)
    {
        sendingBombs.Clear();
        CharacterController[] lastSurvivors = FindObjectsOfType<CharacterController>();
        foreach (CharacterController survivor in lastSurvivors)
        {
            survivor.enabled = false;
        }
        if (winningPlayer != null)
        {
            winnerText.text = winningPlayer.playerMovement.playerName + " is the Winner!";
        }
        else
        {
            winnerText.text = "Draw!";
        }

        gameOverGO.SetActive(true);
    }

    private IEnumerator DestroyPlayerWithBomb(SendingBomb winningPlayer)
    {
        SendingBomb playerWithBomb = sendingBombs.Find(player => player.holdingBomb);
        if (playerWithBomb != null)
        {
            if (sendingBombs.Contains(playerWithBomb))
            {
                sendingBombs.Remove(playerWithBomb);
            }
            playerWithBomb.isAlive = false;
            playerCount--;
            ResetBombOwner();
            Destroy(playerWithBomb.gameObject);
            if(particleSystemz != null)
            {
                Vector3 location = new Vector3(playerWithBomb.transform.position.x, playerWithBomb.transform.position.y + 5f, playerWithBomb.transform.position.z);
                ParticleSystem newParticleSystem = Instantiate(particleSystemz, location, Quaternion.identity);
                StartCoroutine(DelayExplode(newParticleSystem));               
            }
        }

        yield return new WaitForSeconds(0.5f);

        if(playerCount == 1)
        {
            ShowGameOver(winningPlayer);
        }
    }

    public void ResetBombOwner()
    {
        List<SendingBomb> alivePlayers = sendingBombs.FindAll(player => player.isAlive);

        if (alivePlayers.Count > 1)
        {
            StopCoroutine(BlinkBombColor());
            isBlinking = false;
            int randomIndex = Random.Range(0, alivePlayers.Count);
            SendingBomb newBombOwner = alivePlayers[randomIndex];
            newBombOwner.holdingBomb = true;
            Instantiate(bombSign, alivePlayers[randomIndex].transform);
            bombSign.transform.localPosition = new Vector3(0f, 11f, 1f);
            currentTime = countdownTime;
            isCountingDown = true;
        }
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
