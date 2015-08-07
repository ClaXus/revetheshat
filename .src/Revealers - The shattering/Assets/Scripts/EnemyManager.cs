using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class EnemyManager : NetworkBehaviour
{

	[SerializeField]
	GameObject[] enemies;

	[SerializeField]
	float spawnTime = 3f;            // How long between each spawn.

	[SerializeField]
	Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.

	//[SyncVar]
	public List<GameObject> InGameEnnemies;

	//public GameManagerFastGame Gmfg;

	private int enemyNumber = 0;

	void Start () {
		List<GameObject> InGameEnnemies = new List<GameObject> ();
		if(isServer)
			InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}
	
	public void Spawn() {
		Debug.LogWarning ("Spawn");
		Debug.LogWarning ("RPCSpawn");
		// Find a random index between zero and one less than the number of spawn points.
		int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
		GameObject instantiateObject = (GameObject) Instantiate(enemies[0], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
		instantiateObject.AddComponent (typeof(NetworkIdentity));
		InGameEnnemies.Add (instantiateObject);
		//addEnnemyOnServer (enemies [enemyNumber]));
		enemyNumber++;
		if (isServer)
			Rpc_Spawn ();
		else 
			Cmd_Spawn();
	}
	
	[ClientRpc]
	void Rpc_Spawn() {
		Debug.LogWarning ("RPCSpawn");
		// Find a random index between zero and one less than the number of spawn points.
		int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
		GameObject instantiateObject = (GameObject) Instantiate(enemies[0], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
		instantiateObject.AddComponent (typeof(NetworkIdentity));
		InGameEnnemies.Add (instantiateObject);
		//addEnnemyOnServer (enemies [enemyNumber]));
		enemyNumber++;
	}

	[Command]
	void Cmd_Spawn (){
		Debug.LogWarning ("CMDSpawn");
		
		Rpc_Spawn ();
		/*// Find a random index between zero and one less than the number of spawn points.
		int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
		GameObject instantiateObject = (GameObject) Instantiate(enemies[0], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
		instantiateObject.AddComponent (typeof(NetworkIdentity));
		InGameEnnemies.Add (instantiateObject);
		//addEnnemyOnServer (enemies [enemyNumber]));
		enemyNumber++;*/
	}
}