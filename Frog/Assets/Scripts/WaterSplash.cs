using UnityEngine;

public class WaterSplash : MonoBehaviour
{
	public MeshFilter meshFilter;
	public float rippleStrength = 1f;
	public float rippleSpeed = 1f;
	private Vector3[] originalVertices;
	private Vector3[] displacedVertices;

	void Start()
	{
		meshFilter = GetComponent<MeshFilter>();
		// Store the original vertices of the mesh
		originalVertices = meshFilter.mesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];
		originalVertices.CopyTo(displacedVertices, 0);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			// Get the impact point
			Vector3 impactPoint = collision.transform.position;
			StartCoroutine(CreateRipple(impactPoint));
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			// Get the impact point
			Vector3 impactPoint = other.transform.position;
			StartCoroutine(CreateRipple(impactPoint));
		}
	}

	System.Collections.IEnumerator CreateRipple(Vector3 impactPoint)
	{
		float time = 0f;

		while (time < rippleSpeed)
		{
			time += Time.deltaTime;
			float waveHeight = rippleStrength * Mathf.Sin(time * Mathf.PI * 2f);

			for (int i = 0; i < displacedVertices.Length; i++)
			{
				Vector3 vertex = originalVertices[i];
				float distance = Vector3.Distance(vertex, impactPoint);

				// Adjust the vertex y position based on the distance from the impact point
				displacedVertices[i].y = vertex.y + waveHeight * Mathf.Exp(-distance);
			}

			meshFilter.mesh.vertices = displacedVertices;
			meshFilter.mesh.RecalculateNormals();
			yield return null;
		}

		// Reset the mesh to its original state
		originalVertices.CopyTo(displacedVertices, 0);
		meshFilter.mesh.vertices = displacedVertices;
		meshFilter.mesh.RecalculateNormals();
	}
}
