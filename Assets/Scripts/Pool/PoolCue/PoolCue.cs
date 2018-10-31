using UnityEngine;
using System.Collections;


namespace PoolKit
{
	//the pool cues that you should use should depend on which game you are using -- 8 ball or 9 ball the only real difference is it will check if the ball is okay or not.
	public class PoolCue : MonoBehaviour {
		//our line renderer
		public LineRenderer lineRenderer;

		//ref to the white ball
		protected WhiteBall m_whiteBall;

		//our layer mask
		public LayerMask layerMask;

		//the ball radius
		public float ballScalarRadius = 25f;

		//the reflect distance -- when we will hit a ball try to predict where it will go
		public float reflectDistance = 2f;

		//our second line renderer
		protected LineRenderer m_lr2;

		//the angle reflect
		public float angleReflect = 180f;


		public enum State
		{
			ROTATE,
			ROLL
		};

		//the current state
		protected State m_state;

		//our current power
		public float power=10f;

		/// <summary>
		/// The pool cue model
		/// </summary>
		public GameObject poolCueGO;

		//the audio to play when we hit a pool cue
		public AudioClip onHitCueAC;

		//the inital rotation
		protected Quaternion m_initalRot;

		//the inital psoition
		protected Vector3 m_initalPos;

		//the power scale -- between 1 and 100
		protected float m_powerScalar = 1f;
		//the minimum power we want ot use for this shot. 
		public float minPowerScalar = 0.125f;

		//the gui skin
		public GUISkin skin0;

		//the target ball
		protected PoolBall m_targetBall;

		//the taret position
		protected Vector3 m_targetPos;

		//do we want to rotate
		protected bool m_requestRotate = false;

		//do we want to fire the ball
		protected bool m_requestFire = false;

		//are all the balls down
		public bool areAllBallsDown=false;

		//are we greater then 8
		public int greaterThen8 = 0;

		public void Awake()
		{
			
			m_initalPos = transform.localPosition;
			m_initalRot = transform.localRotation;
			m_whiteBall =transform.parent.GetComponentInChildren<WhiteBall>();
			//(WhiteBall) GameObject.FindObjectOfType(typeof(WhiteBall));
			
			
			GameObject go = new GameObject();
			if(go)
			{
				m_lr2 = go.AddComponent<LineRenderer>();
				m_lr2.material = lineRenderer.material;
				m_lr2.SetWidth(0.1f,0.1f);
				m_lr2.SetColors(Color.yellow,Color.yellow);
				m_lr2.SetVertexCount(2);
				m_lr2.gameObject.transform.parent = poolCueGO.transform;
				m_lr2.transform.localPosition = Vector3.zero;
			}
			lineRenderer.SetVertexCount(2);
			if(lineRenderer && m_whiteBall)
				lineRenderer.SetPosition(0,m_whiteBall.transform.position);
			
			//poolCueGO.SetActive(false);
		}
		public void OnEnable()
		{
			PoolKit.BaseGameManager.onBallStop += onBallStop;
			PoolKit.BaseGameManager.onGameStart += onStartGame;
			//PoolKit.BaseGameManager.onBallHitBall += onBallHitBall;
		}
		public void OnDisable()
		{
			PoolKit.BaseGameManager.onBallStop -= onBallStop;
			PoolKit.BaseGameManager.onGameStart -= onStartGame;
		}
		public void setPower(float power)
		{
			m_powerScalar = power * 0.01f;
		}
		public void OnGUI()
		{
			//if we are in the rotate state and not in the menu scene.


			if(m_state==State.ROTATE && Application.loadedLevel>0)
			{
				GUI.skin = skin0;
				m_powerScalar = GUI.HorizontalSlider(new Rect(20,Screen.height-32,400,32),m_powerScalar,minPowerScalar,1f);

			}
		}

		public void onStartGame()
		{
			m_whiteBall =transform.parent.GetComponentInChildren<WhiteBall>();
			if(m_whiteBall)
			{
				transform.parent = m_whiteBall.transform;
			}
			transform.localScale = new Vector3(1,1,1);
			
			transform.localRotation = m_initalRot;
			transform.localPosition = m_initalPos;
			if(lineRenderer && m_whiteBall)
				lineRenderer.SetPosition(0,m_whiteBall.transform.position);
		}

		
		void onBallStop()
		{
			ballStopRPC();
		}
		public virtual void ballStopRPC()
		{
			m_state = State.ROTATE;

			if(m_whiteBall)
			{
				transform.parent = m_whiteBall.transform;
			}
			transform.localScale = new Vector3(1,1,1);

			transform.localRotation = m_initalRot;
			transform.localPosition = m_initalPos;
		}
		public virtual  void requestRotateRPC(){m_requestRotate=true;}
		public void requestRotate(){
			requestRotateRPC();
		}
		public virtual void requestFireRPC(){m_requestFire=true;}
		public  void requestFire(){
			requestFireRPC();
		}
		void Update () {
			if(m_state==State.ROTATE)
			{

				if(poolCueGO)
				{
					Vector3 pos = Vector3.zero;
					pos.z = Mathf.Lerp(-.015f,-.065f,m_powerScalar);
					poolCueGO.transform.localPosition = pos;
				}

				if(m_requestRotate)
				{
					handleRotate();
					m_requestRotate=false;
				}
				if(m_requestFire)
				{
					fireBall();
					m_requestFire=false;
				}

			}

		}


		public void fireBall()
		{
			fireBallRPC();
		}

		public void fireBallRPC()
		{
			m_requestFire=false;
			if(GetComponent<AudioSource>())
			{
				GetComponent<AudioSource>().PlayOneShot(onHitCueAC,1f);
			}

			//lets set the balls target and the target position. When the white ball hits the first ball we will set the ball to point at the target.
			m_whiteBall.setTarget(m_targetBall,m_targetPos);

			Debug.Log ("FIRE BALL" + m_whiteBall.name);
			m_whiteBall.fireBall(m_whiteBall.transform.forward * m_powerScalar * power);
			m_state = State.ROLL;
			poolCueGO.SetActive(false);
			
			transform.parent = null;

		}

		public void setTarget(PoolBall ball, Vector3 p2)
		{
			poolCueGO.SetActive(true);
			lineRenderer.SetColors(Color.green,Color.green);
			m_lr2.SetColors(Color.blue,Color.blue);
			lineRenderer.SetPosition(0,m_whiteBall.transform.position);
			lineRenderer.SetPosition(1,ball.transform.position);


			if(m_lr2)
			{
				m_lr2.gameObject.SetActive(true);
				m_lr2.SetPosition(0,ball.transform.position);		
				m_lr2.SetPosition(1,p2);
			}
		}

		public virtual bool isBallOkay(PoolBall ball)
		{
			return true;
		}
		void handleRotate()
		{
			if(m_whiteBall && m_whiteBall.sphereCollider)
			{
				poolCueGO.SetActive(true);
				SphereCollider sc = m_whiteBall.sphereCollider;
				Ray ray = new Ray(m_whiteBall.transform.position + new Vector3(0,sc.radius*ballScalarRadius,0),sc.transform.forward);
				RaycastHit rch;
				if(Physics.SphereCast(ray,sc.radius*ballScalarRadius,out rch,1000f,layerMask.value))
				{


					Vector3 pos = rch.point;
					lineRenderer.SetPosition(0,m_whiteBall.transform.position);
					float radius = sc.radius * ballScalarRadius;

					Vector3 vec =  pos-sc.transform.position;

					pos = sc.transform.position + vec.normalized * (vec.magnitude - radius);

					if(lineRenderer)
						lineRenderer.SetPosition(1,pos);
					Vector3 nrm = rch.normal;
					nrm.y=0;
					if(m_lr2)
					{
						m_lr2.gameObject.SetActive(false);
					}
					m_targetBall=null;


					lineRenderer.SetColors(Color.yellow,Color.yellow);


					if(rch.collider.name.Contains("Ball"))
					{
						m_targetBall = rch.collider.GetComponent<PoolBall>();
						bool isOkay = isBallOkay(m_targetBall);

						if(isOkay)
						{
							lineRenderer.SetColors(Color.green,Color.green);
							m_lr2.SetColors(Color.blue,Color.blue);

						}else{
							lineRenderer.SetColors(Color.red,Color.red);
							m_lr2.SetColors(Color.magenta,Color.magenta);

						}
						if(m_lr2)
						{
							m_lr2.gameObject.SetActive(true);
						}
						Vector3 otherBallPos = rch.point;

						Vector3 incomingVec = rch.point - sc.transform.position;
						Vector3 reflectVec = Vector3.Reflect(-incomingVec, rch.normal);
						Debug.DrawLine(sc.transform.position, rch.point, Color.red);
						Debug.DrawRay(rch.point, reflectVec, Color.green);


						Vector3 dir = Vector3.Reflect ( m_whiteBall.transform.forward,nrm).normalized;
						m_lr2.SetPosition(0,rch.point);

						Vector3 mirrorPos = rch.point + Quaternion.AngleAxis(angleReflect,Vector3.up) * dir.normalized * reflectDistance;
						m_lr2.SetPosition(1,mirrorPos);

						m_targetPos = mirrorPos;

						mirrorPos = pos - Quaternion.AngleAxis(angleReflect,Vector3.up) * dir.normalized * reflectDistance;

					}
				}
			 }
		}
	}
}
