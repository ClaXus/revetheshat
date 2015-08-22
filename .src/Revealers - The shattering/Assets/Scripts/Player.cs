using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System;

public class Player : UnitControl {
	[SerializeField]
	Button[] spellButtons;
	
	[SerializeField]
	Spell[] associateSpells;
	
	[SerializeField]
	JobPattern[] associateJobs;

	[SerializeField]
	Rigidbody ridgidbody;

	[SerializeField]
	Camera myCamera;

	[SerializeField]
	GameManagerFastGame Gmfg;

	private bool isRunning = false;
	private Button currentButton;
	private int currentIndex;
	private bool canLevelUp;
	private bool accelerating = false;
	private float acceleratingRatio = 1.4f;
	private float baseSpeed;
	
	void Start(){
		if (isServer)
			gm = this;
		if (!isLocalPlayer)
			myCamera.enabled = (false);
		Gmfg = FindObjectOfType(typeof(GameManagerFastGame)) as GameManagerFastGame;
		Gmfg.initializeButtons (ref spellButtons, this);
		baseSpeed = myU.Speed;
	}
	
	void Update() {		
		DoAction ();
	}

	void FixedUpdate(){
		TransmitPosition ();
		LerpPosition ();
		if (isServer)
			Rpc_TransmitPosition ();
	}
	
	protected void LerpPosition () {
		if (!isLocalPlayer) {
			Debug.LogWarning ("LerpPosition");
			myUnit.transform.position = Vector3.Lerp (myUnit.position, syncPos, Time.deltaTime * rateSync);
		}
	}
	
	[Command]
	protected void CmdProvidePositionToServer (Vector3 pos) {
		Debug.LogWarning ("CmdPosition");
		syncPos = pos;
	}
	
	[ClientCallback]
	protected void TransmitPosition(){
		if (isLocalPlayer) {
			Debug.LogWarning ("CallBackPosition");
			CmdProvidePositionToServer (myUnit.position);
		}
	}

	[ClientRpc]
	protected void Rpc_TransmitPosition(){
		//Debug.LogWarning ("Rpc Position");
		//if(isServer)
			//myUnit.transform.position = Vector3.Lerp (myUnit.position, syncPos, Time.deltaTime * rateSync);
	}

	void DoAction(){
		if (!isLocalPlayer)
			return;
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
			currentIndex = 7;
			currentButton = spellButtons [4];
			StartCoroutine (b1Timer ());
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			currentIndex = 8;
			currentButton = spellButtons [5];
			StartCoroutine (b1Timer ());
		}

		if (Input.GetKey (KeyCode.LeftShift)) {
			accelerating = true;
		} else if(accelerating) {
			accelerating = false;
		}
		myU.Speed = accelerating?baseSpeed*acceleratingRatio:baseSpeed;

		if (Input.GetKey (KeyCode.T)) {
			if (canLevelUp) {

			}
		}

		direction = Vector3.forward * (Input.GetAxisRaw("Run/Back") + (Input.GetMouseButton(0) && Input.GetMouseButton(1) ? 1:0));
		direction += Vector3.right * Input.GetAxisRaw("Left/Right");
		direction += Vector3.up * Input.GetAxisRaw("Jump");
		rotation = Quaternion.Euler(Input.GetAxis("Mouse Y") * (Input.GetMouseButton(0) ? 1 : 0), Input.GetAxis("Mouse X") * (Input.GetMouseButton(1) ? 1 : 0), 0);
	}
	
	public void btnClicked(Button b){
		currentButton = b;
		currentIndex = Array.IndexOf (spellButtons, b);
		Debug.LogWarning ("CurrentIndex:" + currentIndex);
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


	/*
	 * public virtual void  LerpPosition () {
		if (!isLocalPlayer) {
			myTransform.position = myTransform.position;//Vector3.Lerp (myTransform.position, syncPos, Time.deltaTime * 15);
		}
	}
	
	[Command]
	public virtual void  CmdProvidePositionToServer (Vector3 pos) {
		//syncPos = pos;
	}
	
	[ClientRpc]
	public virtual void  Rpc_TransmitPosition(){
		//if(isLocalPlayer)
			//CmdProvidePositionToServer (myTransform.position);
	}
	 */
	


	/*
	void FixedUpdate(){
		if(isServer)
			Rpc_TransmitPosition ();
		LerpPosition ();
	}

	void LerpPosition () {
		if (!isLocalPlayer) {
			myTransform.position = Vector3.Lerp (myTransform.position, syncPos, Time.deltaTime * rateSync);
		}
	}
	
	[Command]
	void CmdProvidePositionToServer (Vector3 pos) {
		syncPos = pos;
	}
	
	[ClientRpc]
	void Rpc_TransmitPosition(){
		if(isLocalPlayer)
			CmdProvidePositionToServer (myTransform.position);
	}*/
}