using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;
using Photon.Realtime;

namespace Max_Almog.MyCompany.MyGame
{
    public class PlayerMovement : MonoBehaviourPunCallbacks, IPunObservable
    {
        public LayerMask Enemy;
        public Transform attackPos;
        public Animator playerAnimator;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        public float playerSpeed;
        public float playerJump;
        public float attackRange;
        public float startTimeBTWAtck;
        public int playerAttackDamage;
        int PlayerDamage;
        public int minusManaAfterAttack;
        public int minusManaAfterSuperAttack;
        private float timeBTWAttack;
        private bool isGrounded;
        private bool isAttacking = false;
        private bool isSuperAttacking = false;
        private Rigidbody2D rb;
        private PlayerUI playerUI;

        #region IPunObservable implementation


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(playerUI.HP);
                stream.SendNext(isAttacking);
                stream.SendNext(isSuperAttacking);
            }
            else
            {
                // Network player, receive data
                playerUI.HP = (float)stream.ReceiveNext();
                isAttacking = (bool)stream.ReceiveNext();
                isSuperAttacking = (bool)stream.ReceiveNext();
            }
        }


        #endregion

        private void Awake()
        {
            
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                LocalPlayerInstance = gameObject;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
            playerUI = GetComponent<PlayerUI>();
        }

        void Start()
        {
            if (photonView.IsMine)
            {
                MultiplayerCam.followCamera.Follow = transform;
            }
            timeBTWAttack = startTimeBTWAtck;
            rb = GetComponent<Rigidbody2D>();
            PlayerDamage = playerAttackDamage;
            
#if UNITY_5_4_OR_NEWER
            // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
        }

#if !UNITY_5_4_OR_NEWER
/// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
void OnLevelWasLoaded(int level)
{
    this.CalledOnLevelWasLoaded(level);
}
#endif


        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }
        }

#if UNITY_5_4_OR_NEWER
        public override void OnDisable()
        {
            // Always call the base to remove callbacks
            base.OnDisable();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }
#endif

        void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if ((Input.GetKeyDown(KeyCode.Space)) && isGrounded)
            {
                photonView.RPC("Jump", RpcTarget.All);
            }

            if (timeBTWAttack <= 0)
            {
                if (Input.GetKeyDown(KeyCode.LeftControl)&&isGrounded)
                {
                    isAttacking = true;
                    photonView.RPC("Attack", RpcTarget.All);
                    isAttacking = false;
                }
            }
            else
            {
                timeBTWAttack -= Time.deltaTime;
            }
            if (playerUI.superAttackTimer <= 0)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift)&&isGrounded)
                {
                    isSuperAttacking = true;
                    photonView.RPC("SuperAttack", RpcTarget.All);
                    playerUI.superAttackTimer = 10;
                    isSuperAttacking = false;
                }
            }
            FlipPlayer();
        }

        private void FixedUpdate()
        {
            if (!photonView.IsMine)
            {
                return;
            }
            Move();
        }

        private void Move()
        {
            float horizontalMove = Input.GetAxis("Horizontal") * playerSpeed;
            rb.velocity = new Vector2(horizontalMove, rb.velocity.y);
            playerAnimator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        }

        [PunRPC]
        void Jump()
        {
            rb.AddForce(transform.up * playerJump, ForceMode2D.Impulse);
        }

        void FlipPlayer()
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
        
        [PunRPC]
        void Attack()
        {
            PlayerDamage = playerAttackDamage;
            if (isGrounded)
            {
                playerAnimator.SetTrigger("Attack");
                timeBTWAttack = startTimeBTWAtck;
            }
        }

        [PunRPC]
        void SuperAttack()
        {
            PlayerDamage = playerAttackDamage;
            PlayerDamage *= 2;
            if (isGrounded)
            {
                if ((playerUI.Mana < minusManaAfterSuperAttack) && (playerUI.HP > minusManaAfterSuperAttack))
                {
                    playerUI.HP = playerUI.HP - (minusManaAfterSuperAttack - playerUI.Mana);
                    playerUI.Mana = 0;
                    playerAnimator.SetBool("Super Attack",true);
                }
                else if (playerUI.HP > minusManaAfterSuperAttack)
                {
                    playerUI.Mana -= minusManaAfterSuperAttack;
                    playerAnimator.SetBool("Super Attack", true);
                }
            }
        }

        void AttackAnimation()
        {
            if (isGrounded)
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, Enemy);
                timeBTWAttack = startTimeBTWAtck;
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(PlayerDamage, playerUI);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPos.position, attackRange);
        }

        private void OnTriggerStay2D(Collider2D player)
        {
            if (player.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
                playerAnimator.SetBool("Jump", false);
            }

            if (player.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                playerAnimator.SetBool("Damage", true);
            }
        }
        private void OnTriggerExit2D(Collider2D player)
        {
            if (player.gameObject.CompareTag("Ground"))
            {
                isGrounded = false;
                playerAnimator.SetBool("Jump", true);
            }

        }
        private void OnCollisionEnter2D(Collision2D player)
        {
            if (player.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                playerAnimator.SetBool("Damage", true);
            }
        }

#if UNITY_5_4_OR_NEWER
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }
#endif
    }
}
