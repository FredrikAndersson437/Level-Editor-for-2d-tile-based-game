using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileElementEditor : MonoBehaviour
{
    public void ReplaceWithNewTile(GameObject newTile)
    {
        Instantiate(newTile, transform.position, transform.rotation);
        DestroyImmediate(gameObject);
    }
}
