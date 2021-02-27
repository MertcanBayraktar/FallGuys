using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;
public class AIManager : MonoBehaviour
{
	NavMeshAgent myNavMeshAgent;
	[SerializeField] private GameObject finishLine;
	Animator _anim;
	[SerializeField] private Vector3 startingPosition;
	[SerializeField] private Quaternion startingRotation;
	Rigidbody rb;
	Vector3 startingVelocity;
	public static Action aiSetDestination;
	public static Action aiSpeed;
	public Vector3 remainingDistance;
	void Start()
	{
		aiSetDestination += DestinationSet;
		aiSpeed += AgentSpeed;
		rb = this.GetComponent<Rigidbody>();
		rb.velocity = startingVelocity;
		startingRotation = transform.rotation;
		startingPosition = transform.position;
		_anim = this.GetComponent<Animator>();
		myNavMeshAgent = GetComponent<NavMeshAgent>();

	}
	private void Update()
	{
		if (this.GetComponent<NavMeshAgent>().enabled)
		{
			if (myNavMeshAgent.remainingDistance > myNavMeshAgent.stoppingDistance)
			{
				_anim.SetBool("running", true);
			}
			if (myNavMeshAgent.velocity == Vector3.zero)
				_anim.SetBool("running", false);
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "rollingplatform")
			this.gameObject.transform.SetParent(collision.gameObject.transform);
		if (collision.transform.tag == "Reset_Obstacle")
		{
			this.GetComponent<NavMeshAgent>().enabled = false;
			this._anim.SetBool("FallFlat", true);
			StartCoroutine(ResetCharacter());
		}
		if (collision.transform.tag == "spinningstick")
		{
			this.GetComponent<NavMeshAgent>().enabled = false;
			this._anim.SetBool("FallFlat", true);
			Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
			if (rb != null)
			{
				Vector3 direction = transform.position - collision.transform.position;
				direction.y = 0;
				rb.AddForce(direction.normalized * 35f, ForceMode.Impulse);
			}
			StartCoroutine(ResetCharacter());
		}
	}
	private void OnCollisionExit(Collision collision)
	{
		this.gameObject.transform.parent = null;
	}
	IEnumerator ResetCharacter()
	{
		yield return new WaitForSeconds(1f);
		this._anim.SetBool("running", true);
		this._anim.SetBool("FallFlat", false);
		rb.velocity = startingVelocity;
		transform.position = startingPosition;
		transform.rotation = startingRotation;
		this.GetComponent<NavMeshAgent>().enabled = true;
		myNavMeshAgent.SetDestination(finishLine.transform.position);
	}
	public void DestinationSet()
	{
		myNavMeshAgent.SetDestination(new Vector3(finishLine.transform.position.x + (UnityEngine.Random.Range(-5, +5)), finishLine.transform.position.y, finishLine.transform.position.z + (UnityEngine.Random.Range(0, 5))));
	}
	public void AgentSpeed()
	{
		myNavMeshAgent.speed = 4;
	}
}
