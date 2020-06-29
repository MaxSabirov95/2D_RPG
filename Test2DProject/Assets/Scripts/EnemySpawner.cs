using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Max_Almog.MyCompany.MyGame
{
    public class EnemySpawner : MonoBehaviourPun
    {
        public static int maxEnemySpawn = 8;
        int enemyToSpawn = 2;
        public static int enemyCountOnScreen;

        private void Awake()
        {
            BlackBoard.spawnerInstance = this;
        }

        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                enemyCountOnScreen++;
                Vector2 spawnPoint = new Vector2(Random.Range(minX, maxX), transform.position.y);
                PhotonNetwork.Instantiate("FireSlime", spawnPoint, Quaternion.identity);
            }
        }

        public float minX;
        public float maxX;
        
        public void CountEnemyDeath()
        {
            photonView.RPC("FlagEnemyDeath", RpcTarget.MasterClient);
        }

        [PunRPC]
        public void FlagEnemyDeath()
        {
            //if (!PhotonNetwork.IsMasterClient) return;
            enemyCountOnScreen--;
            if (enemyCountOnScreen == 0)
            {
                for (int i = 0; i < enemyToSpawn; i++)
                {
                    Vector2 spawnPoint = new Vector2(Random.Range(minX, maxX), transform.position.y);
                    PhotonNetwork.Instantiate("FireSlime", spawnPoint,Quaternion.identity);
                }
                enemyCountOnScreen = enemyToSpawn;
                if (enemyToSpawn < maxEnemySpawn)
                {
                    //enemyCountOnScreen = enemyToSpawn;
                    enemyToSpawn++;
                }
            }
        }
    }
}
