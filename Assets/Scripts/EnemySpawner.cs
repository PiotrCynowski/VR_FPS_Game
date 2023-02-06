using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Piotr.EnemySpawnerObjectPool
{
    public class EnemySpawner
    {
        private readonly bool collectionChecks = true;
        private readonly int maxPoolSize = 10;

        private readonly List<ObjectPool<GameObject>> poolEnemyList = new List<ObjectPool<GameObject>>();
        private Transform EnemyContainer;

        public float spawnDistanceFromPlayer;

        public void Spawn(int enemyIndex) //get from pool
        {
            poolEnemyList[enemyIndex].Get();
        }

        public void AddPoolForEnemyWithData(GameObject enemyObject, float enemySpeed, int enemyHP, Color enemyColor, int index)
        {
            EnemyContainer = new GameObject().GetComponent<Transform>();
            EnemyContainer.name = "EnemyContainer";

            ObjectPool<GameObject> poolEnemy = new ObjectPool<GameObject>(() =>
            {
                // On adding enemy to pool, with extra data from Enemy Manager           
                var enemy = GameObject.Instantiate(enemyObject, getPosAroundPlayer(), Quaternion.identity, EnemyContainer);
                if (enemy.GetComponent<IHaveEnemyData>() != null)
                {
                    enemy.GetComponent<IHaveEnemyData>().addEnemyData(thisEnemyKilled, enemySpeed, enemyHP, enemyColor, index);
                }
                return enemy;
            },
            OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);

            poolEnemyList.Add(poolEnemy);
        }


        #region poolOperations
        private void OnReturnedToPool(GameObject system)
        {
            system.gameObject.SetActive(false);
        }

        private void OnTakeFromPool(GameObject system)
        {
            system.transform.position = getPosAroundPlayer();
            system.gameObject.SetActive(true);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        private void OnDestroyPoolObject(GameObject system)
        {
            GameObject.Destroy(system.gameObject);
        }
        #endregion


        private Vector3 getPosAroundPlayer()
        {
            Vector2 outsideCirclePos = Random.insideUnitCircle.normalized * spawnDistanceFromPlayer;
            Vector3 pos = new Vector3(outsideCirclePos.x, 0, outsideCirclePos.y);
            return pos;
        }

        private void thisEnemyKilled(GameObject enemy, int index) //release from pool
        {
            poolEnemyList[index].Release(enemy);
        }
    }
}