using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
	[SerializeField] [TextArea] private string[] dialogue;
	[SerializeField] private Response[] responses;

	public string[] Dialogue => dialogue;

	public bool HasResponses => responses is {Length: > 0};

	public Response[] Responses => responses;
}
