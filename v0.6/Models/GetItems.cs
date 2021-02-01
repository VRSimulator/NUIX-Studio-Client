﻿using Microsoft.MixedReality.Toolkit.UI;
using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class GetItems : MonoBehaviour
{
	private readonly string basePath = "http://localhost:8080/rest";

	List<EquipmentItemModel> EquipmentItems;
	List<string> EquipmentNames = new List<string>();
	List<ItemModel2> Items = new List<ItemModel2>();
	GameObject[] Things;
	[SerializeField] public GameObject CubeWithText;
	

	public void GetEquipmentItems()
	{
		// We can add default request headers for all requests
		//RestClient.DefaultRequestHeaders["Authorization"] = "Bearer eyJraWQiOm51bGwsImFsZyI6IlJTMjU2In0.eyJpc3MiOiJvcGVuaGFiIiwiYXVkIjoib3BlbmhhYiIsImV4cCI6MTYxMTUyODU0MCwianRpIjoiMlM3RS1NYTZ5QkxBVTRSOFNETUtEdyIsImlhdCI6MTYxMTUyNDk0MCwibmJmIjoxNjExNTI0ODIwLCJzdWIiOiJhZG1pbiIsImNsaWVudF9pZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6ODA4MCIsInNjb3BlIjoiYWRtaW4iLCJyb2xlIjpbImFkbWluaXN0cmF0b3IiXX0.NgikNqvk7A61e3z-KZOKgaRUxLmyZqgGYobwOPNZ6pga40vRakfjr-c0Mthw4i80zOc_36wCgD3ylYNDTdnAkpJZQ7TUZ_6ZNitLLuV-NO4skAOeJoYVr-gQTKPHScXTMWixxDBht-m7i93L8R8IgawOdlbGHGHjhe4gJMN1zrMQg78l0XsA_zoAU-pjMf5iCqTe1VV3X1MMFh4a6Q08SWagutm1SqPReuMOFeXd2GI81uu5eAxdihWQ0CUTcHPgIiXIrOpkLVUXYI6JmDzFWk6pU3nvrGy4MtQ7L3zvQrVZT-YPE_5-8Vu62z3yIDxn51P6r4W6I_CzBV2Sq5Dj0w";
		Debug.Log("Get Request");
		RequestHelper requestOptions = null;

		RestClient.Get(basePath + "/items?tags=Equipment").Then(res => {
			if (res.StatusCode >= 200 && res.StatusCode < 300)
			{
				EquipmentItems = JsonUtility.FromJson<EquipmentItemModelList>("{\"result\":" + res.Text + "}").result;
				SetEquipmentNames();
			}
			else
			{
				Debug.Log("Rest GET status for Item: " + " was not in OK span. (" + res.StatusCode + ")\n" + res.Error);
			}
		});
	}

	public void CreateGameObjectsWithNames()
	{
		GetEquipmentItems();
		Vector3 _shift = Vector3.one;
		float delta = 0.5f;
		foreach (string equipmentName in EquipmentNames)
        {

			GameObject createdItem;
			createdItem = Instantiate(CubeWithText, this.transform.position + _shift, Quaternion.identity) as GameObject;
			_shift.Set(_shift.x + delta, _shift.y, _shift.z);
			//createdItem.GetComponentInChildren<ToolTip>().ToolTipText = equipmentName;
			createdItem.transform.GetChild(0).GetComponent<ToolTip>().ToolTipText = equipmentName;
		}
	}




	public void ParseEquipmentItems()
    {

    }
	// Start is called before the first frame update
	void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void SetEquipmentNames()
    {
		print(EquipmentItems.Count);
		int count = 0;
		foreach (EquipmentItemModel model in EquipmentItems)
        {
			print($"Element #{count}: {model.name}");
			EquipmentNames.Add(model.name);
			count++;
			GetItemsByEquipmentName(model.name);
		}
    }

	public void GetItemsByEquipmentName(string equipmentName)
    {

		RestClient.Get(basePath + "/items/" + equipmentName).Then(res => {
			if (res.StatusCode >= 200 && res.StatusCode < 300)
			{
				print("Equipment " + equipmentName + " REST API items response " +  res.Text);
				EquipmentItemModel itemsOnEquipment = JsonUtility.FromJson<EquipmentItemModel>(res.Text);
				print(equipmentName + " state " + itemsOnEquipment.state);

				print(equipmentName + " has " + itemsOnEquipment.members.Capacity + " items");
				foreach (ItemModel2 model in itemsOnEquipment.members)
                {
					print(equipmentName + " member " + model.name);
                }
			}

			else
			{
				Debug.Log("Rest GET status for Item: " + " was not in OK span. (" + res.StatusCode + ")\n" + res.Error);
			}
		});
	}

	public void GetPointItems()
    {
		RestClient.Get(basePath + "/items?tags=Point").Then(res => {
			if (res.StatusCode >= 200 && res.StatusCode < 300)
			{
				Items = JsonUtility.FromJson<ItemListModel>("{\"itemList\":" + res.Text + "}").itemList;
			}
			else
			{
				Debug.Log("Rest GET status for Item: " + " was not in OK span. (" + res.StatusCode + ")\n" + res.Error);
			}
		});

		foreach (ItemModel2 model in Items)
		{
			print(model.name);
		}
	}


}
