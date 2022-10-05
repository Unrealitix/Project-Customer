using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.XR.Interaction.Toolkit;

public class Outline : MonoBehaviour
{
	private const float SCALE = 0.01f;
	private static readonly int Size = Shader.PropertyToID("_Size");

	private GameObject _outline;

	private void Start()
	{
		Transform parentTransform = transform;
		_outline = Instantiate(gameObject, parentTransform.position, parentTransform.rotation);
		_outline.name += " Outline";

		CleanInstance(_outline);

		MeshRenderer[] meshRenderers = _outline.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in meshRenderers)
		{
			// Flip the X scale, to make up for the flipped normals
			Transform meshRendererTransform = meshRenderer.transform;
			Vector3 localScale = meshRendererTransform.localScale;
			localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
			meshRendererTransform.localScale = localScale;

			// Set the outline material
			meshRenderer.material = Resources.Load<Material>("Outline");
			// Scale the outline to the scale of the object, so objects of different sizes have the same outline size
			meshRenderer.material.SetFloat(Size, SCALE / gameObject.transform.localScale.magnitude);
		}

		// Add parent constraint, to make the outline follow the prop
		ParentConstraint parentConstraint = _outline.AddComponent<ParentConstraint>();
		parentConstraint.SetSources(new List<ConstraintSource> {new() {sourceTransform = parentTransform, weight = 1}});
		parentConstraint.constraintActive = true;

		Disable();
	}

	public void Enable()
	{
		_outline.SetActive(true);
	}

	public void Disable()
	{
		_outline.SetActive(false);
	}

	/// <summary>
	///     Strips a GameObject of all components except for the MeshRenderer and MeshFilter.
	/// </summary>
	private static void CleanInstance(GameObject gameObject)
	{
		Component[] components = gameObject.GetComponentsInChildren<Component>();

		//Sort all components, putting XRGrabInteractable first to make sure that one is removed before all others,
		// because it relies on Rigidbody, making it impossible to be removed before the Rigidbody.
		// This is a hacky solution, but it works.
		for (int i = 0; i < components.Length; i++)
			if (components[i].GetType() == typeof(XRGrabInteractable))
				(components[i], components[0]) = (components[0], components[i]);

		foreach (Component comp in components)
			if (comp is not (MeshFilter or MeshRenderer or Transform))
				Destroy(comp);
	}
}
