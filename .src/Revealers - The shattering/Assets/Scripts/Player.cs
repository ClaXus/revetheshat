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


	private bool isRunning = false;
	private Button currentButton;
	private int currentIndex;
	private bool canLevelUp;
	
	private Vector3 _prevDirection = Vector3.zero;
	private Quaternion _prevRotation = Quaternion.identity;
	private Vector3 _direction = Vector3.zero;
	private Quaternion _rotation = Quaternion.identity;
	
	void Start(){
	}
	
	void Update()
	{		
		DoAction ();
	}

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
	
	void DoAction(){
		if (!isLocalPlayer)
			return;
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			currentIndex = 0;
			currentButton = spellButtons [0];
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
		
		if (Input.GetKeyDown (KeyCode.T)) {
			if (canLevelUp) {

			}
		}

		if (Input.GetKey (KeyCode.Z)) {
			ridgidbody.MovePosition (ridgidbody.position + Vector3.forward * rateSync * Time.deltaTime);
			Debug.LogWarning ("Z");
		}
		
		if (Input.GetKey(KeyCode.S))
			ridgidbody.MovePosition(ridgidbody.position - Vector3.forward * rateSync * Time.deltaTime);
		
		if (Input.GetKey(KeyCode.D))
			ridgidbody.MovePosition(ridgidbody.position + Vector3.right * rateSync * Time.deltaTime);
		
		if (Input.GetKey(KeyCode.A))
			ridgidbody.MovePosition(ridgidbody.position - Vector3.right * rateSync * Time.deltaTime);

		direction = Vector3.forward * (Input.GetAxisRaw("Run/Back") + (Input.GetMouseButton(0) && Input.GetMouseButton(1) ? 1:0));
		direction += Vector3.right * Input.GetAxisRaw("Left/Right");
		direction += Vector3.up * Input.GetAxisRaw("Jump");
		rotation = Quaternion.Euler(Input.GetAxis("Mouse Y") * (Input.GetMouseButton(0) ? 1 : 0), Input.GetAxis("Mouse X") * (Input.GetMouseButton(1) ? 1 : 0), 0);
	}
	
	void btnClicked(Button b){
		currentButton = b;
		currentIndex = Array.IndexOf (spellButtons, b);
		StartCoroutine(b1Timer());
	}
	
	IEnumerator b1Timer(){
		yield return StartCoroutine( changeColor() );
	}
	
	IEnumerator changeColor(){
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
	
	/// <summary>
	/// Tell if the controls changed from the last time this fuction was called.
	/// </summary>
	public bool hasChanged()
	{
		if(_prevDirection != _direction || _prevRotation != _rotation)
		{
			_prevDirection = _direction;
			_prevRotation = _rotation;
			return true;
		}
		return false;
	}
}