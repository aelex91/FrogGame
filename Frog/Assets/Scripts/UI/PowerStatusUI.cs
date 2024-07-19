using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerStatusUI : MonoBehaviour
{	
	private TextMeshProUGUI textMeshProUGUI;
	private PlayerAbillityManager _playerAbillityManager;

	[SerializeField]
	private Image energyImage;
	// Start is called before the first frame update


	void Start()
	{
		textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();

		_playerAbillityManager = GameObject.FindWithTag("Player").GetComponent<PlayerAbillityManager>();

		_playerAbillityManager.OnDisableAbillities += PlayerController_OnJump;

	}



	private void PlayerController_OnJump()
	{

	}

	public void SetChargeUI(float value)
	{
		var uiFriendly = (int)value;

		textMeshProUGUI.text = uiFriendly.ToString();

		var fillAmount = (value / 100);

		energyImage.color = GetColorFromValue(uiFriendly);

		energyImage.fillAmount = fillAmount;
	}

	private Color GetColorFromValue(int uiFriendly)
	{

		uiFriendly = Mathf.Clamp(uiFriendly, 0, 100);

		// Calculate the percentage
		var percentage = uiFriendly / 100f;

		if (percentage < 0.5f)
			return Color.Lerp(Color.green, Color.yellow, percentage * 2);

		return Color.Lerp(Color.yellow, Color.red, (percentage - 0.5f) * 2);

	}
}
