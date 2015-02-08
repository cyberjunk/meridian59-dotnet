using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Meridian59.Common;

namespace Meridian59.UnitTest
{
    /// <summary>
    /// Tests for Common.Util
    /// </summary>
    [TestClass]
    public class UtilTest
    {
        /// <summary>
        /// Test for GetQuote()
        /// </summary>
        [TestMethod]
        public void GetQuote()
        {
            string teststr;
            Tuple<int, int, string> expected;
            Tuple<int, int, string> quote;

            // --- TEST 1 ---

            teststr  = "\"Mister X\"";            
            expected = new Tuple<int,int,string>(0, 10, "Mister X");
            quote = teststr.GetQuote();

            Assert.AreNotEqual(quote, null);
            Assert.AreEqual(quote.Item1, expected.Item1);
            Assert.AreEqual(quote.Item2, expected.Item2);
            Assert.AreEqual(quote.Item3, expected.Item3);

            // --- TEST 2 ---

            teststr = "\"\"";
            expected = new Tuple<int, int, string>(0, 2, "");
            quote = teststr.GetQuote();

            Assert.AreNotEqual(quote, null);
            Assert.AreEqual(quote.Item1, expected.Item1);
            Assert.AreEqual(quote.Item2, expected.Item2);
            Assert.AreEqual(quote.Item3, expected.Item3);

            // --- TEST 3 ---

            teststr = "";
            expected = null;
            quote = teststr.GetQuote();

            Assert.AreEqual(quote, null);

            // --- TEST 4 ---

            teststr = "\"";
            expected = null;
            quote = teststr.GetQuote();

            Assert.AreEqual(quote, null);

            // --- TEST 5 ---

            teststr = " \"";
            expected = null;
            quote = teststr.GetQuote();

            Assert.AreEqual(quote, null);

            // --- TEST 6 ---

            teststr = "\"something";
            expected = null;
            quote = teststr.GetQuote();

            Assert.AreEqual(quote, null);
        }

        /// <summary>
        /// Test for CountSubstringOccurences()
        /// </summary>
        [TestMethod]
        public void CountSubStringOccurences()
        {
            string pattern;
            string data;
            bool skipsubstrlenonmatch;
            int expected;
            int returned;

            // --- TEST 1 ---
            
            pattern = "aa";
            data    = "aaaa";
            skipsubstrlenonmatch = true;
            expected = 2;
            returned = Util.CountSubStringOccurences(pattern, data, skipsubstrlenonmatch);

            Assert.AreEqual(expected, returned); 

            // --- TEST 2 ---
            
            pattern = "aa";
            data = "aaaa";
            skipsubstrlenonmatch = false;
            expected = 3;
            returned = Util.CountSubStringOccurences(pattern, data, skipsubstrlenonmatch);

            Assert.AreEqual(expected, returned);

            // --- TEST 3 ---

            pattern = "aaa";
            data = "aaaa";
            skipsubstrlenonmatch = true;
            expected = 1;
            returned = Util.CountSubStringOccurences(pattern, data, skipsubstrlenonmatch);

            Assert.AreEqual(expected, returned);

            // --- TEST 4 ---

            pattern = "aaa";
            data = "aaaa";
            skipsubstrlenonmatch = false;
            expected = 2;
            returned = Util.CountSubStringOccurences(pattern, data, skipsubstrlenonmatch);

            Assert.AreEqual(expected, returned);

            // --- TEST 5 ---

            pattern = "aaaa";
            data = "aaaa";
            skipsubstrlenonmatch = false;
            expected = 1;
            returned = Util.CountSubStringOccurences(pattern, data, skipsubstrlenonmatch);

            Assert.AreEqual(expected, returned);

            // --- TEST 6 ---

            pattern = "aaaaa";
            data = "a";
            skipsubstrlenonmatch = false;
            expected = 0;
            returned = Util.CountSubStringOccurences(pattern, data, skipsubstrlenonmatch);

            Assert.AreEqual(expected, returned);

            // --- TEST 7 ---

            pattern = null;
            data = "a";
            skipsubstrlenonmatch = false;
            expected = 0;
            returned = Util.CountSubStringOccurences(pattern, data, skipsubstrlenonmatch);

            Assert.AreEqual(expected, returned);

            // --- TEST 8 ---

            pattern = "a";
            data = null;
            skipsubstrlenonmatch = false;
            expected = 0;
            returned = Util.CountSubStringOccurences(pattern, data, skipsubstrlenonmatch);

            Assert.AreEqual(expected, returned);
        }
    }
}
