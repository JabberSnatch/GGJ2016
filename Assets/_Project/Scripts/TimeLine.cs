﻿using UnityEngine;
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

	public bool _timeLineIsDone = false;

	private bool _startReached = false;
	private bool _endReached = false;
	private bool _gateReached = false;

    private bool _IsPausedForDayNight = false;
    public bool PauseForDayNight { get { return _IsPausedForDayNight; } set { _IsPausedForDayNight = value; } }
	#endregion


	#region Properties
	public Timer CurrentTimer
	{
		get { return _timers[_currentTimerIndex]; }
	}

	public float CurrentTimerIndex
	{
		get { return _currentTimerIndex; }
	}

	public InputChain Chain
	{
		get { return _chain; }
	}

	public InputCombination CurrentKeyCombination
	{
		get { return _chain.Chain[_currentTimerIndex]; }
	}
	#endregion

	#region Methods
    void Awake()
    {
        TimerEnded += EverythingManager.Instance.RebelUpdateSubroutine;
    }

	void Update()
	{
        if (_IsPausedForDayNight) return;

		if (_chain.Completed())
			_timeLineIsDone = true;

		_currentElapsedTime += Time.deltaTime;

		if (_currentElapsedTime >= CurrentTimer.TotalTimeCount)
		{
			OnTimerEnds(EventArgs.Empty);
            _currentTimerIndex++;
            if (_currentTimerIndex >= _timers.Count)
            {
                _currentTimerIndex = 0;
                LevelManager.Instance.RitualsCount++;
                _IsPausedForDayNight = true;
                StartCoroutine(EverythingManager.Instance.DayNightCycle(this, 5f));
            }
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