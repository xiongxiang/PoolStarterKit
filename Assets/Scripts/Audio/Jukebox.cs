using UnityEngine;
using System.Collections;

namespace PoolKit
{
	public class Jukebox : MonoBehaviour {
		private ListPicker m_listPicker;
		public AudioClip[] songs;
		void Start () {
			m_listPicker = new ListPicker(songs.Length);
			//DontDestroyOnLoad(gameObject);
			playRandomSong();

			//audio.clip = m_listPicker.pickRandomIndex();
			//audio.Play();
		}

		// Update is called once per frame
		void Update () {
			if(GetComponent<AudioSource>().isPlaying==false)
			{
				playRandomSong();
			}
		}
		public void onLoadedMiniGame(string name, string desc)
		{
		}
		public void playRandomSong()
		{
			int index = m_listPicker.pickRandomIndex();
			GetComponent<AudioSource>().clip = songs[index];
			GetComponent<AudioSource>().Play();
		}
	}
}
