using UnityEngine;

public class enemyFlying : Enemy, ICanGetEnemyData
{
    protected override void Move()
    {
        Vector3 pos = new Vector3(transform.position.x, 0.5f+Mathf.Sin(Vector3.Distance(transform.position, Vector3.zero)/2), transform.position.z);
        transform.position = Vector3.MoveTowards(pos, Vector3.zero, Time.deltaTime);
    }
}