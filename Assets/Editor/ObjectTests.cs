using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class Tests
{
    [Test]
    /* 
     * Currently just creates objects
     * Will draw grid on screen if you run test then click gizmos
     */
    public void test1()
    {
        Assert.AreEqual(1, 1); // Does nothing just shows green test :)
    }

}
