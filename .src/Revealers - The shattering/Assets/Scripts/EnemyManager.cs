using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
	[SerializeField]
	Enemy[] enemies;

	[SerializeField]
	float spawnTime = 3f;            // How long between each spawn.

	[SerializeField]
	Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.

	private bool firstTimeConnecting=true;

	//[SyncVar]
	public List<Enemy> InGameEnnemies;

	//public GameManagerFastGame Gmfg;
	void Awake(){


		//Gmfg = FindObjectOfType(typeof(GameManagerFastGame)) as GameManagerFastGame;
		
	}

	private int enemyNumber = 0;
	void Start() {
		List<Enemy> InGameEnnemies = new List<Enemy> ();
		/*if (isLocalPlayer && fizrstTimeConnecting) {
			foreach (GameObject enemy in InGameEnnemies){
				enemy.GetComponent<NetworkIdentity>().RebuildObservers(true);
			}zzz
			firstTimeConnecting = false;
		}*/
		//InvokeRepeating ("Spawn", spawnTime, spawnTime);

	}
	//[Command]
	public void Spawn() {
		if (enemyNumber<3) {
			//Debug.LogWarning ("RPCSpawn");
			// Find a random index between zero and one less than the number of spawn points.
			//if (isServer) {
			int spawnPointIndex = Random.Range (0, spawnPoints.Length);
			// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
			Enemy instantiateObject = (Enemy)Instantiate (enemies [0], spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation);
			//instantiateObject.AddComponent (typeof(NetworkIdentity));
			//NetworkServer.Spawn (instantiateObject);
			instantiateObject.SpawnMe (instantiateObject);
			//addEnnemyOnServer (enemies [enemyNumber]));
			//thisObject.AddComponent (instantiateObject);
			InGameEnnemies.Add (instantiateObject);
			//InGameEnnemies [enemyNumber].SpawnMe ();
			enemyNumber++;
		}


		//}
		/* else {
			Rpc_Spawn();
		}
		if (isServer)
			Rpc_Spawn ();
		else 
			Cmd_Spawn();*/
	}

	/*[ClientRpc]
	public void Rpc_Spawn(){
		Debug.LogWarning ("RPC_Spawn");


		int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
		GameObject instantiateObject = (GameObject)Instantiate (enemies [0], spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation);
		//instantiateObject.AddComponent (typeof(NetworkIdentity));
		NetworkServer.Spawn (instantiateObject);
		//addEnnemyOnServer (enemies [enemyNumber]));
		//thisObject.AddComponent (instantiateObject);
		InGameEnnemies.Add (instantiateObject);
		enemyNumber++;
	}*/
	
}