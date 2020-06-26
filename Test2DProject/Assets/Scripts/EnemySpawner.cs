using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Max_Almog.MyCompany.MyGame
{
    public class EnemySpawner : MonoBehaviourPun
    {
        public List<Enemy> enemiesOnScreen = new List<Enemy>();
        int enemyToSpawn = 2;

        private void Awake()
        {
            BlackBoard.spawnerInstance = this;
        }

        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Vector2 spawnPoint = new Vector2(Random.Range(minX, maxX), transform.position.y);
                PhotonNetwork.Instantiate("FireSlime", spawnPoint, Quaternion.identity);
            }
        }

        public float minX;
        public float maxX;
        
        public void FlagEnemyDeath(Enemy e)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            enemiesOnScreen.Remove(e);
            if (enemiesOnScreen.Count == 0)
            {
                for (int i = 0; i < enemyToSpawn; i++)
                {
                    Vector2 spawnPoint = new Vector2(Random.Range(minX, maxX), transform.position.y);
                    PhotonNetwork.Instantiate("FireSlime", spawnPoint,Quaternion.identity);
                }
            }
            if (enemyToSpawn < 5)
                enemyToSpawn++;
        }

        public void AddEnemy(Enemy e)
        {
            enemiesOnScreen.Add(e);
        }
    }
}
