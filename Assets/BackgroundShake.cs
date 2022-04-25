using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundShake : MonoBehaviour
{
    public float LeanTime;
    
	private Vector3 lastPos;
	private Vector3 startPos;

	private void Start()
	{
		startPos = transform.position;
		//Debug.Log("Start: " + startPos);
	}

	void Update()
    {
        if (transform.position == lastPos)
		{
			Vector3 newPos = new Vector2(startPos.x * Random.Range(0.85f, 1.15f), startPos.y * Random.Range(0.85f, 1.15f));
			//Debug.Log(newPos);
            transform.LeanMove(newPos, LeanTime).setEase(LeanTweenType.linear);
		}

        lastPos = transform.position;
    }
}