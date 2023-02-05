using System;
using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, ICanGetEnemyData
{
    private Action<GameObject, int> thisEnemyKilled;

    private float moveSpeed;
    private int initHP;
    private int currentHP;
    private int whichListPool;
    private Color color;
    private Renderer[] renderers;

    public void init(Action<GameObject, int> enemyKilled, float _moveSpeed, int enemyHP, Color colorBase, int index)
    {
        thisEnemyKilled = enemyKilled;
        renderers = GetComponentsInChildren<Renderer>();

        moveSpeed = _moveSpeed;
        currentHP = initHP = enemyHP;
        color = colorBase;
        whichListPool = index;

        foreach (Renderer r in renderers)
        {
            r.material.SetColor("_Color", colorBase);
        }
    }

    public void OnEnable()
    {
        transform.rotation = Quaternion.LookRotation(-transform.position, Vector3.up);
    }

    public void OnDisable()
    {
        foreach (Renderer r in renderers)
            r.material.SetColor("_Color", color);

        currentHP = initHP;
    }

    private void Update()
    {
        Move();
        attackWhenClose();
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, moveSpeed * Time.deltaTime);     
    }

    private void attackWhenClose()
    {
        if (Vector3.Distance(transform.position, Vector3.zero) <= 2.5f)
        {
            thisEnemyKilled(gameObject, whichListPool);
        }
    }

    private void OnDamage()
    {
        currentHP--;
        
        if (currentHP <= 0)
        {
            thisEnemyKilled(gameObject, whichListPool);
        }
        else
        {
            StartCoroutine(colorBlink());
        }
    }

    private IEnumerator colorBlink()
    {
        foreach (Renderer r in renderers)
            r.material.SetColor("_Color", Color.red);

        yield return new WaitForSeconds(.1f);

        foreach (Renderer r in renderers)
            r.material.SetColor("_Color", color);
    }

    private void ResetThisEnemy() //shut down enemy on game restart
    {
        thisEnemyKilled(gameObject, whichListPool);
    }  
}