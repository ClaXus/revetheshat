using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PlayerControlScript : ControlScript
{
	[SerializeField]
	Button[] spellButtons;

	[SerializeField]
	GameManagerFastGame gm;

	[SerializeField]
	Spell[] associateSpells;

	[SerializeField]
	JobPattern[] associateJobs;

	private bool isRunning = false;
	private Button currentButton;
	private int currentIndex;
	private bool canLevelUp;

    void Start(){

    }

    void Update()
    {		
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			currentIndex = 0;
			currentButton = spellButtons[0];
			StartCoroutine(b1Timer());
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			currentIndex = 1;
			currentButton = spellButtons[1];
			StartCoroutine(b1Timer());
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			currentIndex = 2;
			currentButton = spellButtons[0];
			StartCoroutine(b1Timer());
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			currentIndex = 3;
			currentButton = spellButtons[3];
			StartCoroutine(b1Timer());
		}

		if (Input.GetKeyDown (KeyCode.A)) {
			currentIndex = 6;
			currentButton = spellButtons[4];
			StartCoroutine(b1Timer());
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			currentIndex = 7;
			currentButton = spellButtons[5];
			StartCoroutine(b1Timer());
		}

		if (Input.GetKeyDown (KeyCode.T)) {
			if(canLevelUp){
				
			
			}

		
		}

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


}