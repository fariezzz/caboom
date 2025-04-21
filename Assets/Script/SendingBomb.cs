using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SendingBomb : MonoBehaviour
{
    [Header("Core")]
    public PlayerMovement playerMovement;
    public bool holdingBomb;
    public float bombExchangeCooldown = 5.0f;
    public float timeSinceLastExchange;
    private bool isCooldown = false;
    public bool isAlive = true;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (isCooldown)
        {
            timeSinceLastExchange += Time.deltaTime;

            if (timeSinceLastExchange >= bombExchangeCooldown)
            {
                isCooldown = false;
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            SendingBomb otherScript = hit.gameObject.GetComponent<SendingBomb>();

            if(holdingBomb && !otherScript.holdingBomb && !isCooldown)
            {
                foreach (Transform child in transform)
                {
                    if (child.CompareTag("Bomb Sign"))
                    {
                        Destroy(child.gameObject);
                    }
                }

                timeSinceLastExchange = 0;
                otherScript.timeSinceLastExchange = 0;
                isCooldown = true;
                otherScript.isCooldown = true;
                holdingBomb = false;
                otherScript.holdingBomb = true;
                PlayerMovement otherPlayerMovement = hit.gameObject.GetComponent<PlayerMovement>();
                playerMovement.normalSpeed -= 5f;
                playerMovement.runningSpeed -= 5f;
                otherPlayerMovement.normalSpeed += 5f;
                otherPlayerMovement.runningSpeed += 5f;

                GameObject bombSignPrefab = BombManager.Instance.bombSign;
                GameObject bombSign = Instantiate(bombSignPrefab, hit.transform);
                bombSign.transform.localPosition = new Vector3(0f, 11f, 1f);
            }
            else if (!holdingBomb && otherScript.holdingBomb && !isCooldown)
            {
                foreach (Transform child in hit.transform)
                {
                    if (child.CompareTag("Bomb Sign"))
                    {
                        Destroy(child.gameObject);
                    }
                }

                timeSinceLastExchange = 0;
                otherScript.timeSinceLastExchange = 0;
                isCooldown = true;
                otherScript.isCooldown = true;
                holdingBomb = true;
                otherScript.holdingBomb = false;
                PlayerMovement otherPlayerMovement = hit.gameObject.GetComponent<PlayerMovement>();
                playerMovement.normalSpeed += 5f;
                playerMovement.runningSpeed += 5f;
                otherPlayerMovement.normalSpeed -= 5f;
                otherPlayerMovement.runningSpeed -= 5f;

                GameObject bombSignPrefab = BombManager.Instance.bombSign;
                GameObject bombSign = Instantiate(bombSignPrefab, transform);
                bombSign.transform.localPosition = new Vector3(0f, 11f, 1f);
            }
        }
    }
}
