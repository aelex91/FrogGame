using Assets.Scripts.Enums;
using UnityEngine;

public class OnAbillityTouch : TouchableObject
{
	private MeshRenderer mesh;

	public Color onTouchColor = Color.red;

	private Color onLeaveColor;

	[SerializeField]
	private AbillityType abillityType;	


	protected override void Start()
	{
		base.Start();

		mesh = GetComponentInChildren<MeshRenderer>();

		onLeaveColor = mesh.material.color;
	}
	protected override void OnCollision(Collision collision)
	{
		base.OnCollision(collision);

		mesh.material.color = onTouchColor;

		PlayerController.SetAbillity(abillityType);
		
	}

	protected override void OnLeave(Collision collision)
	{
		mesh.material.color = onLeaveColor;

		base.OnLeave(collision);
	}
}
