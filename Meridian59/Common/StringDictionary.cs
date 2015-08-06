/*
 Copyright (c) 2012-2013 Clint Banzhaf
 This file is part of "Meridian59 .NET".

 "Meridian59 .NET" is free software: 
 You can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 either version 3 of the License, or (at your option) any later version.

 "Meridian59 .NET" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 See the GNU General Public License for more details.

 You should have received a copy of the GNU General Public License along with "Meridian59 .NET".
 If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.Collections.Generic;
using Meridian59.Common.Enums;
using System.Collections;

namespace Meridian59.Common
{
    /// <summary>
    /// Uses an underlying LockingDictionary with some
	/// specific additions to handle Meridian 59 resource strings.
    /// </summary>
	/// <remarks>
	/// Multilanguage dictionaries don't have a single unique primary key (resource id).
	/// Instead a string is addressed by a key tuple (id, language-code).
	/// To handle this issue we map the 2d tuple address range (id, language code) into a
	/// 1d addressrange where each language-code gets its own bracket of the target id space.
	/// Using UInt32 as target keyspace (4294967295) for the transformation and assuming all 184 languages
	/// specified in LanguageCode enum leaves 4294967295/184 = 23342213 available ids for each language.
	/// </remarks>
    public class StringDictionary : IEnumerable<KeyValuePair<uint, string>>
	{
		/// <summary>
		/// Language used if the defined lanuage has no result
		/// </summary>
		public const LanguageCode FALLBACKLANGUAGE = LanguageCode.English;

		/// <summary>
		/// The size of an ID space bracket for multilanguage support
		/// </summary>
		public const uint BRACKETSIZE = UInt32.MaxValue / (uint)LanguageCode._COUNT;

		/// <summary>
		/// This is the embedded dictionary we use.
		/// Seemed cleaner than deriving from it, because of impossible
		/// override/new due to different function signatures etc.
		/// </summary>
		protected readonly LockingDictionary<uint, string> dictionary =
			new LockingDictionary<uint, string>();

		/// <summary>
		/// Count property
		/// </summary>
		public uint Count { get { return (uint)dictionary.Count; } }

		/// <summary>
		/// This is the default language the dictionary is using,
		/// if not specified explicitly in function signatures.
		/// Note: This is NOT the only contained language!
		/// </summary>
		public LanguageCode Language { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
 		public StringDictionary(LanguageCode Language = FALLBACKLANGUAGE) : base()
        {
			this.Language = Language;
        }

		/// <summary>
		/// Clears all elements from the dictionary
		/// </summary>
		public void Clear()
		{
			dictionary.Clear();
		}

		/// <summary>
		/// Tries to get a string for given ID and Language only.
		/// </summary>
		/// <param name="ResourceID"></param>
		/// <param name="Value"></param>
		/// <param name="Language"></param>
		/// <returns></returns>
		public bool TryGetValue(uint ResourceID, out string Value, LanguageCode Language)
		{
			return dictionary.TryGetValue(CombineKeys(ResourceID, Language), out Value);
		}

		/// <summary>
		/// Tries to get a string for given ID for language set in 'Language' property.
		/// If no entry for this language, will return FALLBACK language
		/// </summary>
		/// <param name="ResourceID"></param>
		/// <param name="Value"></param>
		/// <returns>False if not found in language AND fallback language</returns>
		public bool TryGetValue(uint ResourceID, out string Value)
		{
			if (!TryGetValue(ResourceID, out Value, Language) && Language != FALLBACKLANGUAGE)
				return TryGetValue(ResourceID, out Value, FALLBACKLANGUAGE);

			return true;
		}

		/// <summary>
		/// Tries to add an element to the dictionary.
		/// </summary>
		/// <param name="ResourceID"></param>
		/// <param name="Value"></param>
		/// <param name="Language"></param>
		/// <returns></returns>
		public bool TryAdd(uint ResourceID, string Value, LanguageCode Language)
		{
			return dictionary.TryAdd(CombineKeys(ResourceID, Language), Value);
		}

		/// <summary>
		/// Adds all entries from Values to this instance.
		/// </summary>
		/// <param name="Values"></param>
		public void AddRange(IEnumerable<KeyValuePair<uint, string>> Values)
		{
			// copies with combined keys
			foreach (KeyValuePair<uint, string> pair in Values)
				dictionary.TryAdd(pair.Key, pair.Value);
		}

		/// <summary>
		/// Tries to remove an element from the dictionary
		/// </summary>
		/// <param name="ResourceID"></param>
		/// <param name="Value"></param>
		/// <param name="Language"></param>
		/// <returns></returns>
		public bool TryRemove(uint ResourceID, out string Value, LanguageCode Language)
		{
			return dictionary.TryRemove(CombineKeys(ResourceID, Language), out Value);
		}

		/// <summary>
		/// Implements foreach
		/// </summary>
		/// <returns></returns>
		public IEnumerator<KeyValuePair<uint, string>> GetEnumerator()
		{
			return dictionary.GetEnumerator();
		}

		/// <summary>
		/// Implements foreach
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return dictionary.GetEnumerator();
		}

		/// <summary>
		/// Returns a combined unique key for given two keys by
		/// mapping each language into a id space bracket.
		/// </summary>
		/// <param name="ResourceID"></param>
		/// <param name="Language"></param>
		/// <returns></returns>
		public static uint CombineKeys(uint ResourceID, LanguageCode Language)
		{
			return ((uint)Language * BRACKETSIZE) + ResourceID;
		}

		public static LanguageCode GetLanguageFromCombined(uint CombinedKey)
		{
			return (LanguageCode)(CombinedKey / BRACKETSIZE);
		}

		public static uint GetResourceIDFromCombined(uint CombinedKey)
		{
			return (CombinedKey % BRACKETSIZE);
		}
	}
}
