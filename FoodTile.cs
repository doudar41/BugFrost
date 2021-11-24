using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTile : MonoBehaviour
{
    public int food;
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private SpriteRenderer foodSprite;

    private void Start()
    {
        foodSprite.sprite = sprites[ Random.Range(0, sprites.Length-1)];

    }

}
