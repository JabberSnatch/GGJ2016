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
		LevelManager.Instance.GetComponent<TimeLine>().TimePeriodStarted += OnTimePeriodStart;
		LevelManager.Instance.GetComponent<TimeLine>().TimePeriodEnded += OnTimePeriodEnd;
		LevelManager.Instance.GetComponent<TimeLine>().TimerEnded += OnTimerEnd;
	}

	void Update()
	{
		if (_closeToDissident)
		{
			Debug.Log("I am close to a dissident");
		}
	}
	
	private void OnTimePeriodStart(InputCombination button, float timePeriod, EventArgs e)
	{

	}

	private void OnTimePeriodEnd(EventArgs e)
	{

	}

	private void OnTimerEnd(EventArgs e)
	{
		// check the snapshot of the gamepad to see if it matches the current requested combination, or, the combination of the dissident if you are close enough
	}
}