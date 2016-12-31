using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Meridian59.Bot.IRC
{
    /// <summary>
    /// General IRCBot utility functions
    /// </summary>
    public static class Util
    {
        #region Constants
        private const string UTIL_GARBAGECOLLECTSTRING = "GARBAGE COLLECTION: ALL REFERENCES REORDERED";
        #endregion Constants

        /// <summary>
        /// Utility: IRC does not allow line breaks - 
        /// so we have to split up into lines first.
        /// </summary>
        /// <returns></returns>
        public static string[] SplitLinebreaks(string Text)
        {
            // replace \r\n linebreaks (adminconsole) with \n
            string s = Text.Replace("\r\n", "\n");

            // remove single \r
            s = s.Replace("\r", String.Empty);

            // now split into lines by \n
            return s.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Returns true if the string matches the GC admin message sent by the server.
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static bool IsGarbageCollectMessage(string Message)
        {
            if (Message != null && Message.Contains(UTIL_GARBAGECOLLECTSTRING))
                return true;
            return false;
        }

        /// <summary>
        /// Returns true if the string contains a disallowed DM command.
        /// Example would be 'getplayer'.
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static bool IsDisallowedDMCommand(string Message)
        {
            if (Message == null)
                return false;
            return Regex.Match(Message, @"^(get|go|echo)", RegexOptions.IgnoreCase).Success;
        }
    }
}
