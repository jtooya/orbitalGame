using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SimonSaysTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void SimonSaysTestSimplePasses()
    {
        // Use the Assert class to test conditions
        Assert.AreEqual(true, expectedInput);
        Assert.AreNotEqual(0, remainingTime);
    }
}
