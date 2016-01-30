using UnityEngine;
using System.Collections;

public class FollowUpCamera : MonoBehaviour {

    [SerializeField] private Transform m_TargetObject;
    [SerializeField] private float m_Pitch = 45f;
    [SerializeField] private float m_Distance = 5f;

    [SerializeField] [Range(0f, 1f)] private float m_DampingFactor = 1f; // 0 is no movement, 1 is instant

    private Vector3 m_TargetPosition;

    void Awake()
    {
        m_TargetPosition = transform.position;
        MoveToTargetPosition();
    }

    void Update()
    {
        MoveToTargetPosition();
    }

    private void MoveToTargetPosition()
    {
        float radPitch = Mathf.Deg2Rad * (-m_Pitch + 90f);

        Vector3 normalizedPosition = new Vector3(0f, Mathf.Cos(radPitch), -Mathf.Sin(radPitch));
        m_TargetPosition = m_TargetObject.position + normalizedPosition * m_Distance;
        transform.position = Vector3.Lerp(transform.position, m_TargetPosition, m_DampingFactor);

        Vector3 lookDirection = m_TargetObject.position - m_TargetPosition;
        Vector3 up = Vector3.Cross(lookDirection, m_TargetObject.right);

        transform.rotation = Quaternion.LookRotation(lookDirection, up);
    }
}
