using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Outline
{
	private readonly List<Renderer> _renderers;

	private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");
	private static readonly int Scale = Shader.PropertyToID("_Scale");

	public Outline(GameObject parent, Material outlineMat, float scaleFactor, Color color)
	{
		_renderers = new List<Renderer>();

		foreach (MeshFilter originalMeshFilter in parent.GetComponentsInChildren<MeshFilter>())
		{
			//copy mesh
			GameObject outlineObject = new($"{originalMeshFilter.name} Outline");
			MeshFilter meshFilter = outlineObject.AddComponent<MeshFilter>();
			meshFilter.mesh = originalMeshFilter.mesh;

			//set parent
			outlineObject.transform.parent = originalMeshFilter.transform;
			outlineObject.transform.localPosition = Vector3.zero;
			outlineObject.transform.localRotation = new Quaternion(180, 0, 0, 0);
			outlineObject.transform.localScale = Vector3.one;
			outlineObject.layer = LayerMask.GetMask("Ignore Raycast");

			//add renderer
			Renderer rend = outlineObject.AddComponent<MeshRenderer>();

			//set material with outline properties
			rend.material = outlineMat;
			rend.material.SetColor(OutlineColor, color);
			rend.material.SetFloat(Scale, -scaleFactor);
			rend.shadowCastingMode = ShadowCastingMode.Off;

			_renderers.Add(rend);
		}
	}

	public void Enable()
	{
		foreach (Renderer rend in _renderers) rend.enabled = true;
	}

	public void Disable()
	{
		foreach (Renderer rend in _renderers) rend.enabled = false;
	}
}
