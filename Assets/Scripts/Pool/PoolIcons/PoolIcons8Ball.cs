using UnityEngine;
using System.Collections;
namespace PoolKit
{

	//this is our pool icons for the 8 ball game its going to use stripe or solids 
	//1.7 8 9..15 
	//OR
	//9..15 8 1..7
	public class PoolIcons8Ball : PoolIcons
	{


		public override void startGame () {
			PoolBall[] balls = (PoolBall[])GameObject.FindObjectsOfType(typeof(PoolBall));
			m_nomBalls = balls.Length;
			Debug.Log ("poolIcons" + m_nomBalls);
			if(m_nomBalls>0)
			{	
				base.startGame();
				setTextureOffset(0,8,Color.black);
				setTextureOffset(8,0,Color.black);
			}
		}
		



		public override void enterPocket(PoolBall ball)
		{
			if(ball.ballType!=PoolBall.BallType.WHITE)  	
			{
				if(ball.ballType!=PoolBall.BallType.BLACK)
				{
					int ballIndex = ball.ballIndex;

					bool greater8 = (ballIndex>8);
					if(m_setEvenOdd==false)
					{
						setEvenOdd(greater8);
						m_setEvenOdd=true;
					}
				}
				base.enterPocket(ball);
			}

		}
		void setEvenOdd(bool greater8)
		{
			PoolKit.BaseGameManager.setStripesOrSolids(m_playerTurn,greater8);
			
			bool g8 = true;
			if(greater8==true)g8=false;
			
			
			PoolKit.BaseGameManager.setStripesOrSolids(m_playerTurn^1,g8);
				
				
			if(m_playerTurn==0)
			{
				if(greater8)
				{
					setTextureOffset(0,8,Color.gray);
					setTextureOffset(8,0,Color.gray);
				}else{
					setTextureOffset(0,0,Color.gray);
					setTextureOffset(8,8,Color.gray);
				}
			}else{
				if(greater8)
				{
					setTextureOffset(8,8,Color.gray);
					setTextureOffset(0,0,Color.gray);
				}else
				{
					setTextureOffset(8,0,Color.gray);
					setTextureOffset(0,8,Color.gray);
				}
					
			}
		}
		
		void setTextureOffset(int indexOffset, int textureIndex,Color c)
		{
			for(int i=0; i<7; i++)
			{
				if(m_ballIcons!=null && indexOffset < m_ballIcons.Length)
				{
					Rect r = m_ballIcons[i+indexOffset].pixelInset;
					
					int jOffset=0;
					if(textureIndex>7)
					{
						jOffset = 2;
					}
					r.x = 50+50*(i+textureIndex+1+jOffset);
					m_ballIcons[i + indexOffset].pixelInset = r;
					m_ballIcons[i + indexOffset].color = c;
				}
			}
		}

	}
}