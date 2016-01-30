using UnityEngine;
using System.Collections.Generic;
using System;

public delegate void TimerDefaultHandler(EventArgs e);

public delegate void TimerStartHandler(InputCombination button, float timePeriod, EventArgs e);

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

	private bool _startReached = false;
	private bool _endReached = false;
	private bool _gateReached = false;
	#endregion


	#region Properties
	public Timer CurrentTimer
	{
		get { return _timers[_currentTimerIndex]; }
	}

	public InputCombination CurrentKeyCombination
	{
		get { return _chain.Chain[_currentTimerIndex]; }
	}
	#endregion


	#region Methods
	void FixedUpdate()
	{
		_currentElapsedTime += Time.deltaTime;

		//Debug.Log(_currentElapsedTime);

		if (_currentElapsedTime >= CurrentTimer.TotalTimeCount && !_gateReached)
		{
			OnTimerEnds(EventArgs.Empty);
			++_currentTimerIndex;
			_currentElapsedTime = CurrentTimer.TotalTimeCount - _currentElapsedTime;
			_startReached = false;
			_endReached = false;
		}
		else if (_currentElapsedTime >= CurrentTimer.WaitingTimePeriodEnd && !_endReached)
		{
			OnPeriodEnd(EventArgs.Empty);
			_endReached = true;
		}
		else if (_currentElapsedTime >= CurrentTimer.WaitingTimePeriodStart && !_startReached) 
		{
			OnPeriodStart(CurrentKeyCombination, CurrentTimer.TimePeriod, EventArgs.Empty);
			_startReached = true;
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

	protected virtual void OnPeriodStart(InputCombination button, float timePeriod, EventArgs e)
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