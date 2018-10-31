using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PoolKit
{
	//our level loader. -- loads the level and sets teh player prefs.
	public class LevelLoader : MonoBehaviour 
	{

		public void Start()
		{
			DontDestroyOnLoad(gameObject);
		}
		public void OnEnable()
		{
			PoolKit.BaseGameManager.onConnect 	+=connect;

		}
		public void OnDisable()
		{
			PoolKit.BaseGameManager.onConnect 	-=connect;
		}

		//lets handle connect. Do we want to load offline mode, or connect to photon.
		void connect(bool offlineMode, 
		             int levelToLoad,
		             int nomHumans,
		             int nomAI)
		{
			PlayerPrefs.SetInt("nomHumans",nomHumans);
			PlayerPrefs.SetInt("nomAI",nomAI);

			Application.LoadLevel(levelToLoad);

		}
	}
}