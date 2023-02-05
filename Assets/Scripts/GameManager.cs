using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    [SerializeField] private Text scoreInfo;
    [SerializeField] private Text currentResult;
    [SerializeField] private Text bestResult;
    [SerializeField] private Image spritePlayerHP;
    [SerializeField] private Image hpAttackInfo;
    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject panelMission;

    [Header("Player")]
    public float PlayerInitialHealth;
    private PlayerCharacter Player;

    [Header("Enemies")]
    public EnemyCharacter[] Enemies;
    private EnemySpawner EnemySpawn;

    public float spawnDistanceFromPlayer;

    public static Action resetEnemies;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Player = new PlayerCharacter(); //add new player
        Player.HP = (int)PlayerInitialHealth;
        spritePlayerHP.fillAmount = 1; //player HP bar

        EnemySpawn = new EnemySpawner(); //add enemies
        EnemySpawn.enemyAttackPlayer = PlayerGetDamage;
        EnemySpawn.playerKilledEnemy = PlayerGetPoint;
        EnemySpawn.spawnDistanceFromPlayer = spawnDistanceFromPlayer;

        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i].Speed <= 0)
            {
                Enemies[i].Speed = 1;
            }

            EnemySpawn.AddPoolForEnemy(Enemies[i]._enemyObject, Enemies[i].Speed, Enemies[i].HP, Enemies[i].EnemyColor, i);
            StartCoroutine(spawnEnemy(Enemies[i].spawnDelayBetweenThisEnemyType, i));
        } 
    }

    IEnumerator spawnEnemy(float timeDelay, int index)
    {
        while (true)
        {
            yield return new WaitForSeconds(timeDelay);
            EnemySpawn.Spawn(index);
        }
    }

    public void PlayerGetDamage()
    {
        StartCoroutine(playerDamaged());
    }

    IEnumerator playerDamaged()
    {
        hpAttackInfo.gameObject.SetActive(true);

        Player.HP--;

        spritePlayerHP.fillAmount -= 1f / PlayerInitialHealth;

        if (Player.HP <= 0)
        {
            PlayerLost();
        }
        yield return new WaitForSeconds(.1f);

        hpAttackInfo.gameObject.SetActive(false);
    }

    public void PlayerGetPoint()
    {
        Player.PlayerScore++;
        scoreInfo.text = Player.PlayerScore.ToString();
    }

    void PlayerLost()
    {
        if (PlayerPrefs.GetInt("score") < Player.PlayerScore) //check highest score
        {
            PlayerPrefs.SetInt("score", Player.PlayerScore);
        }

        currentResult.text = "current: " + Player.PlayerScore.ToString();
        bestResult.text = "best: " + PlayerPrefs.GetInt("score");

        panelMenu.SetActive(true);
        panelMission.SetActive(false);

        StopAllCoroutines();
        resetEnemies();
    }

    public void RestartGame() //button game restart
    {
        hpAttackInfo.gameObject.SetActive(false);

        Player = new PlayerCharacter();
        Player.HP = (int)PlayerInitialHealth;
        spritePlayerHP.fillAmount = 1;
        scoreInfo.text = "0";

        for (int i = 0; i < Enemies.Length; i++)
        {
            StartCoroutine(spawnEnemy(Enemies[i].spawnDelayBetweenThisEnemyType, i));
        }   
    }

    class PlayerCharacter
    {
        public int HP;
        public int PlayerScore = 0;
    }

    [Serializable]
    public class EnemyCharacter
    {
        public GameObject _enemyObject;
        public float Speed;
        public int HP;
        [Range(2, 7)]
        public float spawnDelayBetweenThisEnemyType;
        public Color EnemyColor;
    }
}