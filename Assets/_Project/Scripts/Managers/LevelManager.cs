using UnityEngine;
using System.Collections.Generic;

public class LevelManager : Singleton<LevelManager>
{
	#region Fields
	private int _currentLevelIndex = -1;
	private GameObject _currentLevel;

	[SerializeField]
	private List<GameObject> _levels;

    private int _ritualsCount = 0;
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

	public GameObject CurrentTimeline
	{
		get { return _currentLevel.GetComponent<LevelController>().Timeline; }
	}

	public List<GameObject> Levels
	{
		get { return _levels; }
	}

    public int RitualsCount
    {
        get { return _ritualsCount; }
    }
	#endregion

    void Start()
    {
        LoadLevel(0);
    }

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