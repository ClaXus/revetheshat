using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;

public class MyNetworkManager : NetworkManager {

	public List<GameObject> players;
	public List<GameObject> ennemies;

	void Start(){
		players = new List<GameObject>();
		ennemies = new List<GameObject>();
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId){
		GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

		players.Add(player);

		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		//NetworkServer.AddPlayerForConnection (conn, players [indexPlayer], playerControllerId);
		//indexPlayer++;
	}


	public void addEnnemyOnServer(GameObject ennemy){
		//NetworkServer.AddPlayerForConnection
	}

	/* 
	 * 

	private HostData[] hostList;
    [SerializeField]
    private GameObject graphicalHostList;
    [SerializeField]
    private GameObject graphicalHost;

	// Use this for initialization
	void Start () {
        Application.runInBackground = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private const string typeName = "R:TSAlpha";
	private const string gameName = "R:TShTest";
	
	public void StartServer()
	{
		Network.InitializeServer(2, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}
	void OnServerInitialized()
	{
        Application.LoadLevel(1);
		Debug.Log("Server Initializied");
	}
	
	public void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
        foreach(Transform children in graphicalHostList.transform)
        {
            Destroy(children.gameObject);
        }
		if (msEvent == MasterServerEvent.HostListReceived)
        {
            hostList = MasterServer.PollHostList();
        }
        if (hostList != null)
        {
            for (int i = 0; i < hostList.Length; i++)
            {
                //if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
                //    JoinServer(hostList[i]);
                GameObject host = Instantiate(graphicalHost) as GameObject;
                host.transform.SetParent(graphicalHostList.transform, false);
                host.GetComponentInChildren<Text>().text = hostList[i].gameName;
                int index = i;
                host.GetComponent<Button>().onClick.AddListener(delegate { JoinServer(hostList[index]); });
            }
        }
	}
    
	public void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
        Application.LoadLevel(1);
	}

	void OnConnectedToServer()
	{
		Debug.Log("Server Joined");
	}*/
}