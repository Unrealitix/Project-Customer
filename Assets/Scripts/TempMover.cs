using UnityEngine;

public class TempMover : MonoBehaviour
{
	[SerializeField] private float strength = 1f;

	private Rigidbody _rb;

	private void Start()
	{
		_rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.W)) _rb.AddForce(Vector3.forward * strength);
		if (Input.GetKey(KeyCode.S)) _rb.AddForce(Vector3.back * strength);
		if (Input.GetKey(KeyCode.A)) _rb.AddForce(Vector3.left * strength);
		if (Input.GetKey(KeyCode.D)) _rb.AddForce(Vector3.right * strength);
	}
}
