using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Max_Almog.MyCompany.MyGame
{
    public class EnemySpawner : MonoBehaviourPun
    {
        private void Awake()
        {
            BlackBoard.spawnerInstance = this;
        }

        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 spawnPoint = new Vector2(Random.Range(minX, maxX), transform.position.y);
                    PhotonNetwork.Instantiate("FireSlime", spawnPoint, Quaternion.identity);
                }
            }
        }

        public float minX;
        public float maxX;
        
        public void EnemySpawn()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            for (int i = 0; i < 2; i++)
            {
                Vector2 spawnPoint = new Vector2(Random.Range(minX, maxX), transform.position.y);
                PhotonNetwork.Instantiate("FireSlime", spawnPoint,Quaternion.identity);
            }
        }
    }
}
