using UnityEngine;
using System.Collections;
namespace PoolKit
{
	public class PoolIcons : MonoBehaviour
	{
		//refeence to the gui textures
		protected GUITexture[] m_ballIcons;

		//reference to the textures
		protected Texture[] m_textures;

		//the players turn
		protected int m_playerTurn = 0;

		//are we on add or even
		protected bool m_setEvenOdd=false;

		//the number of balls
		protected int m_nomBalls;

		//a ref to the gamescript
		protected PoolGameScript m_gamescript;


		public void Start()
		{
			onGameStart();
		}
		public void onGameStart()
		{
			startGame();
		}
		public virtual void startGame () {
			PoolBall[] balls = (PoolBall[])GameObject.FindObjectsOfType(typeof(PoolBall));
			m_nomBalls = balls.Length;

			if(m_nomBalls>0)
			{
				m_gamescript  = (PoolGameScript)GameObject.FindObjectOfType(typeof(PoolGameScript));

				m_ballIcons = new GUITexture[m_nomBalls];
				m_textures = new Texture[m_nomBalls];

				for(int i=0; i<m_nomBalls; i++)	
				{
					string str = (i+1).ToString();
					GameObject go = GameObject.Find(str);

					if(go)
					{
						if(i+1 < 10)
						{
							str = "0" + (i+1);
						}
						string ballName = "GUI/GUI_ball" + str;
						Debug.Log ("load texture " + ballName);
						m_textures[i] = Resources.Load (ballName) as Texture;

						m_ballIcons[i] = go.GetComponent<GUITexture>();
						m_ballIcons[i].texture = m_textures[i];
						m_ballIcons[i] .color=Color.gray*.4f;	
					}

				}
			}
		}
		void onPlayerTurn(int playerTurn)
		{
			m_playerTurn = playerTurn;
		}
		void OnEnable()
		{
			PoolKit.BaseGameManager.onGameStart		  += onGameStart;

			PoolKit.BaseGameManager.onBallEnterPocket += onEnterPocket;
			PoolKit.BaseGameManager.onPlayerTurn		+= onPlayerTurn;
		}
		void OnDisable()
		{
			PoolKit.BaseGameManager.onBallEnterPocket -= onEnterPocket;
			PoolKit.BaseGameManager.onPlayerTurn		-= onPlayerTurn;
			PoolKit.BaseGameManager.onGameStart		  -= onGameStart;

		}


		void onEnterPocket(string pocketIndex, PoolBall ball)
		{
			enterPocket(ball);
		}

		//a ball lands in the pocket lets gray out the texture.
		public virtual void enterPocket(PoolBall ball)
		{
			if(ball.ballType!=PoolBall.BallType.WHITE)  	
			{
				int ballIndex = ball.ballIndex;
				ballIndex--;
				m_ballIcons[ballIndex].color = Color.gray*.2f	;
			}

		}
	}
}