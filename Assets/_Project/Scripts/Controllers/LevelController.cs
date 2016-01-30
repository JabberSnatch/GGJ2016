using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour
{
	[SerializeField]
	private GameObject _timeline;

	public GameObject Timeline
	{
		get { return _timeline; }
	}
}