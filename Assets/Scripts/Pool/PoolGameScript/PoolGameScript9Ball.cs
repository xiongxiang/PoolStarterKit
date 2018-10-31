using UnityEngine;
using System.Collections;
namespace PoolKit
{

	public class PoolGameScript9Ball : PoolGameScript {
		private int m_totalBallsPocketed=0;
		private int m_ballIndex;

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
		public override void changeTurnRPC(bool foul,
		                                   int turn)
		{
			base.changeTurnRPC(foul,turn);
		}

		public override void handleFirstBallHitByWhiteBall (PoolBall ball)
		{
			if(ball)
			{
				m_ballIndex=ball.ballIndex;
				Debug.Log ("m_ballIndex"+ m_ballIndex + " getLowesBall() + "+ getLowestPoolBall());
				m_foul = m_ballIndex != getLowestPoolBall();
			}else{
				Debug.Log ("NUllball");
			}
		}
		public override  void enterPocket(PoolBall ball)
		{
			if(ball && ball == m_whiteBall)
			{
				m_whiteEnteredPocket = true;
			}else 
			{
				m_totalBallsPocketed++;
				m_ballsPocketed++;
			}
			//we sunk the 8 ball
			if(ball.ballIndex==9)
			{
				m_gameover=true;
				if(m_totalBallsPocketed==9)
				{	
					PoolKit.BaseGameManager.gameover(m_players[m_playerTurn].playerName + " Wins!");
				}else{
					PoolKit.BaseGameManager.gameover( m_players[m_playerTurn].playerName + " Loses!");
				}
			}
			

			Debug.Log ("enterPocket"  + ball.ballIndex + "  m_totalBallsPocketed " + m_totalBallsPocketed);

		}

		//handle the fouls for 9-ball.
		public override bool handleFouls()
		{

			Debug.Log ("hitBall"  + m_ballIndex);
			if(m_foul)
			{
				m_foulSTR = "FOUL - Failed to hit the lowest ball first!";
			}

			if(m_whiteEnteredPocket)
			{
				m_foulSTR = "FOUL - White ball pocketed!";
				m_foul=true;
			}

			if(m_foul)
			{
				PoolKit.BaseGameManager.showTitleCard(m_foulSTR);

			}

			m_ballIndex=-1;
			return m_foul;
		}
	}
}
