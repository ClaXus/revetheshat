using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SynchronousPosition : NetworkBehaviour {

	[SyncVar]
	private Vector3 syncPos;

	[SerializeField] 
	Transform myPlayer;
	[SerializeField]
	float rateSync = 15f;

	void FixedUpdate(){
		TransmitPosition ();
		LerpPosition ();
	}

	// Use this for initialization
	void LerpPosition () {
		if(!isLocalPlayer)
			myPlayer.transform.position = Vector3.Lerp(myPlayer.position, syncPos, Time.deltaTime*rateSync);
	}
	
	[Command]
	void CmdProvidePositionToServer (Vector3 pos) {
		syncPos = pos;
	}

	[ClientCallback]
	void TransmitPosition(){
		if(isLocalPlayer)
			CmdProvidePositionToServer (myPlayer.position);
	}
}
