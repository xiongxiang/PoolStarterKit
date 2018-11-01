using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace PoolKit
{
	//The title card will display when you want to show the players score.
	public class TitleCard : MonoBehaviour 
	{

		//the gui text to use for the title card.
		public GUIText label;

		//the time to display the title card
		public float displayTime = 2f;
		public void OnEnable()
		{
			BaseGameManager.onShowTitleCard += onShowTitleCard;
		}
		public void OnDisable()
		{
			BaseGameManager.onShowTitleCard -= onShowTitleCard;
		}
		public void onShowTitleCard(string str)
		{
			if(label)
			{
				label.text = str;
			}
			moveDown();
		}
		public void moveUp()
		{
			iTween.MoveTo(gameObject,  iTween.Hash("delay",displayTime,
											   "time",1,"y",0f,"x",3));
		}
		public void moveDown()
		{
			iTween.MoveTo(gameObject, iTween.Hash("time",1,"y",0f,"x",0f,
			                                   "oncomplete","moveUp",
			                                   "oncompletetarget",gameObject));
		}


	}
}