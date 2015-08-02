using UnityEngine;
using System.Collections;

public class AssignUnit : MonoBehaviour {
    [SerializeField]
    NetworkView networkView;
    [SerializeField]
    GameObject[] characters;
    int i = 0;

	// Use this for initialization
	void Start ()
    {
        if(Network.isServer)
            networkView.RPC("AssignCharacter", RPCMode.AllBuffered, Network.player, i++);
        Debug.Log("Start");
	}

    void OnConnectedToServer()
    {
        
        //Buffered mode allow to affect even future players.
    }


    void OnPlayerConnected(NetworkPlayer player)
    {
        networkView.RPC("AssignCharacter", RPCMode.AllBuffered, player, i++);
    }
    

    [RPC]
    void AssignCharacter(NetworkPlayer player, int nb)
    {
        if (i < characters.Length)
        {
            characters[nb].SetActive(true);
            Debug.Log(Network.player == player);
            if (Network.player == player)
            {

                characters[nb].GetComponent<Player>().enabled = true;
                characters[nb].GetComponentInChildren<Camera>().enabled = true;
                characters[nb].GetComponentInChildren<AudioListener>().enabled = true;
                Debug.Log("You got a slot");
            }
        }
        else
        {
            Debug.Log("No more slot available, kicking you from the game.");
            Network.Disconnect();
            Application.LoadLevel(0);
        }
    }

    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        Application.LoadLevel(0);
    }
}
