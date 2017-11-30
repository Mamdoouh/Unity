// Patrol.cs
using UnityEngine;
using System.Collections;
using UnityEngine.AI;


public class patrol : MonoBehaviour {

	public Transform[] points;
	private int destPoint = 0;
	private NavMeshAgent agent;
	GameObject player;
	private bool chasing = false;

	void Start () {
		agent = GetComponent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag ("player");
		agent.autoBraking = false;
		GotoNextPoint();
	}


	void GotoNextPoint() {
		// Returns if no points have been set up


		if (points.Length == 0 || chasing)
			return;

		// Set the agent to go to the currently selected destination.
		agent.destination = points[destPoint].position;

		// Choose the next point in the array as the destination,
		// cycling to the start if necessary.
		destPoint = (destPoint + 1) % points.Length;
	}

	void OnTriggerStay(Collider c){
		if (c.gameObject.CompareTag ("player")) {
			agent.isStopped = true;
			transform.LookAt (player.transform);
			if (playerVisible ()) {
				
				chasing = true;
				agent.destination = player.transform.position;
				agent.isStopped = false;
			}
			chasing = false;
		}
	}

	bool playerVisible(){
		Vector3 forward = transform.TransformDirection (Vector3.forward) * 50;
		RaycastHit hit;
		if (Physics.Raycast (transform.position, forward, out hit)) {
			return (hit.collider.gameObject.CompareTag ("player"));
		} else {
			return false;
		}
	}
	void Update () {
		// Choose the next destination point when the agent gets
		// close to the current one.
		if (!agent.pathPending && agent.remainingDistance < 0.5f)
			GotoNextPoint();
	}

}