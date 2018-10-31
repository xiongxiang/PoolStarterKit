using UnityEngine;
using System.Collections;
namespace PoolKit
{

	public class PoolBall :MonoBehaviour 
	{
		public enum BallType
		{
			STRIPE,
			SOLID,
			BLACK,
			WHITE
		};
		//the type of ball
		public BallType ballType;

		//the ball texture to use.
		public Texture ballTex;
		public Vector3 ballTorque;
//		protected PhotonView m_view;
		protected Rigidbody m_rigidbody;

		//the inital position
		protected Vector3 m_initalPos;

		//the inital rotation 
		protected Quaternion m_initalRot;
		public enum State
		{
			IDLE,
			ROLL,
			DONE
		};

		//what state is the ball in
		protected State m_state;

		//the minimum speed
		public float minSpeed = 0.5f;

		//the current time the ball has to be slowed down
		protected float m_slowTime;

		//the time the ball has to be slowed down before its considered stopped
		public float slowTime = 1;

		//the balls last position
		protected Vector3 lastPosition = Vector3.zero;

		//the balls currnet speed.
		protected float Speed = 0;
		
		//did we hit the wall.
		public bool hitWall=false;

		//has the ball been pocketed
		public bool pocketed=false;

		//the balls index
		public int ballIndex=-1;
		public virtual void Awake()
		{
			m_rigidbody =gameObject.GetComponent<Rigidbody>();
		}
		public virtual void Start()
		{
			m_initalPos = transform.position;
			m_initalRot = transform.rotation;
			m_rigidbody.useConeFriction=true;
			//m_view = gameObject.GetComponent<PhotonView>();
			
			if(name.Length>4 && ballType!=BallType.WHITE)
			{
				string str = name.Substring(4,name.Length-4);;
				ballIndex = int.Parse(str);
			}
		}
		//point the ball at the target
		public void pointAtTarget(Vector3 target)
		{
			Vector3 vel = m_rigidbody.velocity;
			float mag = vel.magnitude;
			Vector3 newDir = target - transform.position;
			m_rigidbody.velocity = newDir.normalized * mag;
		}


		public virtual void OnCollisionEnter (Collision col){
			if(col.gameObject.name.Contains("Wall"))
			{
				//we hit the wall.
				PoolKit.BaseGameManager.ballHitWall(GetComponent<Rigidbody>().velocity);
				hitWall=true;
			}
			if (col.gameObject.name.Contains("Ball"))
			{
				PoolKit.BaseGameManager.ballHitBall(GetComponent<Rigidbody>().velocity);
			}
			
		}


		public void OnEnable()
		{
			PoolKit.BaseGameManager.onBallStop += onBallStop;
			PoolKit.BaseGameManager.onFireBall	+= onFireBall;
		}
		public void OnDisable()
		{
			PoolKit.BaseGameManager.onBallStop -= onBallStop;
			PoolKit.BaseGameManager.onFireBall	-= onFireBall;
		}
		public void onFireBall()
		{
			m_rigidbody.isKinematic=false;
			m_state = State.ROLL;
		}
		public void Update()
		{
			if(m_state==State.ROLL)
			{
				if(Speed<minSpeed)
				{
					m_slowTime+=Time.deltaTime;
					if(m_slowTime>slowTime)
					{
						m_state = State.DONE;
					}

				}
			}
		}


		void FixedUpdate()
		{
			Speed = (transform.position - lastPosition).magnitude / Time.deltaTime * 3.6f;
			lastPosition = transform.position;
		}

		
		public void enterPocket()
		{
			if(m_rigidbody)
			{
				pocketed=true;
				//we entered a pocket lets freeze the x and z constraints so it doesnt bounce around. 
				m_rigidbody.velocity = Vector3.zero;		
				m_state = State.DONE;
				if(ballType!=BallType.WHITE)
				{
					Destroy(gameObject);
				}else{
					transform.position = m_initalPos;
				}
			}
		}
		public virtual void onBallStop()
		{
			m_rigidbody.angularVelocity = Vector3.zero;
			m_rigidbody.velocity = Vector3.zero;
			m_rigidbody.isKinematic=true;
		}

		public bool isDoneRolling()
		{
			return m_state == State.DONE;
		}


	}
}
