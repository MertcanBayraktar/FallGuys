using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingPart : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerPaintingPosition;
    [SerializeField] private Camera _mainCameraDisable;
    [SerializeField] private float movSpeed;
    [Header("Painting Object Settings")]
    [SerializeField] private GameObject GUI;
    [SerializeField] private GameObject DynamicCanvas;
    [SerializeField] private GameObject MainPaintingScene;
    public bool playerHasFinished = false;
    void Update()
    {
        if (GameManager.instance.levelState == GameManager.LevelState.End)
        {
            if (playerHasFinished)
                MoveToPaintingArea();
        }
       
    }
	private void OnTriggerEnter(Collider other)
	{
        if(other.transform.tag == "Player")
		{
            playerHasFinished = true;
            GameManager.instance.levelState = GameManager.LevelState.End;
        }
	}
	void MoveToPaintingArea()
	{
        Vector3 lookAtTarget = new Vector3(playerPaintingPosition.transform.position.x - player.transform.position.x, player.transform.position.y, playerPaintingPosition.transform.position.z - player.transform.position.z);
        Quaternion playerRot = Quaternion.LookRotation(lookAtTarget);
        player.GetComponent<PlayerController>().enabled = false;
        transform.rotation = Quaternion.Slerp(transform.rotation, playerRot, 5f * Time.deltaTime);
        player.transform.position = Vector3.MoveTowards(player.transform.position, playerPaintingPosition.transform.position, movSpeed * Time.deltaTime);
        if (Vector3.Distance(player.transform.position,playerPaintingPosition.transform.position) <= 1)
		{
            player.GetComponent<Animator>().SetBool("running", false);
            player.GetComponent<Rigidbody>().isKinematic = true;
            GUI.SetActive(true);
            DynamicCanvas.SetActive(true);
            MainPaintingScene.SetActive(true);
		}
            
    }
}
