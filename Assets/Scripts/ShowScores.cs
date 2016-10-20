using UnityEngine;
using System.Collections;
using UnityEngine;

public class ShowScores : MonoBehaviour {
	public string username = "";
	// OnGUI show scores
    // Using OnGUI for this because it's far simpler for basic things like this
    // Who needs a canvas :^)
	

	void OnGUI(){
		int x = 50;
		int y = 20;
		GUILayout.BeginArea (new Rect(25, 25, 200, 30 * PhotonNetwork.playerList.Length), GUI.skin.box);
		foreach (PhotonPlayer p in PhotonNetwork.playerList) {
			GUILayout.Label(p.name + ": " + ScoreExtensions.GetScore(p), GUILayout.Height(30));
			y+=50;
		}
		GUILayout.EndArea ();

	}
}
