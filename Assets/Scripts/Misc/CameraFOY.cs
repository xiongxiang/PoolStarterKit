using UnityEngine;
using System.Collections;

namespace PoolKit {

	/*
	 *The camera FOV will change the field of view based on field of view.
	 */
	[ExecuteInEditMode()]  
	public class CameraFOY : MonoBehaviour 
	{
		public float cameraFOY3by2 = 45;
		public float cameraFOY16by9 = 45;
		public float camera4by3     = 45;
		private float m_ratio = 4.0f / 3.0f;
		void Start () {

			changeFOV();
		}
		public void OnGUI()
		{
			if(inEditor())
				changeFOV();
		}
		public void Update()
		{
			if(inEditor())
				changeFOV();
		}
		public bool inEditor()
		{
			return Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor;
		}
		public void changeFOV()
		{
			float fWidth = Screen.width;
			float fHeight = Screen.height;
			float cameraRatio = fWidth / fHeight;
			if(m_ratio!=cameraRatio)
			{
				if(isAlmostCorrect(cameraRatio,3.0f/2.0f))
				{
					GetComponent<Camera>().fieldOfView = cameraFOY3by2;

				}
				if(isAlmostCorrect(cameraRatio,16.0f/9.0f))
				{
					GetComponent<Camera>().fieldOfView = cameraFOY16by9;

				}
				if(isAlmostCorrect(cameraRatio,4.0f/3.0f))
				{
					GetComponent<Camera>().fieldOfView = camera4by3;
				}
			}
		}
		public bool isAlmostCorrect(float cameraRatio, float ratio)
		{
			return (cameraRatio >= ratio-0.01f) && (cameraRatio <= ratio+0.01f);
		}

	}
}
