using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerRitualController : MonoBehaviour
{
	private bool _closeToDissident = false;

	public bool CloseToDissident
	{
		get { return _closeToDissident; }
		set { _closeToDissident = value; }
	}

	void Start()
	{
		LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().TimePeriodStarted += OnTimePeriodStart;
		LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().TimePeriodEnded += OnTimePeriodEnd;
		LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().TimerEnded += OnTimerEnd;
	}
	
	void Update()
	{
	}


	private void OnTimePeriodStart(InputCombination button, float timePeriod, EventArgs e)
	{

	}

	private void OnTimePeriodEnd(EventArgs e)
	{

	}

	private void OnTimerEnd(EventArgs e)
	{
		Debug.Log("Log is on Gate number : " + LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().CurrentTimer);

		if (_closeToDissident)
		{
			Debug.Log("Close enough to Rebel when the Gate happened");

			InputCombination combination = LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().CurrentKeyCombination;
			List<bool> snapshot = InputManager.Instance.Snapshot;

			List<EGamePadButton> keysPressed = new List<EGamePadButton>();

			for (int i = 0; i < snapshot.Count; ++i)
			{
				if (snapshot[i]) keysPressed.Add((EGamePadButton)i);
			}

			// DEBUG PURPOSE
			string s = "";
			s += "Pressed Sequence : [";
			for (int i = 0; i < keysPressed.Count; ++i)
			{
				s += InputCombination.InputToAnimatorField(keysPressed[i]) + ", ";
			}
			s = s.Remove(s.Length - 2);
			s += "]";
			Debug.Log(s);
			////////////////

			//TODO
		}
	}
}