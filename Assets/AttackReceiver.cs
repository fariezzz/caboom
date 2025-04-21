using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackReceiver : MonoBehaviour
{
    public AIController ai;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (SceneManager.GetActiveScene().name == "HideAndSeek")
        {
            if (other.CompareTag("Hitbox"))
            {
                ai.agent.enabled = false;
                CamouflagePlayer camouflagePlayer = other.transform.parent.transform.parent.GetComponent<CamouflagePlayer>();
                ai.anim.SetTrigger("Dead");
                StartCoroutine(ai.DelayBetweenDeath());
                if (camouflagePlayer != null && camouflagePlayer.isHunter)
                {
                    camouflagePlayer.npcKills++;

                    if (camouflagePlayer.npcKills >= 5)
                    {
                        CamouflageManager.Instance.GameOver(false);
                    }
                }
            }
        }
    }
}
