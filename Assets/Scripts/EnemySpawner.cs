using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner 
{
    public float spawnDistanceFromPlayer;

    public Action enemyAttackPlayer;
    public Action playerKilledEnemy;

    private List<ObjectPool<GameObject>> _poolEnemyList = new List<ObjectPool<GameObject>>();

    public void AddPoolForEnemy(GameObject _enemyObject, float enemySpeed, int enemyHP, Color enemyColor, int index) //add object pool
    {
        ObjectPool<GameObject> _poolEnemy = new ObjectPool<GameObject>(() =>
        {
            var enemy = GameObject.Instantiate(_enemyObject, getPosAroundPlayer(), Quaternion.identity);
            enemy.GetComponent<ICanGetEnemyData>().init(thisEnemyKilled, enemySpeed, enemyHP, enemyColor, index);
            return enemy;
        }, objectEnemy =>
        {
            objectEnemy.gameObject.SetActive(true);
        }, objectEnemy =>
        {
            objectEnemy.gameObject.SetActive(false);
            objectEnemy.transform.position = getPosAroundPlayer();
        }, objectEnemy =>
        {
            //keeping enemies spawned
        }, false, 10, 100);

        _poolEnemyList.Add(_poolEnemy);
    }

    public void Spawn(int enemyIndex) //get from pool
    {
        _poolEnemyList[enemyIndex].Get();
    }

    Vector3 getPosAroundPlayer()
    {
        Vector2 outsideCirclePos = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(spawnDistanceFromPlayer, spawnDistanceFromPlayer+5);
        Vector3 pos = new Vector3(outsideCirclePos.x, 0, outsideCirclePos.y);
        return pos;
    }

    private void thisEnemyKilled(GameObject enemy, bool isByPlayer, bool attackPlayer, int index) //release from pool
    {
        if(isByPlayer)
            playerKilledEnemy();

        if (attackPlayer)
            enemyAttackPlayer();

        _poolEnemyList[index].Release(enemy);
    }
}