using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
	#region TimerStuff
	#region Fields
	[SerializeField]
	private float _waitingTimePeriodStart;

	[SerializeField]
	private float _waitingTimePeriodEnd;

	[SerializeField]
	private float _totalTimeCount;
	#endregion


	#region Properties
	public float WaitingTimePeriodStart
	{
		get { return _waitingTimePeriodStart; }
		set { _waitingTimePeriodStart = value; }
	}

	public float WaitingTimePeriodEnd
	{
		get { return _waitingTimePeriodEnd; }
		set { _waitingTimePeriodEnd = value; }
	}
	
	public float TotalTimeCount
	{
		get { return _totalTimeCount; }
		set { _totalTimeCount = value; }
	}

	public float TimePeriod
	{
		get { return _waitingTimePeriodEnd - _waitingTimePeriodStart; }
	}
	#endregion
	#endregion

	#region Methods
	public Timer(float totalTimeCount, float waitingTimePeriodStart, float waitingTimePeriodEnd)
	{
		if (!(waitingTimePeriodStart < waitingTimePeriodEnd && waitingTimePeriodEnd < totalTimeCount))
			Debug.Log("Your timer has some wrong values and will not be usable...");

		_totalTimeCount = totalTimeCount;
		_waitingTimePeriodStart = waitingTimePeriodStart;
		_waitingTimePeriodEnd = waitingTimePeriodEnd;
	}
	#endregion
}