using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AIController : MonoBehaviour
{
    public NavMeshAgent agent;

    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    private float timer;
    public Animator anim;

    void Start()
    {
        timer = wanderTimer;
        wanderTimer = Random.Range(1, 2);
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
            wanderTimer = Random.Range(3, 5);
        }

        bool isWalking = agent.velocity.magnitude < 0.1f;

        anim.SetBool("Idle", isWalking);

    }

    Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;
        randomDirection += origin;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, dist, layermask);

        return hit.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(SceneManager.GetActiveScene().name == "HideAndSeek")
        {
            if (other.CompareTag("Hitbox"))
            {
                CamouflagePlayer camouflagePlayer = other.transform.parent.transform.parent.GetComponent<CamouflagePlayer>();
                anim.SetTrigger("Dead");
                StartCoroutine(DelayBetweenDeath());
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

    public IEnumerator DelayBetweenDeath()
    {
        yield return new WaitForSeconds(1.12f);
        Destroy(gameObject);
    }
}
