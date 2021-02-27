using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float goUpForce;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	private void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce((collision.gameObject.transform.forward * jumpForce) + (Vector3.up * goUpForce),ForceMode.Impulse);
        }
    }
}
