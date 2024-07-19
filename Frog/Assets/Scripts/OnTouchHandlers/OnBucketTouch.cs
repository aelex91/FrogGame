using UnityEngine;

public class OnBucketTouch : TouchableObject
{

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame


	protected override void OnCollision(Collision collision)
	{
		PlayerController.SetOnCompleteLevel();
		base.OnCollision(collision);
	}

}
