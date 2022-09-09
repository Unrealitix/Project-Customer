using UnityEngine;
using UnityEngine.Events;

public class TempGrabber : MonoBehaviour
{
	[SerializeField] private UnityEvent onGrab;
	[SerializeField] private UnityEvent onRelease;
	[SerializeField] private KeyCode grabKey = KeyCode.Space;

	private void Update()
	{
		if (Input.GetKeyDown(grabKey)) onGrab.Invoke();

		if (Input.GetKeyUp(grabKey)) onRelease.Invoke();
	}
}
