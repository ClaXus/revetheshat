using UnityEngine;
using System.Collections;

public class ScheduleMenu : MonoBehaviour {
	[SerializeField]
	Canvas[] relatingCanvas;
	// Use this for initialization
	void OnEnable () {
		for (int i=0; relatingCanvas.Length>i; i++) {	
			relatingCanvas[i].gameObject.SetActive (true);
		}
	}

	void OnDisable(){
		for (int i=0;relatingCanvas.Length>i;i++)
			relatingCanvas[i].gameObject.SetActive (false);
	}
}
