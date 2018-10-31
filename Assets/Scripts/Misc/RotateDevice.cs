using UnityEngine;
using System.Collections;
namespace PoolKit
{
	//This simple script will allow you to rotate your device, but dsiable portrait mode,
	//so if you want to be able to flip your device to say landscape right, or landscape right the gui etc will simply auto-rotate with it -- then simply attach this 
	//script to a gameobject in the mainmenu scene

	public class RotateDevice : MonoBehaviour {

		void Start () {
			Screen.orientation = ScreenOrientation.AutoRotation;
			Screen.autorotateToPortrait=false;
			Screen.autorotateToPortraitUpsideDown=false;
			DontDestroyOnLoad(gameObject);
		}
		

	}

}