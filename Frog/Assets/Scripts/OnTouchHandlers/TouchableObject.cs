using Assets.Scripts;
using UnityEngine;

public abstract class TouchableObject : MonoBehaviour
{
	protected PlayerAbillityManager PlayerAbillity;
	protected PlayerController PlayerController;

	protected virtual void Start()
	{
		PlayerAbillity = GameObject.FindWithTag("Player").GetComponent<PlayerAbillityManager>();
		PlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
	}

	protected virtual void OnCollision(Collision collision) {
	
	}
	protected virtual void OnLeave(Collision collision) {
	
	}

	public void OnCollisionEnter(Collision collision)
	{
		if (PlayerController.IsFreezed)
			return;

		OnCollision(collision);
	}

	private void OnCollisionExit(Collision collision)
	{
		if (PlayerController.IsFreezed)
			return;

		OnLeave(collision);
	}
}