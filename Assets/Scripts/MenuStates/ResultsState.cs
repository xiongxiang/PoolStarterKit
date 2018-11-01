using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace PoolKit {
    public class ResultsState : MonoBehaviour {

        //the game over panel
        public GameObject gameoverPanel;
        public Text txtWinnerLabel;

        public Button btnPause;
        public Button btnRestart;
        public Button btnMainMenu;

        /*****************************************/

        void Start() {
            if (gameoverPanel)
                gameoverPanel.SetActive(false);

            this.btnMainMenu.onClick.AddListener(this.OnBtnMainMenuClick);
            this.btnRestart.onClick.AddListener(this.OnBtnRestartClick);
        }

        public void OnEnable() {
            BaseGameManager.onGameOver += onGameOver;
        }
        public void OnDisable() {
            BaseGameManager.onGameOver -= onGameOver;

        }

        #region button callbacks
        private void OnBtnRestartClick() {
            SceneManager.LoadScene(Application.loadedLevel);
        }

        private void OnBtnMainMenuClick() {
            SceneManager.LoadScene(0);
        }
        #endregion

        public void onGameOver(string results) {
            this.btnPause.gameObject.SetActive(false);
            this.txtWinnerLabel.text = results;
            if (gameoverPanel) {
                gameoverPanel.SetActive(true);
            }
        }
    }
}