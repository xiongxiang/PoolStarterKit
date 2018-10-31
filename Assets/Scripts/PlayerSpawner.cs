using UnityEngine;
using System.Collections;
namespace PoolKit
{
	//the player spawner -- will spawn the various players.
	public class PlayerSpawner : MonoBehaviour 
	{
		//the ai type that we want to spawn.
		public string aiToSpawn = "9Ball";

		

		//spawn the objects owned by the master client.
		public void Start()
		{
			int nomHumans = PlayerPrefs.GetInt("nomHumans");
			int nomAI = PlayerPrefs.GetInt("nomAI");

			//if its the menu -- scene load use 2 ais.
			if(Application.loadedLevel==0)
			{
				nomHumans=0;
				nomAI=2;
			}
			//how many humans do we want.
			for(int i=0; i<nomHumans; i++)
			{
				spawnPlayer(i+1);
			}
			
			//the ai will be owned by the master client
			for(int i=0; i<nomAI; i++)
			{
				spawnAI(i);
			}
			
			
			BaseGameManager.startGame();
		}




		void spawnPlayer (int i) 
		{
			string objectToSpawn = "HumanPlayer" + i;
			Instantiate(Resources.Load(objectToSpawn),
			                            Vector3.zero,
			                            Quaternion.identity);	
		}
		void spawnAI (int i) 
		{
			string objectToSpawn = aiToSpawn + "AIPlayer" + i;
			Instantiate(Resources.Load(objectToSpawn),
			                                          Vector3.zero,
			                                          Quaternion.identity);	
		}




	}
}
