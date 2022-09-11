using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Question
{
	[SerializeField] public string question = "";
	[SerializeField] private Answer answer1;
	[SerializeField] private Answer answer2;
	[SerializeField] private Answer answer3;
	[SerializeField] private Answer answer4;

	[HideInInspector] public List<Answer> answers;

	public void Init(Prop owner)
	{
		// Check if the question is empty
		if (question.Trim().Length == 0) Debug.LogError($"[{owner.name}] [{question}] Question is empty");

		// Check if answers are empty, and if they aren't, add them to the list
		answers = new List<Answer>();
		if (answer1.answer.Trim().Length > 0) answers.Add(answer1);
		if (answer2.answer.Trim().Length > 0) answers.Add(answer2);
		if (answer3.answer.Trim().Length > 0) answers.Add(answer3);
		if (answer4.answer.Trim().Length > 0) answers.Add(answer4);

		// Check if there are enough answers
		if (answers.Count < 2) Debug.LogError($"[{owner.name}] [{question}] Not enough answers");

		// Check if there's at least one correct answer
		if (answers.Count(a => a.isCorrect) == 0)
			Debug.LogError($"[{owner.name}] [{question}] No correct answer");

		//TODO (in implementation) shuffle answers
	}
}

[Serializable]
public class Answer
{
	[SerializeField] public string answer;
	[SerializeField] public bool isCorrect;
}
