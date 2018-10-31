using UnityEngine;
using System.Collections;
namespace PoolKit
{
	public class AIPlayer9Ball : AIPlayer 
	{
		//find the closest ball to a pocket. 
		public override void findBallAndPocket()
		{

			//find the first ball to hit.
			int ballIndex = 10;
			for(int i=0; i<m_balls.Length; i++)
			{
				if(m_balls[i] && m_balls[i].ballType!=PoolBall.BallType.WHITE)
				{
					//get the ball index
					string ballstr = m_balls[i].name.Substring(4,1);
					int ballIndex0  = int.Parse(ballstr);


					if(ballIndex0<ballIndex && 
					   m_balls[i].pocketed==false && 
					   m_balls[i]!=m_whiteBall )
					{
						m_targetBall = m_balls[i];
						ballIndex = ballIndex0;
					}
				}
			}


			float d0 = 100000f;
			m_targetPos = findClosestPocketToBall(m_targetBall,ref d0);

		}




	}	
}
