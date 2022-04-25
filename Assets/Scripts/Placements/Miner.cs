using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FactoryGame.Placements;

public class Miner : MonoBehaviour
{
    public Item bauxite;
    public Item hematite;
    public Item malachite;

    public ContainerInventory OutputInventory;
    public int timePerOre = 2;

    List<Item> ores = new List<Item>();
    int oreIndex = 0;
    Placement minerPlacement;
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlacementPlaced()
    {
        minerPlacement = GetComponent<Placement>();
        for (int x = 0; x < minerPlacement.BlockWidth; x++)
        {
            for (int y = 0; y < minerPlacement.BlockHeight; y++)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(x - 0.5f, y - 0.5f), Vector2.zero);
                if (hit.collider != null)
                {
                    switch (hit.collider.tag)
                    {
                        case "Malachite-deposit":
                            ores.Add(malachite);
                            break;
                        case "Hematite-deposit":
                            ores.Add(hematite);
                            break;
                        case "Bauxite-deposit":
                            ores.Add(bauxite);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ores.Count > 0)
        {
            if (time < (timePerOre * 4 / ores.Count))
            {
                time += Time.deltaTime;
                return;
            }
            time = 0;

            if (oreIndex == ores.Count - 1)
            {
                oreIndex = 0;
            }
            else
            {
                oreIndex++;
            }
            OutputInventory.AddItems(ores[oreIndex], 1);
            // Send ores[oreIndex] til inventory, hvis der er plads i inventory, eller sker der ikke noget
        }
    }
}
