using UnityEngine;
using System.Collections.Generic;
using System;

public class NPCController : PolarCharacter
{
	private bool _inTimePeriod = false;
	private bool _timePeriodEnded = false;
	private float _timePeriod = 0.0f;

	private bool _posing = false;

	private float _detectionRadius = 0.0f;

	[SerializeField]
	private InputCombination _dissidentCombination;

	private GameObject _inputCombinationGao;

	void Start()
	{
		LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().TimePeriodStarted += OnTimePeriodStart;
		LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().TimePeriodEnded += OnTimePeriodEnd;
		LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().TimerEnded += OnTimerEnd;
	}

	public void YOLOTranscendSQUAD(float detectionRadius, List<EGamePadButton> dissidentCombination)
	{
		_inputCombinationGao = Instantiate(new GameObject());
		_inputCombinationGao.transform.SetParent(this.gameObject.transform);
		_inputCombinationGao.AddComponent<InputCombination>();
		_inputCombinationGao.GetComponent<InputCombination>().Populate(dissidentCombination.ToArray());
		_dissidentCombination = _inputCombinationGao.GetComponent<InputCombination>();

		_detectionRadius = detectionRadius;
	}

	public void YOLOBringMeBackToLifeSQUAD()
	{
		Destroy(_inputCombinationGao);
		_inputCombinationGao = null;

		_dissidentCombination = null;

		_detectionRadius = 0.0f;
	}

	override protected void Update()
	{
		base.Update();

		if (_inTimePeriod)
		{
			// do the random to determine the amount of time to wait for
			// substract the random found to the time Period
			// then each frame it gets subtracted by the delta time resulting in 0 in a random time

			if (_dissidentCombination)
			{
				// depending on the random, do the dissident behavior
				if (_timePeriod <= 0.0f)
				{
					//trigger animation and pose
					_posing = true;
				}
			}
			else
			{
				// depending on the random, do the programmed behavior
				if (_timePeriod <= 0.0f)
				{
					// trigger animation and pose
					_posing = true;
				}
			}
		}
		else if (_timePeriodEnded)
		{
			// if the time period has ended and it has not yet activated (in case, shouldn't happen with the random but oh well)
			// force trigger it
			if (!_posing)
			{
				// pose
				_posing = true;
			}
		}

		_timePeriod -= Time.deltaTime;

		CheckForPlayer();
	}

	void CheckForPlayer()
	{
		int layerMask = 1 << LayerMask.NameToLayer("Player");

		Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, _detectionRadius, layerMask);

		if (colliders.Length != 0)
		{
			foreach (Collider col in colliders)
			{
				col.gameObject.GetComponent<PlayerRitualController>().CloseToDissident = true;
			}
		}
	}

	#region Subscribers
	private void OnTimePeriodStart(InputCombination button, float timePeriod, EventArgs e)
	{
		_inTimePeriod = true;
		_timePeriod = timePeriod;

	}

	private void OnTimePeriodEnd(EventArgs e)
	{
		_inTimePeriod = false;
		_timePeriodEnded = true;
		_timePeriod = 0.0f;
	}

	private void OnTimerEnd(EventArgs e)
	{
		// release their state of animations
		_timePeriodEnded = false;
		_posing = false;
	}
	#endregion
}