using UnityEngine;

namespace Assets.Scripts
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField]
		public GameObject PlayerMovementParticle;

		[SerializeField]
		public GameObject Balloon;

		[SerializeField]
		public GameObject LeafParticle;

		private LevelManager currentLevelManager;

		public static GameManager Instance;

		private void Awake()
		{
			if (Instance == null)
				Instance = this;
		}

		private void Start()
		{
			if (Camera.main != null)
				Camera.main.gameObject.SetActive(false);
		}

		public void SetCurrentLevelManager(LevelManager levelManager)
		{
			currentLevelManager = levelManager;
		}	
	}
}
