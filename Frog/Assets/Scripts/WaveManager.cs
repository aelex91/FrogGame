using UnityEngine;

public class WaveManager : MonoBehaviour
{
	public static WaveManager instance;

	public float Amplitude = 1f;
	public float Length = 2f;
	public float Speed = 1f;
	public float Offset = 0f;

	// Start is called before the first frame update
	void Start()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
		{
			Debug.Log("Instance already exists, destroying object!");
			Destroy(this);
		}
	}

	// Update is called once per frame
	void Update()
	{
		Offset += Time.deltaTime * Speed;
	}

	public float GetWaveHeight(float _x)
	{
		return Amplitude * Mathf.Sin(_x / Length * Offset);
	}

}
