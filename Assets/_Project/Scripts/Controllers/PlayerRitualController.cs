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
		Debug.Log("Log is on Gate number : " + LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().CurrentTimer);

		Debug.Log("Close enough to Rebel when the Gate happened");

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

        // TODO
        // now that I have my combination of keys, I want to compare it to the current InputCombination
        if (keysPressed == rebelCombination && _closeToDissident)
		{
			Debug.Log("CORRECT COMBINATION");
			// reduce the size of the chain and remove that timer from the timeline
			LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().Chain.NullifyCombination((int)currentTimerIndex);
		}
        else if (keysPressed == combination)
        {
            Debug.Log("NEUTRAL COMBINATION");
            if (rebelExists)
                EverythingManager.Instance.BooCharacter(EverythingManager.Instance.Rebel);
        }
		else
		{
			Debug.Log("WRONG COMBINATION");
            // trigger booing towards the player
            EverythingManager.Instance.BooCharacter(EverythingManager.Instance.Player);
            if (rebelExists)
                EverythingManager.Instance.ResetRebelSearch();
		}

        if (!rebelExists)
            Destroy(rebelCombination);
    }
}