using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPool : MonoBehaviour
{

    private static ObjectPool _instance;
    public static ObjectPool Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("Poolobject is null");
            }
            return _instance;
        }
    }
    [SerializeField]
    private GameObject _imagePrefab;
    [SerializeField]
    private List<GameObject> _imagePool;
    public GameObject imageContainer;
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        _imagePool = GenerateImages(10);
    }
    List<GameObject> GenerateImages(int amountOfImages)
    {
        for (int i = 0; i<amountOfImages; i++)
        {
            GameObject image = Instantiate(_imagePrefab);
            image.transform.SetParent(imageContainer.transform);
            image.SetActive(false);
            _imagePool.Add(image);
            
        }
        return _imagePool;
    }

    public GameObject RequestImage()
    {
        foreach(var image in _imagePool)
        {
            if(image.activeInHierarchy == false)
            {
                image.SetActive(true);
                return image;
            }
        }
        GameObject newImage = Instantiate(_imagePrefab);
        newImage.transform.SetParent(imageContainer.transform);
        _imagePool.Add(newImage);
        return newImage;
        
    }

}
