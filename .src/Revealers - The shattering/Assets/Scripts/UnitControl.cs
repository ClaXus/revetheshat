using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class UnitControl : NetworkBehaviour {
	[SyncVar]
	private Vector3 _direction = Vector3.zero;
	[SyncVar]
	private Quaternion _rotation = Quaternion.identity;
	[SyncVar]
	private Vector3 _syncPos;
	
	[SerializeField]
	public GameObject gameObject;

	[SerializeField]
	protected float rateSync = 15f;

	[SerializeField]
	protected Transform myUnit;
	
	public UnitControl gm;
	
	[SerializeField]
	protected Unit myU;

	private Vector3 lastPos;
	private Quaternion lastRot;
	private float lerpRate = 10;
	private float posThreshold = 0.5f;
	private float rotThreshold = 5;


	void Start(){
		if (isServer) {
			gm = this;
			Debug.LogError ("New GM : " + gm.ToString ());
		}
	}

	public void RefreshControls(Vector3 control, Quaternion rot, Unit u) {
		//myUTmp = u;
		LerpControls (control, rot);
		TransmitControls (control, rot);
		/*
		if (isServer) {
			Rpc_NetworkRefeshControls (control, rotation);
			Callback_NetworkRefeshControls(control, rotation);
		}
		else if (isLocalPlayer)
			CmdRefeshControl(control, rotation);
		*/
		/*else
			Callback_NetworkRefeshControls(control, position, rotation);*/
	}
	
	protected void LerpControls(Vector3 control, Quaternion rot) {
		if (!isLocalPlayer) {
			myU._controlvector = control;
			myU._rotation = _rotation;
		}
	}
	
	[Command]
	protected void CmdProvideControlsToServer (Vector3 control, Quaternion rot) {
		myU._controlvector = control;
		myU._rotation = rot;
	}
	
	[ClientCallback]
	protected void TransmitControls(Vector3 control, Quaternion rot){
		if (isLocalPlayer) {
			myU._controlvector = control;
			myU._rotation = _rotation;
		}
	}
	
	[ClientRpc]
	protected void Rpc_TransmitControls(Vector3 control, Quaternion rot){
		if (isServer) {
			myU._controlvector = control;
			myU._rotation = _rotation;
		}
	}
	/*

	[ClientRpc]
	void Rpc_NetworkRefeshControls(Vector3 control, Quaternion rot) {
		if (myU) {
			myU._controlvector = control;
			myU._rotation = rotation;
		}
	}

	[ClientCallback]
	void Callback_NetworkRefeshControls(Vector3 control, Quaternion rot) {
		if (myU) {
			myU._controlvector = control;
			myU._rotation = rotation;
		}
	}
	
	[Command]
	public void CmdRefeshControl (Vector3 control, Quaternion rot) {
		Rpc_NetworkRefeshControls (control, rotation);
		gm.RefreshControls (control, rotation, myU);
	}
	*/
	public void RefreshRealPosition(Vector3 position, Unit u){
		myU = u;
		//myU.transform.position = position;
		//_syncPos = position;
		if (isServer) {
			Rpc_RefreshRealPosition (position);
			Callback_NetworkRefeshRealPosition(position);
		}
		else if (isLocalPlayer)
			CmdRefreshRealPosition(position);
		//syncPos = position;
	}
	
	[ClientRpc]
	void Rpc_RefreshRealPosition(Vector3 position) {
		if (myU) {
			myU.transform.position = position;
			syncPos = position;
		}
	}
	
	[ClientCallback]
	void Callback_NetworkRefeshRealPosition(Vector3 position) {
		if (myU) {
			myU.transform.position = position;
			syncPos = position;
		}
	}
	
	[Command]
	public void CmdRefreshRealPosition (Vector3 position) {
		Rpc_RefreshRealPosition (position);
		gm.RefreshRealPosition (position, myU);
	}
	/*void Update(){
		if(isServer)
			Rpc_TransmitPosition ();
		//	CmdProvidePositionToServer (myTransform.position);
		LerpPosition ();
	}
	
	public void LerpPosition () {
		if (!isLocalPlayer) {
			myTransform.position = Vector3.Lerp (myTransform.position, syncPos, Time.deltaTime * rateSync);
		}
	}
	
	[Command]
	public void CmdProvidePositionToServer (Vector3 pos) {
		syncPos = pos;
	}
	
	[ClientRpc]
	public void Rpc_TransmitPosition(){
		if(isLocalPlayer)
			CmdProvidePositionToServer (myTransform.position);
	}*/
	
	public Vector3 direction
	{
		get
		{
			return _direction;
		}
		protected set
		{
			_direction = value;
		}
	}
	public Vector3 syncPos {
		get {
			return _syncPos;
		}
		protected set {
			_syncPos = value;
		}
	}
	public Quaternion rotation {
		get {
			return _rotation;
		}
		protected set {
			_rotation = value;
		}
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
	
	
	void LerpMotion() {
		if (isServer) {
			return;
		}
		
		myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
		Vector3 newRot = new Vector3(0, rotThreshold, 0);
		myTransform.rotation = Quaternion.Lerp(myTransform.rotation, Quaternion.Euler(newRot), Time.deltaTime * lerpRate);
}*/

}
