using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OpenFogTile : MonoBehaviour
{
    [SerializeField]
    private Tilemap FogMap;
    [SerializeField]
    private TileBaseToAdd changabletiles;

    public void OpenFogTileAction()
    {
        Vector3Int underTile = FogMap.WorldToCell(this.transform.position);

        if (FogMap.GetTile(underTile) == changabletiles.tiles[0])
        {
            FogMap.SetTile(underTile, changabletiles.tiles[1]); 
        }
    }


}
