using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
	[SerializeField] private float typeWriterSpeed = 50.0f;

	public Coroutine Run(string textToType, TMP_Text textLabel)
	{
		return StartCoroutine(TypeText(textToType, textLabel));
	}

	private IEnumerator TypeText(string textToType, TMP_Text textLabel)
	{
		textLabel.text = "";

		float t = 0.0f;
		int charIndex = 0;

		while (charIndex < textToType.Length)
		{
			t += Time.deltaTime * typeWriterSpeed;
			charIndex = Mathf.FloorToInt(t);
			charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

			textLabel.text = textToType[..charIndex];

			yield return null;
		}

		textLabel.text = textToType;
	}
}
