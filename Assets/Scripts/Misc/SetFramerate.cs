using UnityEngine;
using System.Collections;
namespace PoolKit
	{
	public class SetFramerate : MonoBehaviour {

		//want to set the framerate for your game simply attach this script to any gameobject.
		public int framerate=60;
		// Use this for initialization
		void Start () {
			Application.targetFrameRate = framerate;
		}
		

	}
}