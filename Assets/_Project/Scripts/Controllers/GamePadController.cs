using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class GamePadController : MonoBehaviour {

    [SerializeField] [Range(0f, 100f)] private float m_DampingFactor = 100f; // 0 is no movement, 100 is instant velocity change
    [SerializeField] private float m_Speed = 5f; // Speed in meters per sec

    private Vector3 m_TargetVelocity = Vector3.zero;
    private Vector3 m_Velocity = Vector3.zero;

    private GamePadState GPState;
    private GamePadState OldState;

    void Update()
    {
        OldState = GPState;
        GPState = GamePad.GetState(PlayerIndex.One);

        if (!GPState.IsConnected)
            Debug.LogError("No controller on PlayerIndex.One");

		// OnKeyDown event
        if (GPState.Buttons.A == ButtonState.Pressed && OldState.Buttons.A == ButtonState.Released)
        {
            GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value, 1.0f);
        }

        m_TargetVelocity = (Vector3.forward * GPState.ThumbSticks.Left.Y + 
                           Vector3.right * GPState.ThumbSticks.Left.X) * Time.deltaTime * m_Speed;
        //Debug.Log(GPState.ThumbSticks.Left.X + "; " + GPState.ThumbSticks.Left.Y);

        m_Velocity = Vector3.Lerp(m_Velocity, m_TargetVelocity, m_DampingFactor / 100f);

        transform.position += m_Velocity; 
    }

}
