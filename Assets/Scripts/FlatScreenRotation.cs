using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class FlatScreenRotation : MonoBehaviour
{
	[SerializeField] private float xSensitivity = 1f;
	[SerializeField] private float ySensitivity = 1f;
	[SerializeField] private float scrollSpeed = 0.1f;

	[SerializeField] private GameObject reticleCanvas;

	[SerializeField] private Transform heldPropLocation;
	[SerializeField] private Transform panelOrigin;

	private Rigidbody _heldPropRb;
	private Prop _heldPropProp;
	private bool _isHoldingProp;

	private void Start()
	{
		if (XRIsPresent())
		{
			enabled = false;
			reticleCanvas.SetActive(false);
			return;
		}

		//Disable VR camera rotation override
		TrackedPoseDriver trackedPoseDriver = GetComponent<TrackedPoseDriver>();
		trackedPoseDriver.enabled = false;

		//Loop through all controllers in the scene and disable them.
		foreach (XRRayInteractor xrRayInteractor in FindObjectsOfType<XRRayInteractor>()) xrRayInteractor.gameObject.SetActive(false);
	}

	private void Update()
	{
		Cursor.lockState = CursorLockMode.Locked;

		//TODO: Make this better, so it stops being able to rotate in a loop-de-loop
		transform.eulerAngles -= new Vector3(
			Input.GetAxis("Mouse Y") * ySensitivity,
			Input.GetAxis("Mouse X") * -xSensitivity,
			0);


		if (!_isHoldingProp)
		{
			//if not holding a prop

			//look for an object in front of the player
			if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
			{
				Prop prop = hit.transform.GetComponent<Prop>();
				if (!prop) return;
				//if the object is a prop, pick it up
				if (Input.GetMouseButtonDown(0)) GrabProp(prop, hit);
			}
		}
		else
		{
			//if holding a prop

			//on scroll, move the held prop target position closer or further away
			if (Input.mouseScrollDelta.y != 0) heldPropLocation.localPosition += new Vector3(0, 0, Input.mouseScrollDelta.y * scrollSpeed);

			//if a prop is held, move it to the heldPropLocation
			_heldPropRb.MovePosition(heldPropLocation.position);

			//if the player lets go of the prop, drop it
			if (Input.GetMouseButtonUp(0)) ReleaseProp();
		}
	}

	private void GrabProp(Prop prop, RaycastHit hit)
	{
		//variable setup
		_heldPropRb = hit.rigidbody;
		_heldPropProp = prop;
		_isHoldingProp = true;

		//disable physics
		hit.rigidbody.useGravity = false;

		//set target position to the right distance in front of the player
		heldPropLocation.localPosition = new Vector3(0, 0, hit.distance);

		prop.SpawnPanel(panelOrigin);
	}

	private void ReleaseProp()
	{
		//re-enable physics
		_heldPropRb.useGravity = true;

		_heldPropProp.DestroyPanel();

		//variable de-setup
		_isHoldingProp = false;
		_heldPropRb = null;
		_heldPropProp = null;
	}

	private static bool XRIsPresent()
	{
		List<XRDisplaySubsystem> xrDisplaySubsystems = new();
		SubsystemManager.GetInstances(xrDisplaySubsystems);
		return xrDisplaySubsystems.Any(xrDisplay => xrDisplay.running);
	}
}