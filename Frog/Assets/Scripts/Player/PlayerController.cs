using Assets.Scripts.Enums;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{

	public class PlayerController : MonoBehaviour
	{
		private Vector3 startPosition;

		private Quaternion startRotation;

		private float spawnCloudMaxTime = 0.2f;

		private float destroyAfterTime = 1f;

		private float spawnCloudTime;

		private BoxCollider _boxCollider;

		private bool hasFinishedLevel = false;

		public bool IsFreezed;

		public event Action OnCompleteLevel;

		private void Start()
		{
			startPosition = transform.position;

			startRotation = transform.rotation;

			_boxCollider = GetComponent<BoxCollider>();
		}

		public void SpawnMovementParticlesUI()
		{
			spawnCloudTime += Time.deltaTime;

			if (spawnCloudTime < UnityEngine.Random.Range(spawnCloudMaxTime - 0.1f, spawnCloudMaxTime))
				return;

			var movementParticle = Instantiate(GameManager.Instance.PlayerMovementParticle, transform, true);

			movementParticle.transform.localPosition = new Vector3(0, -1);

			Destroy(movementParticle, destroyAfterTime);

			spawnCloudTime = 0;
		}

		public bool AboveGround()
		{
			return IsGrounded() == false;
		}

		public bool IsGrounded()
		{
			var boxSizeY = _boxCollider.bounds.size.y;

			if (Physics.Raycast(transform.position, Vector3.down, out var hit, boxSizeY))
			{
				return true;
			}

			return false;
		}

		public void SetOnCompleteLevel()
		{
			hasFinishedLevel = true;
			OnCompleteLevel?.Invoke();
		}

		public void TeleportPlayerToStartWithDelay()
		{

			if (hasFinishedLevel)
				return;

			StartCoroutine(MoveTowardsStartPosition());
		}
	
		private IEnumerator MoveTowardsStartPosition()
		{
			IsFreezed = true;

			const float moveTowardsSpeed = 100f;

			while (Vector3.Distance(transform.position, startPosition) > 0.01f)
			{
				transform.position = Vector3.MoveTowards(transform.position, startPosition, moveTowardsSpeed * Time.deltaTime);
				yield return null;
			}

			transform.position = startPosition;
			transform.rotation = startRotation;
			//rb.useGravity = true;
			yield return new WaitForSeconds(0.5f);


			IsFreezed = false;
		}

		internal void SetAbillity(AbillityType abillity)
		{
			//todo add to list?
			
		}
		void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.CompareTag("Leaf"))
			{
				Debug.Log("collision parent");
				// Set the object as a child of the plane
				transform.SetParent(collision.transform);
			}
		}

		void OnCollisionExit(Collision collision)
		{
			if (collision.gameObject.CompareTag("Leaf"))
			{
				Debug.Log("OnCollisionExit parent");

				// Remove the parent relationship when the object leaves the plane
				transform.SetParent(null);
			}
		}


	}
}
