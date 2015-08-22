using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Enemy : UnitControl {	

	[SerializeField]
	Enemy meThis;

	[SerializeField]
	NetworkIdentity nI;

	void Start(){



		//NetworkServer.Spawn (meThis);
		//nI.RebuildObservers (true);
		//NetworkServer.Spawn (meThis);
	}


	public void SpawnMe(Enemy instObject){
		meThis = instObject;
		//if (isServer)
		Spawn (meThis.gameObject);
		//NetworkServer.Spawn (meThis.gameObject);
	}

	public void Spawn(GameObject g){
		if(isServer)
			Rpc_Spawn (g);
		else if(isLocalPlayer)
			Cmd_Spawn (g);
		else 
			NetworkServer.Spawn (g);

		Debug.LogWarning ("Spawn");
	}

	[ClientRpc]
	public void Rpc_Spawn (GameObject g){
		if (!isServer)
		{
			return;
		}
		if (isLocalPlayer)
			Cmd_Spawn (g);
		else
			NetworkServer.Spawn (g);
	}

	[Command]
	public void Cmd_Spawn(GameObject g){
		if (!isServer)
		{
			return;
		}
		NetworkServer.Spawn (g);
	}


	/*void Update(){	
		TransmitMotion();
		LerpMotion();
	}

	[ClientCallback]
	void TransmitMotion()
	{
		if (!isServer)
		{
			return;
		}
		//nI.RebuildObservers (true);


		if (Vector3.Distance(myTransform.position, lastPos) > posThreshold || Quaternion.Angle(myTransform.rotation, lastRot) > rotThreshold){
			lastPos = myTransform.position;
			lastRot = myTransform.rotation;
			
			syncPos = myTransform.position;
			rotation = myTransform.rotation;
		}
		CmdProvidePositionToServer ();
	}

	[Command]
	void CmdProvidePositionToServer () {
		lastPos = myTransform.position;
		lastRot = myTransform.rotation;
		
		syncPos = myTransform.position;
		rotation = myTransform.rotation;//myTransform.localEulerAngles.y;
	}
	
	
	void LerpMotion()
	{
		if (isServer)
		{
			return;
		}
		
		myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
		
		Vector3 newRot = new Vector3(0, rotation, 0);
		myTransform.rotation = Quaternion.Lerp(myTransform.rotation, Quaternion.Euler(newRot), Time.deltaTime * lerpRate);
	}*/
	
}
