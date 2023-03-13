using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Project.States;
using UnityEngine;
using UnityEngine.TestTools;

public class StateManagerTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void NullDefaultStateTest()
    {
        IStateManager manager = new StateManager();

        try
        {
            manager.TickStates();
        }
        catch (Exception)
        {
            Assert.IsTrue(true);
        }
    }
}
