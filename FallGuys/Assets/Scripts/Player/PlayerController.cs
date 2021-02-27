using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
	private Vector3 targetPosition;
	private Quaternion playerRot;
	private Vector3 lookAtTarget;
	[SerializeField] private float movSpeed;
	[SerializeField] private float rotationSpeed;
	[SerializeField] private Vector3 startingPoint;
	[SerializeField] private Quaternion startingRotation;
	Animator _anim;
	Rigidbody rb;
	bool moving = false;
	Vector3 startingVelocity;
	private float rotateVelocity;

	private void Start()
	{
		rb = this.GetComponent<Rigidbody>();
		startingVelocity = rb.velocity;
		startingPoint = this.transform.position;
		startingRotation = this.transform.rotation;
		_anim = GetComponent<Animator>();
		this._anim.SetBool("running", false);
	}
	void FixedUpdate()
	{
		if (GameManager.instance.levelState == GameManager.LevelState.Play)
		{
			if (Input.GetMouseButton(0))
			{
				SetTargetPosition();
			}
			if (moving)
				Move();
			if (this.transform.position.magnitude == 0)
				_anim.SetBool("running", false);
		}


	}

	private void SetTargetPosition()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			if (hit.transform.tag != "Player")
			{
				if (hit.transform.tag == "Ground" || hit.transform.tag == "rollingplatform")
				{
					targetPosition = hit.point;
					Quaternion rotationToLookAt = Quaternion.LookRotation(hit.point - transform.position);
					float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
						rotationToLookAt.eulerAngles.y,
						ref rotateVelocity,
						0.5f * (Time.deltaTime * 5));
					transform.eulerAngles = new Vector3(0, rotationY, 0);
					moving = true;
				}
			}
		}
	}
	private void Move()
	{
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, movSpeed * Time.fixedDeltaTime);
		_anim.SetBool("running", true);
		if (transform.position == targetPosition)
		{
			moving = false;
			_anim.SetBool("running", false);
		}
		if (Vector3.Distance(targetPosition, transform.position) <= 2)
		{
			moving = false;
			_anim.SetBool("running", false);
		}

	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "rollingplatform")
		{
			this.gameObject.transform.SetParent(collision.gameObject.transform);
		}
		if (collision.transform.tag == "spinningstick")
		{
			this._anim.SetBool("FallFlat", true);
			if (rb != null)
			{
				Vector3 direction = transform.position - collision.transform.position;
				direction.y = 0;
				rb.AddForce(direction.normalized * 20f, ForceMode.Impulse);
			}
		}
		if (collision.transform.name == "Water")
		{
			rb.velocity = startingVelocity;
			this._anim.SetBool("running", false);
			this.transform.position = startingPoint;
			this.transform.rotation = startingRotation;
		}

	}
	private void OnCollisionExit(Collision collision)
	{
		this.gameObject.transform.parent = null;
		if (collision.gameObject.tag == "spinningstick")
		{
			StartCoroutine(SetAnimationNormal());
		}

	}
	private void OnCollisionStay(Collision collision)
	{
		if (collision.transform.tag == "rollingplatform")
			this.gameObject.transform.SetParent(collision.gameObject.transform);
	}
	IEnumerator SetAnimationNormal()
	{
		yield return new WaitForSeconds(1.5f);
		this._anim.SetBool("running", true);
		this._anim.SetBool("FallFlat", false);
	}
}
