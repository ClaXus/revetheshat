using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System;

public class Player : NetworkBehaviour {
	[SerializeField]
	Button[] spellButtons;
	
	[SerializeField]
	Spell[] associateSpells;
	
	[SerializeField]
	JobPattern[] associateJobs;

	[SyncVar]
	private Vector3 syncPos;

	[SerializeField] 
	Transform myPlayer;

	[SerializeField]
	float rateSync = 15f;

	[SerializeField]
	GameObject gameObject;

	[SerializeField]
	Rigidbody ridgidbody;

	[SerializeField]
	Camera myCamera;

	[SerializeField]
	GameManagerFastGame Gmfg;

	[SerializeField]
	Unit myUnit;

	public Player gm;

	private bool isRunning = false;
	private Button currentButton;
	private int currentIndex;
	private bool canLevelUp;

	[SyncVar]
	Vector3 _direction = Vector3.zero;
	[SyncVar]
	Quaternion _rotation = Quaternion.identity;
	
	void Start(){
		if (!isLocalPlayer && !isServer)
			myCamera.enabled = (false);
		if (isServer)
			gm = this;
		Gmfg = FindObjectOfType(typeof(GameManagerFastGame)) as GameManagerFastGame;
		Gmfg.initializeButtons (ref spellButtons, this);
	}
	
	void Update()
	{		

		DoAction ();
	}

	/*void FixedUpdate(){
		TransmitPosition ();
		LerpPosition ();
	}

	void LerpPosition () {
		if (!isLocalPlayer) {
			myPlayer.transform.position = Vector3.Lerp (myPlayer.position, syncPos, Time.deltaTime * rateSync);
		}
	}
	
	[Command]
	void CmdProvidePositionToServer (Vector3 pos) {
		syncPos = pos;
	}
	
	[ClientCallback]
	void TransmitPosition(){
		if(isLocalPlayer)
			CmdProvidePositionToServer (myPlayer.position);
	}*/
	
	void DoAction(){
		if (!isLocalPlayer)
			return;
		//Debug.LogWarning ("Do Action !");
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			currentIndex = 0;
			currentButton = spellButtons [0];
			
			Debug.LogWarning ("Fire Ball !");
			StartCoroutine (b1Timer ());
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			currentIndex = 1;
			currentButton = spellButtons [1];
			StartCoroutine (b1Timer ());
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			currentIndex = 2;
			currentButton = spellButtons [0];
			StartCoroutine (b1Timer ());
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			currentIndex = 3;
			currentButton = spellButtons [3];
			StartCoroutine (b1Timer ());
		}
		
		if (Input.GetKeyDown (KeyCode.A)) {
			currentIndex = 6;
			currentButton = spellButtons [4];
			StartCoroutine (b1Timer ());
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			currentIndex = 7;
			currentButton = spellButtons [5];
			StartCoroutine (b1Timer ());
		}

		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			myUnit.Speed *= 1.4f;
		} else if (Input.GetKeyUp (KeyCode.LeftShift)) {
			myUnit.Speed /= 1.4f;
		}
		if (Input.GetKeyDown (KeyCode.T)) {
			if (canLevelUp) {

			}
		}

		/*if (Input.GetKey (KeyCode.Z)) {
			ridgidbody.MovePosition (ridgidbody.position + Vector3.forward * rateSync * Time.deltaTime);
			//Debug.LogWarning ("Z");
		}
		
		if (Input.GetKey(KeyCode.S))
			ridgidbody.MovePosition(ridgidbody.position - Vector3.forward * rateSync * Time.deltaTime);
		
		if (Input.GetKey(KeyCode.D))
			ridgidbody.MovePosition(ridgidbody.position + Vector3.right * rateSync * Time.deltaTime);
		
		if (Input.GetKey(KeyCode.Q))
			ridgidbody.MovePosition(ridgidbody.position - Vector3.right * rateSync * Time.deltaTime);*/

		direction = Vector3.forward * (Input.GetAxisRaw("Run/Back") + (Input.GetMouseButton(0) && Input.GetMouseButton(1) ? 1:0));
		direction += Vector3.right * Input.GetAxisRaw("Left/Right");
		direction += Vector3.up * Input.GetAxisRaw("Jump");
		rotation = Quaternion.Euler(Input.GetAxis("Mouse Y") * (Input.GetMouseButton(0) ? 1 : 0), Input.GetAxis("Mouse X") * (Input.GetMouseButton(1) ? 1 : 0), 0);

		//if (Input.GetKeyDown(KeyCode.LeftShift)){
		//	myUnit.Speed/=2;
		//}
	}
	
	public void btnClicked(Button b){
		currentButton = b;
		currentIndex = Array.IndexOf (spellButtons, b);
		StartCoroutine(b1Timer());
	}
	
	IEnumerator b1Timer(){
		yield return StartCoroutine( changeAspect() );
	}
	
	IEnumerator changeAspect(){
		if (currentButton.gameObject.activeSelf) {
			Button veryCurrentButton = currentButton;
			veryCurrentButton.interactable = false;
			ColorBlock cb = veryCurrentButton.colors;
			cb.normalColor = cb.disabledColor;
			cb.highlightedColor = cb.disabledColor;
			veryCurrentButton.colors = cb; 
			yield return new WaitForSeconds (associateSpells[currentIndex].castTime);
			//yield return StartCoroutine( launchSpell() );
			yield return new WaitForSeconds (associateSpells[currentIndex].cooldown);
			cb.normalColor = Color.white;
			cb.highlightedColor = Color.white;
			veryCurrentButton.interactable = true;
			veryCurrentButton.colors = cb;
		}
	}

	IEnumerator launchSpell(){
		if (currentButton.gameObject.activeSelf) {
			Button veryCurrentButton = currentButton;
			veryCurrentButton.interactable = false;
			ColorBlock cb = veryCurrentButton.colors;
			cb.normalColor = cb.disabledColor;
			cb.highlightedColor = cb.disabledColor;
			veryCurrentButton.colors = cb; 
			yield return new WaitForSeconds (associateSpells[currentIndex].castTime);
			yield return StartCoroutine( launchSpell() );
			yield return new WaitForSeconds (associateSpells[currentIndex].cooldown);
			cb.normalColor = Color.white;
			cb.highlightedColor = Color.white;
			veryCurrentButton.interactable = true;
			veryCurrentButton.colors = cb;
		}
		this.tag = "lauchingSpell";
		currentButton.tag = "launchingSpell";
	}

	
	/***************************** THIS IS THE UNITSCRIPT PART ****************************************************/

	private Unit myU;

	public void RefreshControls(Vector3 control, Quaternion rot, Unit u) {
		myU = u;
		if (isServer)
			Rpc_NetworkRefeshControls (control, _rotation);
		else 
			CmdRefeshControl(control, _rotation);

	}

	[ClientRpc]
	void Rpc_NetworkRefeshControls(Vector3 control, Quaternion rot) {
		if (myU) {
			myU._controlvector = control;
			myU._rotation = _rotation;
		}
	}
	[Command]
	public void CmdRefeshControl (Vector3 control, Quaternion rot) {
		Rpc_NetworkRefeshControls (control, _rotation);
		gm.RefreshControls (control, _rotation, myU);
	}


	/***************************** THIS IS THE CONTROLSCRIPT PART ****************************************************/
	
	
	/// <summary>
	/// Direction vector provided by Unit controller.
	/// </summary>
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
	
	/// <summary>
	/// Rotation angle provided by Unit controller.
	/// </summary>
	public Quaternion rotation
	{
		get
		{
			return _rotation;
		}
		protected set
		{
			_rotation = value;
		}
	}
}