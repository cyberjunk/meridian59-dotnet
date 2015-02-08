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
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using Meridian59.Common;
using Meridian59.Common.Constants;
using Meridian59.Files.BGF;
using Meridian59.Files.ROO;
using Meridian59.Files.RSB;
using Meridian59.Data.Lists;
using Meridian59.Data.Models;

namespace Meridian59.Files
{
    /// <summary>
    /// Handles access to legacy M59 resources (ROO, BGF, ...)
    /// </summary>
    public class ResourceManager
    {
        #region Constants
        protected const string NOTFOUND = "Error: StringResources file or Bgf/Roo/Wav/Music folder not found.";
        protected const string DEFAULTSTRINGFILE = "rsc0000.rsb";
        #endregion

        #region Properties

        /// <summary>
        /// The dictionary containing all bgf filenames related to objects (no grdXXXX.bgf)
        /// </summary>
        public LockingDictionary<string, BgfFile> Objects { get; protected set; }

        /// <summary>
        /// The dictionary containing all bgf filenames related to roomtextures (grdXXXX.bgf)
        /// </summary>
        public LockingDictionary<string, BgfFile> RoomTextures { get; protected set; }

        /// <summary>
        /// The dictionary containing the room resources (filenames)
        /// </summary>
        public LockingDictionary<string, RooFile> Rooms { get; protected set; }

        /// <summary>
        /// WAV soundfiles
        /// </summary>
        public LockingDictionary<string, Tuple<IntPtr, uint>> Wavs { get; protected set; }

        /// <summary>
        /// Music
        /// </summary>
        public LockingDictionary<string, Tuple<IntPtr, uint>> Music { get; protected set; }

        /// <summary>
        /// True if InitConfig was executed.
        /// </summary>
        public bool Initialized { get; protected set; }

        /// <summary>
        /// Configuration for ResourceManager
        /// </summary>
        public ResourceManagerConfig Config { get; protected set; }

        /// <summary>
        /// Stores the string resources from rsc0000.rsb
        /// </summary>
        public LockingDictionary<uint, string> StringResources { get; protected set; }

        /// <summary>
        /// The mail objects.
        /// Adding or removing entries here will save/delete them also
        /// from the disk once InitConfig was called.
        /// </summary>
        public MailList Mails { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// Tries to retrieve a BGF file from the Objects dictionary.
        /// Will load the file from disk, if not yet loaded.
        /// </summary>
        /// <param name="File">Plain filename with extension (e.g. mushroom.bgf)</param>
        /// <returns></returns>
        public BgfFile GetObject(string File)
        {
            BgfFile bgfFile = null;

            // if the file is known
            if (Objects.TryGetValue(File, out bgfFile))
            {
                // haven't loaded it yet?
                if (bgfFile == null)
                {
                    // load it
                    bgfFile = new BgfFile(Config.ObjectsFolder + "/" + File);

                    // update the registry                 
                    Objects.TryUpdate(File, bgfFile, null);
                }
            }

            return bgfFile;
        }

        /// <summary>
        /// Tries to retrieve a ROO file from the Rooms dictionary.
        /// Will load the file from disk, if not yet loaded.
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        public RooFile GetRoom(string File)
        {           
            RooFile rooFile = null;

            // if the file is known
            if (Rooms.TryGetValue(File, out rooFile))
            {
                // haven't loaded it yet?
                if (rooFile == null)
                {                  
                    // load it
                    rooFile = new RooFile(Config.RoomsFolder + "/" + File);

                    // resolve resource references (may load texture bgfs)
                    rooFile.ResolveResources(this);

                    // update the registry
                    Rooms.TryUpdate(File, rooFile, null);                  
                }
            }

            return rooFile;            
        }

        /// <summary>
        /// Tries to retrieve a BGF file from the RoomTextures dictionary.
        /// Will load the file from disk, if not yet loaded.
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        public BgfFile GetRoomTexture(string File)
        {
            BgfFile bgfFile = null;

            // if the file is known
            if (RoomTextures.TryGetValue(File, out bgfFile))
            {
                // haven't loaded it yet?
                if (bgfFile == null)
                {
                    // load it
                    bgfFile = new BgfFile(Config.RoomTexturesFolder + "/" + File);

                    // update the registry
                    RoomTextures.TryUpdate(File, bgfFile, null);
                }
            }

            return bgfFile;
        }

        /// <summary>
        /// Tries to retrieve a BGF file from the RoomTextures dictionary.
        /// Will load the file from disk, if not yet loaded.
        /// </summary>
        /// <param name="GrdNumber">The XXXXX number in grdXXXXX</param>
        /// <returns></returns>
        public BgfFile GetRoomTexture(ushort GrdNumber)
        {
            string file = ConvertGrdNumberToFileName(GrdNumber, FileExtensions.BGF);
            return GetRoomTexture(file);
        }

        /// <summary>
        /// Tries to retrieve a WAV file from the Wavs dictionary.
        /// Will load the file from disk, if not yet loaded.
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        public Tuple<IntPtr, uint> GetWavFile(string File)
        {
            Tuple<IntPtr, uint> wavData = null;

            // if the file is known
            if (Wavs.TryGetValue(File, out wavData))
            {
                // haven't loaded it yet?
                if (wavData == null)
                {
                    // load it
                    wavData = Util.LoadFileToUnmanagedMem(Config.WavFolder + "/" + File);
                    
                    // update the registry
                    Wavs.TryUpdate(File, wavData, null);
                }
            }

            return wavData;
        }

        /// <summary>
        /// Tries to retrieve a MP3 file from the Music dictionary.
        /// Will load the file from disk, if not yet loaded.
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        public Tuple<IntPtr, uint> GetMusicFile(string File)
        {
            Tuple<IntPtr, uint> musicData = null;

            // if the file is known
            if (Music.TryGetValue(File, out musicData))
            {
                // haven't loaded it yet?
                if (musicData == null)
                {
                    // load it
                    musicData = Util.LoadFileToUnmanagedMem(Config.MusicFolder + "/" + File);

                    // update the registry
                    Music.TryUpdate(File, musicData, null);
                }
            }

            return musicData;
        }

        /// <summary>
        /// Gets a BGF filename for a grd-number
        /// </summary>
        /// <param name="GridNumber">The gridnumber to get texture files for.</param>
        /// <param name="Extension">Append this file extension to result.</param>
        /// <returns>Filename like grd00032.bgf</returns>
        public static string ConvertGrdNumberToFileName(ushort GridNumber, string Extension)
        {
            // start with pure number, e.g. 5
            string filter = GridNumber.ToString();

            // add 0 prefixed until number has length of 5
            int loops = 5 - filter.Length;
            for (int i = 0; i < loops; i++)
                filter = filter.Insert(0, "0");

            // so we have 00005, add "grd" prefix now for grd00005
            filter = filter.Insert(0, "grd");

            // add "-*.png" for the png indices filter
            filter += Extension;

            return filter;
        }

        /// <summary>
        /// Initializes the ResourceManager from given config.
        /// </summary>
        /// <param name="Config"></param>
        public void InitConfig(ResourceManagerConfig Config)
        {
            string[] files;

            // save new config
            this.Config = Config;

            // already executed once?
            if (Initialized)
            {
                // clear current string dictionary
                StringResources.Clear();

                // clear current lookup dictionaries
                Objects.Clear();
                RoomTextures.Clear();
                Rooms.Clear();
                Wavs.Clear();
                Music.Clear();

                // detach mail listener so we don't delete them
                Mails.ListChanged -= OnMailsListChanged;

                // clear our objects
                Mails.Clear();
            }

            // register strings
            if (File.Exists(Config.StringResourcesFile))
            {
                // load string resources
                RsbFile rsbFile = new RsbFile(Config.StringResourcesFile);

                // add to dictionary
                foreach (KeyValuePair<uint, string> pair in rsbFile.StringResources)
                    StringResources.TryAdd(pair.Key, pair.Value);
            }

            // register objects
            if (Directory.Exists(Config.ObjectsFolder))
            {
                // get available files
                files = Directory.GetFiles(Config.ObjectsFolder, '*' + FileExtensions.BGF);
                
                foreach (string s in files)
                {
                    string filename = Path.GetFileName(s);

                    if (!filename.StartsWith("grd"))
                        Objects.TryAdd(filename, null);                  
                }
            }

            // register roomtextures
            if (Directory.Exists(Config.RoomTexturesFolder))
            {
                // get available files
                files = Directory.GetFiles(Config.RoomTexturesFolder, "grd*" + FileExtensions.BGF);
                
                foreach (string s in files)                
                    RoomTextures.TryAdd(Path.GetFileName(s), null);                
            }

            // register rooms           
            if (Directory.Exists(Config.RoomsFolder))
            {
                // get available files
                files = Directory.GetFiles(Config.RoomsFolder, '*' + FileExtensions.ROO);
                
                foreach (string s in files)               
                    Rooms.TryAdd(Path.GetFileName(s), null);              
            }

            // register wav sounds          
            if (Directory.Exists(Config.WavFolder))
            {
                // get available files
                files = Directory.GetFiles(Config.WavFolder, '*' + FileExtensions.WAV);
                
                foreach (string s in files)                                
                    Wavs.TryAdd(Path.GetFileName(s), null);                                  
            }

            // register music         
            if (Directory.Exists(Config.MusicFolder))
            {
                // get available files
                files = Directory.GetFiles(Config.MusicFolder, '*' + FileExtensions.MP3);
                
                foreach (string s in files)                
                    Music.TryAdd(Path.GetFileName(s), null);                                  
            }

            // load mails          
            if (Directory.Exists(Config.MailFolder))
            {
                // read available mail files
                files = Directory.GetFiles(Config.MailFolder, '*' + FileExtensions.XML);
                foreach (string s in files)
                {
                    // create mail object
                    Mail mail = new Mail();

                    // load values from xml
                    mail.Load(s);

                    // add to list
                    Mails.Add(mail);
                }
            }

            // hookup mails listener to write/delete the files
            Mails.ListChanged += OnMailsListChanged;

            // forced GC collection
            GC.Collect(2);

            // mark initialized
            Initialized = true;          
        }

        /// <summary>
        /// Clears and reloads the strings from another dictionary file.
        /// </summary>
        /// <param name="RsbFile"></param>
        public void ReloadStrings(string RsbFile)
        {
            // update stringfile
            Config.StringResourcesFile = RsbFile;

            // clear old entries
            StringResources.Clear();

            // check if new string dictionary exists
            if (File.Exists(Config.StringResourcesFile))
            {
                // load string resources
                RsbFile rsbFile = new RsbFile(Config.StringResourcesFile);

                // add to dictionary
                foreach (KeyValuePair<uint, string> pair in rsbFile.StringResources)
                    StringResources.TryAdd(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// Preloads all elements in the Objects dictionary.
        /// </summary>
        public void PreloadObjects()
        {
            IEnumerator<KeyValuePair<string, BgfFile>> it = Objects.GetEnumerator();
            BgfFile file;

            while (it.MoveNext())
            {
                // load
                file = new BgfFile(Path.Combine(Config.ObjectsFolder, it.Current.Key));
                file.DecompressAll();

                // update
                Objects.TryUpdate(it.Current.Key, file, null);
            }

            GC.Collect(2);
        }

        /// <summary>
        /// Preloads all elements in the RoomTextures dictionary.
        /// </summary>
        public void PreloadRoomTextures()
        {
            IEnumerator<KeyValuePair<string, BgfFile>> it = RoomTextures.GetEnumerator();
            BgfFile file;

            while (it.MoveNext())
            {
                // load
                file = new BgfFile(Path.Combine(Config.RoomTexturesFolder, it.Current.Key));
                file.DecompressAll();

                // update
                RoomTextures.TryUpdate(it.Current.Key, file, null);
            }

            GC.Collect(2);
        }

        /// <summary>
        /// Preloads all elements in the Rooms dictionary.
        /// </summary>
        public void PreloadRooms()
        {
            IEnumerator<KeyValuePair<string, RooFile>> it = Rooms.GetEnumerator();
            RooFile file;

            while (it.MoveNext())
            {
                // load
                file = new RooFile(Path.Combine(Config.RoomsFolder, it.Current.Key));
                
                // update
                Rooms.TryUpdate(it.Current.Key, file, null);
            }

            GC.Collect(2);
        }

        /// <summary>
        /// Preloads all elements in the Wavs dictionary.
        /// </summary>
        public void PreloadSounds()
        {
            IEnumerator<KeyValuePair<string, Tuple<IntPtr, uint>>> it = Wavs.GetEnumerator();
            Tuple<IntPtr, uint> wavData = null;

            while (it.MoveNext())
            {
                // load it
                wavData = Util.LoadFileToUnmanagedMem(
                    Path.Combine(Config.WavFolder, it.Current.Key));

                // update
                Wavs.TryUpdate(it.Current.Key, wavData, null);
            }

            GC.Collect(2);
        }

        /// <summary>
        /// Preloads all elements in the Music dictionary.
        /// </summary>
        public void PreloadMusic()
        {
            IEnumerator<KeyValuePair<string, Tuple<IntPtr, uint>>> it = Music.GetEnumerator();
            Tuple<IntPtr, uint> mp3Data = null;

            while (it.MoveNext())
            {
                // load it
                mp3Data = Util.LoadFileToUnmanagedMem(
                    Path.Combine(Config.MusicFolder, it.Current.Key));

                // update
                Music.TryUpdate(it.Current.Key, mp3Data, null);
            }

            GC.Collect(2);
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ResourceManager()
        {
            // string lookup dictionary
            StringResources = new LockingDictionary<uint, string>();

            // maillist
            Mails = new MailList(200);

            // lookup dictionaries for resources
            Objects = new LockingDictionary<string, BgfFile>(StringComparer.OrdinalIgnoreCase);
            RoomTextures = new LockingDictionary<string, BgfFile>(StringComparer.OrdinalIgnoreCase);
            Rooms = new LockingDictionary<string, RooFile>(StringComparer.OrdinalIgnoreCase);
            Wavs = new LockingDictionary<string, Tuple<IntPtr, uint>>(StringComparer.OrdinalIgnoreCase);
            Music = new LockingDictionary<string, Tuple<IntPtr, uint>>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Executed when MailList changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnMailsListChanged(object sender, ListChangedEventArgs e)
        {
            string file;

            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:                    
                    // get full path of new mail
                    file = Path.Combine(
                        Config.MailFolder,
                        Mails.LastAddedItem.GetFilename());

                    // save it
                    Mails.LastAddedItem.Save(file);                    
                    break;

                case ListChangedType.ItemDeleted:
                    // get full path of deleted mail
                    file = Path.Combine(
                        Config.MailFolder,
                        Mails.LastAddedItem.GetFilename());

                    // delete it
                    File.Delete(file);
                    break;
            }
        }
    }
}
