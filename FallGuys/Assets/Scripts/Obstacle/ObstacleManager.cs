using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ObstacleManager : MonoBehaviour
{
    public GameObject objectToMove;
    public PathType pathSystem = PathType.Linear;
    public Vector3[] pathVal = new Vector3[5];
    [SerializeField] private float movSpeed;
    void Start()
    {
        int RepeatSeconds = Random.Range(4, 10);
        InvokeRepeating("StartPatrol", 1f,RepeatSeconds);
    }
    void StartPatrol()
	{
        objectToMove.transform.DOPath(pathVal, movSpeed, pathSystem);
    }
	private void OnCollisionEnter(Collision collision)
	{
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
		if (rb != null)
		{
            Vector3 direction = collision.transform.position - transform.position;
            direction.y = 0;
            rb.AddForce(direction.normalized * 10f, ForceMode.Impulse);
		}
	}
}

