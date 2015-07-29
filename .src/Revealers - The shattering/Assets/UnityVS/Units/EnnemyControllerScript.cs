using UnityEngine;
using System.Collections;

public class EnnemyControllerScript : ControlScript
{
    [SerializeField]
	NetworkView networkView;

    [SerializeField]
    private Unit thisControlled;
	
	[SerializeField]
	private Collider Collider;

	[SerializeField]
	private Collider targetCollider;

	Vector3 startPosition;

	bool playerHit = false;

	RaycastHit hit;


    void Start()
    { 
		print ("an ennemy START");
		startPosition = gameObject.transform.position;
    }

    void Update() 
    {
		transform.position = Vector3.MoveTowards(transform.position, targetCollider.transform.position, (thisControlled.Speed/5f)* Time.deltaTime);
		playerHit = Physics.SphereCast (transform.position, 1, transform.forward, out hit);

		if (playerHit && hit.collider.tag.Equals("Player")) 
		{
			gameObject.transform.position = startPosition;
			gameObject.SetActive (false);
		}
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other == targetCollider) 
		{
			gameObject.transform.position = startPosition;
			gameObject.SetActive (false);
		} 
	}

}