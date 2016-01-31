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
		float				currentTimerIndex	= LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().CurrentTimerIndex;
		InputCombination	combination			= LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().CurrentKeyCombination;
		List<bool>			snapshot			= InputManager.Instance.Snapshot;

		List<EGamePadButton> keysPressed = new List<EGamePadButton>();

		for (int i = 0; i < snapshot.Count; ++i)
			if (snapshot[i]) keysPressed.Add((EGamePadButton)i);

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

		//if (_closeToDissident)
		//{
		//	if (keysPressed == rebelCombination)
		//	{
		//		// you match the rebel's combination, he becomes back a sheep but you nullify that Timer combination ! PROGRESS
		//	}
		//	else if (keysPressed == combination)
		//	{
		//		// you are close to the rebel but do the regular combination. The rebel loses durability and everyone booes the rebel
		//	}
		//	else
		//	{
		//		// you mess up even close to the rebel, everyone booes you including rebel who becomes a sheep again
		//	}
		//}
		//else
		//{
		//	if (keysPressed == combination)
		//	{
		//		// the rebel loses durability and everyone booes the rebel
		//	}
		//	else
		//	{
		//		// everyone turns to you including the rebel who becomes a sheep again and booes you
		//	}
		//}
	}
}