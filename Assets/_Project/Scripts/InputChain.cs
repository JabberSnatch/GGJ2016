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