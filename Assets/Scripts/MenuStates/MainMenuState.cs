using UnityEngine;
using System.Collections;
namespace PoolKit {
    public class MainMenuState : MonoBehaviour {
        //graphics quality button
        public TouchButton2 graphicsQuality;
        //the list of graphics quality textures
        public Texture[] graphicsTextures;


        /// <summary>
        /// main panel
        /// </summary>
        public GameObject mainPanel;

        //options panel
        public GameObject optionsPanel;

        //the config panel
        public GameObject configPanel;

        //the enemy button 
        public TouchButton2 enemyButton;
        //the enemy textures
        public Texture[] enemyTextures;




        public int lvltoLoad = 1;


        void Start() {
            updateGraphicsQuality();
            updateEnemy();
        }
        public void OnEnable() {
            BaseGameManager.onButtonPress += onButtonClickCBF;
        }
        public void OnDisable() {
            BaseGameManager.onButtonPress -= onButtonClickCBF;
        }
        void toggleAudio() {
            if (PlayerPrefs.GetFloat("AudioVolume", 0) == 0) {
                PlayerPrefs.SetFloat("AudioVolume", 1f);
            } else {
                PlayerPrefs.SetFloat("AudioVolume", 0f);
            }
            BaseGameManager.toggleAudio();
        }


        public void onButtonClickCBF(string buttonID) {
            switch (buttonID) {

                case "SinglePlayer":
                    configPanel.SetActive(true);
                    mainPanel.SetActive(false);
                    //BaseGameManager.connect(true,1,false);
                    break;


                case "Player2Toggle":
                    toggleEnemies();
                    //BaseGameManager.connect(true,1,false);
                    break;
                case "StartGame":
                    handleStartGame();
                    break;
                case "8BallButton":
                    PlayerPrefs.SetInt("GameType", 0);
                    configPanel.SetActive(true);
                    mainPanel.SetActive(false);
                    break;
                case "9BallButton":
                    PlayerPrefs.SetInt("GameType", 1);
                    configPanel.SetActive(true);
                    mainPanel.SetActive(false);
                    break;


                case "Options":
                    optionsPanel.SetActive(true);
                    mainPanel.SetActive(false);
                    break;



                case "OptionsBack":
                    Debug.Log("optionsBack");
                    optionsPanel.SetActive(false);
                    mainPanel.SetActive(true);
                    break;

                case "GraphicsToggle":
                    toggleQuality();
                    break;
            }
        }
        public int getLevelToLoad() {
            return PlayerPrefs.GetInt("GameType", 0) + 1;
        }


        void handleStartGame() {
            int enemy = PlayerPrefs.GetInt("Enemy", 0);
            int nomHumans = 1;
            int nomAI = 0;
            if (enemy == 1) {
                nomAI = 1;
            }

            if (enemy == 2) {
                nomHumans = 2;
            }
            BaseGameManager.connect(true, getLevelToLoad(), nomHumans, nomAI);
        }


        void toggleEnemies() {
            int val = PlayerPrefs.GetInt("Enemy", 0);
            val++;
            if (val >= enemyTextures.Length) {
                val = 0;
            }
            PlayerPrefs.SetInt("Enemy", val);
            updateEnemy();
        }
        void updateEnemy() {
            int enemy = PlayerPrefs.GetInt("Enemy", 0);
            if (enemyButton) {
                if (enemy > -1 && enemy < enemyTextures.Length) {
                    enemyButton.setTexture(enemyTextures[enemy]);
                }
            }
        }



        public void toggleQuality() {
            int currentQuality = QualitySettings.GetQualityLevel();

            if (currentQuality == 0) {
                QualitySettings.SetQualityLevel(1);
            } else if (currentQuality == 1) {
                QualitySettings.SetQualityLevel(2);
            } else if (currentQuality == 2) {
                QualitySettings.SetQualityLevel(0);
            }
            Debug.Log("toggleQuality" + QualitySettings.GetQualityLevel() + " oldquality " + currentQuality);
            updateGraphicsQuality();
        }

        void updateGraphicsQuality() {
            if (graphicsQuality) {
                int qualityLevel = QualitySettings.GetQualityLevel();
                if (qualityLevel < graphicsTextures.Length) {
                    graphicsQuality.setTexture(graphicsTextures[QualitySettings.GetQualityLevel()]);
                }
            }
        }
    }

}