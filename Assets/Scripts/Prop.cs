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
	[SerializeField] private AudioClip collisionSound;
	[Range(1.0f, 1.5f)] [SerializeField] private float outlineScale = 1.1f;
	private readonly Color _outlineColor = new(0.57f, 0.79f, 0.91f);

	public delegate void OnResetEvent();

	public event OnResetEvent OnReset;

	private GameObject _descriptionPanel;
	private GameObject _descriptionPanelPrefab;
	private XRGrabInteractable _grabInteractable;

	private Rigidbody _rigidbody;

	private Vector3 _startPosition;
	private Quaternion _startRotation;

	private Outline _outline;
	private bool _grabbed;

	[HideInInspector] public bool insideFetchZone;

	private void Start()
	{
		Transform t = transform;
		_startPosition = t.position;
		_startRotation = t.rotation;
		_rigidbody = GetComponent<Rigidbody>();

		_grabInteractable = GetComponent<XRGrabInteractable>();
		if (_grabInteractable == null)
		{
			Debug.LogError($"Prop {propName} does not have an XRGrabInteractable component attached to it.");
		}
		else
		{
			_grabInteractable.selectEntered.AddListener(XRGrab);
			_grabInteractable.selectExited.AddListener(XRRelease);
			_grabInteractable.movementType = XRBaseInteractable.MovementType.VelocityTracking;
		}

		//Respawn the prop if it falls out of the world
		OnReset += Respawn;

		_descriptionPanelPrefab = Resources.Load<GameObject>("Prop Description Panel");

		_outline = new Outline(gameObject, Resources.Load<Material>("Outline"), outlineScale, _outlineColor);
		_outline.Disable();
	}

	private void OnMouseEnter()
	{
		if (_grabbed) return;
		_outline.Enable();
	}

	private void OnMouseExit()
	{
		_outline.Disable();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (Time.timeSinceLevelLoad > 2.0f) //wait until everything has settled in, before allowing to make interaction noises
			if (collisionSound != null)
				AudioSource.PlayClipAtPoint(collisionSound, transform.position, collision.relativeVelocity.magnitude);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Fetch Zone")) insideFetchZone = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Resetter")) OnReset?.Invoke();

		if (other.CompareTag("Fetch Zone")) insideFetchZone = false;
	}

	private void Respawn()
	{
		_outline.Disable();
		_rigidbody.position = _startPosition;
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.rotation = _startRotation;
		_rigidbody.angularVelocity = Vector3.zero;
	}

	private void XRGrab(SelectEnterEventArgs selectEnterEventArgs)
	{
		Grab(selectEnterEventArgs.interactorObject.transform.gameObject.GetNamedChild("Panel Origin").transform);
	}

	public void Grab(Transform parent)
	{
		_grabbed = true;
		_outline.Disable();
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

	private void XRRelease(SelectExitEventArgs selectExitEventArgs)
	{
		Release();
	}

	public void Release()
	{
		_grabbed = false;
		_outline.Disable();
		Destroy(_descriptionPanel);
	}
}
