using UnityEngine;
using System.Collections;

using XInputDotNetPure;

public enum EGamePadButton
{
	A = 0,
	B,
	X,
	Y,
	LeftStick,
	LeftShoulder,
	RightStick,
	RightShoulder,
	Start,
	Back,
	Guide,
	None,

	NbStates
}

public class Inputs : Singleton<Inputs>
{
	private GamePadState _oldState;
	private GamePadState _newState;

	public bool IsPressed(EGamePadButton button)
	{
		switch ((int)button)
		{
			case 0:
				return _newState.Buttons.A == ButtonState.Pressed;
			case 1:
				return _newState.Buttons.B == ButtonState.Pressed;
			case 2:
				return _newState.Buttons.X == ButtonState.Pressed;
			case 3:
				return _newState.Buttons.Y == ButtonState.Pressed;
			case 4:
				return _newState.Buttons.LeftStick == ButtonState.Pressed;
			case 5:
				return _newState.Buttons.LeftShoulder == ButtonState.Pressed;
			case 6:
				return _newState.Buttons.RightStick == ButtonState.Pressed;
			case 7:
				return _newState.Buttons.RightShoulder == ButtonState.Pressed;
			case 8:
				return _newState.Buttons.Start == ButtonState.Pressed;
			case 9:
				return _newState.Buttons.Back == ButtonState.Pressed;
			case 10:
				return _newState.Buttons.Guide == ButtonState.Pressed;
			default:
					return false;
		}
	}

	public bool IsReleased(EGamePadButton button)
	{
		switch ((int)button)
		{
			case 0:
				return _newState.Buttons.A == ButtonState.Released;
			case 1:
				return _newState.Buttons.B == ButtonState.Released;
			case 2:
				return _newState.Buttons.X == ButtonState.Released;
			case 3:
				return _newState.Buttons.Y == ButtonState.Released;
			case 4:
				return _newState.Buttons.LeftStick == ButtonState.Released;
			case 5:
				return _newState.Buttons.LeftShoulder == ButtonState.Released;
			case 6:
				return _newState.Buttons.RightStick == ButtonState.Released;
			case 7:
				return _newState.Buttons.RightShoulder == ButtonState.Released;
			case 8:
				return _newState.Buttons.Start == ButtonState.Released;
			case 9:
				return _newState.Buttons.Back == ButtonState.Released;
			case 10:
				return _newState.Buttons.Guide == ButtonState.Released;
			default:
				return false;
		}
	}

	public bool WasPressed(EGamePadButton button)
	{
		switch ((int)button)
		{
			case 0:
				return _oldState.Buttons.A == ButtonState.Pressed;
			case 1:
				return _oldState.Buttons.B == ButtonState.Pressed;
			case 2:
				return _oldState.Buttons.X == ButtonState.Pressed;
			case 3:
				return _oldState.Buttons.Y == ButtonState.Pressed;
			case 4:
				return _oldState.Buttons.LeftStick == ButtonState.Pressed;
			case 5:
				return _oldState.Buttons.LeftShoulder == ButtonState.Pressed;
			case 6:
				return _oldState.Buttons.RightStick == ButtonState.Pressed;
			case 7:
				return _oldState.Buttons.RightShoulder == ButtonState.Pressed;
			case 8:
				return _oldState.Buttons.Start == ButtonState.Pressed;
			case 9:
				return _oldState.Buttons.Back == ButtonState.Pressed;
			case 10:
				return _oldState.Buttons.Guide == ButtonState.Pressed;
			default:
				return false;
		}
	}

	public bool WasReleased(EGamePadButton button)
	{
		switch ((int)button)
		{
			case 0:
				return _oldState.Buttons.A == ButtonState.Released;
			case 1:
				return _oldState.Buttons.B == ButtonState.Released;
			case 2:
				return _oldState.Buttons.X == ButtonState.Released;
			case 3:
				return _oldState.Buttons.Y == ButtonState.Released;
			case 4:
				return _oldState.Buttons.LeftStick == ButtonState.Released;
			case 5:
				return _oldState.Buttons.LeftShoulder == ButtonState.Released;
			case 6:
				return _oldState.Buttons.RightStick == ButtonState.Released;
			case 7:
				return _oldState.Buttons.RightShoulder == ButtonState.Released;
			case 8:
				return _oldState.Buttons.Start == ButtonState.Released;
			case 9:
				return _oldState.Buttons.Back == ButtonState.Released;
			case 10:
				return _oldState.Buttons.Guide == ButtonState.Released;
			default:
				return false;
		}
	}

	void Start()
	{
		_oldState = GamePad.GetState(PlayerIndex.One);
		_newState = GamePad.GetState(PlayerIndex.One);
	}

	void Update()
	{
		_oldState = _newState;
		_newState = GamePad.GetState(PlayerIndex.One);
	}
};