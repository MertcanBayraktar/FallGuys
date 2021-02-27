using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	#region Singleton
	public static GameManager instance;
	private void Awake()
	{
		if (instance != null)
			Debug.Log("More than one GM in scene");
		instance = this;
	}
	#endregion
	public enum LevelState { Begin, Countdown, Play,End }
	public LevelState levelState;
	public TMP_Text countDownTimer;
	public GameObject[] characters;
	public GameObject finishLine;
	public TMP_Text racingPosition;
	private int racingPos;
	bool AiSetToGo = false;
	void Start()
	{
		levelState = LevelState.Begin;
	}

	// Update is called once per frame
	void Update()
	{
		if (levelState == LevelState.Begin && Input.GetMouseButtonDown(0))
		{
			StartCoroutine(Countdown(3));
			if (!AiSetToGo)
				AIManager.aiSetDestination();
			AiSetToGo = true;

		}
		if (levelState == LevelState.Play)
		{
			AIManager.aiSpeed();
			ClosestToFinishLine();
		}
		if(levelState == LevelState.End)
		{
			if (racingPos == 1)
			{
				Debug.Log("Player Has Won");
			}
			else
				Debug.Log("Player Has Lost");
		}
	}
	IEnumerator Countdown(int seconds)
	{
		levelState = LevelState.Countdown;
		for (int i = seconds; i > 0; i--)
		{
			countDownTimer.text = i.ToString();
			yield return new WaitForSeconds(1f);
		}
		levelState = LevelState.Play;
		countDownTimer.enabled = false;
	}
	void ClosestToFinishLine()
	{
		characters = characters.OrderBy(point => Vector3.Distance(finishLine.transform.position, point.transform.position)).ToArray();
		racingPos= Array.IndexOf(characters, GameObject.FindGameObjectWithTag("Player"));
		racingPos += 1;
		racingPosition.text = racingPos.ToString() + " / " + "11";
	}
}

