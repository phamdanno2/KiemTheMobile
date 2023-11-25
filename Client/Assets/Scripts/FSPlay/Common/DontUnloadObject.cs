using UnityEngine;

public class DontUnloadObject : TTMonoBehaviour
{
	public bool IntantDisable = false;
	
	// Use this for initialization
	private void Start()
	{
		GameObject.DontDestroyOnLoad(this.gameObject);
		if (this.IntantDisable)
		{
			this.gameObject.SetActive(false);
		}
	}
}
