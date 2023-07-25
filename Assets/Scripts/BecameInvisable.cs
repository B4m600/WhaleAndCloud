using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BecameInvisable : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile tile;
    public Rigidbody2D rb;

    void OnBecameInvisible()
    {
        Vector3Int pos = new Vector3Int((int)rb.position.x, (int)rb.position.y, 0);
        tilemap.SetTile(pos, tile);
        
    }
}
