using System;
using System.Collections;
using UnityEngine;
using Piotr.EnemySpawnerObjectPool;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private EnemyCharacter[] enemyPrefabs;
    [SerializeField] private EnemySpawner enemySpawner;

    public static Action resetEnemies;

    [Range(15, 30)]
    public float spawnDistanceFromPlayer;

    private void Start()
    {
        enemySpawner = new EnemySpawner();
        enemySpawner.spawnDistanceFromPlayer = spawnDistanceFromPlayer;

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            enemySpawner.AddPoolForEnemyWithData(enemyPrefabs[i].enemyObject, enemyPrefabs[i].speed, enemyPrefabs[i].hp, enemyPrefabs[i].enemyColor, i);
            
            StartCoroutine(spawnEnemy(enemyPrefabs[i].spawnDelayBetweenThisEnemyType, i));
        }
    }

    private IEnumerator spawnEnemy(float timeDelay, int index)
    {
        while (true)
        {
            yield return new WaitForSeconds(timeDelay);
            enemySpawner.Spawn(index);
        }
    }

    private void PlayerLost()
    {
        resetEnemies();
    }

    private void RestartGame() 
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            StartCoroutine(spawnEnemy(enemyPrefabs[i].spawnDelayBetweenThisEnemyType, i));
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

public interface IHaveEnemyData
{
    void addEnemyData(Action<GameObject, int> enemyKilled, float _moveSpeed, int _enemyHP, Color _color, int index);

    void getDamage();
}