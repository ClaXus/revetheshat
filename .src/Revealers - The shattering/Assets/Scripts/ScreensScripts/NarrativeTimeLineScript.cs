using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NarrativeTimeLineScript : MonoBehaviour {

	/* This script is designed for a specific use: an image that appears at the bottom with scrolling text above
	 * Must be attached on a Canvas
	 * TextTimeline : the scrollingText
	 * speed : speed of scroll
	 * SpeedApparitionTImeline : speed of RawImageTimeline appearing 
	 * TimeOfAnimation : time of all the animation scrolling text
	 * GoToThisEvent : an element you want to put visible now
	 * 
	*/

	[SerializeField]
	private GameObject TextTimeline;

	[SerializeField]
	private int Speed=18;

	[SerializeField]
	private RawImage RawImageTimeline;
	
	[SerializeField]
	private int SpeedApparitionTimeline=15;
	
	[SerializeField]
	private float TimeOfAnimation;

	[SerializeField]
	private GameObject GoToThisEvent;

	private float currentTime=0f;


	void Start()
	{
		currentTime = Time.fixedTime;
	}

	void Update(){
		if (Input.GetKeyDown ("space")) 
		{
			ExitTimeline();
		}
	}

	void FixedUpdate () 
	{
		currentTime = Time.fixedTime;
		TextTimeline.transform.Translate(Vector3.up*Time.deltaTime*Speed);
		RawImageTimeline.color = new Color(RawImageTimeline.color.r,RawImageTimeline.color.g,RawImageTimeline.color.b,RawImageTimeline.color.a+(SpeedApparitionTimeline/10000f));
		if (currentTime > TimeOfAnimation) 
		{
			ExitTimeline();
		}
	}

	void ExitTimeline()
	{
		GoToThisEvent.gameObject.SetActive (true);
		gameObject.SetActive(false);
	}



}
