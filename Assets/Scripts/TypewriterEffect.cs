using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
	[SerializeField] private float defaultTypeWriterSpeed = 12.0f;
	[SerializeField] private float fastTypeWriterSpeed = 50.0f;
	[SerializeField] private AudioSource audioSource;

	private float _currentTypeWriterSpeed;

	public Coroutine Run(string textToType, TMP_Text textLabel)
	{
		_currentTypeWriterSpeed = defaultTypeWriterSpeed;
		return StartCoroutine(TypeText(textToType, textLabel));
	}

	private IEnumerator TypeText(string textToType, TMP_Text textLabel)
	{
		audioSource.Play();
		audioSource.pitch = 1.0f;
		textLabel.text = "";

		float t = 0.0f;
		int charIndex = 0;

		while (charIndex < textToType.Length)
		{
			t += Time.deltaTime * _currentTypeWriterSpeed;
			charIndex = Mathf.FloorToInt(t);
			charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

			textLabel.text = textToType[..charIndex];

			yield return null;
		}

		textLabel.text = textToType;
		audioSource.Stop();
	}

	public void GoFast()
	{
		_currentTypeWriterSpeed = fastTypeWriterSpeed;
		audioSource.pitch = 2.0f;
	}
}
