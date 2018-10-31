using UnityEngine;
using System.Collections;
namespace PoolKit
{
	public class PoolCue8Ball : PoolCue {

	
		public override bool isBallOkay(PoolBall ball)
		{
			bool isOkay=true;
			if(areAllBallsDown==false)
			{
				if(greaterThen8==0)
				{
					isOkay = m_targetBall.ballType != PoolBall.BallType.BLACK;
				}
				if(greaterThen8==1)
				{
					isOkay = m_targetBall.ballType == PoolBall.BallType.STRIPE;
				}else if(greaterThen8==-1)
				{
					isOkay = m_targetBall.ballType == PoolBall.BallType.SOLID;
				}
			}else{
				isOkay = m_targetBall.ballType == PoolBall.BallType.BLACK;

			}
			return isOkay;
		}
	}
}