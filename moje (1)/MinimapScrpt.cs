using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScrpt : MonoBehaviour
{
    public Transform player;

    private void LateUpdate()
    {
        Vector3 newPos = player.transform.position;
        newPos.y = transform.position.y;
        transform.position = newPos;
    }
}
