using UnityEngine;
using System.Collections;

public class zombieController : MonoBehaviour {
	public bool gameStarted = true;
	public Transform[] SpawnPositions;
	public Transform SpawnPosition;
	public float PositionOffset = 1f;
	public GameObject zombiePrefab;

	// Deals with zombie spawning, makes sure only the master client is spawning

	void Update () {
		if (PhotonNetwork.isMasterClient) {
			int temp = Random.Range (0, 140/PhotonNetwork.playerList.Length);
			if (temp <= 1) {
				spawnZombie ();
			}
		}
	}

	void OnPhotonPlayerConnected(PhotonPlayer other){
		if (PhotonNetwork.isMasterClient && PhotonNetwork.playerList.Length >= 2) {
			gameStarted = true;
		}
	}

	void spawnZombie(){
		int r = Random.Range(0, SpawnPositions.Length);
		SpawnPosition = SpawnPositions[r];
		Vector3 spawnPos = Vector3.up;
		if (this.SpawnPosition != null)
		{
			spawnPos = this.SpawnPosition.position;
		}

		Vector3 random = new Vector3 (Random.Range (SpawnPosition.localScale.x / 2, -SpawnPosition.localScale.x / 2), 0.5f, Random.Range (SpawnPosition.localScale.z / 2, -SpawnPosition.localScale.z / 2));
		Vector3 itempos = spawnPos + this.PositionOffset * random;

		PhotonNetwork.Instantiate(zombiePrefab.name, itempos, Quaternion.identity, 0);
	}
}
