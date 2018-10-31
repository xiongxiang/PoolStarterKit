using UnityEngine;
using System.Collections;
namespace PoolKit
{

	public class PoolGameScript8Ball : PoolGameScript 
	{

		public override void handleFirstBallHitByWhiteBall(PoolBall ball)
		{
			m_foul = false;
		}
		public override  void enterPocket(PoolBall ball)
		{
			enterPocketRPC(ball.name,m_playerTurn);
		}
		void enterPocketRPC(string name,int playerTurn)
		{
			m_playerTurn = playerTurn;
			GameObject go = GameObject.Find(name);
			PoolBall ball = null;
			if(go)
			{
				ball = go.GetComponent<PoolBall>();
			}


			//we sunk the 8 ball
			if(ball && 
			   ball.ballType == PoolBall.BallType.BLACK)
			{
				m_gameover=true;

				//we got all the balls down.
				if(m_players[m_playerTurn].areAllBallsDown()==false)
				{	
					PoolKit.BaseGameManager.gameover(m_players[m_playerTurn].playerName + " Loses!");
				}else{
					PoolKit.BaseGameManager.gameover( m_players[m_playerTurn].playerName + " Wins!");
				}
			}

			if(ball && ball == m_whiteBall)
			{
				m_whiteEnteredPocket = true;
			}else if(ball && ball.pocketed==false)
			{
				m_ballsPocketed++;
			}
		}

		public override void changeTurnRPC(bool foul,
		                                   int turn)
		{
			base.changeTurnRPC(foul,turn);
		}


		//handle the fouls for 8-ball.
		public override bool handleFouls()
		{
			bool fouls = false;
			int wallHit = 0;
			for(int i=0; i<m_balls.Length; i++)
			{
				if(m_balls[i] && m_balls[i].pocketed==false && m_balls[i].hitWall)
				{
					wallHit++;
				}
			}

			string foulSTR = null;

			if(m_whiteEnteredPocket)
			{
				PoolKit.BaseGameManager.showTitleCard("FOUL - White ball pocketed!");

				fouls=true;
			}
			
			if(m_foul)
			{
				PoolKit.BaseGameManager.showTitleCard(m_foulSTR);
				
			}

			if(m_break==false)
			{
				if(wallHit<4)
				{
					//it was a foul ball.
					m_break = true;
					PoolKit.BaseGameManager.showTitleCard("FOUL - At least 4 balls must hit the wall after a break!");
					fouls=true;
				}else{
					m_break=true;
				}
			}
			
			if(wallHit==0 && m_ballsPocketed==0)
			{
				PoolKit.BaseGameManager.showTitleCard("FOUL - No balls hit wall, or were pocketed!");
				fouls=true;
				
			}
			m_foul = fouls;
			
			
			clearWallHit();
			return fouls;
		}
	}
}
