using UnityEngine;

public class TempCamRot : MonoBehaviour
{
	[SerializeField] private float xSensitivity = 1f;
	[SerializeField] private float ySensitivity = 1f;

	private void Update()
	{
		Cursor.lockState = CursorLockMode.Locked;

		transform.eulerAngles -= new Vector3(
			Input.GetAxis("Mouse Y") * ySensitivity,
			Input.GetAxis("Mouse X") * -xSensitivity,
			0);
	}
}
