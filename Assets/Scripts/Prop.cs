using UnityEngine;

public class Prop : MonoBehaviour
{
	[SerializeField] private string propName;
	[TextArea(3, 10)] [SerializeField] private string description;

	private PropDescManager _propDescManager;

	private void Start()
	{
		_propDescManager = FindObjectOfType<PropDescManager>();
	}

	public void Grab()
	{
		// Debug.Log("Grabbed");
		_propDescManager.GrabProp(this, propName, description);
	}

	public void Release()
	{
		// Debug.Log("Released");
		_propDescManager.DisablePropDesc();
	}
}
