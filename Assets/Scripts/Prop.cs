using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Prop : MonoBehaviour
{
	[SerializeField] private string propName;
	[TextArea(3, 10)] [SerializeField] private string description;
	private GameObject _descriptionPanel;
	private GameObject _descriptionPanelPrefab;
	private XRGrabInteractable _grabInteractable;

	private Rigidbody _rigidbody;

	private Vector3 _startPosition;

	private void Start()
	{
		_startPosition = transform.position;
		_rigidbody = GetComponent<Rigidbody>();

		_grabInteractable = GetComponent<XRGrabInteractable>();
		if (_grabInteractable == null)
		{
			Debug.LogError($"Prop {propName} does not have an XRGrabInteractable component attached to it.");
		}
		else
		{
			_grabInteractable.selectEntered.AddListener(Grab);
			_grabInteractable.selectExited.AddListener(Release);
			_grabInteractable.movementType = XRBaseInteractable.MovementType.VelocityTracking;
		}

		_descriptionPanelPrefab = Resources.Load<GameObject>("Prop Description Panel");
	}

	private void OnTriggerExit(Collider other)
	{
		Debug.Log("Exited collision " + other.gameObject.name);
		if (other.gameObject.CompareTag("Resetter"))
		{
			_rigidbody.position = _startPosition;
			_rigidbody.velocity = Vector3.zero;
			_rigidbody.rotation = Quaternion.identity;
			_rigidbody.angularVelocity = Vector3.zero;
		}
	}

	private void Grab(SelectEnterEventArgs selectEnterEventArgs)
	{
		// Debug.Log("Grabbed");
		SpawnPanel(selectEnterEventArgs.interactorObject.transform.gameObject.GetNamedChild("Panel Origin").transform);
	}

	private void SpawnPanel(Transform parent)
	{
		if (_descriptionPanel != null) return;

		_descriptionPanel = Instantiate(_descriptionPanelPrefab, parent);
		_descriptionPanel.GetComponent<Canvas>().worldCamera = Camera.main;

		//put in the right strings into the text objects
		List<TextMeshProUGUI> texts = _descriptionPanel.GetComponentsInChildren<TextMeshProUGUI>().ToList();
		texts.ForEach(text =>
		{
			text.text = text.text switch
			{
				{ } a when a.IndexOf("name", StringComparison.OrdinalIgnoreCase) >= 0 => propName,
				{ } a when a.IndexOf("description", StringComparison.OrdinalIgnoreCase) >= 0 => description,
				_ => text.text,
			};
		});
	}

	private void Release(SelectExitEventArgs selectExitEventArgs)
	{
		// Debug.Log("Released");
		Destroy(_descriptionPanel);
	}
}
