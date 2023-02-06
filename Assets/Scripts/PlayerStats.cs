using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [SerializeField] private Canvas PlayerStatsCanvas;
    [SerializeField] private Canvas PlayerRestartMenuCanvas;

    [Header("UI References")]
    [SerializeField] private Text scoreInfoText;
    [SerializeField] private Text menuScoreText;
    [SerializeField] private Text menuBestResultText;
    [SerializeField] private Image hpImageFill;

    [Header("Player Setup")]
    public int initialPlayerHealth;

    #region attributes
    private int _playerScore;

    public int PlayerScore
    {
        get
        {
            return _playerScore;
        }
        set
        {
            _playerScore = value;
            scoreInfoText.text = _playerScore.ToString();
            menuScoreText.text = "Score: " + _playerScore;
        }
    }

    private int _playerBestResult;

    public int PlayerBestResult
    {
        get
        {
            return _playerBestResult;
        }
        set
        {
            _playerBestResult = value;
            menuBestResultText.text = "Best Score: " + _playerBestResult;
        }
    }

    #endregion


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
        PlayerStatsCanvas.gameObject.SetActive(true);
        PlayerRestartMenuCanvas.gameObject.SetActive(false);
    }

    public void StatReset()
    {
        hpImageFill.fillAmount = 1;
    }

    public void DecreaseHP(float amountOfDrop)
    {
        hpImageFill.fillAmount -= amountOfDrop;
    }

    void PlayerLost()
    {
        if (PlayerPrefs.GetInt("score") < PlayerScore) //check highest score
        {
            PlayerPrefs.SetInt("score", PlayerScore);
        }

        PlayerStatsCanvas.gameObject.SetActive(false);
        PlayerRestartMenuCanvas.gameObject.SetActive(true);
    }
}
