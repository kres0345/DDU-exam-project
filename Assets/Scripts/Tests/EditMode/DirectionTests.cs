using System.Collections;
using System.Collections.Generic;
using FactoryGame.Placements;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DirectionTests
{
    [Test]
    public void RotationTest()
    {
        Orientation orientation = Orientation.North;

        orientation = Placement.Rotate(orientation, 1);
        Assert.AreEqual(Orientation.East, orientation);

        orientation = Placement.Rotate(orientation, 1);
        Assert.AreEqual(Orientation.South, orientation);

        orientation = Placement.Rotate(orientation, 1);
        Assert.AreEqual(Orientation.West, orientation);

        orientation = Placement.Rotate(orientation, 1);
        Assert.AreEqual(Orientation.North, orientation);
    }

    [Test]
    public void OppositeRotationTest()
    {
        Orientation orientation = Orientation.North;

        orientation = Placement.Rotate(orientation, 3);
        Assert.AreEqual(Orientation.West, orientation);

        orientation = Placement.Rotate(orientation, 3);
        Assert.AreEqual(Orientation.South, orientation);

        orientation = Placement.Rotate(orientation, 3);
        Assert.AreEqual(Orientation.East, orientation);

        orientation = Placement.Rotate(orientation, 3);
        Assert.AreEqual(Orientation.North, orientation);
    }

    [Test]
    public void RotateFlaggedOrientation()
    {
        Orientation orientation = Orientation.North | Orientation.East;

        orientation = Placement.Rotate(orientation, 1);
        Assert.AreEqual(Orientation.East | Orientation.South, orientation);

        orientation = Placement.Rotate(orientation, 1);
        Assert.AreEqual(Orientation.South | Orientation.West, orientation);

        orientation = Placement.Rotate(orientation, 1);
        Assert.AreEqual(Orientation.West | Orientation.North, orientation);

        orientation = Placement.Rotate(orientation, 1);
        Assert.AreEqual(Orientation.North | Orientation.East, orientation);
    }
}
