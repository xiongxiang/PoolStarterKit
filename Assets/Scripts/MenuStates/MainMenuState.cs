using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace PoolKit {
    public class MainMenuState : MonoBehaviour {


        /// <summary>
        /// main panel
        /// </summary>
        public GameObject mainPanel;

        //options panel
        public GameObject optionsPanel;

        //the config panel
        public GameObject configPanel;

        public Button btn8Ball;
        public Button btn9Ball;
        public Button btnOption;

        public Button btnPlayerToggle;
        public Button btnStartGame;

        public Button btnGraphicsQuality;
        public Button btnAudioToggle;
        public Button btnOptionsBack;

        public Button btnEnemyButton;

        //the list of graphics quality textures
        public Sprite[] graphicsSprites;
        //the enemy textures
        public Sprite[] enemySprites;

        /******************************************************/

        void Start() {
            this.btn8Ball.onClick.AddListener(this.OnBtn8BallClick);
            this.btn9Ball.onClick.AddListener(this.OnBtn9BallClick);
            this.btnOption.onClick.AddListener(this.OnBtnOptionsClick);


            this.btnPlayerToggle.onClick.AddListener(this.OnBtnPlayer2ToggleClick);
            this.btnStartGame.onClick.AddListener(this.OnBtnStartGameClick);

            this.btnGraphicsQuality.onClick.AddListener(this.OnBtnGraphicsToggleClick);
            this.btnAudioToggle.onClick.AddListener(this.OnBtnAudioToggleClick);
            this.btnOptionsBack.onClick.AddListener(this.OnBtnAudioToggleClick);


            this.btnEnemyButton.onClick.AddListener(this.OnBtnEnemyButtonClick);
        }

        void OnBtnAudioToggleClick() {
            if (PlayerPrefs.GetFloat("AudioVolume", 0) == 0) {
                PlayerPrefs.SetFloat("AudioVolume", 1f);
            } else {
                PlayerPrefs.SetFloat("AudioVolume", 0f);
            }
            BaseGameManager.toggleAudio();
        }

        #region btn callbacks
        private void OnBtnSinglePlayerClick() {
            configPanel.SetActive(true);
            mainPanel.SetActive(false);
        }

        private void OnBtnPlayer2ToggleClick() {
            OnBtnEnemyButtonClick();
        }

        private void OnBtnStartGameClick() {
            int enemy = PlayerPrefs.GetInt("Enemy", 0);
            int nomHumans = 1;
            int nomAI = 0;
            if (enemy == 1) {
                nomAI = 1;
            }

            if (enemy == 2) {
                nomHumans = 2;
            }
            string sceneName = PlayerPrefs.GetString("GameScene", "8Ball");
            BaseGameManager.connect(true, sceneName, nomHumans, nomAI);
        }

        private void OnBtn8BallClick() {
            PlayerPrefs.SetString("GameScene", "8Ball");
            configPanel.SetActive(true);
            mainPanel.SetActive(false);
        }

        private void OnBtn9BallClick() {
            PlayerPrefs.SetString("GameScene", "9Ball");
            configPanel.SetActive(true);
            mainPanel.SetActive(false);
        }

        private void OnBtnOptionsClick() {
            optionsPanel.SetActive(true);
            mainPanel.SetActive(false);
        }

        private void OnBtnOptionsBackClick() {
            Debug.Log("optionsBack");
            optionsPanel.SetActive(false);
            mainPanel.SetActive(true);
        }

        private void OnBtnGraphicsToggleClick() {
            int currentQuality = QualitySettings.GetQualityLevel();

            if (currentQuality == 0) {
                QualitySettings.SetQualityLevel(1);
            } else if (currentQuality == 1) {
                QualitySettings.SetQualityLevel(2);
            } else if (currentQuality == 2) {
                QualitySettings.SetQualityLevel(0);
            }
            Debug.LogError("toggleQuality" + QualitySettings.GetQualityLevel() + " oldquality " + currentQuality);
            this.UpdateBtnImage(this.btnGraphicsQuality, graphicsSprites[QualitySettings.GetQualityLevel()]);
        }

        void OnBtnEnemyButtonClick() {
            int val = PlayerPrefs.GetInt("Enemy", 0);
            val++;
            if (val >= enemySprites.Length) {
                val = 0;
            }
            PlayerPrefs.SetInt("Enemy", val);
            this.UpdateBtnImage(this.btnEnemyButton, enemySprites[val]);
        }
        #endregion

        private void UpdateBtnImage(Button button, Sprite sprite) {
            Image img = button.transform.GetComponent<Image>();
            img.sprite = sprite;
        }
    }

}