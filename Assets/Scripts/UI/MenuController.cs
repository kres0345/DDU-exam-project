using System.Collections.Generic;
using UnityEngine;

namespace FactoryGame.UI
{
    public class MenuController : MonoBehaviour
	{
		public List<GameObject> menues = new List<GameObject>();
		public Menues lastMenu;

		private float moveTime = 0.5f;
		private LeanTweenType tweenType = LeanTweenType.easeInOutSine;
		private GameObject gameObj;
		private Vector3 moveTo;

		public void Awake()
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				menues.Add(transform.GetChild(i).gameObject);
			}
		}

		public enum Menues
		{
			Main = 0,
			Saves = 1, // is never used.
			Achievements = 2, // is never used.
			Settings = 3, // is never used.
			Tutorial = 4, // is never used.
			Left = 5,
			Middle = 6,
			Right = 7
		}

		public void Move(Menues thisMenu, bool MoveToVisible)
		{
			if (MoveToVisible)
			{
				gameObj = menues[(int)thisMenu].gameObject;
				moveTo = menues[(int)Menues.Middle].transform.position;
				LeanTween.move(gameObj, moveTo, moveTime).setEase(tweenType);
				
				gameObj = menues[(int)Menues.Main].gameObject;
				moveTo = menues[(int)Menues.Left].transform.position;
				LeanTween.move(gameObj, moveTo, moveTime).setEase(tweenType);

				//menues[(int)thisMenu].transform.LeanMove(menues[(int)Menues.Middle].transform.position, moveTime);
				//menues[(int)Menues.Main].transform.LeanMove(menues[(int)Menues.Left].transform.position, moveTime);
			}
			else
			{
				gameObj = menues[(int)thisMenu].gameObject;
				moveTo = menues[(int)Menues.Right].transform.position;
				LeanTween.move(gameObj, moveTo, moveTime).setEase(tweenType);

				gameObj = menues[(int)Menues.Main].gameObject;
				moveTo = menues[(int)Menues.Middle].transform.position;
				LeanTween.move(gameObj, moveTo, moveTime).setEase(tweenType);

				//menues[(int)thisMenu].transform.LeanMove(menues[(int)Menues.Right].transform.position, moveTime);
				//menues[(int)Menues.Main].transform.LeanMove(menues[(int)Menues.Middle].transform.position, moveTime);
			}
		}

		public void QuitGame()
		{
			Application.Quit();
		}
	}
}