using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class menuControl : MonoBehaviour {
    public Text username;
	// Controls the menu buttons

    public void play()
    {
        if(username.text != "")
        {
            PhotonNetwork.playerName = username.text;
            SceneManager.LoadScene("main");
        }
    }

    public void exit()
    {
        Application.Quit();
    }
}
