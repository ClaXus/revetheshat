using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPositionSetup : NetworkBehaviour {
	[SerializeField] Camera CharacterCam;
	public override void OnStartLocalPlayer ()
	{
		CharacterCam.enabled = true;
		
	}
}