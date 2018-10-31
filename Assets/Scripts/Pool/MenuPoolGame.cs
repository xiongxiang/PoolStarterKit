using UnityEngine;
using System.Collections;
namespace PoolKit
	{
	//the main menui pool game
	public class MenuPoolGame : MonoBehaviour 
	{
		void Start()
		{
			loadGame();
		}
		void OnEnable()
		{

			PoolKit.BaseGameManager.onGameOver += onGameOver;
			PoolKit.BaseGameManager.onButtonPress += onButtonClickCBF;

		}
		void OnDisable()
		{

			PoolKit.BaseGameManager.onGameOver -= onGameOver;
			PoolKit.BaseGameManager.onButtonPress -= onButtonClickCBF;

		}

		public void onButtonClickCBF(string buttonID)
		{
			switch (buttonID) 
			{
			case "GameToggle":
				loadGame();
			break;	
			}
		}
		void onGameOver(string vic)
		{
			if(Application.loadedLevel==0)
			{
				loadGame();
			}

		}
		void loadGame()
		{
			int gameType = PlayerPrefs.GetInt("GameType",1);
			if(gameType==0)
			{
				load("8Ball");
			}else{
				load("9Ball");
			}
		}
		void load(string str)
		{
			Destroy(GameObject.Find("8Ball"));
			Destroy(GameObject.Find("9Ball"));

			GameObject go = (GameObject)Instantiate(Resources.Load (str),Vector3.zero,Quaternion.identity);
			if(go){
				go.name = str;
			}


		}

	}
}