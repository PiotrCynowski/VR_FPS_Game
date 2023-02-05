using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Action<GameObject, bool, bool, int> _thisEnemyKilled;
    // (which enemy, did he was killed by player, did he attacked player)

    private float moveSpeed;
    private int initHP;
    private int currentHP;
    private int whichListPool;
    private Color Color;

    private Renderer[] ren;

    public void init(Action<GameObject, bool, bool, int> enemyKilled, float _moveSpeed, int _enemyHP, Color _color, int index)
    {
        _thisEnemyKilled = enemyKilled;
        GameManager.resetEnemies += ResetThisEnemy;

        ren = GetComponentsInChildren<Renderer>();

        moveSpeed = _moveSpeed;
        currentHP = initHP = _enemyHP;
        Color = _color;
        whichListPool = index;
        
        foreach (Renderer r in ren)
        {
            r.material.SetColor("_Color", Color);
        }
    }

    public void OnEnable()
    {
        transform.rotation = Quaternion.LookRotation(-transform.position, Vector3.up);
    }

    public void OnDisable()
    {
        foreach (Renderer r in ren)
            r.material.SetColor("_Color", Color);

        currentHP = initHP;
    }

    private void Update()
    {
        Move();
        attackWhenClose();
    }

    protected virtual void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, moveSpeed * Time.deltaTime);     
    }

    void attackWhenClose()
    {
        if (Vector3.Distance(transform.position, Vector3.zero) <= 2.5f)
        {
            _thisEnemyKilled(gameObject, false, true, whichListPool);
        }
    }

    public void OnPointerClick()
    {
        currentHP--;
        
        if (currentHP <= 0)
        {
            _thisEnemyKilled(gameObject, true, false, whichListPool);
        }
        else
        {
            StartCoroutine(colorBlink());
        }
    }

    IEnumerator colorBlink()
    {
        foreach (Renderer r in ren)
            r.material.SetColor("_Color", Color.red);

        yield return new WaitForSeconds(.1f);

        foreach (Renderer r in ren)
            r.material.SetColor("_Color", Color);
    }

    private void ResetThisEnemy() //shut down enemy for game restart
    {
        _thisEnemyKilled(gameObject, false, false, whichListPool);
    }
}

public interface ICanGetEnemyData
{
    void init(Action<GameObject, bool, bool, int> enemyKilled, float _moveSpeed, int _enemyHP, Color _color, int index);
}