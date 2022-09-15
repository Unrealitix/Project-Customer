using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
	[SerializeField] private GameObject dialogueBox;
	[SerializeField] private TextMeshProUGUI textLabel;
	[SerializeField] private DialogueObject testDialogue;
	private ResponseHandler _responseHandler;

	private TypewriterEffect _typewriterEffect;

	private void Start()
	{
		_typewriterEffect = GetComponent<TypewriterEffect>();
		_responseHandler = GetComponent<ResponseHandler>();
		CloseDialogueBox();
		ShowDialogue(testDialogue);
	}

	public void ShowDialogue(DialogueObject dialogueObject)
	{
		dialogueBox.SetActive(true);
		StartCoroutine(StepThroughDialogue(dialogueObject));
	}

	private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
	{
		for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
		{
			string dialogue = dialogueObject.Dialogue[i];
			yield return _typewriterEffect.Run(dialogue, textLabel);

			if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

			yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
		}

		if (dialogueObject.HasResponses)
			_responseHandler.ShowResponses(dialogueObject.Responses);
		else
			CloseDialogueBox();
	}

	private void CloseDialogueBox()
	{
		dialogueBox.SetActive(false);
		textLabel.text = "";
	}
}
