﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class NPCController : PolarCharacter
{
	private bool _inTimePeriod = false;
	private float _timePeriod = 0.0f;

	private bool _posing = false;

    private bool _isRebel = false;
	private float _detectionRadius = 0.0f;
    private float _gatesToLive = 0;

    private InputCombination _expectedCombination;

	private GameObject _inputCombinationGao;

    private PolarCharacter _booedCharacter = null;
    private float _booDuration = 0f;
    private float _booElapsedTime = 0f;

	void Start()
	{
		LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().TimePeriodStarted += OnTimePeriodStart;
		LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().TimePeriodEnded += OnTimePeriodEnd;
		LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().TimerEnded += OnTimerEnd;
	}

    void OnDestroy()
    {
		if (LevelManager.Instance)
		{
			if (LevelManager.Instance.CurrentTimeline)
			{
				LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().TimePeriodStarted -= OnTimePeriodStart;
				LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().TimePeriodEnded -= OnTimePeriodEnd;
				LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().TimerEnded -= OnTimerEnd;
			}
		}
	}

	public void YOLOTranscendSQUAD(float detectionRadius, int gatesToLive)
	{
        /*
		_inputCombinationGao = Instantiate(new GameObject());
		_inputCombinationGao.transform.SetParent(this.gameObject.transform);
		_inputCombinationGao.AddComponent<InputCombination>();
		_inputCombinationGao.GetComponent<InputCombination>().Populate(dissidentCombination.ToArray());
		_dissidentCombination = _inputCombinationGao.GetComponent<InputCombination>();
        */

		_detectionRadius = detectionRadius;
        _isRebel = true;
		_gatesToLive = gatesToLive;
	}

	public void YOLOBringMeBackToLifeSQUAD()
	{
		Destroy(_inputCombinationGao);
		_inputCombinationGao = null;

        _isRebel = false;
        _gatesToLive = 0;

		_detectionRadius = 0.0f;

		EverythingManager.Instance.ResetRebelSearch();
	}

	override protected void Update()
	{
		base.Update();

		_timePeriod -= Time.deltaTime;

        if (_inTimePeriod)
		{
            if(_timePeriod <= 0f)
            {
                ActivatePose();
            }
		}

		if (_isRebel)
			CheckForPlayer();

        if (_booedCharacter != null && !_posing)
        {
            LookAt(_booedCharacter.transform.position);
            _booElapsedTime += Time.deltaTime;

            if (_booElapsedTime >= _booDuration)
                StopBooing();
        }
	}

	void CheckForPlayer()
	{
		RotatingPlayerController player = EverythingManager.Instance.Player;

		Vector3 sub = player.transform.position - gameObject.transform.position;

		if (sub.magnitude <= _detectionRadius)
			player.gameObject.GetComponent<PlayerRitualController>().CloseToDissident = true;
		else
			player.gameObject.GetComponent<PlayerRitualController>().CloseToDissident = false;
	}

	public void ActivatePose(bool instant = false)
    {
        LookAt(m_WorldCenter.position);
        List<string> poseElements = new List<string>();

        if (_expectedCombination)
            poseElements = _expectedCombination.ToAnimatorGrammar();

        foreach(var pose in poseElements)
        {
            SetAnimatorPoseElement(pose, true, instant);
        }

        _posing = true;
    }

    public void DeactivatePose()
    {
        List<string> poseElements = new List<string>();

        if(_expectedCombination)
            poseElements = _expectedCombination.ToAnimatorGrammar();

        foreach (var pose in poseElements)
        {
            SetAnimatorPoseElement(pose, false);
        }

        _posing = false;
    }

    public void BooCharacter(PolarCharacter target, float duration)
    {
        if (!EverythingManager.Instance.PlayerIsOutcast)
        {
            _booedCharacter = target;
            _booDuration = duration;
            _booElapsedTime = 0f;
        }
    }

    public void BooOutcastPlayer(RotatingPlayerController player)
    {
        _booedCharacter = player;
        _booDuration = Mathf.Infinity;
        _booElapsedTime = 0f;
    }

    public void StopBooing()
    {
        LookAt(m_WorldCenter.position);
        _booedCharacter = null;
    }

	#region Subscribers
	private void OnTimePeriodStart(InputCombination button, float timePeriod, EventArgs e)
	{
		_inTimePeriod = true;
        _expectedCombination = button;
		_timePeriod = UnityEngine.Random.Range(0f, timePeriod);

        /*
        if (_isRebel)
        {
            _expectedCombination.Randomize();
        }
        */
	}

	private void OnTimePeriodEnd(EventArgs e)
	{
		_inTimePeriod = false;
		_timePeriod = 0.0f;
	}

	private void OnTimerEnd(EventArgs e)
	{
		// release their state of animations
        DeactivatePose();

        if (_isRebel)
        {
            _gatesToLive--;
            if (_gatesToLive <= 0)
                YOLOBringMeBackToLifeSQUAD();
        }
	}
	#endregion
}