using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
	[SerializeField] [TextArea] private string[] dialogue;
	[SerializeField] private AudioClip[] audioClips;

	[SerializeField] private string propToFetch;

	[SerializeField] private Response[] responses;

	public string[] Dialogue => dialogue;
	public AudioClip[] AudioClips => audioClips;

	public bool HasResponses => responses is {Length: > 0};
	public Response[] Responses => responses;

	public bool IsFetchQuest => !string.IsNullOrEmpty(propToFetch);
	public string PropToFetch => propToFetch;
}
