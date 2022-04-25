using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryGame.UI
{
    public class ScrollViewContentController : MonoBehaviour
    {
        public float offset;

        RectTransform rect;
        RectTransform childTransform;

        void Start()
        {
            rect = gameObject.GetComponent<RectTransform>();
			
            UpdateContentSize();
        }

        public void UpdateContentSize()
        {
            if (!(transform.childCount > 0))
                return;

            childTransform = transform.GetChild(0).GetComponent<RectTransform>();

            if (childTransform != null)
            {
                rect.sizeDelta = new Vector2(0, (childTransform.sizeDelta.y + offset) * transform.childCount);
                Debug.Log(childTransform.sizeDelta.y + " : " + offset + " : " + transform.childCount);
            }
        }
    }
}