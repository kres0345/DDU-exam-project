using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FactoryGame.Placements;
using FactoryGame.Placements;

public class FindSpecificStorageView : MonoBehaviour
{
    public void FindStorageView(ContainerInventory inventory)
    {
        SpecificStorageView.Instance.ShowStorageView(inventory);
    }

    public void FindMachineRecipeView(Machine machine)
	{
        MachineRecipeSelectView.Instance.ShowSelectRecipeView(machine);
    }
}
