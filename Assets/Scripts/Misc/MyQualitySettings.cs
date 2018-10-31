using UnityEngine;
using System.Collections;

namespace PoolKit
	{
	//myquality settings should be attached to the main camera...
	public class MyQualitySettings : MonoBehaviour {
		private int QUALITY_SETTINGS = -1;
		public bool highqualityFog = true;
		private MonoBehaviour m_fastBloomComponent;
		void Start()
		{
			m_fastBloomComponent = (MonoBehaviour)gameObject.GetComponent("FastBloom");
		}
		// Update is called once per frame
		void Update () {
			int currentLevel = QualitySettings.GetQualityLevel();

			if(QUALITY_SETTINGS!=currentLevel)
			{
//				Debug.Log ("Changing quality settings:" + currentLevel);
				changeQuality();
				currentLevel = QUALITY_SETTINGS;
			}
		}

		void changeQuality()
		{
			int currentLevel = QualitySettings.GetQualityLevel();

			//low disable fog and bloom
			if(currentLevel==0)
			{
				RenderSettings.fog = false;
				if(m_fastBloomComponent)

				{
					m_fastBloomComponent.enabled=false;
				}
			}
			//medium no fog, bloom
			if(currentLevel==1)
			{
				RenderSettings.fog = true;
				if(m_fastBloomComponent)
					
				{
					m_fastBloomComponent.enabled=false;
				}
			}
			//high no fog, bloom
			if(currentLevel==2)
			{
				RenderSettings.fog = true;
				if(m_fastBloomComponent)
					
				{
					m_fastBloomComponent.enabled=highqualityFog;
				}
			}

		}
	}
}