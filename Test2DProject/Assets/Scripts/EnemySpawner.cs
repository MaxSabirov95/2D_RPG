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

        public void FlagSpawnEnemy()
        {
            photonView.RPC("EnemySpawn", RpcTarget.MasterClient);
        }

        public float minX;
        public float maxX;

        [PunRPC]
        public void EnemySpawn()
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 spawnPoint = new Vector2(Random.Range(minX, maxX), transform.position.y);
                PhotonNetwork.Instantiate("FireSlime", spawnPoint,Quaternion.identity);
            }
        }
    }
}
