using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
	[SerializeField] private GameObject dialoguePanel;
	private RectTransform _dialoguePanelRectTransform;
	[SerializeField] private TextMeshProUGUI textLabel;
	[SerializeField] private DialogueObject startDialogue;
	[SerializeField] private GameObject nextHintSprite;
	private ResponseHandler _responseHandler;

	private TypewriterEffect _typewriterEffect;

	private bool _finishedTyping;
	private bool _nextDialogueAllowed;

	private void Start()
	{
		_typewriterEffect = GetComponent<TypewriterEffect>();
		_responseHandler = GetComponent<ResponseHandler>();
		_dialoguePanelRectTransform = dialoguePanel.GetComponent<RectTransform>();
		CloseDialogueBox();
		ShowDialogue(startDialogue);
	}

	public void ShowDialogue(DialogueObject dialogueObject)
	{
		dialoguePanel.SetActive(true);
		StartCoroutine(StepThroughDialogue(dialogueObject));
	}

	private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
	{
		for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
		{
			string dialogue = dialogueObject.Dialogue[i];
			yield return _typewriterEffect.Run(dialogue, textLabel);

			if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;
			nextHintSprite.SetActive(true);
			_finishedTyping = true;

			yield return new WaitUntil(() => _nextDialogueAllowed);
			_nextDialogueAllowed = false;
			nextHintSprite.SetActive(false);
			_finishedTyping = false;
		}

		if (dialogueObject.HasResponses)
		{
			SetPanelHeight(dialogueObject.Responses.Length <= 2 ? 0.75f : 1.0f);

			_responseHandler.ShowResponses(dialogueObject.Responses);
		}
		else
		{
			CloseDialogueBox();
		}
	}

	public void TryGoNextDialogue()
	{
		if (!_finishedTyping)
		{
			_typewriterEffect.GoFast();
			return;
		}

		_nextDialogueAllowed = true;
	}

	public void SetPanelHeight(float h)
	{
		RectTransform rt = _dialoguePanelRectTransform;
		rt.sizeDelta = new Vector2(rt.sizeDelta.x, h);
	}

	private void CloseDialogueBox()
	{
		dialoguePanel.SetActive(false);
		textLabel.text = "";
		nextHintSprite.SetActive(false);
	}
}
