using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameBuildings
{
    public string Name { get; set; }
    public bool planned { get; set; }
    public bool built { get; set; }

    public GameObject sprite { get; set; }
    public Button buttonToBuild { get; set; }

    public int cost { get; set; }


    public GameBuildings AddBuilding(string name, bool plan, bool ready, Button button, int cost)
    {
        GameBuildings building = new GameBuildings();
        building.Name = name;
        building.planned = plan;
        building.built = ready;
        building.buttonToBuild = button;
        building.cost = cost;
        return building;
    } 


}
public class Warehouse : MonoBehaviour
{
    private int Food;
    private int Resource;
    [SerializeField]
    private TextMeshProUGUI foodText , resourceText, storageLeft;
    [SerializeField]
    private GameObject Storage;
    [SerializeField]
    private GameObject RepairShop;
    [SerializeField]
    private GameObject LandingPlatform;
    [SerializeField]
    private int StorageCapacity;
    [SerializeField]
    private int FoodGoal;
    [SerializeField]
    private int ResourceGoal;

    [SerializeField]
    private bool[] gameGoals;
    private bool[] gameProgress = new bool[4];

    private int CurrentStorageCapacity;
    [SerializeField]
    private MessagePanel messagePanel;
    [SerializeField]
    private Button[] buildingsButtons;
    [SerializeField]
    private GameObject BuildButtonAmin;
    [SerializeField]
    private TimeController timeControl;

   


    private List<GameBuildings> gameBuildingsPlanned = new List<GameBuildings>();
    private GameBuildings constructor = new GameBuildings();

    [SerializeField]
    private GameObject BuildingMenu;
    public int amountLeft = 0;

    private bool swithBuildMenu = true;
    private void Start()
    {
        foodText.text = Food.ToString();
        resourceText.text = Resource.ToString();
        storageLeft.text = StorageCapacity.ToString();
        CurrentStorageCapacity = StorageCapacity;
        gameBuildingsPlanned.Add(constructor.AddBuilding("Storage Building", true, false, buildingsButtons[0], 10));
        gameBuildingsPlanned.Add(constructor.AddBuilding("Living Blocks", true, false, buildingsButtons[1], 20));


      
    }

    private void Update()
    {
        CheckForEndGame();
    }

    public void AddRemoveFoodOrResources(bool addRemove, bool foodResourse, int amount)
    {
        
        if (addRemove)
        {
            if(StorageCapacity - (Food + Resource) > 0)
            {
                if (foodResourse)
                {
                    amountLeft = StorageCapacity - amount;
                    if (amountLeft >= 0)
                    {
                        Food += amount;
                        StorageCapacity -= amount;
                        amountLeft = 0;
                    }
                    else
                    {
                        Food += amount + amountLeft;
                        amountLeft = Mathf.Abs(amountLeft);
                        StorageCapacity = 0;
                    }

                }
                else
                {
                    amountLeft = StorageCapacity - amount;
                    if (amountLeft >= 0)
                    {
                        Resource += amount;
                        StorageCapacity -= amount;
                        amountLeft = 0;
                    }
                    else
                    {
                        Resource += amount + amountLeft;
                        amountLeft = Mathf.Abs(amountLeft);
                        StorageCapacity = 0;
                    }
                }
            }
        }
        else
        {
            if (foodResourse)
            {
                Food -= amount;
                StorageCapacity += amount;
            }
            else
            {
                Resource -= amount;
                StorageCapacity += amount;
            }
        }
        
        foodText.text = Food.ToString();
        resourceText.text = Resource.ToString();
        storageLeft.text = StorageCapacity.ToString();
    }

    public void BuildStorage()
    {
        int resources = gameBuildingsPlanned[0].cost;
        if (resources <= Resource)
        {
            Storage.SetActive(true);
            AddRemoveFoodOrResources(false, false, resources);
            LandingPlatform.gameObject.tag = "Untagged";
            StorageCapacity += 50;
            CurrentStorageCapacity =StorageCapacity;
            storageLeft.text = StorageCapacity.ToString();
            OpenFogTile fog = Storage.GetComponent<OpenFogTile>();
            fog.OpenFogTileAction();
            gameBuildingsPlanned[0].built = true;
            buildingsButtons[0].interactable = false;
        }
        EndGameBuildandResourceCheck();
    }
    public void BuildLivingBlocks()
    {
        int resources = gameBuildingsPlanned[1].cost;
        if (resources <= Resource)
        {
            RepairShop.SetActive(true);
            AddRemoveFoodOrResources(false, false, resources);
            OpenFogTile fog = RepairShop.GetComponent<OpenFogTile>();
            fog.OpenFogTileAction();
            gameProgress[1] = true;
            gameBuildingsPlanned[1].built = true;
            buildingsButtons[1].interactable = false;
        }
        EndGameBuildandResourceCheck();
    }

    public void OpenBuildMenu()
    {
        swithBuildMenu = !swithBuildMenu;
        if (!swithBuildMenu) { BuildingMenu.SetActive(true); }
        else
        {
            BuildingMenu.SetActive(false); 
        }
    }

    public void CheckResource()
    {
        CheckStorageForResources(Resource, gameBuildingsPlanned);
    }

    private void CheckStorageForResources(int resource, List<GameBuildings> buildings)
    {
        string listOfBuildings = "";

        foreach (GameBuildings x in buildings)
        {
            if (resource >= x.cost & !x.built)
            {
                listOfBuildings = listOfBuildings + "\n" + x.Name;
                foreach(Button but in buildingsButtons)
                {
                    if (but == x.buttonToBuild )
                    {
                        but.interactable = true;
                        messagePanel.ActivateTextPanel(true);
                        messagePanel.PrintTextToPanel("You can build" + "\n" + listOfBuildings, false);
                    }
                }
            }
        }

      
    }

    public void CallForChoiceMenu(bool foodOrResource)
    {
        string message;
        if (foodOrResource) message = "Do you want waste all food?";
        else message = "Do you want destroy all resources";
        messagePanel.ActivateTextPanel(true);
        messagePanel.PrintTextToPanel(message, true);
        messagePanel.Yes.onClick.AddListener(delegate { EmptyFoodStored(foodOrResource); });
        messagePanel.No.onClick.AddListener(delegate { CancelMessage(); });
    }

    private void EmptyFoodStored(bool foodOrRsource)
    {
        if (foodOrRsource)
        {
            StorageCapacity += Food;
            Food = 0;
            foodText.text = Food.ToString();
            resourceText.text = Resource.ToString();
            storageLeft.text = StorageCapacity.ToString();
            messagePanel.ActivateTextPanel(false);
        }
        else EmptyResourceStored();
    }

    private void CancelMessage()
    {
        messagePanel.ActivateTextPanel(false);
    }

    private void EmptyResourceStored()
    {
        StorageCapacity += Resource;
        Resource = 0;
        foodText.text = Food.ToString();
        resourceText.text = Resource.ToString();
        storageLeft.text = StorageCapacity.ToString();
        messagePanel.ActivateTextPanel(false);
    }


    private void CheckForEndGame()
    {

        if (EndGameTimeCheck()|| EndGameBuildandResourceCheck())
        {
            Debug.Log("End Game");
            if (EndGameTimeCheck())
            {
                if (EndGameBuildandResourceCheck())
                {
                    SceneManager.LoadScene("GoodEnd");
                }
                else
                {
                    SceneManager.LoadScene("BadEnd");
                }
            }
            if (EndGameBuildandResourceCheck())
            {
                SceneManager.LoadScene("GoodEnd");
            }
        }
    }

    private bool EndGameTimeCheck()
    {
        bool end =false;

        if (timeControl.GivingTime <= 0)
        {
            end = true;
        }
        return end;
    }
    private bool EndGameBuildandResourceCheck()
    {
        bool end = false;
        int countbuilt=0, countplanned=0;
        foreach(GameBuildings building in gameBuildingsPlanned)
        {
            if (building.planned == true) countplanned++;
            if (building.built == true) countbuilt++;
        }
        if(countplanned == countbuilt)
        {
            end = true;
        }
        //Debug.Log(countplanned.ToString()+ "   vs. "+ countbuilt.ToString() );
        return end;
    }


    private void GoodEnd()
    {
        string message = "People from orbit comes saying \n \"how could we live here without proper internet connection \n Minecraft? HighPixel? Noooo! \"  \n And they lunch to space again";
        messagePanel.ActivateTextPanel(true);
        messagePanel.PrintTextToPanel(message, false);

    }

    private void BadEnd()
    {
        string message = "People at orbit died due to lack of negotiation skill with each other.";
        messagePanel.ActivateTextPanel(true);
        messagePanel.PrintTextToPanel(message, false);
    }


}


