using UnityEngine;
using System.Collections;

public class zombieNav : MonoBehaviour {
	int counter = 10;
	// navigate the zombies woo

    //only check closest every 10 frames, it ain't that cheap
	void Update () {
		if (GetComponent<PhotonView> ().isMine) {
			if (counter > 0) {
				counter--;
			} else {
				counter = 10;
				GetComponent<NavMeshAgent> ().destination = getClosest ().position;
			}
		}
	}

    //get the closest player
	Transform getClosest(){
		Transform tMin = null;
		float minDist = Mathf.Infinity;
		Vector3 currentPos = transform.position;
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player")) {
			Transform t = g.transform;
			float dist = Vector3.Distance (t.position, currentPos);
			if (dist < minDist) {
				tMin = t;
				minDist = dist;
			}
		}
		return tMin;
	}

	void OnCollisionEnter(Collision c){
		if (c.gameObject.tag == "Bullet") {
			GetComponent<PhotonView> ().RPC ("die", PhotonTargets.AllBuffered);
		}
	}

	[PunRPC]
	void die(){
		Destroy (gameObject);
	}
}
