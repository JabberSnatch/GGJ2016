using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
	#region Fields
	private int _currentLevelIndex = -1;
	private GameObject _currentLevel;

	[SerializeField]
	private List<GameObject> _levels;
	#endregion

	#region Properties
	public int CurrentLevelIndex
	{
		get { return _currentLevelIndex; }
	}

	public GameObject CurrentLevel
	{
		get { return _currentLevel; }
	}

	public List<GameObject> Levels
	{
		get { return _levels; }
	}
	#endregion

	public void LoadLevel(int levelIndex)
	{
		UnloadLevel();
		_currentLevelIndex = levelIndex;
		_currentLevel = Instantiate<GameObject>(_levels[_currentLevelIndex]);
	}

	private void UnloadLevel()
	{
		Destroy(_currentLevel);
		_currentLevelIndex = -1;
	}
}