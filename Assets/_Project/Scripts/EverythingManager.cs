using UnityEngine;
using System.Collections;

public class EverythingManager : Singleton<EverythingManager>
{
    [SerializeField] private Transform m_WorldCenter;
    public Transform WorldCenter { get { return m_WorldCenter; } }

    [SerializeField] private RotatingPlayerController m_Player;
    public RotatingPlayerController Player { get { return m_Player; } }

    [SerializeField] private float m_MinRadius;
    public float MinRadius { get { return m_MinRadius; } }
    [SerializeField] private float m_MaxRadius;
    public float MaxRadius { get { return m_MaxRadius; } }
}
