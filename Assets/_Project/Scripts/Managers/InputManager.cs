using UnityEngine;
using System.Collections.Generic;

using XInputDotNetPure;

public enum EGamePadButton
{
	A = 0,
	B,
	X,
	Y,
	LeftTrigger,
	LeftShoulder,
	RightTrigger,
	RightShoulder,
	Start,
	Back,
	Guide,
	None,

	NbStates
}

public class InputManager : Singleton<InputManager>
{
	private List<bool> _snapshot = new List<bool>((int)EGamePadButton.NbStates - 1);

	private GamePadState _oldState;
	private GamePadState _newState;

	public List<bool> Snapshot
	{
		get { return _snapshot; }
	}

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
				return _newState.Triggers.Left >= 0.8f;
			case 5:
				return _newState.Buttons.LeftShoulder == ButtonState.Pressed;
			case 6:
				return _newState.Triggers.Right >= 0.8f;
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
				return _newState.Triggers.Left < 0.8f;
			case 5:
				return _newState.Buttons.LeftShoulder == ButtonState.Released;
			case 6:
				return _newState.Triggers.Right < 0.8f;
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
				return _oldState.Triggers.Left >= 0.8f;
			case 5:
				return _oldState.Buttons.LeftShoulder == ButtonState.Pressed;
			case 6:
				return _oldState.Triggers.Right >= 0.8f;
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
				return _oldState.Triggers.Left < 0.8f;
			case 5:
				return _oldState.Buttons.LeftShoulder == ButtonState.Released;
			case 6:
				return _oldState.Triggers.Right < 0.8f;
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

		for (int i = 0; i < _snapshot.Capacity; ++i)
		{
			_snapshot.Add(false);
		}
	}

	void Update()
	{
		_oldState = _newState;
		_newState = GamePad.GetState(PlayerIndex.One);

		UpdateSnapshot();
	}

	void UpdateSnapshot()
	{
		_snapshot[0] = _newState.Buttons.A == ButtonState.Pressed ? true : false;
		_snapshot[1] = _newState.Buttons.B == ButtonState.Pressed ? true : false;
		_snapshot[2] = _newState.Buttons.X == ButtonState.Pressed ? true : false;
		_snapshot[3] = _newState.Buttons.Y == ButtonState.Pressed ? true : false;
		_snapshot[4] = _newState.Triggers.Left >= 0.8f ? true : false;
		_snapshot[5] = _newState.Buttons.LeftShoulder == ButtonState.Pressed ? true : false;
		_snapshot[6] = _newState.Triggers.Right >= 0.8f ? true : false;
		_snapshot[7] = _newState.Buttons.RightShoulder == ButtonState.Pressed ? true : false;
		_snapshot[8] = _newState.Buttons.Start == ButtonState.Pressed ? true : false;
		_snapshot[9] = _newState.Buttons.Back == ButtonState.Pressed ? true : false;
		_snapshot[10] = _newState.Buttons.Guide == ButtonState.Pressed ? true : false;
	}
};