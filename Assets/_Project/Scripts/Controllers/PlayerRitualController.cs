﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerRitualController : MonoBehaviour
{
	[SerializeField]
	private GameObject _mesh;

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

		if (!_mesh.GetComponent<Renderer>())
			_mesh.AddComponent<Renderer>();
		_mesh.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>("D_Chara_03");
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

		float currentTimerIndex = LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().CurrentTimerIndex;
		InputCombination combination = LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().CurrentKeyCombination;

        InputCombination rebelCombination = gameObject.AddComponent<InputCombination>();
        bool rebelExists = EverythingManager.Instance.Rebel != null;
        if (rebelExists)
        {
            Destroy(rebelCombination);
            rebelCombination = EverythingManager.Instance.Rebel.ExpectedCombination;
        }

		List<bool> snapshot = InputManager.Instance.Snapshot;

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

		if (keysPressed.Count == 0)
			keysPressed.Add(EGamePadButton.None);

		if (keysPressed == rebelCombination && _closeToDissident)
		{
			Debug.Log("CORRECT COMBINATION");
			EverythingManager.Instance.DeactivateALLPoses();
			LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().Chain.NullifyCombination((int)currentTimerIndex);
			EverythingManager.Instance.Rebel.YOLOBringMeBackToLifeSQUAD();
			AudioPlayer.Instance.PlayPlayerInputWithRebel();
		}
		else if (keysPressed == combination)
        {
            Debug.Log("NEUTRAL COMBINATION");
            if (rebelExists)
                EverythingManager.Instance.BooCharacter(EverythingManager.Instance.Rebel);
			if (EverythingManager.Instance.Rebel)
				AudioPlayer.Instance.PlayPlayerInputFail();
		}
		else
		{
			Debug.Log("WRONG COMBINATION");
            EverythingManager.Instance.BooCharacter(EverythingManager.Instance.Player);
			AudioPlayer.Instance.PlayPlayerInputFail();
            if (rebelExists)
				EverythingManager.Instance.Rebel.YOLOBringMeBackToLifeSQUAD();
		}

        if (!rebelExists)
            Destroy(rebelCombination);
    }
}