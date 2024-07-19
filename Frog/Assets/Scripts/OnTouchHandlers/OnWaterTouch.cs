using UnityEngine;

namespace Assets.Scripts.OnTouchHandlers
{
	public class OnWaterTouch : TouchableObject
	{
		protected override void OnCollision(Collision collision)
		{
			PlayerController.TeleportPlayerToStartWithDelay();
		}
	}
}
