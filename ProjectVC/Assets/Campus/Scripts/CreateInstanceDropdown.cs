using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


public class CreateInstanceDropdown : MonoBehaviour
{

    public List<GameObject> prefabList;
    public Transform spawnPosition;
    private GameObject creatingGameObject;

    
    public Dropdown sceneDropdownMenu;
    private int menuIndex = 0;
    List<Dropdown.OptionData> menuOptions;

    private void Update()
    {
        menuIndex = sceneDropdownMenu.GetComponent<Dropdown>().value;
        //menuOptions = sceneDropdownMenu.GetComponent<Dropdown>().options;
        //Debug.Log("Index: " + menuIndex);
    }

    public void CreateNewInstance()
    {
        creatingGameObject = prefabList[menuIndex];
        Instantiate(creatingGameObject, spawnPosition.position, quaternion.identity);
    }


}
