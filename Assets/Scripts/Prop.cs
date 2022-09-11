using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
	[SerializeField] private string propName;
	[SerializeField] [TextArea(3, 10)] private string description;

	[SerializeField] public List<Question> questions;

	private PropDescManager _propDescManager;

	private void Start()
	{
		_propDescManager = FindObjectOfType<PropDescManager>();

		foreach (Question question in questions) question.Init(this);
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
