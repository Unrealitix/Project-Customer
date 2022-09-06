using UnityEngine;

namespace Player
{
	public class Movement : MonoBehaviour
	{
		private Rigidbody _rb;

		private void Start()
		{
			Debug.Log("Player init");
			_rb = GetComponent<Rigidbody>();
			if(_rb == null)
				Debug.LogError("Rigidbody not found");
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				_rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
			}
		}
	}
}
