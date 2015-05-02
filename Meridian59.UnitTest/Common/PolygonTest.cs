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

            expected = true;
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

            // --- TEST ---
            // 5 points, convex
            poly.Clear();
            poly.Add(new V2(2512.0f, 5490.0f));
            poly.Add(new V2(2304.0f, 5568.0f));
            poly.Add(new V2(2304.0f, 5376.0f));
            poly.Add(new V2(2304.0f, 5120.0f));
            poly.Add(new V2(2512.0f, 5120.0f));

            expected = true;
            returned = poly.IsConvexPolygon();

            Assert.AreEqual(expected, returned);

            // --- TEST ---
            // 6 points, convex
            poly.Clear();
            poly.Add(new V2(5376f, 6032f));
            poly.Add(new V2(5376f, 7936f));
            poly.Add(new V2(5376f, 8192f));
            poly.Add(new V2(5136f, 8192f));
            poly.Add(new V2(4752f, 6656f));
            poly.Add(new V2(4940.339f, 6467.661f));

            expected = true;
            returned = poly.IsConvexPolygon();

            Assert.AreEqual(expected, returned);
        }

        /// <summary>
        /// Test for SplitConvexPolygon()
        /// </summary>
        [TestMethod]
        public void SplitConvexPolygon()
        {
            Polygon poly = new Polygon();
            Polygon expectedLeft = new Polygon();
            Polygon expectedRight = new Polygon();
            V2 p1, p2;

            Tuple<Polygon, Polygon> returned;
            
            // --- TEST ---
            // 4 points, valid clockwise rectangle ABCD
            // intersect nothing from south to north
            //                  |
            //   B----------C   |
            //   |          |   |
            //   |          |   |
            //   |          |   |
            //   A----------D   |
            //                  |
            p1 = new V2(20.5f, -15f);
            p2 = new V2(20.5f, 15);
            poly.Clear();
            expectedLeft.Clear();
            expectedRight.Clear();
            poly.Add(new V2(0.0f, 0.0f)); // A
            poly.Add(new V2(0.0f, 5.0f)); // B
            poly.Add(new V2(5.0f, 5.0f)); // C
            poly.Add(new V2(5.0f, 0.0f)); // D
            // right empty
            // left = old
            expectedLeft.Add(new V2(0.0f, 0.0f));
            expectedLeft.Add(new V2(0.0f, 5.0f));
            expectedLeft.Add(new V2(5.0f, 5.0f));
            expectedLeft.Add(new V2(5.0f, 0.0f));

            returned = poly.SplitConvexPolygon(p1, p2);

            Assert.AreEqual(returned.Item1.IsIdentical(expectedRight), true);
            Assert.AreEqual(returned.Item2.IsIdentical(expectedLeft), true);

            // --- TEST ---
            // 4 points, valid clockwise rectangle ABCD
            // intersect nothing from north to south
            //                  |
            //   B----------C   |
            //   |          |   |
            //   |          |   |
            //   |          |   |
            //   A----------D   |
            //                  |
            p1 = new V2(20.5f, 15f);
            p2 = new V2(20.5f, -15);
            poly.Clear();
            expectedLeft.Clear();
            expectedRight.Clear();
            poly.Add(new V2(0.0f, 0.0f)); // A
            poly.Add(new V2(0.0f, 5.0f)); // B
            poly.Add(new V2(5.0f, 5.0f)); // C
            poly.Add(new V2(5.0f, 0.0f)); // D
            // right = old
            expectedRight.Add(new V2(0.0f, 0.0f));
            expectedRight.Add(new V2(0.0f, 5.0f));
            expectedRight.Add(new V2(5.0f, 5.0f));
            expectedRight.Add(new V2(5.0f, 0.0f));
            // left empty

            returned = poly.SplitConvexPolygon(p1, p2);

            Assert.AreEqual(returned.Item1.IsIdentical(expectedRight), true);
            Assert.AreEqual(returned.Item2.IsIdentical(expectedLeft), true);

            // --- TEST ---
            // 4 points, valid clockwise rectangle ABCD
            // intersect edges BC, DA from south to north
            //        |
            //   B----I1----C
            //   |    |     |
            //   |    |     |
            //   |    |     |
            //   A----I2----D
            //        |
            p1 = new V2(2.5f, -10f);
            p2 = new V2(2.5f, 10);
            poly.Clear();
            expectedLeft.Clear();
            expectedRight.Clear();           
            poly.Add(new V2(0.0f, 0.0f)); // A
            poly.Add(new V2(0.0f, 5.0f)); // B
            poly.Add(new V2(5.0f, 5.0f)); // C
            poly.Add(new V2(5.0f, 0.0f)); // D
            expectedRight.Add(new V2(2.5f, 5.0f));
            expectedRight.Add(new V2(5.0f, 5.0f));
            expectedRight.Add(new V2(5.0f, 0.0f));
            expectedRight.Add(new V2(2.5f, 0.0f));
            expectedLeft.Add(new V2(2.5f, 0.0f));
            expectedLeft.Add(new V2(0.0f, 0.0f));
            expectedLeft.Add(new V2(0.0f, 5.0f));
            expectedLeft.Add(new V2(2.5f, 5.0f));

            returned = poly.SplitConvexPolygon(p1, p2);

            Assert.AreEqual(returned.Item1.IsIdentical(expectedRight), true);
            Assert.AreEqual(returned.Item2.IsIdentical(expectedLeft), true);

            // --- TEST ---
            // 4 points, valid clockwise rectangle ABCD
            // intersect edges BC, DA from nort to south
            //        |
            //   B----I1----C
            //   |    |     |
            //   |    |     |
            //   |    |     |
            //   A----I2----D
            //        |
            p1 = new V2(2.5f, 10f);
            p2 = new V2(2.5f, -10);
            poly.Clear();
            expectedLeft.Clear();
            expectedRight.Clear();
            poly.Add(new V2(0.0f, 0.0f)); // A
            poly.Add(new V2(0.0f, 5.0f)); // B
            poly.Add(new V2(5.0f, 5.0f)); // C
            poly.Add(new V2(5.0f, 0.0f)); // D
            expectedLeft.Add(new V2(2.5f, 5.0f));
            expectedLeft.Add(new V2(5.0f, 5.0f));
            expectedLeft.Add(new V2(5.0f, 0.0f));
            expectedLeft.Add(new V2(2.5f, 0.0f));
            expectedRight.Add(new V2(2.5f, 0.0f));
            expectedRight.Add(new V2(0.0f, 0.0f));
            expectedRight.Add(new V2(0.0f, 5.0f));
            expectedRight.Add(new V2(2.5f, 5.0f));

            returned = poly.SplitConvexPolygon(p1, p2);

            Assert.AreEqual(returned.Item1.IsIdentical(expectedRight), true);
            Assert.AreEqual(returned.Item2.IsIdentical(expectedLeft), true);

            // --- TEST ---
            // 4 points, valid clockwise rectangle ABCD
            // 1 boundary point at C with splitter from north-west to south-east
            //             \
            //   B----------C
            //   |          |\
            //   |          | \
            //   |          |  \
            //   A----------D
            //         
            p1 = new V2(4.0f, 6.0f);
            p2 = new V2(6.0f, 4.0f);
            poly.Clear();
            expectedLeft.Clear();
            expectedRight.Clear();
            poly.Add(new V2(0.0f, 0.0f)); // A
            poly.Add(new V2(0.0f, 5.0f)); // B
            poly.Add(new V2(5.0f, 5.0f)); // C
            poly.Add(new V2(5.0f, 0.0f)); // D
            // left empty
            // right old
            expectedRight.Add(new V2(0.0f, 0.0f));
            expectedRight.Add(new V2(0.0f, 5.0f));
            expectedRight.Add(new V2(5.0f, 5.0f));
            expectedRight.Add(new V2(5.0f, 0.0f));

            returned = poly.SplitConvexPolygon(p1, p2);

            Assert.AreEqual(returned.Item1.IsIdentical(expectedRight), true);
            Assert.AreEqual(returned.Item2.IsIdentical(expectedLeft), true);

            // --- TEST ---
            // 4 points, valid clockwise rectangle ABCD
            // edge CD coincide with splitter from south to north
            //              |
            //   B----------C
            //   |          |
            //   |          |
            //   |          |
            //   A----------D
            //              |
            p1 = new V2(5.0f, -10.0f);
            p2 = new V2(5.0f, 3.0f);
            poly.Clear();
            expectedLeft.Clear();
            expectedRight.Clear();
            poly.Add(new V2(0.0f, 0.0f)); // A
            poly.Add(new V2(0.0f, 5.0f)); // B
            poly.Add(new V2(5.0f, 5.0f)); // C
            poly.Add(new V2(5.0f, 0.0f)); // D          
            // right empty
            // left old
            expectedLeft.Add(new V2(0.0f, 0.0f));
            expectedLeft.Add(new V2(0.0f, 5.0f));
            expectedLeft.Add(new V2(5.0f, 5.0f));
            expectedLeft.Add(new V2(5.0f, 0.0f));

            returned = poly.SplitConvexPolygon(p1, p2);

            Assert.AreEqual(returned.Item1.IsIdentical(expectedRight), true);
            Assert.AreEqual(returned.Item2.IsIdentical(expectedLeft), true);

            // --- TEST ---
            // 5 points, valid clockwise rectangle ABCDE
            // edge CD and edge DE coincide with splitter from south to north
            //              |
            //   B----------C
            //   |          |
            //   |          D
            //   |          |
            //   A----------E
            //              |
            p1 = new V2(5.0f, -10.0f);
            p2 = new V2(5.0f, 3.0f);
            poly.Clear();
            expectedLeft.Clear();
            expectedRight.Clear();
            poly.Add(new V2(0.0f, 0.0f)); // A
            poly.Add(new V2(0.0f, 5.0f)); // B
            poly.Add(new V2(5.0f, 5.0f)); // C
            poly.Add(new V2(5.0f, 2.5f)); // D
            poly.Add(new V2(5.0f, 0.0f)); // E          
            // right empty
            // left old
            expectedLeft.Add(new V2(0.0f, 0.0f));
            expectedLeft.Add(new V2(0.0f, 5.0f));
            expectedLeft.Add(new V2(5.0f, 5.0f));
            expectedLeft.Add(new V2(5.0f, 2.5f));
            expectedLeft.Add(new V2(5.0f, 0.0f));

            returned = poly.SplitConvexPolygon(p1, p2);

            Assert.AreEqual(returned.Item1.IsIdentical(expectedRight), true);
            Assert.AreEqual(returned.Item2.IsIdentical(expectedLeft), true);

            // --- TEST ---
            // 4 points, valid clockwise rectangle ABCD
            // two boundary points at vertices A and C
            // by diagonal infinite line AC
            //         /
            //   B----C
            //   |   /|
            //   |  / |
            //   | /  |
            //   |/   |
            //   A----D
            //  /         
            p1 = new V2(-1.0f, -1.0f);
            p2 = new V2(2.0f, 2.0f);
            poly.Clear();
            expectedLeft.Clear();
            expectedRight.Clear();
            poly.Add(new V2(0.0f, 0.0f)); // A
            poly.Add(new V2(0.0f, 5.0f)); // B
            poly.Add(new V2(5.0f, 5.0f)); // C
            poly.Add(new V2(5.0f, 0.0f)); // D          
            expectedRight.Add(new V2(5.0f, 5.0f)); // C
            expectedRight.Add(new V2(5.0f, 0.0f)); // D
            expectedRight.Add(new V2(0.0f, 0.0f)); // A
            expectedLeft.Add(new V2(0.0f, 0.0f)); // A
            expectedLeft.Add(new V2(0.0f, 5.0f)); // B
            expectedLeft.Add(new V2(5.0f, 5.0f)); // C

            returned = poly.SplitConvexPolygon(p1, p2);

            Assert.AreEqual(returned.Item1.IsIdentical(expectedRight), true);
            Assert.AreEqual(returned.Item2.IsIdentical(expectedLeft), true);

            // --- TEST ---
            // 4 points, valid clockwise rectangle ABCD
            // one intersection at I1, one boundarypoint A

            //         /
            //   B----I1--C
            //   |   /    |
            //   |  /     |
            //   | /      |
            //   |/       |
            //   A--------D
            //  /         
            p1 = new V2(-0.5f, -1.0f);
            p2 = new V2(0.5f, 1.0f);
            poly.Clear();
            expectedLeft.Clear();
            expectedRight.Clear();
            poly.Add(new V2(0.0f, 0.0f)); // A
            poly.Add(new V2(0.0f, 5.0f)); // B
            poly.Add(new V2(5.0f, 5.0f)); // C
            poly.Add(new V2(5.0f, 0.0f)); // D
            expectedRight.Add(new V2(2.5f, 5.0f)); // I1
            expectedRight.Add(new V2(5.0f, 5.0f)); // C
            expectedRight.Add(new V2(5.0f, 0.0f)); // D
            expectedRight.Add(new V2(0.0f, 0.0f)); // A
            expectedLeft.Add(new V2(0.0f, 0.0f)); // A
            expectedLeft.Add(new V2(0.0f, 5.0f)); // B
            expectedLeft.Add(new V2(2.5f, 5.0f)); // I1

            returned = poly.SplitConvexPolygon(p1, p2);

            Assert.AreEqual(returned.Item1.IsIdentical(expectedRight), true);
            Assert.AreEqual(returned.Item2.IsIdentical(expectedLeft), true);

            // --- TEST ---
            // 3 points, valid clockwise triangle ABC
            // one boundarypoint at C by infinite p1p2 from south to north

            //        | 
            //   B----C
            //   |   /|
            //   |  / |
            //   | /
            //   |/
            //   A
            //           
            p1 = new V2(5.0f, -1.0f);
            p2 = new V2(5.0f, 1.0f);
            poly.Clear();
            expectedLeft.Clear();
            expectedRight.Clear();
            poly.Add(new V2(0.0f, 0.0f)); // A
            poly.Add(new V2(0.0f, 5.0f)); // B
            poly.Add(new V2(5.0f, 5.0f)); // C
            // right empty
            expectedLeft.Add(new V2(0.0f, 0.0f)); // A
            expectedLeft.Add(new V2(0.0f, 5.0f)); // B
            expectedLeft.Add(new V2(5.0f, 5.0f)); // C

            returned = poly.SplitConvexPolygon(p1, p2);

            Assert.AreEqual(returned.Item1.IsIdentical(expectedRight), true);
            Assert.AreEqual(returned.Item2.IsIdentical(expectedLeft), true);

            // --- TEST ---
            // 3 points, valid clockwise triangle ABC
            // two intersection points at I1 and I2
            // by infinite p1p2 from north to south

            //      |   
            //   B--I1-C
            //   |  | /
            //   |  I2 
            //   | /|
            //   |/ |
            //   A
            //           
            p1 = new V2(2.5f, 1.0f);
            p2 = new V2(2.5f, -1.0f);
            poly.Clear();
            expectedLeft.Clear();
            expectedRight.Clear();
            poly.Add(new V2(0.0f, 0.0f)); // A
            poly.Add(new V2(0.0f, 5.0f)); // B
            poly.Add(new V2(5.0f, 5.0f)); // C
            expectedRight.Add(new V2(2.5f, 2.5f)); // I2
            expectedRight.Add(new V2(0.0f, 0.0f)); // A
            expectedRight.Add(new V2(0.0f, 5.0f)); // B
            expectedRight.Add(new V2(2.5f, 5.0f)); // I1
            expectedLeft.Add(new V2(2.5f, 5.0f)); // I1
            expectedLeft.Add(new V2(5.0f, 5.0f)); // C
            expectedLeft.Add(new V2(2.5f, 2.5f)); // I2

            returned = poly.SplitConvexPolygon(p1, p2);

            Assert.AreEqual(returned.Item1.IsIdentical(expectedRight), true);
            Assert.AreEqual(returned.Item2.IsIdentical(expectedLeft), true);
        }
    }
}
