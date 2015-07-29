using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwitchScreenGui : MonoBehaviour {

	/* Made to be put on a Canvas and activate by a button
	*/
	public void SwitchCanvas(Canvas canvasOut)
	{
		canvasOut.gameObject.SetActive (true);
		gameObject.SetActive(false);
	}
}