using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BugAI : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;

    private Transform target;
    [SerializeField]
    private Camera cameraMain;
    [SerializeField]
    private Tilemap map;
    [SerializeField]
    private Tilemap selectionMap;
    [SerializeField]
    private Button takeItAll, throwItAll;
    [SerializeField]
    private Text takeButtonText;
    [SerializeField]
    private Text throwButtonText;
    [SerializeField]
    private Warehouse wareHouse;
    [SerializeField]
    private TileBaseToAdd changabletiles;
    [SerializeField]
    private BoxCollider2D collid;
    [SerializeField]
    private Button buildStorage;
    [SerializeField]
    private Button buildRepairShop;
    [SerializeField]
    private MessagePanel messagePanel;



    private Collision2D currentCollision;



    public float StartTime { get; private set; }
    public float TimeOfOneTurn = 2.0f;
    public int TurnCount { get; private set; }

    private Vector3 mousePosition;
    private int foodCarrying =0;
    private int resourceCarrying =0;
    private TileBase targetTile;
    private float speedBug = 10;
    static float t = 1.0f;
    private Quaternion rot;
    private bool isMoving = false;
    private FMOD.Studio.EventInstance movingSound;

    // Start is called before the first frame update
    void Start()
    {
        movingSound = FMODUnity.RuntimeManager.CreateInstance("event:/Moving");
        agent.speed = 0.9f; 
        agent.acceleration = 200.0f;
        mousePosition = transform.position;
    }

    [System.Obsolete]
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            if (Time.time - StartTime > TimeOfOneTurn)
            {
                StartTime = Time.time;
                TurnCount++;
            }

            FillSelectionTile();
            if (Input.GetMouseButtonDown(0))
            {
                selectionMap.SetTile(selectionMap.WorldToCell(mousePosition), changabletiles.tiles[1]);
                targetTile = null;


                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (messagePanel.Panel.active) 
                    {
                        messagePanel.ActivateTextPanel(false);
                    }
                    mousePosition = cameraMain.ScreenToWorldPoint(Input.mousePosition);
                    targetTile = map.GetTile(map.WorldToCell(mousePosition));
                    selectionMap.SetTile(selectionMap.WorldToCell(mousePosition), changabletiles.tiles[2]);
                }
            }
            rot.eulerAngles = new Vector3(0,0, AngleTo2(new Vector2(transform.position.x, transform.position.y), new Vector2(mousePosition.x, mousePosition.y))+90);
            transform.rotation = rot;
            agent.SetDestination(mousePosition);
            PlaySoundWhenMove();
        }
    }

    private float AngleTo2(Vector2 pos, Vector2 target)
    {
        Vector2 diference = target - pos;
        float sign = (target.y < pos.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }


    private void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "FoodTile" || col.gameObject.tag == "ResourceTile")
        {
            if (col.gameObject.tag == "FoodTile")
            {
                if (resourceCarrying == 0)
                {
                    FoodTile x = col.gameObject.GetComponent<FoodTile>();
                    takeButtonText.text = "Take " + x.food.ToString() + " Food";
                    takeItAll.interactable = true;
                }
            }

            if (col.gameObject.tag == "ResourceTile")
            {
                if (foodCarrying == 0)
                {
                    ResourceTile res = col.gameObject.GetComponent<ResourceTile>();
                    takeButtonText.text = "Take " + res.resource.ToString() + " Resource";
                    takeItAll.interactable = true;
                }
            }
        }

        currentCollision = col;
        if (col.gameObject.tag == "Storage")
        {

            if (foodCarrying > 0)
            {
                wareHouse.AddRemoveFoodOrResources(true, true, foodCarrying);
                foodCarrying = wareHouse.amountLeft;
                throwButtonText.text = "Throw " + foodCarrying.ToString() + " Food";
                
            }

            else if (resourceCarrying > 0)
            {
                wareHouse.AddRemoveFoodOrResources(true, false, resourceCarrying);
                resourceCarrying = wareHouse.amountLeft;
                throwButtonText.text = "Throw " + resourceCarrying.ToString() + " Resources";
                wareHouse.CheckResource();
            }


            if (foodCarrying == 0 & resourceCarrying == 0)
            {
                throwItAll.interactable = false;
                throwButtonText.text = "Nothing To Throw";
            }
        }
        Debug.Log(currentCollision.gameObject.name);
    }



    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.name == currentCollision.gameObject.name)
        {
            takeButtonText.text = "Searching";
            takeItAll.interactable = false;
        }
    }

    public void ThrowItAllAction()
    {
        foodCarrying = 0;
        resourceCarrying = 0;
        throwItAll.interactable = false;
    }

    public void TakeItAllAction()
    {
        if (currentCollision.gameObject.tag == "FoodTile")
        {
            if (resourceCarrying == 0)
            {
                FoodTile x = currentCollision.gameObject.GetComponent<FoodTile>();
                foodCarrying += x.food;
                x.food = 0;
                throwButtonText.text = "Throw " + foodCarrying.ToString() + " Food";
            }
        }

        if (currentCollision.gameObject.tag == "ResourceTile")
        {
            if (foodCarrying == 0)
            {
                ResourceTile res = currentCollision.gameObject.GetComponent<ResourceTile>();
                resourceCarrying += res.resource;
                res.resource = 0;
                throwButtonText.text = "Throw " + resourceCarrying.ToString() + " Resource";

            }

        }
        throwItAll.interactable = true;
        takeItAll.interactable = false;
    }

    private void FillSelectionTile()
    {
       Vector3Int underTile =  map.WorldToCell(transform.position);

        if (map.GetTile(underTile) == changabletiles.tiles[0])
        {
            map.SetTile(underTile, changabletiles.tiles[1]);selectionMap.SetTile(underTile, changabletiles.tiles[1]);
        }

    }


    private void PlaySoundWhenMove()
    {
       if(agent.velocity != new Vector3(0,0,0) & !isMoving)
        {
            isMoving = true;
            movingSound.start();
        }
        if (agent.velocity == new Vector3(0, 0, 0) & isMoving)
        {
            isMoving = false;
            movingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

    }


}
