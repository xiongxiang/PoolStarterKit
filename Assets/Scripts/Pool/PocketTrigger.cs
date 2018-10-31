using UnityEngine;
using System.Collections;
namespace PoolKit
{
	//the pocket triggers 
	public class PocketTrigger : MonoBehaviour 
	{
		//the color of the gizmo
		public Color gizmoColor = new Color(1,0,0,0.5f);

		//the box collider ref.
		private BoxCollider m_boxCollider;

		//the trigger id
		public string triggerID = "bowlingPit";


		void Start()
		{
			m_boxCollider = gameObject.GetComponent<BoxCollider>();
		}
	
		void OnDrawGizmos() {
			// Draw a yellow sphere at the transform's position
			Gizmos.color = gizmoColor;
			Gizmos.DrawCube (transform.position, transform.localScale);
		}

		public Vector3 getPosition()
		{
			return m_boxCollider.center;
		}

		//our ball has landed in the water, lets call the ball bowling pit
		void OnTriggerEnter(Collider col)
		{
			PoolBall pb = col.GetComponent<PoolBall>();
			if(col.name.Contains("Ball"))
			{	
				BaseGameManager.ballEnterPocket(triggerID,pb);
				pb.enterPocket();
			}
		}
	}
}