using UnityEngine;
using System.Collections;

public class FollowUpCamera : MonoBehaviour {

    [SerializeField] private Transform m_TargetObject;
    [SerializeField] private float m_Pitch = 45f;
    [SerializeField] private float m_Distance = 5f;

    void Awake()
    {
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
        transform.position = m_TargetObject.position + normalizedPosition * m_Distance;

        Vector3 lookDirection = m_TargetObject.position - transform.position;
        Vector3 up = Vector3.Cross(lookDirection, m_TargetObject.right);

        transform.rotation = Quaternion.LookRotation(lookDirection, up);
    }
}
