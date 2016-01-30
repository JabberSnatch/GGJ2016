using UnityEngine;
using System.Collections.Generic;

public class InputCombination : MonoBehaviour
{
    [SerializeField]
    private List<EGamePadButton> _combination;

    public void Populate(params EGamePadButton[] inputs)
    {
        _combination.Clear();
        _combination = new List<EGamePadButton>(inputs.Length);

        foreach (EGamePadButton button in inputs)
            _combination.Add(button);
    }

    // might be useless or implemented differently, not sure about that yet
    public bool CheckCombination(InputCombination combination)
    {
        return _combination == combination._combination;
    }

    public void Randomize()
    {
        if (_combination.Count == 2)
            return;
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

    private string InputToAnimatorField(EGamePadButton button)
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
            case EGamePadButton.LeftShoulder:
                return "LeftButton";
            case EGamePadButton.LeftTrigger:
                return "LeftTrigger";
            case EGamePadButton.RightShoulder:
                return "RightButton";
            case EGamePadButton.RightTrigger:
                return "RightTrigger";
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

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
	#endregion
}