using System;
using UnityEngine;
using UnityEngine.UI;

public class SpellUI : MonoBehaviour
{
	private PlayerAbillityManager _playerController;

	[SerializeField]
	private Image _image;

	private Color _defaultColor;

	private GameObject[] Images;

	void Start()
	{
		Images = GameObject.FindGameObjectsWithTag("AbillityUI");

		_playerController = GameObject.FindWithTag("Player").GetComponent<PlayerAbillityManager>();

		_playerController.OnDisableAbillities += PlayerController_OnDisableAbillities;
		_playerController.OnEnableAbillities += PlayerController_OnEnableAbillities;

		if (_image == null)
			throw new NullReferenceException("Image is null cant continue.");

		_defaultColor = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a);

		Disable();
	}

	private void PlayerController_OnEnableAbillities()
	{
		foreach (var image in Images)
		{
			image.GetComponent<Image>().color = new Color(_defaultColor.r, _defaultColor.g, _defaultColor.b, _defaultColor.a);
		}
	}

	private void PlayerController_OnDisableAbillities()
	{
		Disable();
	}

	private void Disable()
	{
		foreach (var image in Images)
		{
			image.GetComponent<Image>().color = Color.red;
		}
	}
}
