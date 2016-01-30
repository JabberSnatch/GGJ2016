using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static bool onApplicationQuit = false;

	private static T instance;
	public static T Instance
	{
		get
		{
			if (onApplicationQuit)
			{
				return null;
			}

			if (null == instance)
			{

				instance = GameObject.FindObjectOfType<T>();
				T[] instances = GameObject.FindObjectsOfType<T>();

				if (instances.Length > 1)
				{
					Debug.LogError("Something got wrong, because you have several singleton for the following type: " + typeof(T));
					return instance;
				}
			}

			return instance;
		}
	}

	private void onDestroy()
	{
		onApplicationQuit = true;
	}
}
