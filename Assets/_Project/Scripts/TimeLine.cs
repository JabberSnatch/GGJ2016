using UnityEngine;
using System.Collections.Generic;
using System;

public delegate void TimerDefaultHandler(EventArgs e);

public delegate void TimerStartHandler(EGamePadButton button, float timePeriod, EventArgs e);

public class TimeLine : MonoBehaviour
{
	#region Fields
	private int _currentTimerIndex = 0;

	private float _currentElapsedTime = 0.0f;

	[SerializeField]
	private InputChain _chain;
	[SerializeField]
	private List<Timer> _timers = new List<Timer>();

	public event TimerStartHandler TimePeriodStarted;
	public event TimerDefaultHandler TimePeriodEnded;
	public event TimerDefaultHandler TimerEnded;
	#endregion


	#region Properties
	public Timer CurrentTimer
	{
		get { return _timers[_currentTimerIndex]; }
	}

	public EGamePadButton CurrentKey
	{
		get { return _chain.Chain[_currentTimerIndex]; }
	}
	#endregion


	#region Methods
	void Update()
	{
		_currentElapsedTime += Time.unscaledDeltaTime;

		if (_currentElapsedTime >= CurrentTimer.WaitingTimePeriodStart) OnPeriodStart(CurrentKey, CurrentTimer.TimePeriod, EventArgs.Empty);
		else if (_currentElapsedTime >= CurrentTimer.WaitingTimePeriodEnd) OnPeriodEnd(EventArgs.Empty);
		else if (_currentElapsedTime >= CurrentTimer.TotalTimeCount)
		{
			OnTimerEnds(EventArgs.Empty);
			++_currentTimerIndex;
			_currentElapsedTime = CurrentTimer.TotalTimeCount - _currentElapsedTime;
		}
	}
	#endregion


	#region Publishers
	protected virtual void OnTimerEnds(EventArgs e)
	{
		TimerDefaultHandler handler = TimerEnded;

		if (handler != null)
			handler(e);
	}

	protected virtual void OnPeriodStart(EGamePadButton button, float timePeriod, EventArgs e)
	{
		TimerStartHandler handler = TimePeriodStarted;

		if (handler != null)
			handler(button, timePeriod, e);
	}

	protected virtual void OnPeriodEnd(EventArgs e)
	{
		TimerDefaultHandler handler = TimePeriodEnded;

		if (handler != null)
			handler(e);
	}
	#endregion
}