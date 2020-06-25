using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;
using Photon.Realtime;

namespace Max_Almog.MyCompany.MyGame
{
    public class FireSlime : Enemy
    {
        public float SlimeJump;
        private bool isGrounded;
        private float JumpCoolDown;
        public Animator fireSlimeAnimator;

        private bool movingRight;
        public Transform groundDetection;

        void Start()
        {
            photonView.RPC("StartProperties", RpcTarget.AllBuffered);
            JumpCoolDown = Random.Range(2, 5);
        }

        void Update()
        {
            photonView.RPC("CheckGround", RpcTarget.AllBuffered);
            JumpCoolDown -= Time.deltaTime;
            if ((JumpCoolDown <= 0))
            {
                photonView.RPC("Jump", RpcTarget.All);
            }
        }

        [PunRPC]
        private void CheckGround()
        {
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down);

            if (groundInfo.collider == false)
            {
                if (movingRight == true)
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    HPText.rectTransform.localScale = new Vector3(-1f, 1f, 1f);
                    movingRight = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    HPText.rectTransform.localScale = new Vector3(1f, 1f, 1f);
                    movingRight = true;
                }
            }
        }

        [PunRPC]
        void Jump()
        {
            rb.AddForce(transform.up * SlimeJump + -transform.right * SlimeJump, ForceMode2D.Impulse);
            JumpCoolDown = Random.Range(2, 5); ;
        }
    
        private void OnCollisionStay2D(Collision2D Enemy)
        {
            if (Enemy.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
                fireSlimeAnimator.SetBool("Jump", false);
            }
        }
        private void OnCollisionExit2D(Collision2D Enemy)
        {
            if (Enemy.gameObject.CompareTag("Ground"))
            {
                isGrounded = false;
                fireSlimeAnimator.SetBool("Jump", true);
            }
        }
    }
}
