using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Indicator")]
    public GameObject indicator;
    public Material redMaterial;
    public Material greenMaterial;
    public Material blueMaterial;
    public Material whiteMaterial;
    public Renderer meshRenderer;
    public int idPlayer;

    [Header("Movement")]
    public CharacterController characterController;
    public float normalSpeed = 5f;
    public float runningSpeed = 8f;
    public string HorizontalInput;
    public string VerticalInput;
    public string run;
    public Animator anim;
    public Animator camoAnim;
    [SerializeField] private float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public AudioSource footstepSFX;

    [Header("Attacking")]
    private bool isAttacking = false;

    [Header("Other Modes")]
    public SendingBomb sendingBomb;
    public CamouflagePlayer camouflagePlayer;
    public string actionKey;
    public string playerName;

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "HideAndSeek")
        {
            indicator.SetActive(false);
        }

        if(idPlayer == 1)
        {
            playerName = "Player 1";
            meshRenderer.material = redMaterial;
        }
        else if(idPlayer == 2)
        {
            playerName = "Player 2";
            meshRenderer.material = blueMaterial;
        }
        else if(idPlayer == 3)
        {
            playerName = "Player 3";
            meshRenderer.material = greenMaterial;
        }
        else if(idPlayer == 4)
        {
            playerName = "Player 4";
            meshRenderer.material = whiteMaterial;
        }
    }

    public float Speed()
    {
        if (Input.GetButton(run))
        {
            return runningSpeed;
        }

        return normalSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(SceneManager.GetActiveScene().name == "HideAndSeek")
        {
            if(!CamouflageManager.Instance.gameIsOver)
            {
                float horizontalInput = Input.GetAxisRaw(HorizontalInput);
                float verticalInput = Input.GetAxisRaw(VerticalInput);
                Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;       
                if(movement.magnitude >= 0.1f && !isAttacking)
                {
                    float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    anim.SetBool("Idle", false);
                    camoAnim.SetBool("Idle", false);
                    characterController.Move(movement * Speed() * Time.deltaTime);
                    if (!footstepSFX.isPlaying && camouflagePlayer.isHunter)
                    {
                        footstepSFX.Play();
                    }
                }
                else if(movement.magnitude < 0.1f)
                {
                    if (camouflagePlayer.isHunter)
                    {
                        footstepSFX.Stop();
                    }
                    anim.SetBool("Idle", true);
                    camoAnim.SetBool("Idle", true);
                }

                if (Input.GetButton(actionKey) && !isAttacking && SceneManager.GetActiveScene().name != "BombRelay")
                {
                    StartAttack();
                }
            }
        }
        else
        {
            float horizontalInput = Input.GetAxisRaw(HorizontalInput);
            float verticalInput = Input.GetAxisRaw(VerticalInput);
            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;
            if (movement.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                anim.SetBool("Idle", false);
                camoAnim.SetBool("Idle", false);
                characterController.Move(movement * Speed() * Time.deltaTime);
                if (!footstepSFX.isPlaying)
                {
                    footstepSFX.Play();
                }
            }
            else
            {
                footstepSFX.Stop();
                anim.SetBool("Idle", true);
                camoAnim.SetBool("Idle", true);
            }
        }
    }

    void StartAttack()
    {
        if (SceneManager.GetActiveScene().name == "HideAndSeek")
        {
            CamouflagePlayer camouflagePlayer = GetComponent<CamouflagePlayer>();

            if (camouflagePlayer != null && camouflagePlayer.isHunter)
            {
                Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("NPC"), true);
                anim.SetBool("Idle", true);
                isAttacking = true;
                anim.SetBool("Attack", true);
                characterController.enabled = false;
            }
        }
    }

    void EndAttack()
    {
        anim.SetBool("Idle", true);
        Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("NPC"), false);
        StartCoroutine(DelayAttack());
    }

    public IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(.6f);
        isAttacking = false;
        anim.SetBool("Attack", isAttacking);
        characterController.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hitbox") && !other.transform.IsChildOf(gameObject.transform))
        {
            anim.SetTrigger("Dead");
            camoAnim.SetTrigger("Dead");
        }
    }

    public void DestroyPlayer()
    {
        if (SceneManager.GetActiveScene().name == "HideAndSeek")
        {
            CamouflagePlayer camouflagePlayer = GetComponent<CamouflagePlayer>();
            if (camouflagePlayer != null && !camouflagePlayer.isHunter)
            {
                CamouflageManager.Instance.HunterKillsTarget();
                Debug.Log("test");
            }
        }
        Destroy(gameObject);
    }
}
