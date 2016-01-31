using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class LeaderNPCController : NPCController
{
    [SerializeField] private Transform m_SpriteNode;
    [SerializeField] float m_VerticalOffset;

    List<Sprite> m_Resources = new List<Sprite>();
    GameObject m_Sprite;

    void Awake()
    {
        for (int i = 0; i < (int)EGamePadButton.NbStates; ++i)
        {
            m_Resources.Add(new Sprite());
        }

        m_Resources[(int)EGamePadButton.A] = Resources.Load<Sprite>("A");
        m_Resources[(int)EGamePadButton.B] = Resources.Load<Sprite>("B");
        m_Resources[(int)EGamePadButton.X] = Resources.Load<Sprite>("X");
        m_Resources[(int)EGamePadButton.Y] = Resources.Load<Sprite>("Y");
        m_Resources[(int)EGamePadButton.LeftShoulder] = Resources.Load<Sprite>("LB");
        m_Resources[(int)EGamePadButton.LeftTrigger] = Resources.Load<Sprite>("LT");
        m_Resources[(int)EGamePadButton.RightShoulder] = Resources.Load<Sprite>("RB");
        m_Resources[(int)EGamePadButton.RightTrigger] = Resources.Load<Sprite>("RT");
    }

    public override void Initialize(float _angle, float _offset, Transform _center)
    {
        m_Animator = transform.GetChild(0).GetComponentInChildren<Animator>();

        m_WorldCenter = _center;
        m_Angle = _angle;
        m_DistanceToCenter = _offset;

        m_LookDirection = m_WorldCenter.position - transform.position;
        transform.rotation = Quaternion.LookRotation(m_LookDirection, Vector3.up);
        m_TargetRotation = transform.rotation;
    }

    public override void ActivatePose(bool instant = false)
    {
        base.ActivatePose(instant);

        m_Sprite = new GameObject("SpritesRoot");
        m_Sprite.transform.SetParent(m_SpriteNode);
        m_Sprite.transform.localPosition = Vector3.zero;

        int i = 0;
        foreach (var input in ExpectedCombination.Combination)
        {
            Debug.Log(input.ToString());
            GameObject localSprite = new GameObject(input.ToString());
            localSprite.transform.SetParent(m_Sprite.transform);
            localSprite.transform.localPosition = Vector3.zero + Vector3.up * i * m_VerticalOffset;
            localSprite.AddComponent<SpriteRenderer>().sprite = m_Resources[(int)input];
            localSprite.AddComponent<FacingCameraElement>();

            ++i;
        }
    }

    public override void DeactivatePose()
    {
        base.DeactivatePose();

        Destroy(m_Sprite);
    }

    protected override void OnTimePeriodStart(InputCombination button, float timePeriod, EventArgs e)
    {
        _inTimePeriod = true;
        _expectedCombination = button;
        _timePeriod = 0f;
    }
}
