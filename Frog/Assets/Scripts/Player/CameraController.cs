using UnityEngine;

public class CameraController : MonoBehaviour
{
	private Camera cameraComponent;

	[SerializeField] private float mouseSensitivity;
	private Transform _target;

	public float targetHeight = 1.7f;
	public float Distance = 15.0f;
	public float offsetFromWall = 0.1f;

	public float maxDistance = 20;
	public float minDistance = .6f;
	public float speedDistance = 5;

	public float xSpeed = 200.0f;
	public float ySpeed = 200.0f;

	public int yMinLimit = -40;
	public int yMaxLimit = 80;

	public int zoomRate = 40;

	public float rotationDampening = 6.0f;
	public float zoomDampening = 5.0f;

	public LayerMask collisionLayers = -1;

	private float xDeg = 0.0f;
	private float yDeg = 0.0f;
	private float _currentDistance;
	private float _desiredDistance;
	private float _correctedDistance;

	private float lookSpeed = 5f;
	private float rotationX = 0f;

	private CameraShake cameraShake;

	private void Start()
	{
		cameraComponent = GetComponent<Camera>();
		cameraShake = GetComponent<CameraShake>();
		cameraComponent.enabled = true;

		Vector3 angles = transform.eulerAngles;
		xDeg = angles.x;
		yDeg = angles.y;

		

		_target = gameObject.transform.parent;

		var _boxCollider = _target.GetComponent<BoxCollider>();

		targetHeight = _boxCollider.bounds.size.y;


		_currentDistance = Distance;
		_desiredDistance = Distance;
		_correctedDistance = Distance;
	}

	private void Update()
	{
		if (cameraShake.IsShaking)
			return;

		HandleInput();
	}

	private void LateUpdate()
	{
		if (cameraShake.IsShaking)
			return;

		UpdateCameraPosition();
	}

	private void HandleInput()
	{
		if (GUIUtility.hotControl == 0)
		{
			if (Input.GetMouseButton(1))
			{
				Rotate();
				float targetRotationAngle = _target.eulerAngles.y;
				float currentRotationAngle = transform.eulerAngles.y;
				xDeg = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);
			}

			if (Input.GetMouseButton(0))
			{
				xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
				yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			}

			_desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(_desiredDistance) * speedDistance;
			_desiredDistance = Mathf.Clamp(_desiredDistance, minDistance, maxDistance);

			yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
		}
	}

	private void UpdateCameraPosition()
	{
		Quaternion rotation = Quaternion.Euler(yDeg, xDeg, 0);
		_correctedDistance = _desiredDistance;

		Vector3 vTargetOffset = new Vector3(0, -targetHeight, 0);
		Vector3 position = _target.position - (rotation * Vector3.forward * _desiredDistance + vTargetOffset);

		RaycastHit collisionHit;
		Vector3 trueTargetPosition = new Vector3(_target.position.x, _target.position.y, _target.position.z) - vTargetOffset;

		if (Physics.Linecast(trueTargetPosition, position, out collisionHit, collisionLayers.value))
		{
			_correctedDistance = Vector3.Distance(trueTargetPosition, collisionHit.point) - offsetFromWall;
		}

		_currentDistance = Mathf.Lerp(_currentDistance, _correctedDistance, Time.deltaTime * zoomDampening);

		position = _target.position - (rotation * Vector3.forward * _currentDistance + vTargetOffset);

		transform.rotation = rotation;
		transform.position = position;
	}

	private void Rotate()
	{
		rotationX += Input.GetAxis("Mouse X") * lookSpeed;
		transform.parent.localRotation = Quaternion.Euler(transform.parent.rotation.y, rotationX, 0f);
	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp(angle, min, max);
	}
}
