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

namespace Meridian59.Files
{
    /// <summary>
    /// Configuration for the ResourceManager
    /// </summary>
    public class ResourceManagerConfig
    {
        /// <summary>
        /// The DownloadVersion of your resources (see Download= in meridian.ini)
        /// </summary>
        public uint DownloadVersion { get; set; }
              
        /// <summary>
        /// The file with all the strings (usually rsc0000.rsb)
        /// </summary>
        public string StringResourcesFile { get; set; }

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
        /// Constructor
        /// </summary>
        /// <param name="DownloadVersion"></param>
        /// <param name="StringResourcesFile"></param>
        /// <param name="RoomsFolder"></param>
        /// <param name="ObjectsFolder"></param>
        /// <param name="RoomTexturesFolder"></param>
        /// <param name="WavFolder"></param>
        /// <param name="MusicFolder"></param>
        /// <param name="MailFolder"></param>
        public ResourceManagerConfig(
            uint DownloadVersion,
            string StringResourcesFile,
            string RoomsFolder,
            string ObjectsFolder,
            string RoomTexturesFolder,
            string WavFolder,
            string MusicFolder,
            string MailFolder)
        {
            this.DownloadVersion = DownloadVersion;
            this.StringResourcesFile = StringResourcesFile;
            this.RoomsFolder = RoomsFolder;
            this.ObjectsFolder = ObjectsFolder;
            this.RoomTexturesFolder = RoomTexturesFolder;
            this.WavFolder = WavFolder;
            this.MusicFolder = MusicFolder;
            this.MailFolder = MailFolder;
        }
    }
}
