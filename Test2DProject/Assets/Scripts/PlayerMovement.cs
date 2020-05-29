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
        private Rigidbody2D rb;
        private PlayerUI playerUI;

        #region IPunObservable implementation


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }


        #endregion

        void Start()
        {
            timeBTWAttack = startTimeBTWAtck;
            rb = GetComponent<Rigidbody2D>();
            PlayerDamage = playerAttackDamage;
            playerUI = GetComponent<PlayerUI>();
        }

        void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            if ((Input.GetKeyDown(KeyCode.Space)) && isGrounded)
            {
                Jump();
            }
            if (timeBTWAttack <= 0)
            {
                if (Input.GetKeyDown(KeyCode.LeftControl)&&isGrounded)
                {
                    Attack();
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
                    SuperAttack();
                    playerUI.superAttackTimer = 15;
                }
            }
            FlipPlayer();
        }

        private void FixedUpdate()
        {
            float horizontalMove = Input.GetAxis("Horizontal") * playerSpeed;
            rb.velocity = new Vector2(horizontalMove, rb.velocity.y);
            playerAnimator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        }

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
        void Attack()
        {
            PlayerDamage = playerAttackDamage;
            if (isGrounded)
            {
                if ((playerUI.Mana < minusManaAfterAttack) && (playerUI.HP > minusManaAfterAttack))
                {
                    playerUI.HP = playerUI.HP - (minusManaAfterAttack - playerUI.Mana);
                    playerUI.Mana = 0;
                    playerAnimator.SetTrigger("Attack");
                }
                else if (playerUI.HP > minusManaAfterAttack)
                {
                    playerUI.Mana -= minusManaAfterAttack;
                    playerAnimator.SetTrigger("Attack");
                }
                timeBTWAttack = startTimeBTWAtck;
            }
        }
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
                    playerAnimator.SetTrigger("SuperAttack");
                }
                else if (playerUI.HP > minusManaAfterSuperAttack)
                {
                    playerUI.Mana -= minusManaAfterSuperAttack;
                    playerAnimator.SetTrigger("SuperAttack");
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
                    enemiesToDamage[i].GetComponent<Enemy>().EnemyTakeDamage(PlayerDamage, playerUI);
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
                playerAnimator.SetTrigger("Damage");
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
                playerAnimator.SetTrigger("Damage");
            }
        }
    }
}
