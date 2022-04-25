using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PageController : MonoBehaviour
{
    public Button PageButton;

    private int activeChild = 0;

    void Start()
    {
        PageButton.onClick.AddListener(delegate { NextPage(); });
    }

    void NextPage()
    {
        //Debug.Log("Active child: " + activeChild);
        //Disables page
        transform.GetChild(activeChild).gameObject.SetActive(false);

        activeChild = (activeChild >= transform.childCount - 1) ? 0 : activeChild + 1;

        //Debug.Log("Active child: " + activeChild);
        //Activates new page
        transform.GetChild(activeChild).gameObject.SetActive(true);
    }
}