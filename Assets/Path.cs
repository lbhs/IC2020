using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {
	
	private Transform[] PathNodes;
	public float MoveSpeed;
	public GameObject Player;
	float Timer;
	static Vector3 CurrentPositionHolder;
	int CurrentNode;
	private Vector2 startPosition;

	// Use this for initialization
	void Start ()
	{
		PathNodes = GetComponentsInChildren<Transform>();
		CheckNode();
	}

	void CheckNode()
	{
		Timer = 0;
		startPosition = transform.position;
		CurrentPositionHolder = PathNodes[CurrentNode].position;
	}

	// Update is called once per frame
	void Update ()
	{
		Timer += Time.deltaTime * MoveSpeed;

		if (Player.transform.position != CurrentPositionHolder)
		{
			Player.transform.position = Vector3.Lerp(startPosition, CurrentPositionHolder, Timer);
		}
		else //if(CurrentNode <PathNode.Length -1)
		{
			CurrentNode++;
			//loop
			if(CurrentNode == PathNodes.Length)
			{
				CurrentNode = 0;
			}
			CheckNode();
		}
	}
}