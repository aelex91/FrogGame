using UnityEngine;

namespace Assets.Scripts.OnTouchHandlers
{
	public class OnLeafTouch : TouchableObject
	{
		private MeshRenderer mesh;

		public Color onTouchColor = Color.red;

		private Color onLeaveColor;

		protected override void Start()
		{
			base.Start();

			mesh = GetComponentInChildren<MeshRenderer>();

			onLeaveColor = mesh.material.color;
		}
		protected override void OnCollision(Collision collision)
		{
			mesh.material.color = onTouchColor;

			base.OnCollision(collision);
		}

		protected override void OnLeave(Collision collision)
		{
			mesh.material.color = onLeaveColor;

			base.OnLeave(collision);
		}
	}
}
