using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
	private List<Question> _questions;

	private void Start()
	{
		_questions = new List<Question>();

		// Get the questions from all the props in the scene
		foreach (Prop prop in FindObjectsOfType<Prop>())
			_questions.AddRange(prop.questions);

		// Just simply log them for now, but TODO: Integrate them in conversation
		LogQuestions(_questions);

		LogQuestions(GetRandomQuestions(3));
	}

	private static void LogQuestions(List<Question> questions)
	{
		string log = $"All questions: {questions.Count}\n";
		log += "Click this log to see all questions\n";
		foreach (Question question in questions)
		{
			log += $"Question: \"{question.question}\"\n";
			foreach (Answer answer in question.answers)
				log += $"\t{(answer.isCorrect ? ">" : "X")} Answer: \"{answer.answer}\"\n";
		}

		Debug.Log(log);
	}

	public List<Question> GetRandomQuestions(int amount)
	{
		List<Question> shuffle = new(_questions);
		for (int i = 0; i < shuffle.Count; i++)
		{
			int randomIndex = Random.Range(i, shuffle.Count);
			(shuffle[i], shuffle[randomIndex]) = (shuffle[randomIndex], shuffle[i]);
		}

		if (amount > shuffle.Count)
		{
			amount = shuffle.Count;
			Debug.LogWarning("Amount of questions to get is higher than the amount of questions in the list. Simply returning all questions.");
		}

		return shuffle.GetRange(0, amount);
	}
}
