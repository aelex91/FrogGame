using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public Transform cameraTransform;
	public float shakeDuration = 0.5f;
	private const float DEFAULT_MAGNITUDE = 0.2F;
	public float shakeMagnitude = 0.2f;

	private Vector3 originalPosition;
	private float currentShakeTime = 0f;
	public bool IsShaking;

	private CameraController cameraFollowPlayer;

	void Start()
	{
		cameraFollowPlayer = GetComponent<CameraController>();
		if (cameraTransform == null)
		{
			cameraTransform = GetComponent<Camera>().transform;
		}

		originalPosition = cameraTransform.localPosition;
	}

	void Update()
	{
		if (currentShakeTime > 0)
		{
			cameraTransform.localPosition = cameraFollowPlayer.transform.localPosition + Random.insideUnitSphere * shakeMagnitude;
			currentShakeTime -= Time.deltaTime;
			IsShaking = true;
			return;
		}

		IsShaking = false;
	}

	public void TriggerShake(float? magnitude = null)
	{

		if (magnitude.HasValue == false)
		{
			currentShakeTime = DEFAULT_MAGNITUDE;
			return;

		}
		currentShakeTime = magnitude.GetValueOrDefault() / 100;
	}
}
