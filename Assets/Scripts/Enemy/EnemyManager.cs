using System;
using System.Collections;
using UnityEngine;
using Piotr.EnemySpawnerObjectPool;
using Player;

namespace Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [Header("Enemies")]
        [SerializeField] private EnemyCharacter[] enemyPrefabs;
        [SerializeField] private EnemySpawner enemySpawner;

        [Range(15, 30)]
        [SerializeField] private float spawnDistanceFromPlayer;

        private Coroutine[] spawningCoroutines;


        private void Start()
        {
            PlayerStats.GameOver += PlayerLost;
            PlayerStats.GameRestart += RestartGame;

            enemySpawner = new EnemySpawner
            {
                spawnDistanceFromPlayer = spawnDistanceFromPlayer
            };

            for (int i = 0; i < enemyPrefabs.Length; i++)
            {
                enemySpawner.AddPoolForEnemyWithData(enemyPrefabs[i].enemyObject, enemyPrefabs[i].speed, enemyPrefabs[i].hp, enemyPrefabs[i].enemyColor, i);
            }

            RestartGame();
        }

        private IEnumerator SpawnEnemy(float timeDelay, int index)
        {
            while (true)
            {
                yield return new WaitForSeconds(timeDelay);
                enemySpawner.Spawn(index);
            }
        }

        private void PlayerLost()
        {
            enemySpawner.ResetAllEnemies();

            foreach (Coroutine spawn in spawningCoroutines)
            {
                StopCoroutine(spawn);
            }
        }

        private void RestartGame()
        {
            spawningCoroutines = new Coroutine[enemyPrefabs.Length];

            for (int i = 0; i < enemyPrefabs.Length; i++)
            {
                spawningCoroutines[i] = StartCoroutine(SpawnEnemy(enemyPrefabs[i].spawnDelayBetweenThisEnemyType, i));
            }
        }

        [Serializable]
        public class EnemyCharacter
        {
            public GameObject enemyObject;

            [Range(2, 7)]
            public float spawnDelayBetweenThisEnemyType;
            [Range(1, 15)]
            public float speed;
            [Range(1, 15)]
            public int hp;
            public Color enemyColor;
        }
    }
}