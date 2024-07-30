using Assets.Scripts;
using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerAbillityManager : MonoBehaviour
{
	public event Action OnDisableAbillities;

	public event Action OnDisableCharge;

	public event Action OnEnableAbillities;

	public bool CanUseAbillitties = true;

	private bool isFalling = false;

	private CameraShake camera;

	private float abillityPushForce = 25;

	private float slowDownRate = 1f;

	private Rigidbody rigidbody;

	private PlayerController _playerController;

	public float moonGravityScale = 1.0f / 6.0f;

	public List<AbillityType> AbillityTypes = new List<AbillityType>();

	private void Start()
	{
		camera = GetComponentInChildren<CameraShake>();

		rigidbody = GetComponent<Rigidbody>();

		_playerController = GetComponent<PlayerController>();
	}

	private void Update()
	{

		if (isFalling)
		{
			Vector3 moonGravity = Physics.gravity * moonGravityScale;
			rigidbody.AddForce(moonGravity - Physics.gravity, ForceMode.Acceleration);
		}
	}

	public void OnEnableAbillities_PlayerAbillityManager()
	{
		StartCoroutine(EnableAfterSeconds(1));
	}

	private IEnumerator EnableAfterSeconds(float seconds)
	{
		yield return new WaitForSeconds(seconds);

		if (_playerController.IsGrounded() == false)
		{
			OnEnableAbillities?.Invoke();

			CanUseAbillitties = true;
		}
	}

	public void SetDisableAbillities()
	{
		CanUseAbillitties = false;

		OnDisableAbillities?.Invoke();
	}

	public void UseAbillity(AbillityType type)
	{
		SetDisableAbillities();

		switch (type)
		{
			case AbillityType.None:
				break;
			case AbillityType.RocketLauncher:
				{
					rigidbody.AddForce(transform.forward * abillityPushForce, ForceMode.Impulse);
					camera.TriggerShake();
					break;
				}
			case AbillityType.Balloon:
				{
					StartCoroutine(ActivateBalloon());
					break;
				}
			default:
				break;
		}
	}

	private IEnumerator ActivateBalloon()
	{

		var pPos = transform.position;
		var center = new Vector3(pPos.x, pPos.y, pPos.z + 2);

		var balloon = Instantiate(GameManager.Instance.Balloon, center + Vector3.up * 6f, Quaternion.identity, transform);

		var levitationTime = 2.5f;

		var elapsedTime = 0f;

		isFalling = true;

		while (elapsedTime < levitationTime)
		{
			//rigidbody.velocity = new Vector3(rigidbody.velocity.x, Mathf.Sin(elapsedTime * Mathf.PI / levitationTime) * slowDownRate, rigidbody.velocity.z);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		Destroy(balloon);

		isFalling = false;

	}


}