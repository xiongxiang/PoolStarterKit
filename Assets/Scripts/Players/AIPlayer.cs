using UnityEngine;
using System.Collections;
namespace PoolKit
{
	//a simple "AI" it will simply fire the ball at the closest pin.
	//You could make this ai easier or harder by increasing or decreasing the xMax 
	public class AIPlayer : BasePlayer 
	{
		//the minimum power we are going to use.
		public float minPower = 0.25f;
		//the max maxpower to have
		public float maxPower = 1000f;

		//how long should we wait before firing the ball
		public float aiWaitTime = 1f;



		//our target x position
		protected float m_targetX = 0;

		//the target ball
		protected PoolBall m_targetBall;

		//the white ball
		protected WhiteBall m_whiteBall;

		//the pockets
		protected PocketTrigger[] m_pockets;

		//the target position
		protected Vector3 m_targetPos;

		//the pool cue
		protected PoolCue m_poolCue;

		//can we fire
		protected bool m_canFire=true;

		/// <summary>
		/// the layer mask
		/// </summary>
		public LayerMask layerMask;

		//have we fired the shot
		private bool m_firstShot=false;


		public override void Awake()
		{
			base.Awake();
			m_balls = (PoolBall[])GameObject.FindObjectsOfType(typeof(PoolBall));
			m_pockets = (PocketTrigger[])GameObject.FindObjectsOfType(typeof(PocketTrigger));

			m_whiteBall = (WhiteBall)GameObject.FindObjectOfType(typeof(WhiteBall));

			m_cue = (PoolCue)GameObject.FindObjectOfType(typeof(PoolCue));

			m_myTurn = playerIndex==0;
			//onPlayerTurn(0);

		}
		public void Update()
		{
			if(m_myTurn && m_gameOver==false)
			{
				StartCoroutine(fireBallIE());
			}
		}
		private float m_power;

		IEnumerator fireBallIE()
		{
			if (m_fired==false  && m_canFire)
			{

				m_fired = true;
				m_canFire=false;
				yield return new WaitForSeconds(0.1f);

				StartCoroutine(findTarget ());
			}
		}
		IEnumerator findTarget()
		{
			yield return new WaitForSeconds(aiWaitTime);
			
			if(m_gameOver==false)
			{
				findBallAndPocket ();
				findClosestAngleToBall();
				if(m_targetBall)
				{
					
					//we got a foul ball, lets try putting the ball in line with the line.
					getNomBallsInWay();
					
					if(foul)
					{
						//findGoodFoulSpot();
						m_power=30;
					}
					//always shoot 100% on the first shot!
					if(m_firstShot==false)
					{
						m_power=100;
						m_firstShot=true;
					}
					Vector3 targetPos = m_targetPos;
					
					targetPos.y = m_whiteBall.transform.position.y;
					
					m_cue.setTarget(m_targetBall,targetPos);
					
					
					m_cue.setPower(m_power);
					m_whiteBall.setTarget(m_targetBall,targetPos);
					m_whiteBall.transform.LookAt(m_targetBall.transform.position);
					yield return new WaitForSeconds(1f);
					
					m_cue.fireBall();
					m_canFire=true;
				}else{
					Debug.Log ("FIND NEW TARGET" + Time.time);
					StartCoroutine(findTarget ());
				}
			}
		}

		void findGoodFoulSpot()
		{
			Vector3 dir = m_targetBall.transform.position - m_targetPos;
			dir.y=0;
			Vector3 whiteBallPos = m_targetBall.transform.position;
			float dist = m_whiteBall.sphereCollider.radius*45f;
			Vector3 offset = Vector3.zero;
			float d0 = dist;
			bool noGoodPos = true;
			for(int i=0; i<5 && noGoodPos; i++)
			{
				offset+= dir.normalized * dist;
				Collider[] contantacts = Physics.OverlapSphere(whiteBallPos + offset,m_whiteBall.getRadius(),m_whiteBall.layermask.value);
				if(contantacts.Length==0)
				{
					m_whiteBall.transform.position = whiteBallPos + offset;
					noGoodPos=false;
				}
			}


		}
		public Vector3 findClosestPocketToBall(PoolBall pb,
		                                     ref float d0)
		{
			Vector3 rc = Vector3.zero;
			for(int j=0; j<m_pockets.Length; j++)
			{		
				float d1 = (m_pockets[j].getPosition() - pb.transform.position).magnitude;				
				if(d1<d0)
				{
					rc = m_pockets[j].getPosition();
					d0 = d1;
				}
			}
			return rc;
		}

		public virtual void findBallAndPocket()
		{
		}
		public int getNomBallsInPath(PoolBall whiteBall,
		                             PoolBall targetBall,
		                             Vector3 pocketPos,
		                             float radius)
		{
			RaycastHit[] hits = null;
			int count = getNomBallsInWay(whiteBall.transform.position,
			                             targetBall.transform.position,
			                             radius,
			                             ref hits);
			count = getNomBallsInWay(targetBall.transform.position,
			                          pocketPos,
			                          radius,
			                          ref hits);
			return count;
		}

		public int getNomBallsInWay(Vector3 startPos, 
		                            Vector3 targetPos,
		                            float radius,
		                            ref RaycastHit[] hits)
		{
			Vector3 dir = targetPos - startPos;
			Ray ray = new Ray(startPos,dir.normalized);
			RaycastHit[] rch = Physics.SphereCastAll(ray,radius,dir.magnitude,layerMask.value);
			int count = rch.Length;


			if(hits!=null)
			{
				for(int i=0; i<hits.Length; i++)
				{
					bool isSame = false;
					for(int j=0; j<rch.Length && isSame==false; j++)
					{
						if(hits[i].collider.name.Equals(rch[j].collider.name))
						{
							isSame=true;
						}
					}
					if(isSame==false)
					{
						count++;
					}
				}

			}
			hits = rch;

			return count;
		}
		//how much power should we give it, lets caculate how many balls are in path. Lets adjust the power acordingly. 
		//You might want to use a similar function for picking which ball to use -- IE looking at 
		public void getNomBallsInWay()
		{
			Vector3 dir = m_targetBall.transform.position - m_whiteBall.transform.position;
			Ray ray = new Ray(m_whiteBall.transform.position,dir.normalized);
			int nomHits = getNomBallsInPath(m_whiteBall,
			                               m_targetBall,
			                               m_targetPos,
			                               m_whiteBall.getRadius());
			m_power = 40f;
			if(nomHits==2)
			{
				m_power = 60;
			}else if(nomHits==3){
				m_power = 80f;
			}else{
				m_power=100;
			}
		}
		public void findRandomPocket()
		{
			m_targetPos = m_pockets[Random.Range(0,m_pockets.Length)].transform.position;
		}

		//can we use the ball
		public virtual bool canUseBall(PoolBall ball)
		{
			return true;
		}



		public void findClosestAngleToBall()
		{
			float d0 = 1000000f;
			PocketTrigger pt = null;
			
			for(int j=0; j<m_pockets.Length; j++)
			{	
				if(m_targetBall)
				{
					Vector3 targetDir = m_pockets[j].transform.position -m_targetBall.transform.position;// - m_pockets[j].transform.position;
					Vector3 forward = m_targetBall.transform.position - m_whiteBall.transform.position;
					float D1 = Vector3.Angle(targetDir, forward);
							
					if(D1<d0)
					{
						pt = m_pockets[j];
						m_targetPos = m_pockets[j].transform.position;
						d0 = D1;
					}
				}
			}
			if(pt)
			{
//				Debug.Log ("Pocket Trigger " + pt.name);
			}
		}
	

		public void findClosestBall()
		{
			float d0 = 1000000f;
			for(int i=0; i<m_balls.Length; i++)
			{
				if(m_balls[i])
				{
					float d1 = (m_whiteBall.transform.position - m_balls[i].transform.position).magnitude;
					if(d1<d0 && m_balls[i]!=m_whiteBall && m_ball.pocketed==false){
						m_targetBall = m_balls[i];
						d0 = d1;
					}
				}
			}
		}
	}
}
