using UnityEngine;
using System.Collections;

public class AudioPlayer : Singleton<AudioPlayer>
{
	[SerializeField] private GameObject _nightFallGAO;
	[SerializeField] private GameObject _sunRisesGAO;
	[SerializeField] private GameObject _playerInputFailGAO;
	[SerializeField] private GameObject _playerInputWithRebelGAO;
	[SerializeField] private GameObject _playerOutcastGAO;

	public void PlayNightFall()
	{
		_nightFallGAO.GetComponent<AudioSource>().Play();
	}

	public void PlaySunRise()
	{
		_sunRisesGAO.GetComponent<AudioSource>().Play();
	}

	public void PlayPlayerInputFail()
	{
		_playerInputFailGAO.GetComponent<AudioSource>().Play();
	}

	public void PlayPlayerInputWithRebel()
	{
		_playerInputWithRebelGAO.GetComponent<AudioSource>().Play();
	}

	public void PlayPlayerOutcast()
	{
		_playerOutcastGAO.GetComponent<AudioSource>().Play();
	}
}