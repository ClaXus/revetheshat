using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class OptionScreenManager : MonoBehaviour {
		
	[SerializeField]
	Text textResolution;

	[SerializeField]
	Text textFullScreen;

	[SerializeField]
	AudioSource[] audioSources;
	private static KeyValuePair<int, int>[] resolutions = new KeyValuePair<int, int>[]{
		new KeyValuePair<int, int>(800, 600), 
		new KeyValuePair<int, int>(1024, 600),  
		new KeyValuePair<int, int>(1024, 768), 
		new KeyValuePair<int, int>(1200, 800),
		new KeyValuePair<int, int>(1366, 768), 
		new KeyValuePair<int, int>(1440, 900),  
		new KeyValuePair<int, int>(1600, 900), 
		new KeyValuePair<int, int>(1680, 1050), 
		new KeyValuePair<int, int>(1920, 1080)
	};

	private string tag=""; 
	private string text;

	// Use this for initialization
	void Start () {
		if(Screen.fullScreen)
			textFullScreen.text = "Fenêtré";
		tag = Screen.width + "x" + Screen.height;
		text = textResolution.text;
		textResolution.text = text + tag; 
	}

	public void changeResolution(int number){
			tag = Screen.width + "x" + Screen.height;
			textResolution.text = text + tag;
			Screen.SetResolution (resolutions [number].Key, resolutions [number].Value, Screen.fullScreen);
	}

	public void changeFullScreenMode(){
		if (textFullScreen.text.Equals ("Plein écran")) {
			Screen.SetResolution(Screen.width,Screen.height,true);
			textFullScreen.text = "Fenêtré";
		} else {
			Screen.SetResolution(Screen.width,Screen.height,false);
			textFullScreen.text = "Plein écran";
		}
	}

	void Update(){
		tag = Screen.width + "x" + Screen.height;
		textResolution.text = text + tag;
	}

	public void changeSoundActivation(){
		foreach (AudioSource aS in audioSources) {
			if(aS.gameObject.activeSelf)
				aS.gameObject.SetActive(false);
			else
				aS.gameObject.SetActive(true);
		}
	}
}
