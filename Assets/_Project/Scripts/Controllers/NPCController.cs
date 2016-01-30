using UnityEngine;
using System.Collections.Generic;
using System;

public class NPCController : PolarCharacter
{
	private bool _inTimePeriod = false;
	private bool _timePeriodEnded = false;
	private float _timePeriod = 0.0f;

	private bool _posing = false;

	[SerializeField]
	private InputCombination _dissidentCombination;
    private InputCombination _expectedCombination;

	private GameObject _inputCombinationGao;

	void Start()
	{
		LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().TimePeriodStarted += OnTimePeriodStart;
		LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().TimePeriodEnded += OnTimePeriodEnd;
		LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().TimerEnded += OnTimerEnd;
	}

    void OnDestroy()
    {
        LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().TimePeriodStarted -= OnTimePeriodStart;
        LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().TimePeriodEnded -= OnTimePeriodEnd;
        LevelManager.Instance.CurrentTimeline.GetComponent<TimeLine>().TimerEnded -= OnTimerEnd;
    }

	public void YOLOTranscendSQUAD(List<EGamePadButton> dissidentCombination)
	{
		_inputCombinationGao = Instantiate(new GameObject());
		_inputCombinationGao.transform.SetParent(this.gameObject.transform);
		_inputCombinationGao.AddComponent<InputCombination>();
		_inputCombinationGao.GetComponent<InputCombination>().Populate(dissidentCombination.ToArray());

		_dissidentCombination = _inputCombinationGao.GetComponent<InputCombination>();
	}

	public void YOLOBringMeBackToLifeSQUAD()
	{
		Destroy(_inputCombinationGao);
		_inputCombinationGao = null;

		_dissidentCombination = null;
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
	}

    public void ActivatePose(bool instant = false)
    {
        List<string> poseElements;

        if (_dissidentCombination)
            poseElements = _dissidentCombination.ToAnimatorGrammar();
        else
            poseElements = _expectedCombination.ToAnimatorGrammar();

        foreach(var pose in poseElements)
        {
            SetAnimatorPoseElement(pose, true, instant);
        }

        _posing = true;
    }

    public void DeactivatePose()
    {
        List<string> poseElements;

        if (_dissidentCombination)
            poseElements = _dissidentCombination.ToAnimatorGrammar();
        else
            poseElements = _expectedCombination.ToAnimatorGrammar();

        foreach (var pose in poseElements)
        {
            SetAnimatorPoseElement(pose, false);
        }

        _posing = false;
    }

	#region Subscribers
	private void OnTimePeriodStart(InputCombination button, float timePeriod, EventArgs e)
	{
		_inTimePeriod = true;
        _expectedCombination = button;
		_timePeriod = UnityEngine.Random.Range(0f, timePeriod);
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
        DeactivatePose();
		_timePeriodEnded = false;
	}
	#endregion
}