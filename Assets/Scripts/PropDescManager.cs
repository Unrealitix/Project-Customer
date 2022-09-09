using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class PropDescManager : MonoBehaviour
{
	[SerializeField] private Canvas canvas;
	[SerializeField] private TextMeshProUGUI propName;
	[SerializeField] private TextMeshProUGUI propDescription;
	[SerializeField] private LineRenderer lineRenderer;

	[SerializeField] private float lineScale = 0.9f;

	[CanBeNull] private Prop _currentProp;

	private void Start()
	{
		DisablePropDesc();
	}

	private void Update()
	{
		if (canvas.enabled)
		{
			Vector3 uiPos = propDescription.transform.position;
			Vector3 propPos = _currentProp!.transform.position;

			Physics.Raycast(uiPos, propPos - uiPos, out RaycastHit hit);
			Vector3 ray = hit.point - uiPos;
			ray.Scale(lineScale * Vector3.one);

			lineRenderer.positionCount = 2;
			lineRenderer.SetPositions(new[] {uiPos, uiPos + ray});
		}

		// Debug.Log(_currentProp);
	}

	public void GrabProp(Prop prop, string pName, string desc)
	{
		_currentProp = prop;
		canvas.enabled = true;
		propName.text = pName;
		propDescription.text = desc;
	}

	public void DisablePropDesc()
	{
		canvas.enabled = false;
		_currentProp = null;
		lineRenderer.positionCount = 0;
		lineRenderer.SetPositions(Array.Empty<Vector3>());
	}
}
