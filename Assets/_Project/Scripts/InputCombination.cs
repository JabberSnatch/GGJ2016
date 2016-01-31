using UnityEngine;
using System.Collections.Generic;

public class InputCombination : MonoBehaviour
{
	static public int GamePadKeyCount = 8;

    [SerializeField]
    private List<EGamePadButton> _combination = new List<EGamePadButton>();

	private List<EGamePadButton> _savedCombination = new List<EGamePadButton>();

	public List<EGamePadButton> Combination
	{
		get { return _combination; }
	}

    public void Populate(params EGamePadButton[] InputManager)
    {
        _combination.Clear();
        _combination = new List<EGamePadButton>(InputManager.Length);

        foreach (EGamePadButton button in InputManager)
            _combination.Add(button);
    }

    public InputCombination Randomize()
    {
		GameObject combinationGAO = new GameObject();
		combinationGAO.AddComponent<InputCombination>();

		InputCombination ret = combinationGAO.GetComponent<InputCombination>();

		float seed0, seed1, seed2 = 0.0f;

		if (_combination.Count == 1)
		{
			while ((seed0 = Random.Range(0, GamePadKeyCount)) == (int)_combination[0]) { }
			ret.Populate((EGamePadButton)seed0);
		}
		else if (_savedCombination.Count == 2)
		{
			while ((seed0 = Random.Range(0, GamePadKeyCount)) == (int)_combination[0]) { }
			while ((seed1 = Random.Range(0, GamePadKeyCount)) == (int)_combination[1]) { }
			ret.Populate((EGamePadButton)seed0, (EGamePadButton)seed1);
		}
		else if (_savedCombination.Count == 3)
		{
			while ((seed0 = Random.Range(0, GamePadKeyCount)) == (int)_combination[0]) { }
			while ((seed1 = Random.Range(0, GamePadKeyCount)) == (int)_combination[1]) { }
			while ((seed2 = Random.Range(0, GamePadKeyCount)) == (int)_combination[2]) { }
			ret.Populate((EGamePadButton)seed0, (EGamePadButton)seed1, (EGamePadButton)seed2);
		}

		return ret;
    }

    public List<string> ToAnimatorGrammar()
    {
        List<string> result = new List<string>();

        foreach(var input in _combination)
        {
            string name = InputToAnimatorField(input);
            if (name != "")
                result.Add(name);
        }

        return result;
    }

    static public string InputToAnimatorField(EGamePadButton button)
    {
        switch (button)
        {
            case EGamePadButton.A:
                return "A";
            case EGamePadButton.B:
                return "B";
            case EGamePadButton.X:
                return "X";
            case EGamePadButton.Y:
                return "Y";
			case EGamePadButton.LeftTrigger:
				return "LeftTrigger";
			case EGamePadButton.LeftShoulder:
                return "LeftButton";
            case EGamePadButton.RightTrigger:
                return "RightTrigger";
			case EGamePadButton.RightShoulder:
				return "RightButton";
            case EGamePadButton.None:
                return "None";
			default:
                return "";
        }
    }

	#region OperatorOverride
	public override bool Equals(System.Object obj)
	{
		InputCombination inputCombination = obj as InputCombination;
		if ((object)inputCombination == null)
		{
			return false;
		}

		return base.Equals(obj) && _combination == inputCombination._combination;
	}

	public static bool operator == (InputCombination a, InputCombination b)
	{
		int aSize = a._combination.Count;
		int bSize = b._combination.Count;

		if (aSize != bSize)
			return false;
		else
		{
			foreach (EGamePadButton button in a._combination)
			{
				if (!b._combination.Contains(button))
					return false;
			}

			return true;
		}
	}

	public static bool operator == (List<EGamePadButton> a, InputCombination b)
	{
		int aSize = a.Count;
		int bSize = b._combination.Count;

		if (aSize != bSize)
			return false;
		else
		{
			foreach (EGamePadButton button in a)
			{
				if (!b._combination.Contains(button))
					return false;
			}

			return true;
		}
	}

	public static bool operator != (InputCombination a, InputCombination b)
	{
		int aSize = a._combination.Count;
		int bSize = b._combination.Count;

		if (aSize != bSize)
			return true;
		else
		{
			foreach (EGamePadButton button in a._combination)
			{
				if (!b._combination.Contains(button))
					return true;
			}

			return false;
		}
	}

	public static bool operator != (List<EGamePadButton> a, InputCombination b)
	{
		int aSize = a.Count;
		int bSize = b._combination.Count;

		if (aSize != bSize)
			return true;
		else
		{
			foreach (EGamePadButton button in a)
			{
				if (!b._combination.Contains(button))
					return true;
			}

			return false;
		}
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
	#endregion
}