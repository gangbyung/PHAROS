using UnityEngine;
using System.Collections;

public class Footsteps_VFX : MonoBehaviour {

	public GameObject leftFootprintPrefab;
	public GameObject rightFootprintPrefab;
	public Transform leftFootPosition;
	public Transform rightFootPosition;


	void LeftFootSplash ()
	{

		Instantiate(leftFootprintPrefab, leftFootPosition.position, leftFootPosition.rotation);

	}




	void RightFootSplash ()
	{
	
		Instantiate(rightFootprintPrefab, rightFootPosition.position, rightFootPosition.rotation);

	}

}


