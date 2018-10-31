using UnityEngine;
using System.Collections;

//instead of using unitys play on wake, we want to use play on strat, which will simply play the audio on the start, so if the audio is muted it will have
//a chance to it at the start rather afterwards, which gives a weird 1 frame sound.
namespace PoolKit
{
	public class PlayOnStart : MonoBehaviour {

		// Use this for initialization
		void Start () {
			if(GetComponent<AudioSource>())
			{
				GetComponent<AudioSource>().Play();
			}
		}
	}
}