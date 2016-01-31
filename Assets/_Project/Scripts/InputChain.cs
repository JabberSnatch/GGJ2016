using UnityEngine;
using System.Collections.Generic;

using XInputDotNetPure;

public class InputChain : MonoBehaviour
{
	public GameObject timeLine;

	[SerializeField]
	private List<InputCombination> _chain;

	public List<InputCombination> Chain 
	{
		get { return _chain; }
	}

	void Start()
	{
		for (int i = 0; i < _chain.Count; ++i)
		{
			_chain[i] = Instantiate(_chain[i]);
		}
	}

	void Update()
	{
		int i = 0;
		string s = "{";
		foreach (InputCombination input in _chain)
		{
			if (_chain[i].Combination[0] == EGamePadButton.None)
				s += i + "=, ";
			else
				s += i + "//, ";
			++i;
		}
		s = s.Remove(s.Length - 2);
		s += "}";

		//Debug.Log(s);
	}

	public bool Completed()
	{
		foreach (InputCombination combi in _chain)
		{
			if (!(combi.Combination[0] == EGamePadButton.None))
				return false;
		}

		return true;
	}

	public void NullifyCombination(int index)
	{
		_chain[index].Populate(EGamePadButton.None);
	}

	public void RefreshInputChain(params InputCombination[] InputManager)
	{
		_chain.Clear();
		int count = 0;
		while (count < InputManager.Length)
			_chain.Add(InputManager[count]);
	}
}