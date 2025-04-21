using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    public int playerNumber;
    public GameManager GM;
    public NavMeshSurface navMeshSurface;
    public GameObject playerPrefab;
    public string[] horizontalInputAxes;
    public string[] verticalInputAxes;
    public string[] runInput;
    public string[] actionInput;

    public float randomRange = 10.0f; // Jarak maksimal dari pusat NavMesh
    public bool showGizmos = true; // Apakah menampilkan Gizmos

    private void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        playerNumber = GM.player;

        if (SceneManager.GetActiveScene().name == "BombRelay" || SceneManager.GetActiveScene().name == "HideAndSeek")
        {
            navMeshSurface.BuildNavMesh();
            for (int i = 0; i < playerNumber; i++)
            {
                Vector3 randomPosition = GetRandomNavMeshPosition();
                GameObject player = Instantiate(playerPrefab, randomPosition, Quaternion.identity);

                if (i < horizontalInputAxes.Length)
                {
                    player.GetComponent<PlayerMovement>().HorizontalInput = horizontalInputAxes[i];
                    player.GetComponent<PlayerMovement>().idPlayer = i + 1;
                    player.GetComponent<PlayerMovement>().VerticalInput = verticalInputAxes[i];
                    player.GetComponent<PlayerMovement>().run = runInput[i];
                    player.GetComponent<PlayerMovement>().actionKey = actionInput[i];
                }

                player.name = "Player" + (i + 1);
            }
        }
    }

    private Vector3 GetRandomNavMeshPosition()
    {
        Vector3 randomPosition = Vector3.zero;
        NavMeshHit hit;

        for (int i = 0; i < 30; i++) // Mencoba hingga 30 kali untuk menemukan posisi yang valid
        {
            float randomX = Random.Range(-randomRange, randomRange);
            float randomZ = Random.Range(-randomRange, randomRange);

            randomPosition = new Vector3(transform.position.x + randomX, playerPrefab.transform.position.y, transform.position.z + randomZ);

            if (NavMesh.SamplePosition(randomPosition, out hit, 1.0f, NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();
                if (NavMesh.CalculatePath(randomPosition, hit.position, NavMesh.AllAreas, path))
                {
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        randomPosition = hit.position;
                        break;
                    }
                }
            }
        }

        return randomPosition;
    }


    // Menampilkan area acak dengan Gizmos jika diaktifkan
    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(randomRange * 2, 0.1f, randomRange * 2));
        }
    }
}
