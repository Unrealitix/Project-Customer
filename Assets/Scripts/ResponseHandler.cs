using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResponseHandler : MonoBehaviour
{
	[SerializeField] private RectTransform responseBox;

	private DialogueUI _dialogueUI;

	private Button[] _responseButtons;

	private void Start()
	{
		_responseButtons = GetComponentsInChildren<Button>();
		_dialogueUI = GetComponent<DialogueUI>();

		HideButtons();

		responseBox.gameObject.SetActive(false);
	}

	// ReSharper disable Unity.PerformanceAnalysis //Only gets called at most four times per dialogue that has responses in the first place
	public void ShowResponses(Response[] responses)
	{
		for (int i = 0; i < responses.Length; i++)
		{
			Response response = responses[i];
			Button responseButton = _responseButtons[i];
			responseButton.gameObject.SetActive(true);
			responseButton.GetComponentInChildren<TMP_Text>().text = response.ResponseText;
			responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response));
		}

		responseBox.gameObject.SetActive(true);
	}

	private void OnPickedResponse(Response response)
	{
		responseBox.gameObject.SetActive(false);

		HideButtons();

		_dialogueUI.ShowDialogue(response.DialogueObject);
	}

	private void HideButtons()
	{
		_dialogueUI.SetPanelHeight(0.5f);
		foreach (Button responseButton in _responseButtons)
		{
			responseButton.gameObject.SetActive(false);
			responseButton.onClick.RemoveAllListeners();
		}
	}
}
