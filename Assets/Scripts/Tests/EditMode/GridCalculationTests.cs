using System.Collections;
using System.Collections.Generic;
using FactoryGame.Placements;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GridCalculationTests
{
    [Test]
    public void UnityPositionToGridPositionTest()
    {
        Vector2 test1 = new Vector2(1.5f, 1f);
        Assert.AreEqual(new Vector2Int(1, 1), GridCalculation.GetTilePosition(test1));

        Vector2 test2 = new Vector2(-1, 1f);
        Assert.AreEqual(new Vector2Int(-1, 1), GridCalculation.GetTilePosition(test2));

        Vector2 test3 = new Vector2(0.5f, -0.5f);
        Assert.AreEqual(new Vector2Int(0, -1), GridCalculation.GetTilePosition(test3));
    }

    [Test]
    public void NearbyGridPositionTest()
    {
        Vector2Int test1 = new Vector2Int(1, 1);
        Assert.AreEqual(new Vector2Int(1, 2), GridCalculation.GetNearbyTilePosition(test1, Orientation.North));

        Vector2Int test2 = new Vector2Int(1, 1);
        Assert.AreEqual(new Vector2Int(2, 1), GridCalculation.GetNearbyTilePosition(test2, Orientation.East));

        Vector2Int test3 = new Vector2Int(1, 1);
        Assert.AreEqual(new Vector2Int(1, 0), GridCalculation.GetNearbyTilePosition(test3, Orientation.South));

        Vector2Int test4 = new Vector2Int(1, 1);
        Assert.AreEqual(new Vector2Int(0, 1), GridCalculation.GetNearbyTilePosition(test4, Orientation.West));
    }
}
