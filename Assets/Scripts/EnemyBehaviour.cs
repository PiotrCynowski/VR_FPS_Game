using System;
using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IHaveEnemyData
{
    private Action<GameObject, int> thisEnemyRemove;

    private float moveSpeed;
    private int initHP;
    private int currentHP;
    private int whichListPool;

    MaterialPropertyBlock[] propBlock;
    private Color color;

    private Renderer[] renderers;

    public void addEnemyData(Action<GameObject, int> enemyKilled, float moveSpeedE, int enemyHP, Color colorBase, int index)
    {
        thisEnemyRemove = enemyKilled;

        renderers = GetComponentsInChildren<Renderer>();

        moveSpeed = moveSpeedE;
        currentHP = initHP = enemyHP;
        whichListPool = index;

        propBlock = new MaterialPropertyBlock[2];

        propBlock[0] = new MaterialPropertyBlock();
        propBlock[1] = new MaterialPropertyBlock();

        propBlock[0].SetColor("_Color" , colorBase);
        propBlock[1].SetColor("_Color", Color.red);

        foreach (Renderer r in renderers)
        {
            r.material.SetColor("_Color", colorBase);
        }
    }

    public void getDamage()
    {
        currentHP--;

        if (currentHP <= 0)
        {
            thisEnemyRemove(gameObject, whichListPool);

            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.PlayerScore++;
            }
        }
        else
        {
            StartCoroutine(colorBlink());
        }
    }

    public void OnEnable()
    {
        transform.rotation = Quaternion.LookRotation(-transform.position, Vector3.up);
    }

    public void OnDisable()
    {
        foreach (Renderer r in renderers)
        {
            r.SetPropertyBlock(propBlock[0]);
        }

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
            thisEnemyRemove(gameObject, whichListPool);
        }
    }

    private IEnumerator colorBlink()
    {
        foreach (Renderer r in renderers)
        {
            r.SetPropertyBlock(propBlock[1]);
        }

        yield return new WaitForSeconds(.1f);

        foreach (Renderer r in renderers)
        {
            r.SetPropertyBlock(propBlock[0]);
        }
           
    }

    private void ResetThisEnemy() //shut down enemy on game restart
    {
        thisEnemyRemove(gameObject, whichListPool);
    }
}