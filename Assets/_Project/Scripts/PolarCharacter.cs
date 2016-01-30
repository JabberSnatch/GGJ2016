using UnityEngine;
using System.Collections;

public class PolarCharacter : MonoBehaviour
{
    protected Animator      m_Animator;

    protected Vector3       m_LookDirection = Vector3.zero;
    protected Quaternion    m_TargetRotation = Quaternion.identity;

    protected Transform     m_WorldCenter;
    protected float         m_DistanceToCenter;
    protected float         m_Angle;

    public float Offset { get { return m_DistanceToCenter; } }
    public float Angle { get { return m_Angle; } }
    public Vector2 PolarPosition { get { return new Vector2(Offset, Angle); } }

    protected bool          m_Initalized = false;

    protected bool          m_SpeedIsCapped = false;
    public void CapSpeed() { m_SpeedIsCapped = true; }
    public void UncapSpeed() { m_SpeedIsCapped = false; }

    virtual protected void Update()
    {
        if (m_Initalized)
            PositionInWorldSpace();
    }

    public void Initialize(float _angle, float _offset, Transform _center)
    {
        m_Animator = GetComponent<Animator>();
        //Sample lines
        //m_Animator.SetBool("X", false);
        //m_Animator.SetTrigger("XInstant");

        m_WorldCenter = _center;
        m_Angle = _angle;
        m_DistanceToCenter = _offset;

        transform.position = new Vector3(transform.position.x, m_WorldCenter.position.y, transform.position.z);

        m_LookDirection = m_WorldCenter.position - transform.position;
        transform.rotation = Quaternion.LookRotation(m_LookDirection, Vector3.up);
        m_TargetRotation = transform.rotation;

        m_Initalized = true;
    }

    public void PositionInWorldSpace()
    {
        Vector3 nextPosition = m_WorldCenter.position +
                       Vector3.back * m_DistanceToCenter * Mathf.Cos(Mathf.Deg2Rad * m_Angle) +
                       Vector3.right * m_DistanceToCenter * Mathf.Sin(Mathf.Deg2Rad * m_Angle);

        if (nextPosition != transform.position)
            m_LookDirection = nextPosition - transform.position;

        transform.position = nextPosition;

        m_TargetRotation = Quaternion.LookRotation(m_LookDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, m_TargetRotation, 0.5f);
    }

    static public Vector3 PolarToWorld(float _angle, float _offset, Vector3 _center)
    {
        return _center + 
               Vector3.back * _offset * Mathf.Cos(Mathf.Deg2Rad * _angle) +
               Vector3.right * _offset * Mathf.Sin(Mathf.Deg2Rad * _angle);
    }

    public void LookAt(Vector3 _position)
    {
        m_LookDirection = _position - transform.position;
    }

    protected void ImmediateLookAt(Vector3 _position)
    {
        LookAt(_position);

        transform.rotation = Quaternion.LookRotation(m_LookDirection, Vector3.up);
        m_TargetRotation = transform.rotation;
    }

    public void SetAnimatorPoseElement(string _poseName, bool value, bool instant = false)
    {
        if (m_Animator == null) return;
        if (instant)
        {
            m_Animator.SetTrigger(_poseName + "Instant");
            m_Animator.SetBool(_poseName, true);
        }
        else
            m_Animator.SetBool(_poseName, value);
    }
}
