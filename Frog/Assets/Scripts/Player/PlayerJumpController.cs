using Assets.Scripts;
using Assets.Scripts.Enums;
using UnityEngine;

public class PlayerJumpController : MonoBehaviour
{
	public Rigidbody rb;

	public float chargeRate = 33.33f;

	public float maxCharge = 100f;

	public LayerMask groundLayer;

	public float raycastDistance = 2f;

	public float rotationSpeed = 5f;

	private float currentCharge = 0f;

	private PowerStatusUI powerStatusUI;

	private Animator animator;

	private PlayerController playerController;

	private PlayerAbillityManager playerAbillityManager;

	private CameraShake cam;

	private bool touchTheGround = true;

	private LineRenderer lineRenderer;

	private const int N_TRAJECTORY_POINTS = 10;

	private float previousCharge = 0f;

	public float lineIncreaseSpeed = 5f;

	void Start()
	{
		playerAbillityManager = GetComponent<PlayerAbillityManager>();

		cam = GetComponentInChildren<CameraShake>();

		playerController = GetComponent<PlayerController>();

		animator = GetComponent<Animator>();

		powerStatusUI = FindObjectOfType<PowerStatusUI>();

		SetupLineRenderer();

		SetupRigidbody();

	}

	private void SetupLineRenderer()
	{
		lineRenderer = GetComponentInChildren<LineRenderer>();

		lineRenderer.positionCount = N_TRAJECTORY_POINTS;
	}

	private void SetupRigidbody()
	{
		rb = GetComponent<Rigidbody>();

		rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
	}

	void Update()
	{
		if (playerController.IsFreezed)
		{
			return;
		}

		var onGround = playerController.AboveGround() == false;

		if (Input.GetKey(KeyCode.Space) && onGround)
		{
			PrepareJump();
		}

		if (Input.GetKeyUp(KeyCode.Space) && onGround)
		{
			Jump();
		}

		UpdateAnimation();

		HandleInAir();

		if (!playerController.IsGrounded() && touchTheGround)
		{
			touchTheGround = false;
		}

		HandleOnLanding();
	}

	public void TriggerLandingEffect(float latestCharge)
	{
		cam.TriggerShake(latestCharge);
	}

	private void Jump()
	{
		lineRenderer.enabled = false;

		lineRenderer.SetPosition(0, transform.position);

		playerAbillityManager.OnEnableAbillities_PlayerAbillityManager();

		DoJump();

		currentCharge = 0f;

		powerStatusUI.SetChargeUI(GetChargeSpeedUI());
	}

	private void DoJump()
	{
		var shotDirection = transform.up + transform.forward;

		var velocity = shotDirection.normalized * (currentCharge / 2);

		rb.AddForce(velocity, ForceMode.Impulse);
		
	}

	private void PrepareJump()
	{
		lineRenderer.enabled = true;

		ChargeShot();

		powerStatusUI.SetChargeUI(currentCharge);
	}

	private float GetChargeSpeedUI()
	{
		return Mathf.Clamp(currentCharge, 0, maxCharge);
	}

	private void HandleOnLanding()
	{
		if (playerController.IsGrounded() && !touchTheGround)
		{
			Debug.Log("disabling abilites");

			playerAbillityManager.SetDisableAbillities();

			touchTheGround = true;

			TriggerLandingEffect(previousCharge);
		}
	}

	private void HandleInAir()
	{
		if (playerController.AboveGround() == false)
			return;

		rb.constraints = RigidbodyConstraints.FreezeRotation;

		playerController.SpawnMovementParticlesUI();

		if (Input.GetKey(KeyCode.F) && playerAbillityManager.CanUseAbillitties)
		{
			playerAbillityManager.UseAbillity(AbillityType.RocketLauncher);
		}

		if (Input.GetKey(KeyCode.G) && playerAbillityManager.CanUseAbillitties)
		{
			playerAbillityManager.UseAbillity(AbillityType.Balloon);
		}
	}

	private bool StandingStill()
	{
		return rb.velocity.magnitude == 0;
	}

	private void ChargeShot()
	{
		var isPositiveCharge = currentCharge >= previousCharge;

		previousCharge = currentCharge;

		if (currentCharge < maxCharge && isPositiveCharge)
		{
			currentCharge += chargeRate * (Time.deltaTime * 1.5f);
			UpdateLineRenderer();
			return;
		}

		currentCharge -= chargeRate * (Time.deltaTime * 1.5f);

		if (currentCharge >= 100 || currentCharge <= 0)
		{
			currentCharge = 0;
			previousCharge = 0;
		}
	}

	private void UpdateAnimation()
	{
		animator.SetFloat("ChargeSpeed", GetChargeSpeedUI());
	}

	private void UpdateLineRenderer()
	{
		Vector3 startPos = transform.position;
		Vector3 velocity = (transform.up + transform.forward).normalized * (currentCharge / 2);
		float timeStep = 0.1f;

		for (int i = 0; i < N_TRAJECTORY_POINTS; i++)
		{
			float t = i * timeStep;
			Vector3 point = startPos + velocity * t + 0.5f * Physics.gravity * t * t;
			lineRenderer.SetPosition(i, point);
		}
	}
}
