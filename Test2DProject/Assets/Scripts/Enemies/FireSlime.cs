﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;
using Photon.Realtime;

namespace Max_Almog.MyCompany.MyGame
{
    public class FireSlime : Enemy, IPunObservable
    {
        public float SlimeJump;
        private bool isGrounded;
        private float JumpCoolDown;
        public Animator fireSlimeAnimator;

        private bool movingRight;
        public Transform groundDetection;

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

        void Start()
        {
            StartProperties();
            JumpCoolDown = Random.Range(2,5);
        }

        void Update()
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
            JumpCoolDown -= Time.deltaTime;
            if ((JumpCoolDown<=0))
            {
                Jump();
                JumpCoolDown = Random.Range(2, 5); ;
            }
        }

        void Jump()
        {
            rb.AddForce(transform.up * SlimeJump + -transform.right * SlimeJump, ForceMode2D.Impulse);
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
