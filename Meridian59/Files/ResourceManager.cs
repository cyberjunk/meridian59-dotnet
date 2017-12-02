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
using System.Threading;
using Meridian59.Common.Enums;
using Meridian59.Common.Events;

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

        public const string SUBPATHSTRINGS = "strings";
        public const string SUBPATHROOMS = "rooms";
        public const string SUBPATHROOMTEXTURES = "bgftextures";
        public const string SUBPATHOBJECTS = "bgfobjects";
        public const string SUBPATHSOUNDS = "sounds";
        public const string SUBPATHMUSIC = "music";
        public const string SUBPATHMAILS = "mails";
        #endregion

        #region Fields
		protected readonly StringDictionary stringResources = new StringDictionary();
        protected readonly LockingDictionary<string, RsbFile> stringDictionaries = new LockingDictionary<string, RsbFile>(StringComparer.OrdinalIgnoreCase);
        protected readonly LockingDictionary<string, BgfFile> objects = new LockingDictionary<string, BgfFile>(StringComparer.OrdinalIgnoreCase);
        protected readonly LockingDictionary<string, BgfFile> roomTextures = new LockingDictionary<string, BgfFile>(StringComparer.OrdinalIgnoreCase);
        protected readonly LockingDictionary<string, RooFile> rooms = new LockingDictionary<string, RooFile>(StringComparer.OrdinalIgnoreCase);
        protected readonly LockingDictionary<string, Tuple<IntPtr, uint>> sounds = new LockingDictionary<string, Tuple<IntPtr, uint>>(StringComparer.OrdinalIgnoreCase);
        protected readonly LockingDictionary<string, Tuple<IntPtr, uint>> music = new LockingDictionary<string, Tuple<IntPtr, uint>>(StringComparer.OrdinalIgnoreCase);
        protected readonly MailList mails = new MailList(200);

        protected readonly LockingQueue<string> queueAsyncFilesLoaded = new LockingQueue<string>();

        protected int numLoadedObjects;
        protected int numLoadedRoomTextures;
        protected int numLoadedRooms;
        protected int numLoadedWavs;
        protected int numLoadedMusic;

        #endregion

        #region Properties
        /// <summary>
        /// Provides the currently active string resources
        /// </summary>
		public StringDictionary StringResources { get { return stringResources; } }

        /// <summary>
        /// Hash of the currently active string resources file
        /// </summary>
        public Hash128Bit RsbHash { get; set; }

        /// <summary>
        /// All string dictionaries (.rsb) files found
        /// </summary>
        public LockingDictionary<string, RsbFile> StringDictionaries { get { return stringDictionaries; } }
        
        /// <summary>
        /// The dictionary containing all bgf filenames related to objects (no grdXXXX.bgf)
        /// </summary>
        public LockingDictionary<string, BgfFile> Objects { get { return objects; } }

        /// <summary>
        /// The dictionary containing all bgf filenames related to roomtextures (grdXXXX.bgf)
        /// </summary>
        public LockingDictionary<string, BgfFile> RoomTextures { get { return roomTextures; } }

        /// <summary>
        /// The dictionary containing the room resources (filenames)
        /// </summary>
        public LockingDictionary<string, RooFile> Rooms { get { return rooms; } }

        /// <summary>
        /// WAV soundfiles
        /// </summary>
        public LockingDictionary<string, Tuple<IntPtr, uint>> Wavs { get { return sounds; } }

        /// <summary>
        /// Music
        /// </summary>
        public LockingDictionary<string, Tuple<IntPtr, uint>> Music { get { return music; } }

        /// <summary>
        /// The mail objects.
        /// Adding or removing entries here will save/delete them also
        /// from the disk once InitConfig was called.
        /// </summary>
        public MailList Mails { get { return mails; } }

        /// <summary>
        /// True if InitConfig was executed.
        /// </summary>
        public bool Initialized { get; protected set; }

        /// <summary>
        /// Folder containing all .rsb files for different servers.
        /// </summary>
        public string StringsFolder { get; set; }

        /// <summary>
        /// Folder containing all .roo files
        /// </summary>
        public string RoomsFolder { get; set; }

        /// <summary>
        /// Folder containing all object BGFs
        /// </summary>
        public string ObjectsFolder { get; set; }

        /// <summary>
        /// Folder containing all roomtexture BGFs
        /// </summary>
        public string RoomTexturesFolder { get; set; }

        /// <summary>
        /// Folder containing all WAV soundfiles
        /// </summary>
        public string WavFolder { get; set; }

        /// <summary>
        /// Folder containing music
        /// </summary>
        public string MusicFolder { get; set; }

        /// <summary>
        /// Folder containing mails
        /// </summary>
        public string MailFolder { get; set; }

        /// <summary>
        /// Amount of loaded BGF objects
        /// </summary>
        public int NumLoadedObjects {  get { return numLoadedObjects; } }

        /// <summary>
        /// Amount of loaded BGF room textures
        /// </summary>
        public int NumLoadedRoomTextures { get { return numLoadedRoomTextures; } }

        /// <summary>
        /// Amount of loaded ROO files
        /// </summary>
        public int NumLoadedRooms { get { return numLoadedRooms; } }

        /// <summary>
        /// Amount of loaded Wav/Sound files
        /// </summary>
        public int NumLoadedWavs { get { return numLoadedWavs; } }

        /// <summary>
        /// Amount of loaded Mp3/Music files
        /// </summary>
        public int NumLoadedMusic { get { return numLoadedMusic; } }
        #endregion

        public event EventHandler PreloadingStarted;
        public event EventHandler PreloadingEnded;
        public event EventHandler<StringEventArgs> PreloadingFile;
        public event EventHandler StringDictionarySelected;

        #region Methods
        /// <summary>
        /// Tries to retrieve a RSB file from the Strings dictionary.
        /// Will load the file from disk, if not yet loaded.
        /// </summary>
        /// <param name="File">Plain filename with extension (e.g. rsc0000.rsb)</param>
        /// <returns></returns>
        public RsbFile GetStringDictionary(string File)
        {
            RsbFile rsbFile = null;

            // if the file is known
            if (StringDictionaries.TryGetValue(File, out rsbFile))
            {
                // haven't loaded it yet?
                if (rsbFile == null)
                {
                    // load it
                    rsbFile = new RsbFile(StringsFolder + "/" + File);

                    // update the registry                 
                    StringDictionaries.TryUpdate(File, rsbFile, null);
                }
            }

            return rsbFile;
        }

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
                    bgfFile = new BgfFile(ObjectsFolder + "/" + File);

                    // update the registry
                    if (Objects.TryUpdate(File, bgfFile, null))
                        numLoadedObjects++;
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
                    rooFile = new RooFile(RoomsFolder + "/" + File);

                    // resolve resource references (may load texture bgfs)
                    rooFile.ResolveResources(this);

                    // update the registry
                    if (Rooms.TryUpdate(File, rooFile, null))
                        numLoadedRooms++;
                }
                else
                {
                    rooFile.Reset();
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
                    bgfFile = new BgfFile(RoomTexturesFolder + "/" + File);

                    // update the registry
                    if (RoomTextures.TryUpdate(File, bgfFile, null))
                        numLoadedRoomTextures++;
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
                    wavData = Util.LoadFileToUnmanagedMem(WavFolder + "/" + File);

                    // update the registry
                    if (Wavs.TryUpdate(File, wavData, null))
                        numLoadedWavs++;
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
                    musicData = Util.LoadFileToUnmanagedMem(MusicFolder + "/" + File);

                    // update the registry
                    if (Music.TryUpdate(File, musicData, null))
                        numLoadedMusic++;
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
        /// Sets pathes to required resources.
        /// Will also remove any existing resources from the current lists.
        /// </summary>
        /// <param name="PathStrings"></param>
        /// <param name="PathRooms"></param>
        /// <param name="PathObjects"></param>
        /// <param name="PathRoomTextures"></param>
        /// <param name="PathSounds"></param>
        /// <param name="PathMusic"></param>
        /// <param name="PathMails"></param>
        public void Init(
            string PathStrings,
            string PathRooms,
            string PathObjects, 
            string PathRoomTextures, 
            string PathSounds, 
            string PathMusic, 
            string PathMails)        
        {
            this.StringsFolder = PathStrings;
            this.RoomsFolder = PathRooms;
            this.ObjectsFolder = PathObjects;
            this.RoomTexturesFolder = PathRoomTextures;
            this.WavFolder = PathSounds;
            this.MusicFolder = PathMusic;
            this.MailFolder = PathMails;

            string[] files;

            // already executed once?
            if (Initialized)
            {
                StringResources.Clear();
                StringDictionaries.Clear();
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

            // register string dictionaries for different servers
            if (Directory.Exists(StringsFolder))
            {
                // get available files
                files = Directory.GetFiles(StringsFolder, '*' + FileExtensions.RSB);

                foreach (string s in files)               
                    StringDictionaries.TryAdd(Path.GetFileName(s), null);               
            }
            else
                Directory.CreateDirectory(StringsFolder);

            // register objects
            if (Directory.Exists(ObjectsFolder))
            {
                // get available files
                files = Directory.GetFiles(ObjectsFolder, '*' + FileExtensions.BGF);
                
                foreach (string s in files)
                {
                    string filename = Path.GetFileName(s);

                    if (!filename.StartsWith("grd"))
                        Objects.TryAdd(filename, null);                  
                }
            }
            else
                Directory.CreateDirectory(ObjectsFolder);

            // register roomtextures
            if (Directory.Exists(RoomTexturesFolder))
            {
                // get available files
                files = Directory.GetFiles(RoomTexturesFolder, "grd*" + FileExtensions.BGF);
                
                foreach (string s in files)                
                    RoomTextures.TryAdd(Path.GetFileName(s), null);                
            }
            else
                Directory.CreateDirectory(RoomTexturesFolder);

            // register rooms           
            if (Directory.Exists(RoomsFolder))
            {
                // get available files
                files = Directory.GetFiles(RoomsFolder, '*' + FileExtensions.ROO);
                
                foreach (string s in files)               
                    Rooms.TryAdd(Path.GetFileName(s), null);              
            }
            else
                Directory.CreateDirectory(RoomsFolder);

            // register wav sounds          
            if (Directory.Exists(WavFolder))
            {
                // get available files
                files = Directory.GetFiles(WavFolder, '*' + FileExtensions.OGG);
                
                foreach (string s in files)                                
                    Wavs.TryAdd(Path.GetFileName(s), null);                                  
            }
            else
                Directory.CreateDirectory(WavFolder);

            // register music         
            if (Directory.Exists(MusicFolder))
            {
                // get available files
                files = Directory.GetFiles(MusicFolder, '*' + FileExtensions.OGG);
                
                foreach (string s in files)                
                    Music.TryAdd(Path.GetFileName(s), null);                                  
            }
            else
                Directory.CreateDirectory(MusicFolder);

            // load mails          
            if (Directory.Exists(MailFolder))
            {
                // read available mail files
                files = Directory.GetFiles(MailFolder, '*' + FileExtensions.XML);
                foreach (string s in files)
                {
                    // create mail object
                    Mail mail = new Mail();

                    // load values from xml
                    mail.Load(s);

#if !VANILLA && !OPENMERIDIAN
                    // Fix timestamp if necessary and write out mail.
                    if (!mail.IsTimestampUpdated)
                    {
                        mail.Timestamp += MeridianDate.CONVERTOFFSET;
                        mail.IsTimestampUpdated = true;
                        mail.Save(s);
                    }
#endif
                    // add to list
                    Mails.Add(mail);
                }
            }
            else
                Directory.CreateDirectory(MailFolder);

            // hookup mails listener to write/delete the files
            Mails.ListChanged += OnMailsListChanged;

            // forced GC collection
            Util.ForceMaximumGC();

            // mark initialized
            Initialized = true;          
        }

        /// <summary>
        /// Clears and reloads the strings from another dictionary file within the strings folder,
        /// which was previously initialized during Init()
        /// </summary>
        /// <param name="RsbFile">Plain filename, like rsc0000.rsb</param>
		/// <param name="Language"></param>
        public void SelectStringDictionary(string RsbFile, LanguageCode Language)
        {
            // clear old entries
            StringResources.Clear();

			// set preferred language
			StringResources.Language = Language;
            
            // Save the MD5 hash of this rsb file as our RsbHash.
            byte[] rsbMD5Hash = MeridianMD5.ComputeGenericFileMD5(StringsFolder + "/" + RsbFile);
            Hash128Bit rsbHash = new Hash128Bit();
            rsbHash.HASH1 = BitConverter.ToUInt32(rsbMD5Hash, 0);
            rsbHash.HASH2 = BitConverter.ToUInt32(rsbMD5Hash, 4);
            rsbHash.HASH3 = BitConverter.ToUInt32(rsbMD5Hash, 8);
            rsbHash.HASH4 = BitConverter.ToUInt32(rsbMD5Hash, 12);
            RsbHash = rsbHash;

            // try get the dictionary for argument
            RsbFile file = GetStringDictionary(RsbFile);
            
            // load strings of the rsbfile to use
            // into the multithreaded dictionary
            if (file != null)
                foreach (RsbResourceID res in file.StringResources)
                    StringResources.TryAdd(res.ID, res.Text, res.Language);

            // raise event
            if (StringDictionarySelected != null)
                StringDictionarySelected(this, new EventArgs());
        }

        /// <summary>
        /// Starts preloading resources in several threads.
        /// </summary>
        /// <param name="Objects"></param>
        /// <param name="RoomTextures"></param>
        /// <param name="Rooms"></param>
        /// <param name="Sounds"></param>
        /// <param name="Music"></param>
        public void Preload(bool Objects, bool RoomTextures, bool Rooms, bool Sounds, bool Music)
        {
            Thread threadObjects        = null;
            Thread threadRoomTextures   = null;
            Thread threadRooms          = null;
            Thread threadSounds         = null;
            Thread threadMusic          = null;

            if (PreloadingStarted != null)
                PreloadingStarted(this, new EventArgs());

            if (Objects)
            {
                threadObjects = new Thread(LoadThreadObjects);
                threadObjects.Start();          
            }

            if (RoomTextures)
            {
                threadRoomTextures = new Thread(LoadThreadRoomTextures);
                threadRoomTextures.Start();           
            }

            if (Rooms)
            {
                threadRooms = new Thread(LoadThreadRooms);
                threadRooms.Start();
            }

            if (Sounds)
            {
                threadSounds = new Thread(LoadThreadSounds);
                threadSounds.Start();          
            }

            if (Music)
            {
                threadMusic = new Thread(LoadThreadMusic);
                threadMusic.Start();
            }

            string filename;

            // lock until all loaders are finished
            while ( (threadObjects != null && threadObjects.IsAlive) ||
                    (threadRoomTextures != null && threadRoomTextures.IsAlive) ||
                    (threadRooms != null && threadRooms.IsAlive) ||
                    (threadSounds != null && threadSounds.IsAlive) ||
                    (threadMusic != null && threadMusic.IsAlive))
            {

                while (queueAsyncFilesLoaded.TryDequeue(out filename))              
                    if (PreloadingFile != null)
                        PreloadingFile(this, new StringEventArgs(filename));               
            }

            while (queueAsyncFilesLoaded.TryDequeue(out filename))
                if (PreloadingFile != null)
                    PreloadingFile(this, new StringEventArgs(filename));

            if (PreloadingEnded != null)
                PreloadingEnded(this, new EventArgs());
        }

        /// <summary>
        /// Stars loading all objects in a background thread.
        /// </summary>
        protected void LoadThreadObjects()
        {
            IEnumerator<KeyValuePair<string, BgfFile>> it = Objects.GetEnumerator();
            BgfFile file;

            while (it.MoveNext())
            {
                // load
                file = new BgfFile(Path.Combine(ObjectsFolder, it.Current.Key));
                file.DecompressAll();

                // update
                Objects.TryUpdate(it.Current.Key, file, null);
                numLoadedObjects++;

                queueAsyncFilesLoaded.Enqueue(it.Current.Key);
            }
        }

        /// <summary>
        /// Stars loading all roomtextures in a background thread.
        /// </summary>
        protected void LoadThreadRoomTextures()
        {
            IEnumerator<KeyValuePair<string, BgfFile>> it = RoomTextures.GetEnumerator();
            BgfFile file;

            while (it.MoveNext())
            {
                // load
                file = new BgfFile(Path.Combine(RoomTexturesFolder, it.Current.Key));
                file.DecompressAll();

                // update
                RoomTextures.TryUpdate(it.Current.Key, file, null);
                numLoadedRoomTextures++;

                queueAsyncFilesLoaded.Enqueue(it.Current.Key);
            }
        }

        /// <summary>
        /// Stars loading all rooms in a background thread.
        /// </summary>
        protected void LoadThreadRooms()
        {
            IEnumerator<KeyValuePair<string, RooFile>> it = Rooms.GetEnumerator();
            RooFile file;

            while (it.MoveNext())
            {
                // load
                file = new RooFile(Path.Combine(RoomsFolder, it.Current.Key));

                // update
                Rooms.TryUpdate(it.Current.Key, file, null);
                numLoadedRooms++;

                queueAsyncFilesLoaded.Enqueue(it.Current.Key);
            }
        }

        /// <summary>
        /// Stars loading all sounds in a background thread.
        /// </summary>
        protected void LoadThreadSounds()
        {
            IEnumerator<KeyValuePair<string, Tuple<IntPtr, uint>>> it = Wavs.GetEnumerator();
            Tuple<IntPtr, uint> wavData = null;

            while (it.MoveNext())
            {
                // load it
                wavData = Util.LoadFileToUnmanagedMem(
                    Path.Combine(WavFolder, it.Current.Key));

                // update
                Wavs.TryUpdate(it.Current.Key, wavData, null);
                numLoadedWavs++;

                queueAsyncFilesLoaded.Enqueue(it.Current.Key);
            }
        }

        /// <summary>
        /// Stars loading all music in a background thread.
        /// </summary>
        protected void LoadThreadMusic()
        {
            IEnumerator<KeyValuePair<string, Tuple<IntPtr, uint>>> it = Music.GetEnumerator();
            Tuple<IntPtr, uint> mp3Data = null;

            while (it.MoveNext())
            {
                // load it
                mp3Data = Util.LoadFileToUnmanagedMem(
                    Path.Combine(MusicFolder, it.Current.Key));

                // update
                Music.TryUpdate(it.Current.Key, mp3Data, null);
                numLoadedMusic++;

                queueAsyncFilesLoaded.Enqueue(it.Current.Key);
            }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ResourceManager()
        {
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
                        MailFolder,
                        Mails.LastAddedItem.GetFilename());

                    // save it
                    Mails.LastAddedItem.Save(file);                    
                    break;

                case ListChangedType.ItemDeleted:
                    // get full path of deleted mail
                    file = Path.Combine(
                        MailFolder,
                        Mails.LastDeletedItem.GetFilename());

                    // delete it
                    if (File.Exists(file))
                        File.Delete(file);
                    break;
            }
        }
    }
}
