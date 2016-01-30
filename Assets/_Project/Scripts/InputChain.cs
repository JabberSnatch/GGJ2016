using UnityEngine;
using System.Collections.Generic;

using XInputDotNetPure;

public class InputChain : MonoBehaviour
{
	public GameObject timeLine;

	[SerializeField]
	private List<EGamePadButton> _chain;

	public List<EGamePadButton> Chain 
	{
		get { return _chain; }
	}

	public void RefreshInputChain(params EGamePadButton[] inputs)
	{
		_chain.Clear();
		int count = 0;
		while (count < inputs.Length)
			_chain.Add(inputs[count]);
	}
}