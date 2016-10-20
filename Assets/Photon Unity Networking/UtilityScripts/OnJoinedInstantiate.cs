using UnityEngine;
using System.Collections;

public class OnJoinedInstantiate : MonoBehaviour
{
    public Transform SpawnPosition;
    public float PositionOffset = 2.0f;
    public GameObject player;   // set in inspector
    public Transform[] SpawnPositions;

    public void OnJoinedRoom()
    {
        int r = Random.Range(0, SpawnPositions.Length);
        SpawnPosition = SpawnPositions[r];
        Vector3 spawnPos = Vector3.up;
        if (this.SpawnPosition != null)
        {
            spawnPos = this.SpawnPosition.position;
        }

        Vector3 random = new Vector3(Random.Range(SpawnPosition.localScale.x / 2, -SpawnPosition.localScale.x / 2), 0.5f, Random.Range(SpawnPosition.localScale.z / 2, -SpawnPosition.localScale.z / 2));
        Vector3 itempos = spawnPos + this.PositionOffset * random;

        PhotonNetwork.Instantiate(player.name, itempos, Quaternion.identity, 0);
    }
}
