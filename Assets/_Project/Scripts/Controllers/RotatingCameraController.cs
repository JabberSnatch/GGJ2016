using UnityEngine;
using System.Collections;

public class RotatingCameraController : MonoBehaviour
{
    [SerializeField] private float m_Pitch = 45f;
    [SerializeField] private float m_Distance = 5f;

    public float Pitch { get { return m_Pitch; } }
    public float Distance { get { return m_Distance; } }

    [SerializeField] private float m_FocusPointMinDistanceToCenter = 0f;
    [SerializeField] private float m_FocusPointMaxDistanceToCenter = 0f;
    [SerializeField] private float m_MinFOV = 60f;
    [SerializeField] private float m_MaxFOV = 90f;

    private float m_FocusPointDistanceToCenter;
    private float m_FOV;

    [SerializeField] [Range(0f, 1f)] private float m_DampingFactor = 1f; // 0 is no movement, 1 is instant

    private Camera                      m_Camera;
    private RotatingPlayerController    m_Subject;
    private Transform                   m_WorldCenter;
    private Vector3                     m_TargetPosition;

    public Transform WorldCenter { set { m_WorldCenter = value; } }
    public RotatingPlayerController Subject { set { m_Subject = value; MoveToTargetPosition(); } }

    void Awake()
    {
        m_Camera = GetComponent<Camera>();
        m_TargetPosition = transform.position;
        UpdateFocusPointAndFOV();
        MoveToTargetPosition();
    }

    void Update()
    {
        UpdateFocusPointAndFOV();
        MoveToTargetPosition();
    }

    private void UpdateFocusPointAndFOV()
    {
        m_FOV = Mathf.Lerp(m_MinFOV, m_MaxFOV, EverythingManager.Instance.PlayerPositionRatio);
        m_FocusPointDistanceToCenter = Mathf.Lerp(m_FocusPointMinDistanceToCenter, m_FocusPointMaxDistanceToCenter, EverythingManager.Instance.PlayerPositionRatio);

        m_Camera.fieldOfView = m_FOV;
    }

    private void MoveToTargetPosition()
    {
        if (m_Subject != null)
        {
            float radPitch = Mathf.Deg2Rad * (-m_Pitch + 90f);

            Vector3 targetToCenter = (m_WorldCenter.position - m_Subject.transform.position).normalized;
            Quaternion forwardToDirection = Quaternion.FromToRotation(Vector3.forward, targetToCenter);

            Vector3 normalizedPosition = new Vector3(0f, Mathf.Cos(radPitch), -Mathf.Sin(radPitch));
            m_TargetPosition = m_Subject.transform.position + forwardToDirection * (normalizedPosition * m_Distance);
            transform.position = Vector3.Lerp(transform.position, m_TargetPosition, m_DampingFactor);

            Vector3 focusPoint = m_WorldCenter.position - targetToCenter * m_FocusPointDistanceToCenter;
            Vector3 lookDirection = focusPoint - m_TargetPosition;
            transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        }
    }

}
