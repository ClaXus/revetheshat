using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManagerFastGame : MonoBehaviour {

	[SerializeField]
	GameObject[] GameUIDiscoveredOnF;

	[SerializeField]
	Player pcs;

	[SerializeField]
	Text timerText;

	[SerializeField]
	Button panelButtons;

	
	[SerializeField]
	Button[] buttonsPanelButtons;

	[SerializeField]
	Camera placementCamera;

	[SerializeField]
	GameObject myPlayerPosition;

	[SerializeField]
	GameObject myPlayer;

	[SerializeField]
	Text messageText;

	private float targetTime = 2f;

	int TimeToPlace;

	public enum GamesState{
		PlacementState,
		ActionState,
		PauseState
	}

	public int currentState;

	// Use this for initialization
	void Start () {
		currentState = (int)GamesState.PlacementState;
		hideOrShowGameUiOnF ();
		panelButtons.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		if (currentState == (int)GamesState.ActionState) {
			if (Input.GetKeyDown (KeyCode.F)) {
				hideOrShowGameUiOnF ();
			}
			if (Input.GetKeyDown (KeyCode.T)) {
			
			}
		}
		if (currentState == (int)GamesState.PlacementState) {
			if (targetTime <= 0.0f) {
				timerEnded ();
			} else {
				targetTime -= Time.deltaTime;
				timerText.text = targetTime.ToString ("0.##");
			}
		}		
	}

	void timerEnded(){
		timerText.gameObject.SetActive (false);
		currentState = (int)GamesState.ActionState;
		hideOrShowGameUiOnF ();
		panelButtons.gameObject.SetActive (true);
		placementCamera.gameObject.SetActive (false);
		messageText.text = "Défendez le totem !";
	}

	void hideOrShowGameUiOnF(){
		for (int i=0; GameUIDiscoveredOnF.Length>i; i++) {
			if(!GameUIDiscoveredOnF[i].activeSelf)
				GameUIDiscoveredOnF[i].SetActive (true);
			else
				GameUIDiscoveredOnF[i].SetActive (false);
		}
	}

	public void initializeButtons(ref Button[] spellButtons, Player p){
		pcs = p;
		for (int i=0;i<spellButtons.Length && i< buttonsPanelButtons.Length; i++) {
			AddListener(buttonsPanelButtons[i], "init");
			spellButtons[i] = buttonsPanelButtons[i]; 
		}

	}
	void AddListener(Button b, string value)
	{
		b.onClick.AddListener(() => pcs.btnClicked(b));
	}
}