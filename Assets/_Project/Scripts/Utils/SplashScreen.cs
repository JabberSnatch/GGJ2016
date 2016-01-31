using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour
{
    public void Awake()
    {
        StartCoroutine(waitCoroutine());
    }

    public IEnumerator waitCoroutine()
    {
        yield return new WaitForSeconds(8f);
        Application.LoadLevel("prod_level");
    }
}
