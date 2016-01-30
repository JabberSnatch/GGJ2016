using UnityEngine;
using System.Collections;

public class RotatingCameraController : MonoBehaviour
{
    [SerializeField] private float m_Pitch = 45f;
    [SerializeField] private float m_Distance = 5f;

    [SerializeField] private float m_FocusPointMinDistanceToCenter = 0f;
    [SerializeField] private float m_FocusPointMaxDistanceToCenter = 0f;

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
        MoveToTargetPosition();
    }

    void Update()
    {
        MoveToTargetPosition();
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

            Vector3 focusPoint = m_WorldCenter.position - targetToCenter * m_FocusPointMinDistanceToCenter;
            Vector3 lookDirection = focusPoint - m_TargetPosition;
            Vector3 up = Vector3.Cross(lookDirection, m_Subject.transform.right);
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

}
