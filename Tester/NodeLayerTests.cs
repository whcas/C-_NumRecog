using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumRecog;

namespace Tester
{
    [TestClass]
    public class NodeLayerTests
    {
        [TestClass]
        class NodeLayerConstructorTests
        {
            [TestMethod]
            public void invalidNumberOfLayers_Test()
            {
                Assert.ThrowsException<ValidationException>(() => new NodeLayer(0, 0, 0, 0));
            }
        }
    }
}