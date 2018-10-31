using UnityEngine;
using System.Collections;

namespace PoolKit
{

	public class AudioVolume : MonoBehaviour {
		private float m_initalVol;
		void Awake () {
			if(GetComponent<AudioSource>())
			{
				m_initalVol=GetComponent<AudioSource>().volume;
			}
			onSetAudioVolume();
		}
		void OnEnable()
		{
			BaseGameManager.onToggleAudio += onSetAudioVolume;
		}
		void OnDisable()
		{
			BaseGameManager.onToggleAudio -= onSetAudioVolume;
		}

		void onSetAudioVolume () {
			GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("AudioVolume",1) * m_initalVol;
			Debug.Log ("onSetAudioVolume" + GetComponent<AudioSource>().volume);

		}
	}
}