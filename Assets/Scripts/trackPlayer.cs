using UnityEngine;
using System.Collections;

public class trackPlayer : MonoBehaviour {
	public GameObject player;
	public float speed = 7.8f;
	// Follow dat player
	void Update () {
		if (player == null) {
			GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
			foreach (GameObject p in players) {
				if (p.GetComponent<PhotonView> ().isMine) {
					player = p;
                    
					//GetComponent<Camera> ().orthographicSize = 10;
					break;
				}
			}
		} else {
			transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, 20, player.transform.position.z), Time.deltaTime * speed);
		}
	}
}
