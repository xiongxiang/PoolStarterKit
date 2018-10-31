using UnityEngine;
using System.Collections;
namespace PoolKit
{
	public class AIPlayer8Ball : AIPlayer 
	{

		public override bool canUseBall (PoolBall ball)
		{
			bool useBall =  m_greaterThen8 == 0;
			
			bool allBallsDown = areAllBallsDown();
			PoolBall[] balls = (PoolBall[])GameObject.FindObjectsOfType(typeof(PoolKit.PoolBall));
			if(m_greaterThen8==0)
			{
				useBall = ball.ballType != PoolBall.BallType.BLACK;
			}
			if(m_greaterThen8==1)
			{
				useBall = ball.ballType == PoolBall.BallType.STRIPE;
			}
			if(m_greaterThen8==-1)
			{
				useBall =ball.ballType == PoolBall.BallType.SOLID;
			}
			if(m_greaterThen8!=0 && ball.ballType==PoolBall.BallType.BLACK && 
			   (allBallsDown))
			{
				useBall=true;
			}
			if(balls.Length==2 && ball.ballType==PoolBall.BallType.BLACK)
			{
				useBall=true;
			}
			return useBall;
		}

		//find the closest ball to a pocket. 
		public override void findBallAndPocket()
		{
			float d0 = 1000000f;
			PocketTrigger pt = null;
			for(int i=0; i<m_balls.Length; i++)
			{
				bool useBall =  canUseBall(m_balls[i]);


				if(m_balls[i] &&
				   m_balls[i].pocketed==false && 
				   m_balls[i]!=m_whiteBall && 
				   useBall)
				{
					//m_targetPos = findClosestPocketToBall(m_targetBall,ref d0);

					for(int j=0; j<m_pockets.Length; j++)
					{		
						float d1 = (m_pockets[j].getPosition() - m_balls[i].transform.position).magnitude;

						if(d1<d0)
						{
							m_targetBall = m_balls[i];
							pt = m_pockets[j];
							m_targetPos = m_pockets[j].getPosition();
							d0 = d1;
						}
					}
				}
			}
		}


	}	
}
