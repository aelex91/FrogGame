using System;
using UnityEngine;

namespace Assets.Scripts
{
	public class Floater : MonoBehaviour
	{
		public Rigidbody Rigidbody;
		public float DepthBeforeSubmerged = 1f;
		public float DisplacementAmount = 3f;
		public int FloaterCount = 1;
		public float WaterDrag = 0.99f;
		public float WaterAngularDrag = 0.5f;

		private void Start()
		{
			Rigidbody = GetComponent<Rigidbody>();

			if(Rigidbody == null ) {
				throw new NullReferenceException("cant find rigidbody on gameobject!");
			}
		}

		private void FixedUpdate()
		{
			Rigidbody.AddForceAtPosition(Physics.gravity / FloaterCount, transform.position, ForceMode.Acceleration);

			float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);
			
			if(transform.position.y < waveHeight)
			{
				var displacementMultiplier = Mathf.Clamp01(waveHeight -transform.position.y / DepthBeforeSubmerged) * DisplacementAmount;
				Rigidbody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
				Rigidbody.AddTorque(displacementMultiplier * -Rigidbody.angularVelocity * WaterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
			}
		}
	}
}
