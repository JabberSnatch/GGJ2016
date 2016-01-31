using UnityEngine;
using System.Collections;

public class FacingCameraElement : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
