using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAround : MonoBehaviour
{
	[SerializeField]
	private float floatSpeed = 1f;

	[SerializeField]
	private float radiusToMoveAround = 5f;

	private Vector3 initialPosition;

	// Start is called before the first frame update
	void Start()
	{
		initialPosition = transform.position;
		StartCoroutine(MoveAround());
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red; // Set the color of the Gizmo
		Gizmos.DrawWireSphere(transform.position, radiusToMoveAround); // Draw a wireframe sphere at the GameObject's position with the specified radius
	}

	// Coroutine to move the object around
	private IEnumerator MoveAround()
	{
		while (true)
		{
			// Calculate a random point within the radius
			Vector3 randomPoint = Random.insideUnitSphere * radiusToMoveAround;
			randomPoint += initialPosition;

			// Ensure the random point is on the same horizontal plane
			randomPoint.y = transform.position.y;

			// Move towards the target point
			while (Vector3.Distance(transform.position, randomPoint) > 0.1f)
			{
				transform.position = Vector3.MoveTowards(transform.position, randomPoint, floatSpeed * Time.deltaTime);
				yield return null;
			}
		}
	}
}
