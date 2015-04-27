using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Meridian59.Common;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else
using Real = System.Single;
#endif

namespace Meridian59.UnitTest.Common
{
    /// <summary>
    /// Tests for Common.Util
    /// </summary>
    [TestClass]
    public class MathUtilTest
    {
        const Real EPSILON = 0.0000001f;

        /// <summary>
        /// Test for Rotate()
        /// </summary>
        [TestMethod]
        public void IntersectLineLine()
        {
            V2 P1, P2, Q1, Q2;
            
            V2 intersectExpected;
            V2 intersectReturned;
            LineLineIntersectionType retvalExpected;
            LineLineIntersectionType retvalReturned;

            // --- TEST ---

            // lines cross at one point perpendicular
            P1 = new V2(-1.0f, 0.0f);
            P2 = new V2(1.0f, 0.0f);
            Q1 = new V2(0.0f, -1.0f);
            Q2 = new V2(0.0f, 1.0f);

            intersectExpected = new V2(0.0f, 0.0f);
            intersectReturned = new V2(0.0f, 0.0f);
            retvalExpected = LineLineIntersectionType.OneIntersection;
            retvalReturned = MathUtil.IntersectLineLine(P1, P2, Q1, Q2, out intersectReturned);

            Assert.AreEqual(retvalExpected, retvalReturned);
            Assert.AreEqual(intersectExpected.X, intersectReturned.X, EPSILON);
            Assert.AreEqual(intersectExpected.Y, intersectReturned.Y, EPSILON);

            // --- TEST ---

            // lines are on same infinite line and touch at P1=Q1
            P1 = new V2(1.0f, 1.0f);
            P2 = new V2(1.0f, 2.0f);
            Q1 = new V2(1.0f, 1.0f);
            Q2 = new V2(1.0f, 0.0f);

            intersectExpected = new V2(1.0f, 1.0f);
            intersectReturned = new V2(0.0f, 0.0f);
            retvalExpected = LineLineIntersectionType.OneIntersection;
            retvalReturned = MathUtil.IntersectLineLine(P1, P2, Q1, Q2, out intersectReturned);

            Assert.AreEqual(retvalExpected, retvalReturned);
            Assert.AreEqual(intersectExpected.X, intersectReturned.X, EPSILON);
            Assert.AreEqual(intersectExpected.Y, intersectReturned.Y, EPSILON);

            // --- TEST ---

            // lines are on same infinite line and touch at P1=Q2
            P1 = new V2(1.0f, 1.0f);
            P2 = new V2(1.0f, 2.0f);
            Q1 = new V2(1.0f, 0.0f);
            Q2 = new V2(1.0f, 1.0f);

            intersectExpected = new V2(1.0f, 1.0f);
            intersectReturned = new V2(0.0f, 0.0f);
            retvalExpected = LineLineIntersectionType.OneIntersection;
            retvalReturned = MathUtil.IntersectLineLine(P1, P2, Q1, Q2, out intersectReturned);

            Assert.AreEqual(retvalExpected, retvalReturned);
            Assert.AreEqual(intersectExpected.X, intersectReturned.X, EPSILON);
            Assert.AreEqual(intersectExpected.Y, intersectReturned.Y, EPSILON);

            // --- TEST ---

            // lines are on same infinite line and touch at P2=Q1
            P1 = new V2(1.0f, 0.0f);
            P2 = new V2(1.0f, 1.0f);
            Q1 = new V2(1.0f, 1.0f);
            Q2 = new V2(1.0f, 2.0f);

            intersectExpected = new V2(1.0f, 1.0f);
            intersectReturned = new V2(0.0f, 0.0f);
            retvalExpected = LineLineIntersectionType.OneIntersection;
            retvalReturned = MathUtil.IntersectLineLine(P1, P2, Q1, Q2, out intersectReturned);

            Assert.AreEqual(retvalExpected, retvalReturned);
            Assert.AreEqual(intersectExpected.X, intersectReturned.X, EPSILON);
            Assert.AreEqual(intersectExpected.Y, intersectReturned.Y, EPSILON);

            // --- TEST ---

            // lines are on same infinite line and touch at P2=Q2
            P1 = new V2(1.0f, 0.0f);
            P2 = new V2(1.0f, 1.0f);
            Q1 = new V2(1.0f, 2.0f);
            Q2 = new V2(1.0f, 1.0f);

            intersectExpected = new V2(1.0f, 1.0f);
            intersectReturned = new V2(0.0f, 0.0f);
            retvalExpected = LineLineIntersectionType.OneIntersection;
            retvalReturned = MathUtil.IntersectLineLine(P1, P2, Q1, Q2, out intersectReturned);

            Assert.AreEqual(retvalExpected, retvalReturned);
            Assert.AreEqual(intersectExpected.X, intersectReturned.X, EPSILON);
            Assert.AreEqual(intersectExpected.Y, intersectReturned.Y, EPSILON);

            // --- TEST ---

            // lines are parallel
            P1 = new V2(0.0f, 0.0f);
            P2 = new V2(0.0f, 1.0f);
            Q1 = new V2(1.0f, 0.0f);
            Q2 = new V2(1.0f, 1.0f);

            intersectExpected = new V2(0.0f, 0.0f);
            intersectReturned = new V2(0.0f, 0.0f);
            retvalExpected = LineLineIntersectionType.NoIntersection;
            retvalReturned = MathUtil.IntersectLineLine(P1, P2, Q1, Q2, out intersectReturned);

            Assert.AreEqual(retvalExpected, retvalReturned);
            Assert.AreEqual(intersectExpected.X, intersectReturned.X, EPSILON);
            Assert.AreEqual(intersectExpected.Y, intersectReturned.Y, EPSILON);

            // --- TEST ---

            // lines are identical
            P1 = new V2(0.0f, 0.0f);
            P2 = new V2(0.0f, 1.0f);
            Q1 = new V2(0.0f, 0.0f);
            Q2 = new V2(0.0f, 1.0f);

            intersectExpected = new V2(0.0f, 0.0f);
            intersectReturned = new V2(0.0f, 0.0f);
            retvalExpected = LineLineIntersectionType.FullyCoincide;
            retvalReturned = MathUtil.IntersectLineLine(P1, P2, Q1, Q2, out intersectReturned);

            Assert.AreEqual(retvalExpected, retvalReturned);
            //Assert.AreEqual(intersectExpected.X, intersectReturned.X, EPSILON);
            //Assert.AreEqual(intersectExpected.Y, intersectReturned.Y, EPSILON);

            // --- TEST ---

            // Q1Q2 fully lies within P1P2, P1=Q1
            P1 = new V2(0.0f, 0.0f);
            P2 = new V2(0.0f, 5.0f);
            Q1 = new V2(0.0f, 0.0f);
            Q2 = new V2(0.0f, 1.0f);

            intersectExpected = new V2(0.0f, 0.0f);
            intersectReturned = new V2(0.0f, 0.0f);
            retvalExpected = LineLineIntersectionType.PartiallyCoincide;
            retvalReturned = MathUtil.IntersectLineLine(P1, P2, Q1, Q2, out intersectReturned);

            Assert.AreEqual(retvalExpected, retvalReturned);
            //Assert.AreEqual(intersectExpected.X, intersectReturned.X, EPSILON);
            //Assert.AreEqual(intersectExpected.Y, intersectReturned.Y, EPSILON);

            // --- TEST ---

            // P1P2 fully lies within Q1Q2, P1=Q1
            P1 = new V2(0.0f, 0.0f);
            P2 = new V2(0.0f, 1.0f);
            Q1 = new V2(0.0f, 0.0f);
            Q2 = new V2(0.0f, 5.0f);

            intersectExpected = new V2(0.0f, 0.0f);
            intersectReturned = new V2(0.0f, 0.0f);
            retvalExpected = LineLineIntersectionType.PartiallyCoincide;
            retvalReturned = MathUtil.IntersectLineLine(P1, P2, Q1, Q2, out intersectReturned);

            Assert.AreEqual(retvalExpected, retvalReturned);
            //Assert.AreEqual(intersectExpected.X, intersectReturned.X, EPSILON);
            //Assert.AreEqual(intersectExpected.Y, intersectReturned.Y, EPSILON);

            // --- TEST ---

            // P1P2 fully lies within Q1Q2, P2=Q2
            P1 = new V2(0.0f, 1.0f);
            P2 = new V2(0.0f, 0.0f);
            Q1 = new V2(0.0f, 5.0f);
            Q2 = new V2(0.0f, 0.0f);

            intersectExpected = new V2(0.0f, 0.0f);
            intersectReturned = new V2(0.0f, 0.0f);
            retvalExpected = LineLineIntersectionType.PartiallyCoincide;
            retvalReturned = MathUtil.IntersectLineLine(P1, P2, Q1, Q2, out intersectReturned);

            Assert.AreEqual(retvalExpected, retvalReturned);
            //Assert.AreEqual(intersectExpected.X, intersectReturned.X, EPSILON);
            //Assert.AreEqual(intersectExpected.Y, intersectReturned.Y, EPSILON);

            // --- TEST ---

            // P1P2 overlaps notable with Q1Q2, no endpoints are equal
            P1 = new V2(0.0f, 0.0f);
            P2 = new V2(0.0f, 4.0f);
            Q1 = new V2(0.0f, 1.0f);
            Q2 = new V2(0.0f, 3.0f);

            intersectExpected = new V2(0.0f, 0.0f);
            intersectReturned = new V2(0.0f, 0.0f);
            retvalExpected = LineLineIntersectionType.PartiallyCoincide;
            retvalReturned = MathUtil.IntersectLineLine(P1, P2, Q1, Q2, out intersectReturned);

            Assert.AreEqual(retvalExpected, retvalReturned);
            //Assert.AreEqual(intersectExpected.X, intersectReturned.X, EPSILON);
            //Assert.AreEqual(intersectExpected.Y, intersectReturned.Y, EPSILON);           
        }
    }
}
