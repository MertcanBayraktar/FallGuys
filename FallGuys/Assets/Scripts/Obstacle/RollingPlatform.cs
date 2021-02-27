using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RollingPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, 22) * Time.deltaTime);
    }
}
