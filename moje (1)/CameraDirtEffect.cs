using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraDirtEffect : MonoBehaviour
{
    public Image image;
    public GameObject dirtPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(ScreenDirt());
        }
        
    }

    public IEnumerator ScreenDirt()
    {
        Vector2 pos = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
        var makeDirt = Instantiate(image, pos, Quaternion.identity) as Image;
        makeDirt.transform.SetParent(dirtPanel.transform);
        float targetAlpha = 0f;
        Color curColor = makeDirt.material.color;
        Debug.Log(curColor.a);
        while (Mathf.Abs(targetAlpha - curColor.a) > 0.0001f)
        {
            curColor.a = Mathf.Lerp(curColor.a, targetAlpha, 1f * Time.deltaTime);
            makeDirt.color = curColor;
            yield return null;

        }
        Destroy(makeDirt.gameObject);
    }
}
