using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTile : MonoBehaviour
{
    public int resource;
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private SpriteRenderer resourceSprite;




    private void Start()
    {
        resourceSprite.sprite = sprites[Random.Range(0, sprites.Length - 1)];

    }
}
