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


	private void Start()
	{
		if (XRIsPresent())
		{
			enabled = false;
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

		transform.eulerAngles -= new Vector3(
			Input.GetAxis("Mouse Y") * ySensitivity,
			Input.GetAxis("Mouse X") * -xSensitivity,
			0);
	}

	private static bool XRIsPresent()
	{
		List<XRDisplaySubsystem> xrDisplaySubsystems = new();
		SubsystemManager.GetInstances(xrDisplaySubsystems);
		return xrDisplaySubsystems.Any(xrDisplay => xrDisplay.running);
	}
}
