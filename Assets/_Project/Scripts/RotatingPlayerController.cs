using UnityEngine;
using XInputDotNetPure;
using System.Collections;

public class RotatingPlayerController : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] private float m_DampingFactor = 1f; // 0 is no movement, 1 is instant velocity change
    [SerializeField] private float m_Speed = 5f; // Speed in meters per sec

    private Vector3 m_TargetVelocity = Vector3.zero;
    private Vector3 m_Velocity = Vector3.zero;

    private Vector3 m_LookDirection = Vector3.zero;

    private Transform m_WorldCenter;
    private float m_DistanceToCenter;
    private float m_Angle;

    public float DistanceToCenter { get { return m_DistanceToCenter; } }

    private GamePadState GPState;
    private GamePadState OldState;

    void Awake()
    {
        m_WorldCenter = EverythingManager.Instance.WorldCenter;
        transform.position = new Vector3(transform.position.x, m_WorldCenter.position.y, transform.position.z);

        Vector3 vectorToCenter = m_WorldCenter.position - transform.position;
        m_LookDirection = vectorToCenter;
        m_DistanceToCenter = vectorToCenter.magnitude;

        m_Angle = Vector3.Angle(vectorToCenter, Vector3.forward);
    }

    void Update()
    {
        OldState = GPState;
        GPState = GamePad.GetState(PlayerIndex.One);

        m_TargetVelocity = (Vector3.forward * GPState.ThumbSticks.Left.Y +
                           Vector3.right * GPState.ThumbSticks.Left.X) * Time.deltaTime * m_Speed;

        m_Velocity = Vector3.Lerp(m_Velocity, m_TargetVelocity, m_DampingFactor);

        float X = m_Velocity.x;
        float Y = m_Velocity.z;

        float deltaAngle = (X / m_DistanceToCenter) * Mathf.Rad2Deg;
        m_Angle += deltaAngle;

        m_DistanceToCenter -= Y;

        Vector3 nextPosition = m_WorldCenter.position + Vector3.back * m_DistanceToCenter * Mathf.Cos(Mathf.Deg2Rad * m_Angle) + Vector3.right * m_DistanceToCenter * Mathf.Sin(Mathf.Deg2Rad * m_Angle);
        
        if (m_Velocity != Vector3.zero)
            m_LookDirection = nextPosition - transform.position;

        transform.position = nextPosition;
        transform.rotation = Quaternion.LookRotation(m_LookDirection, Vector3.up);
    }
}
