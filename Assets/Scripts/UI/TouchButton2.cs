using UnityEngine;
using System.Collections;
namespace PoolKit
{

	public class TouchButton2 : MonoBehaviour {
		protected GUITexture m_guiTex;	
		protected Rect m_touchZone;
		public bool repeatButton=false;

		private float m_colorTime;
		protected bool m_disabled = false;

		public Color colorDisabled =  new Color(0.25f,0.25f,0.25f,0.5f);
		public Color colorEnabled =  new Color(0.5f,0.5f,0.5f,0.5f);
		public Color endColor = new Color(0.5f,0.5f,0.5f,0.5f);
		public Color activeColor = new Color(1f,1f,1f,0.5f);
		public bool canDisable=true;
		public enum ButtonState
		{
			NULL,
			COLORUP,
			COLORDOWN

		};
		public ButtonState m_state = ButtonState.NULL;
		private float m_colorChangeTime = 0.125f;
		
		public bool onInstantFire=false;
		public void setEnabled(bool enabled)
		{
			if(enabled)
			{
				enable();
			}else{
				disable();
			}
		}
		public void setTexture(Texture tex)
		{
			if(m_guiTex==null)
			{
				m_guiTex = gameObject.GetComponent<GUITexture>();
			}
			if(m_guiTex)
			{
				m_guiTex.texture = tex;
			}
		}

		public virtual void enable()
		{
			if(m_guiTex)
			{
				m_guiTex.color = colorEnabled;
			}
				m_disabled=false;
		}
		public virtual void disable()
		{
			if(canDisable)
			{
				if(m_guiTex)
				{
					m_guiTex.color = colorDisabled; 
				}
				m_disabled=true;
				m_state = TouchButton2.ButtonState.NULL;
			}
		}

		public  virtual void Awake()
		{
			//Debug.Log("Start");

			
			m_guiTex = gameObject.GetComponent<GUITexture>();
			if(m_guiTex==null)
			{
				m_guiTex = gameObject.GetComponentInChildren<GUITexture>();			
				
			}
			resize();
		}
		public void resize(){
			//Debug.Log("resize");


			float w = transform.localScale.x * Screen.width;

			float h = transform.localScale.y * Screen.height;
		

			if(w != 0 && h != 0)
			{
				m_touchZone.x = (transform.position.x * Screen.width) - w*.5f;
				m_touchZone.y = ((transform.position.y * Screen.height) - h*.5f);
				m_touchZone.width = w;
				m_touchZone.height = h;

			}else{
				if(m_guiTex)
				{
					m_touchZone = m_guiTex.pixelInset;
					m_touchZone.x = (transform.position.x * Screen.width) + m_touchZone.x;
					m_touchZone.y =(transform.position.y * Screen.height) + m_touchZone.y;	
				}
			}
		//	Debug.Log("m_touchZone"+m_touchZone);


		//	m_guiTex.pixelInset = m_touchZone;
		//	transform.position= new Vector3(0,0,transform.position.z);

			
		}
		public Rect getTouchZone()
		{
			return m_touchZone;
		}
		
		public void OnEnable()
		{
			m_colorTime = 0f;
			enable();
		}
		public virtual void onPressCBF(string buttonID)
		{
	//		Debug.Log("touchButton OnpressCBF" + buttonID);
		}

		float m_lastTime;
		void colorDown(float rdt)
		{
			if(m_colorTime>0)
			{
				m_colorTime -= rdt;
				float val = m_colorTime / m_colorChangeTime;
				if(m_guiTex)
				{
					m_guiTex.color = Color.Lerp(activeColor,endColor,1.0f - val);
				}
	//			Debug.Log("activeColor " + activeColor);
			}else{
				m_state = TouchButton2.ButtonState.NULL;
			}
		}

		void colorUp(float rdt)
		{

			if(m_colorTime>0)
			{
				m_colorTime -= rdt;
				
				float val = m_colorTime / m_colorChangeTime;
				if(m_guiTex)
				{
					m_guiTex.color = Color.Lerp(colorEnabled,activeColor,1.0f - val);
				}
			}else{
				if(onInstantFire==false)
				{
					onPress();
				}		
				m_colorTime = m_colorChangeTime;
				m_state = TouchButton2.ButtonState.COLORDOWN;
			}
		}
		
		public void Update()
		{

			resize ();
			handleUpdate();
		}
		
		public void handleUpdate()
		{
			float realDT = 0f;
			if(m_disabled)return;
			if(m_lastTime!=-1)
			{
				realDT = Time.realtimeSinceStartup  - m_lastTime;
			}
			m_lastTime = Time.realtimeSinceStartup;
			switch(m_state)
			{
				case ButtonState.COLORUP:
					colorUp(realDT);
				break;
				case ButtonState.COLORDOWN:
					colorDown(realDT);
				break;
				
			}
			

			if(Application.platform == RuntimePlatform.IPhonePlayer ||
			   Application.platform == RuntimePlatform.Android)
			{
				updateIphone();
			}else{	
				if(Input.GetMouseButtonDown(0) ||
					(repeatButton && Input.GetMouseButton(0)))
				{
					checkPress( Input.mousePosition );	
			    }
			}
		}
		public virtual void onPress()
		{

			PoolKit.BaseGameManager.buttonPress(gameObject.name);
		}

		public virtual void checkPress(Vector2 pos)
		{
	//		Debug.Log("checkPress " + m_touchZone + " pos "  + pos);
			if ( pos.x > m_touchZone.x  && pos.x <= m_touchZone.x+m_touchZone.width
			    && pos.y > m_touchZone.y && pos.y <= m_touchZone.y+m_touchZone.height)
			{	
				//Debug.Log("checkPress2 " );

				handlePress();
			}		
		}
		public void handlePress()
		{
			if(repeatButton)
			{
				onPress();
			}else{
				if(onInstantFire)
				{
					onPress ();
				}
				m_state = ButtonState.COLORUP;
				m_colorTime = m_colorChangeTime;
			}		
		}
		
		public virtual bool canClick()
		{
			return true;
		}
		
		public void updateIphone()
		{

			int count = Input.touchCount;
			for(int i = 0;i < count; i++)
			{
				Touch touch = Input.GetTouch(i);			
				Vector2 pos  = touch.position;		
				bool repeatOkay = touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved;
				if(touch.phase == TouchPhase.Began ||(repeatButton && repeatOkay))
				{
					checkPress( pos );
				}
				
			}

		}	


	}
}
