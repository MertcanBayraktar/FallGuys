using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class HalfDonut : MonoBehaviour
{
    [SerializeField] private GameObject objectToMove;
    [SerializeField] private Vector3 donutForwardCoordinate;
    [SerializeField] private Vector3 donutBackwardCoordinate;
    void Start()
    {
        int RepeatSeconds = Random.Range(6, 10);
        InvokeRepeating("DonutMotionBackwards", 1, RepeatSeconds);
    }
    void DonutMotionBackwards()
	{
        objectToMove.transform.DORotate(new Vector3(90, 0, 0), 2,RotateMode.LocalAxisAdd);
	}
}
