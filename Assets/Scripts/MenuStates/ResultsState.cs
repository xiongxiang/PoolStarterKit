using UnityEngine;
using System.Collections;


namespace PoolKit
{
	public class ResultsState : MonoBehaviour {

		//the game over panel
		public GameObject gameoverPanel;
		//the winner label
		public GUIText winnerLabel;

		//the paue button
		public GameObject pauseButton;


		void Start()
		{
			if(gameoverPanel)
				gameoverPanel.SetActive(false);
		}

		public void OnEnable()	
		{
			BaseGameManager.onGameOver += onGameOver;
			BaseGameManager.onButtonPress += onButtonClickCBF;
		}
		public void OnDisable()	
		{
			BaseGameManager.onGameOver -= onGameOver;
			BaseGameManager.onButtonPress -= onButtonClickCBF;

		}
		public void onButtonClickCBF(string buttonID)
		{
			switch (buttonID) {
			case "Restart":
				Application.LoadLevel(Application.loadedLevel);
				break;
			case "Main Menu":
				Application.LoadLevel(0);
				break;
			}
		}

		public void onGameOver(string results)
		{
			if(pauseButton)
			{
				pauseButton.SetActive(false);
			}

			if(winnerLabel)
			{
				winnerLabel.text = results;
			}
			if(gameoverPanel)
			{
				gameoverPanel.SetActive(true);
			}
		}

	
	
	}
}