using UnityEngine;
using System.Collections;
namespace PoolKit
{
	public class PoolCue9Ball : PoolCue {


		public int getLowestPoolBall()
		{
			int index=1000;
			PoolBall[] balls = (PoolBall[])GameObject.FindObjectsOfType(typeof(PoolBall));
			for(int i=0; i<balls.Length; i++)
			{
				if(balls[i] && balls[i]!=m_whiteBall)
				{
					int pi = balls[i].ballIndex;
					if(pi < index)
					{
						index=pi;
					}
				}
			}
			return index;
		}
		public override bool isBallOkay(PoolBall ball)
		{
			return (ball.ballIndex) == getLowestPoolBall();
		}
	}
}