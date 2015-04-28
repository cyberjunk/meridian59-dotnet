using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Meridian59.Common;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else
using Real = System.Single;
#endif

namespace Meridian59.UnitTest
{
    /// <summary>
    /// Tests for Common.V2
    /// </summary>
    [TestClass]
    public class PolygonTest
    {
        const Real EPSILON = 0.0000001f;

        /// <summary>
        /// Test for IsPolygon()
        /// </summary>
        [TestMethod]
        public void IsPolygon()
        {
            Polygon poly = new Polygon();

            bool expected;
            bool returned;

            // --- TEST ---
            // no points
            poly.Clear();

            expected = false;
            returned = poly.IsPolygon();
            
            Assert.AreEqual(expected, returned);

            // --- TEST ---
            // 1 point
            poly.Clear();
            poly.Add(new V2(35.0f, 2.0f));

            expected = false;
            returned = poly.IsPolygon();

            Assert.AreEqual(expected, returned);

            // --- TEST ---
            // 2 points
            poly.Clear();
            poly.Add(new V2(35.0f, 2.0f));
            poly.Add(new V2(365.0f, 23.0f));

            expected = false;
            returned = poly.IsPolygon();

            Assert.AreEqual(expected, returned);

            // --- TEST ---
            // 3 points all equal
            poly.Clear();
            poly.Add(new V2(35.0f, 2.0f));
            poly.Add(new V2(35.0f, 2.0f));
            poly.Add(new V2(35.0f, 2.0f));

            expected = false;
            returned = poly.IsPolygon();

            Assert.AreEqual(expected, returned);

            // --- TEST ---
            // 3 points, 1&3 equal
            poly.Clear();
            poly.Add(new V2(35.0f, 2.0f));
            poly.Add(new V2(3.0f, 6.0f));
            poly.Add(new V2(35.0f, 2.0f));

            expected = false;
            returned = poly.IsPolygon();

            Assert.AreEqual(expected, returned);

            // --- TEST ---
            // 3 points, 1&2 equal
            poly.Clear();
            poly.Add(new V2(3.0f, 6.0f));
            poly.Add(new V2(3.0f, 6.0f));
            poly.Add(new V2(35.0f, 2.0f));

            expected = false;
            returned = poly.IsPolygon();

            Assert.AreEqual(expected, returned);

            // --- TEST ---
            // 3 points, 2&3 equal
            poly.Clear();
            poly.Add(new V2(35.0f, 2.0f));
            poly.Add(new V2(3.0f, 6.0f));
            poly.Add(new V2(3.0f, 6.0f));

            expected = false;
            returned = poly.IsPolygon();

            Assert.AreEqual(expected, returned);

            // --- TEST ---
            // 3 points on same line
            poly.Clear();
            poly.Add(new V2(5.0f, 7.0f));
            poly.Add(new V2(5.0f, 5.0f));
            poly.Add(new V2(5.0f, 10.0f));

            expected = false;
            returned = poly.IsPolygon();

            Assert.AreEqual(expected, returned);

            // --- TEST ---
            // 3 points, valid triangle
            poly.Clear();
            poly.Add(new V2(10.0f, 10.0f));
            poly.Add(new V2(5.0f, 0.0f));
            poly.Add(new V2(10.0f, -10.0f));

            expected = true;
            returned = poly.IsPolygon();

            Assert.AreEqual(expected, returned);

            // --- TEST ---
            // 4 points, rectangle ordered
            poly.Clear();
            poly.Add(new V2(0.0f, 0.0f));
            poly.Add(new V2(0.0f, 5.0f));
            poly.Add(new V2(5.0f, 5.0f));
            poly.Add(new V2(5.0f, 0.0f));

            expected = true;
            returned = poly.IsPolygon();

            Assert.AreEqual(expected, returned);

            // --- TEST ---
            // 4 points, no rectangle order, but still poly
            poly.Clear();
            poly.Add(new V2(0.0f, 0.0f));
            poly.Add(new V2(5.0f, 5.0f));
            poly.Add(new V2(0.0f, 5.0f));
            poly.Add(new V2(5.0f, 0.0f));

            expected = true;
            returned = poly.IsPolygon();

            Assert.AreEqual(expected, returned);
        }

        /// <summary>
        /// Test for RemoveZeroEdges()
        /// </summary>
        [TestMethod]
        public void RemoveZeroEdges()
        {
            Polygon poly = new Polygon();

            int expected;
            int returned;

            // --- TEST ---
            // 5 points, rectangle, but two points not of first three in on same coords
            poly.Clear();
            poly.Add(new V2(0.0f, 0.0f));
            poly.Add(new V2(0.0f, 5.0f));
            poly.Add(new V2(5.0f, 5.0f));
            poly.Add(new V2(5.0f, 0.0f));
            poly.Add(new V2(5.0f, 0.0f));

            expected = 1;
            returned = poly.RemoveZeroEdges();

            Assert.AreEqual(expected, returned);

            // --- TEST ---
            // 5 points, rectangle, but two points not of first three in on same coords
            poly.Clear();
            poly.Add(new V2(0.0f, 0.0f));
            poly.Add(new V2(0.0f, 5.0f));
            poly.Add(new V2(5.0f, 5.0f));
            poly.Add(new V2(5.0f, 0.0f));
            poly.Add(new V2(5.0f, 0.0f));

            expected = 1;
            returned = poly.RemoveZeroEdges();

            Assert.AreEqual(expected, returned);

            // --- TEST ---
            // n points, concave polygon
            poly.Clear();
            poly.Add(new V2(0.0f, 0.0f));
            poly.Add(new V2(10.0f, 0.0f));
            poly.Add(new V2(10.0f, 5.0f));
            poly.Add(new V2(5.0f, 5.0f));
            poly.Add(new V2(5.0f, 10.0f));
            poly.Add(new V2(10.0f, 10.0f));
            poly.Add(new V2(10.0f, 15.0f));
            poly.Add(new V2(0.0f, 15.0f));

            expected = 0;
            returned = poly.RemoveZeroEdges();

            Assert.AreEqual(expected, returned);
        }

        /// <summary>
        /// Test for IsConvexPolygon()
        /// </summary>
        [TestMethod]
        public void IsConvexPolygon()
        {
            Polygon poly = new Polygon();

            bool expected;
            bool returned;

            // --- TEST ---
            // 3 points, valid triangle
            poly.Clear();
            poly.Add(new V2(10.0f, 10.0f));
            poly.Add(new V2(5.0f, 0.0f));
            poly.Add(new V2(10.0f, -10.0f));

            expected = true;
            returned = poly.IsConvexPolygon();

            Assert.AreEqual(expected, returned);

            // --- TEST ---
            // 4 points, valid rectangle
            poly.Clear();
            poly.Add(new V2(0.0f, 0.0f));
            poly.Add(new V2(0.0f, 5.0f));
            poly.Add(new V2(5.0f, 5.0f));
            poly.Add(new V2(5.0f, 0.0f));

            expected = true;
            returned = poly.IsConvexPolygon();

            Assert.AreEqual(expected, returned);

            // --- TEST ---
            // 5 points, rectangle, but two points not of first three in on same coords
            poly.Clear();
            poly.Add(new V2(0.0f, 0.0f));
            poly.Add(new V2(0.0f, 5.0f));
            poly.Add(new V2(5.0f, 5.0f));
            poly.Add(new V2(5.0f, 0.0f));
            poly.Add(new V2(5.0f, 0.0f));

            expected = false;
            returned = poly.IsConvexPolygon();

            Assert.AreEqual(expected, returned);

            // --- TEST ---
            // 4 points, no rectangle ordered, not convex
            poly.Clear();
            poly.Add(new V2(0.0f, 0.0f));
            poly.Add(new V2(5.0f, 5.0f));
            poly.Add(new V2(0.0f, 5.0f));
            poly.Add(new V2(5.0f, 0.0f));

            expected = false;
            returned = poly.IsConvexPolygon();

            Assert.AreEqual(expected, returned);

            // --- TEST ---
            // n points, concave polygon
            poly.Clear();
            poly.Add(new V2(0.0f, 0.0f));
            poly.Add(new V2(10.0f, 0.0f));
            poly.Add(new V2(10.0f, 5.0f));
            poly.Add(new V2(5.0f, 5.0f));
            poly.Add(new V2(5.0f, 10.0f));
            poly.Add(new V2(10.0f, 10.0f));
            poly.Add(new V2(10.0f, 15.0f));
            poly.Add(new V2(0.0f, 15.0f));

            expected = false;
            returned = poly.IsConvexPolygon();

            Assert.AreEqual(expected, returned);
        }
    }
}
