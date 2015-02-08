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
    public class V2Text
    {
        const Real EPSILON = 0.0000001f;

        /// <summary>
        /// Test for Rotate()
        /// </summary>
        [TestMethod]
        public void Rotate()
        {           
            V2 expected;
            V2 returned;

            // --- TEST ---

            expected = new V2(0.0f, 1.0f);
            returned = new V2(1.0f, 0.0f);
            returned.Rotate(2f * (Real)Math.PI * 0.25f);

            Assert.AreEqual(expected.X, returned.X, EPSILON);
            Assert.AreEqual(expected.Y, returned.Y, EPSILON);

            // --- TEST ---

            expected = new V2(-1.0f, 0.0f);
            returned = new V2(1.0f, 0.0f);
            returned.Rotate(2f * (Real)Math.PI * 0.5f);

            Assert.AreEqual(expected.X, returned.X, EPSILON);
            Assert.AreEqual(expected.Y, returned.Y, EPSILON);

            // --- TEST ---

            expected = new V2(-1.0f, 0.0f);
            returned = new V2(0.0f, -1.0f);
            returned.Rotate(-2f * (Real)Math.PI * 0.25f);

            Assert.AreEqual(expected.X, returned.X, EPSILON);
            Assert.AreEqual(expected.Y, returned.Y, EPSILON);
        }

        /// <summary>
        /// Test for Scale()
        /// </summary>
        [TestMethod]
        public void Scale()
        {
            V2 expected;
            V2 returned;

            // --- TEST ---

            expected = new V2(0.0f, 0.0f);
            returned = new V2(0.0f, 0.0f);
            returned.Scale(10f);

            Assert.AreEqual(expected.X, returned.X, EPSILON);
            Assert.AreEqual(expected.Y, returned.Y, EPSILON);

            // --- TEST ---

            expected = new V2(10.0f, 0.0f);
            returned = new V2(1.0f, 0.0f);
            returned.Scale(10f);

            Assert.AreEqual(expected.X, returned.X, EPSILON);
            Assert.AreEqual(expected.Y, returned.Y, EPSILON);

            // --- TEST ---

            expected = new V2(0.0f, -10.0f);
            returned = new V2(0.0f, -1.0f);
            returned.Scale(10f);

            Assert.AreEqual(expected.X, returned.X, EPSILON);
            Assert.AreEqual(expected.Y, returned.Y, EPSILON);

            // --- TEST ---

            expected = new V2(0.0f, 0.0f);
            returned = new V2(1.34f, -5.563f);
            returned.Scale(0.0f);

            Assert.AreEqual(expected.X, returned.X, EPSILON);
            Assert.AreEqual(expected.Y, returned.Y, EPSILON);

            // --- TEST ---

            expected = new V2(EPSILON, EPSILON);
            returned = new V2(1.0f, 1.0f);
            returned.Scale(EPSILON);

            Assert.AreEqual(expected.X, returned.X, EPSILON);
            Assert.AreEqual(expected.Y, returned.Y, EPSILON);
        }

        /// <summary>
        /// Test for MinDistanceToLineSegment()
        /// </summary>
        [TestMethod]
        public void MinDistanceToLineSegment()
        {
            Real expected;
            Real returned;
            V2 s;
            V2 p1, p2;

            // --- TEST ---
            // p1=p2=s=0

            s = new V2(0.0f, 0.0f);
            p1 = new V2(0.0f, 0.0f);
            p2 = new V2(0.0f, 0.0f);
            expected = 0.0f;
            returned = s.MinDistanceToLineSegment(p1, p2);

            Assert.AreEqual(expected, returned, EPSILON);

            // --- TEST ---
            // s on p1

            s = new V2(0.0f, 0.0f);
            p1 = new V2(0.0f, 0.0f);
            p2 = new V2(1.0f, 0.0f);
            expected = 0.0f;
            returned = s.MinDistanceToLineSegment(p1, p2);

            Assert.AreEqual(expected, returned, EPSILON);

            // --- TEST ---
            // s on p2

            s = new V2(1.0f, 0.0f);
            p1 = new V2(0.0f, 0.0f);
            p2 = new V2(1.0f, 0.0f);
            expected = 0.0f;
            returned = s.MinDistanceToLineSegment(p1, p2);

            Assert.AreEqual(expected, returned, EPSILON);

            // --- TEST ---
            // s in mid of p1p2

            s = new V2(0.0f, 0.5f);
            p1 = new V2(0.0f, 0.0f);
            p2 = new V2(1.0f, 0.0f);
            expected = 0.5f;
            returned = s.MinDistanceToLineSegment(p1, p2);

            Assert.AreEqual(expected, returned, EPSILON);

            // --- TEST ---
            // p1=p2

            s = new V2(0.0f, 0.0f);
            p1 = new V2(1.0f, 0.0f);
            p2 = new V2(1.0f, 0.0f);
            expected = 1.0f;
            returned = s.MinDistanceToLineSegment(p1, p2);

            Assert.AreEqual(expected, returned, EPSILON);

            // --- TEST ---
            // s on extension p1p2, closest p2

            s = new V2(2.0f, 0.0f);
            p1 = new V2(0.0f, 0.0f);
            p2 = new V2(1.0f, 0.0f);
            expected = 1.0f;
            returned = s.MinDistanceToLineSegment(p1, p2);

            Assert.AreEqual(expected, returned, EPSILON);

            // --- TEST ---
            // s on extension p2p1, closest p1

            s = new V2(-1.0f, 0.0f);
            p1 = new V2(0.0f, 0.0f);
            p2 = new V2(1.0f, 0.0f);
            expected = 1.0f;
            returned = s.MinDistanceToLineSegment(p1, p2);

            Assert.AreEqual(expected, returned, EPSILON);

            // --- TEST ---
            // s perpendicular, mid p1p2 closest

            s = new V2(0.5f, 0.5f);
            p1 = new V2(0.0f, 0.0f);
            p2 = new V2(1.0f, 0.0f);
            expected = 0.5f;
            returned = s.MinDistanceToLineSegment(p1, p2);

            Assert.AreEqual(expected, returned, EPSILON);

            // --- TEST ---
            // s perpendicular, p1 closest +

            s = new V2(0.0f, 1.0f);
            p1 = new V2(0.0f, 0.0f);
            p2 = new V2(1.0f, 0.0f);
            expected = 1.0f;
            returned = s.MinDistanceToLineSegment(p1, p2);

            Assert.AreEqual(expected, returned, EPSILON);

            // --- TEST ---
            // s perpendicular, p1 closest -

            s = new V2(0.0f, -1.0f);
            p1 = new V2(0.0f, 0.0f);
            p2 = new V2(1.0f, 0.0f);
            expected = 1.0f;
            returned = s.MinDistanceToLineSegment(p1, p2);

            Assert.AreEqual(expected, returned, EPSILON);

            // --- TEST ---
            // s perpendicular, p2 closest +

            s = new V2(1.0f, 1.0f);
            p1 = new V2(0.0f, 0.0f);
            p2 = new V2(1.0f, 0.0f);
            expected = 1.0f;
            returned = s.MinDistanceToLineSegment(p1, p2);

            Assert.AreEqual(expected, returned, EPSILON);

            // --- TEST ---
            // s perpendicular, p2 closest -

            s = new V2(1.0f, -1.0f);
            p1 = new V2(0.0f, 0.0f);
            p2 = new V2(1.0f, 0.0f);
            expected = 1.0f;
            returned = s.MinDistanceToLineSegment(p1, p2);

            Assert.AreEqual(expected, returned, EPSILON);

            // --- TEST ---
            // s 45° to p2 closest +

            s = new V2(2.0f, 1.0f);
            p1 = new V2(0.0f, 0.0f);
            p2 = new V2(1.0f, 0.0f);
            expected = (Real)Math.Sqrt(2.0f);
            returned = s.MinDistanceToLineSegment(p1, p2);

            Assert.AreEqual(expected, returned, EPSILON);

            // --- TEST ---
            // s 45° to p2 closest -

            s = new V2(2.0f, -1.0f);
            p1 = new V2(0.0f, 0.0f);
            p2 = new V2(1.0f, 0.0f);
            expected = (Real)Math.Sqrt(2.0f);
            returned = s.MinDistanceToLineSegment(p1, p2);

            Assert.AreEqual(expected, returned, EPSILON);
        }
    }
}
