using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RightLeftPanelScript : MonoBehaviour
{

    private TMP_Text text;
    public int m_Index = 0;
    public int index
    {
        get
        {
            return m_Index;
        }
        set
        {
            m_Index = value;
            text.text = data[m_Index];
        }
    }
    public List<string> data = new List<string>();
    public int defaultValueIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = transform.Find("Text").GetComponent<TMP_Text>();
        transform.Find("ButtonLeft").GetComponent<Button>().onClick.AddListener(OnLeftClicked); 

        transform.Find("ButtonRight").GetComponent<Button>().onClick.AddListener(OnRightClicked);
        index = defaultValueIndex;
    }
    public string value
    {
        get
        {
            return data[m_Index];
        }
        set
        {
            data[m_Index] = value;
        }
    }
    public void OnLeftClicked()
    {
        if (index == 0)
        {
            index = data.Count - 1;
        }
        else
        {
            index--;
        }
    }
    public void OnRightClicked()
    {
        if((index + 1)>=data.Count)
        {
            index = 0;
        }
        else
        {
            index++;
        }
    }
}
