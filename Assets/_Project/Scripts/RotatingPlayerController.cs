using UnityEngine;
using XInputDotNetPure;
using System.Collections;

public class RotatingPlayerController : PolarCharacter
{
    [SerializeField] [Range(0f, 1f)] private float m_DampingFactor = 1f; // 0 is no movement, 1 is instant velocity change
    [SerializeField] private float m_Speed = 5f; // Speed in meters per sec

    private Vector3 m_TargetVelocity = Vector3.zero;
    private Vector3 m_Velocity = Vector3.zero;

    private GamePadState GPState;
    private GamePadState OldState;

    override protected void Update()
    {
        OldState = GPState;
        GPState = GamePad.GetState(PlayerIndex.One);

        m_TargetVelocity = (Vector3.forward * GPState.ThumbSticks.Left.Y +
                           Vector3.right * GPState.ThumbSticks.Left.X) * Time.deltaTime * m_Speed;

        m_Velocity = Vector3.Lerp(m_Velocity, m_TargetVelocity, m_DampingFactor);
        BoundZVelocity();

        float X = m_Velocity.x;
        float Y = m_Velocity.z;

        float deltaAngle = (X / m_DistanceToCenter) * Mathf.Rad2Deg;
        m_Angle += deltaAngle;

        m_DistanceToCenter -= Y;
        BoundDistanceToCenter();

        base.Update();
    }

    private void BoundZVelocity()
    {
        float nextDistanceToCenter = m_DistanceToCenter - m_Velocity.z;

        if (nextDistanceToCenter < EverythingManager.Instance.MinRadius || 
            nextDistanceToCenter > EverythingManager.Instance.MaxRadius)
            m_Velocity.z = 0f;
    }

    private void BoundDistanceToCenter()
    {
        if (m_DistanceToCenter < EverythingManager.Instance.MinRadius)
            m_DistanceToCenter = EverythingManager.Instance.MinRadius;
        if (m_DistanceToCenter > EverythingManager.Instance.MaxRadius)
            m_DistanceToCenter = EverythingManager.Instance.MaxRadius;
    }
}
