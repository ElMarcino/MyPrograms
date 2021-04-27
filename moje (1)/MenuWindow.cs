using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWindow : MonoBehaviour
{
    public GameObject root;
    public void ShowWindow()
    {
        root.SetActive(true);
    }
    public void HideWindow()
    {
        root.SetActive(false);
    }

}
