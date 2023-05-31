using UnityEngine;
using UnityEngine.UI;

namespace Player
{
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
        [Range(1, 50)]
        [SerializeField] private int initialPlayerHealth;
        private float hpDropFillImage;

        public delegate void OnGameOver();
        public static event OnGameOver GameOver;

        public delegate void OnGameRestart();
        public static event OnGameRestart GameRestart;


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

        private int _playerHealth;

        public int PlayerHealth
        {
            get
            {
                return _playerHealth;
            }
            set
            {
                _playerHealth = value;

                hpImageFill.fillAmount -= hpDropFillImage;

                if (_playerHealth <= 0)
                {
                    PlayerLost();
                }
            }
        }
        #endregion


        #region Unity methods
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
            StatReset();

            PlayerBestResult = PlayerPrefs.GetInt("score");
        }
        #endregion


        public void ButtonGameRestart()
        {
            StatReset();

            GameRestart.Invoke();
        }


        private void StatReset()
        {
            PlayerStatsCanvas.gameObject.SetActive(true);
            PlayerRestartMenuCanvas.gameObject.SetActive(false);

            PlayerHealth = initialPlayerHealth;

            hpImageFill.fillAmount = 1;

            hpDropFillImage = 1f / initialPlayerHealth;

            PlayerScore = 0;
        }

        private void PlayerLost()
        {
            if (PlayerPrefs.GetInt("score") < PlayerScore) //check highest score
            {
                PlayerPrefs.SetInt("score", PlayerScore);

                PlayerBestResult = PlayerScore;
            }

            GameOver.Invoke();

            PlayerStatsCanvas.gameObject.SetActive(false);
            PlayerRestartMenuCanvas.gameObject.SetActive(true);
        }
    }
}