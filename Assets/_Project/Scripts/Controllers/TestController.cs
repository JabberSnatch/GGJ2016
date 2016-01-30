using UnityEngine;
using System.Collections;
using System;

public class TestController : MonoBehaviour
{
	private LevelManager levelMgr;

	void Update()
	{
	}

	void Start()
	{
		levelMgr = LevelManager.Instance;
		levelMgr.LoadLevel(0);
		levelMgr.CurrentTimeline.GetComponent<TimeLine>().TimePeriodStarted += OnStart;
		levelMgr.CurrentTimeline.GetComponent<TimeLine>().TimePeriodEnded += OnEnd;
		levelMgr.CurrentTimeline.GetComponent<TimeLine>().TimerEnded += OnGate;
	}

	private void OnStart(EGamePadButton button, float timePeriod, EventArgs e)
	{
		Debug.Log("Time period started");
	}

	private void OnEnd(EventArgs e)
	{
		Debug.Log("Time period ended");
	}

	private void OnGate(EventArgs e)
	{
		Debug.Log("Gate Reached");
	}
}