using System.Globalization;

namespace Meridian59.Patcher
{
    // Workaround to avoid shipping an extra folder + dll with the updater
    // to display German strings.

    // New languages could be added by:
    //   1. Add the name to the LanguageIdentifier enum.
    //   2. Assign it to languageIdentifier in SetLanguageIdentifier.
    //   3. Add string(s) to the const section.
    //   4. Add the case for your language string to the appropriate string getter.
    public class LanguageHandler
    {
        #region Constants
        private enum LanguageIdentifier
        {
            English,
            German
        }

        private const string ERROR_EN = "Error";
        private const string ERROR_DE = "Fehler";

        private const string ABORT_EN = "Abort";
        private const string ABORT_DE = "Abbrechen";

        private const string INFO_EN = "Info";
        private const string INFO_DE = "Info";

        private const string CONFIRMCANCEL_EN = "Are you sure you want to cancel the client update?";
        private const string CONFIRMCANCEL_DE = "Möchtest du das Update wirklich abbrechen?";

        private const string CLIENTEXEMISSINGERROR_EN = "Fatal error finding the client executable. Try to disable your " + 
            "anti-virus software and repeat patching. If this issue persists please report it in the forums at meridiannext.com/phpbb3.";
        private const string CLIENTEXEMISSINGERROR_DE = "Schwerer Fehler beim Auffinden der Startdatei des Clients. Versuche deine " +
            "Anti-Viren Software zu deaktivieren und das Patchen zu wiederholen. Sollte dieser Fehler weiterhin bestehen, dann " +
            "melde dies bitte im Forum auf meridiannext.com/phpbb3.";

        private const string URLINFOMISSING_EN = "Patch information from file {0} is missing. You may be " +
            "able to obtain this file automatically by using the game client to connect to a valid server. " +
            "If you are unable to do this, you will need to run the client installer again.";
        private const string URLINFOMISSING_DE = "Patch Information der Datei {0} fehlt. Du kannst diese Datei " +
            "möglicherweise automatisch abrufen, indem du den Client startest und dich mit einem gültigen " +
            "Server verbindest. Wenn dies nicht möglich ist, musst du das Installationsprogramm des Clients erneut ausführen ";

        private const string URLINFOBROKEN_EN = "Patch information file {0} is corrupted. You may be " +
            "able to reobtain this file automatically by using the game client to connect to a valid server. " +
            "If you are unable to do this, you will need to run the client installer again.";
        private const string URLINFOBROKEN_DE = "Patch Information der Datei {0} ist beschädigt . Du kannst diese Datei " +
            "möglicherweise automatisch wiederherstellen, indem du den Client startest und dich mit einem gültigen " +
            "Server verbindest. Wenn dies nicht möglich ist, musst du das Installationsprogramm des Clients erneut ausführen ";

        private const string JSONDOWNLOADFAILED_EN = "Download of patch data file failed. This could be due to an " +
            "internet connection issue or patch server issues. If retrying fails, please check your connection " +
            "or try again later.";
        private const string JSONDOWNLOADFAILED_DE = "Der Download der Patch-Datei ist fehlgeschlagen. Dies kann an " +
            "einem Problem mit deiner Internetverbindung oder dem Patch-Server zusammenhängen. Wenn der Wiederholungsversuch " +
            "fehlschlägt, überprüfe deine Internetverbindung oder versuchen es später erneut.";

        private const string RETRYINGFILE_EN = "Download of file {0} failed, retrying...\n";
        private const string RETRYINGFILE_DE = "Download der Datei {0} ist fehlgeschlagen, neuer Versuch...\n";

        private const string FILEFAILED_EN = "Download of file {0} failed. This could be due to an " +
            "internet connection issue or patch server issues. Please try again later.";
        private const string FILEFAILED_DE = "Download der Datei {0} ist fehlgeschlagen. Dies kann an " +
            "einem Problem mit deiner Internetverbindung oder dem Patch-Server zusammenhängen. " +
            "Bitte versuche es später erneut.";

        private const string PATCHDOWNLOADFAILED_EN = "Patch information download failed!\n";
        private const string PATCHDOWNLOADFAILED_DE = "Herunterladen der Patch-Informationen fehlgeschlagen!\n";

        private const string FILEDOWNLOADED_EN = "Downloaded file {0}\n...";
        private const string FILEDOWNLOADED_DE = "Datei {0} heruntergeladen\n...";

        private const string DOWNLOADINGPATCH_EN = "Step 1/{0}: Downloading patch-info...";
        private const string DOWNLOADINGPATCH_DE = "Schritt 1/{0}: Lade Patch-Info...";

        private const string SCANNINGFILES_EN = "Step 2/{0}: Comparing files...";
        private const string SCANNINGFILES_DE = "Schritt 2/{0}: Vergleiche Dateien...";

        private const string DOWNLOADINIT_EN = "Step 3/{0}: Downloading update...";
        private const string DOWNLOADINIT_DE = "Schritt 3/{0}: Lade Update...";

        private const string NGENINIT_EN = "Step 4/4: Optimizing startup...";
        private const string NGENINIT_DE = "Schritt 4/4: Optimiere Start...";

        private const string CLIENTUPTODATE_EN = "Client up to date, nothing to download";
        private const string CLIENTUPTODATE_DE = "Client ist auf dem neusten Stand";

        private const string CLIENTWASUPDATED_EN = "Client update complete";
        private const string CLIENTWASUPDATED_DE = "Client wurde aktualisiert";

        private const string NUMFILES_EN = "{0}/{1} files";
        private const string NUMFILES_DE = "{0}/{1} Dateien";

        private const string UPDATEABORTED_EN = "Update aborted";
        private const string UPDATEABORTED_DE = "Update abgebrochen";

        private const string PROGRESSFINISHED_EN = "Complete";
        private const string PROGRESSFINISHED_DE = "Abgeschlossen";

        private const string PROGRESSABORTED_EN = "Update aborted";
        private const string PROGRESSABORTED_DE = "Abgebrochen";
        #endregion Constants

        private LanguageIdentifier languageIdentifier;

        public LanguageHandler()
        {
            SetLanguageIdentifier();

            /*
            // As far as I know changing languages in Windows isn't possible
            // without a restart, but if it becomes necessary to handle then
            // I think this would catch the change.
            SystemEvents.UserPreferenceChanged += (sender, e) =>
            {
                // Regional settings have changed
                if (e.Category == UserPreferenceCategory.Locale)
                {
                    SetLanguageIdentifier();
                }
            };*/
        }

        private void SetLanguageIdentifier()
        {
            // CurrentUICulture should correspond to the user's OS language choice.
            CultureInfo currentCulture = CultureInfo.CurrentUICulture;

            if (currentCulture.TwoLetterISOLanguageName.Equals(CultureInfo.GetCultureInfo("de").TwoLetterISOLanguageName) // only need to check this one?
                || currentCulture.TwoLetterISOLanguageName.Equals(CultureInfo.GetCultureInfo("de-de").TwoLetterISOLanguageName))
            {
                languageIdentifier = LanguageIdentifier.German;
            }
            else
            {
                // English for any other UI culture.
                languageIdentifier = LanguageIdentifier.English;
            }
        }

        public string ErrorText
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return ERROR_DE;
                    default:
                        return ERROR_EN;
                }
            }
        }

        public string AbortText
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return ABORT_DE;
                    default:
                        return ABORT_EN;
                }
            }
        }

        public string InfoText
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return INFO_DE;
                    default:
                        return INFO_EN;
                }
            }
        }

        public string ConfirmCancel
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return CONFIRMCANCEL_DE;
                    default:
                        return CONFIRMCANCEL_EN;
                }
            }
        }

        public string UrlInfoMissing
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return URLINFOMISSING_DE;
                    default:
                        return URLINFOMISSING_EN;
                }
            }
        }

        public string UrlInfoBroken
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return URLINFOBROKEN_DE;
                    default:
                        return URLINFOBROKEN_EN;
                }
            }
        }

        public string JsonDownloadFailed
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return JSONDOWNLOADFAILED_DE;
                    default:
                        return JSONDOWNLOADFAILED_EN;
                }
            }
        }

        public string RetryingFile
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return RETRYINGFILE_DE;
                    default:
                        return RETRYINGFILE_EN;
                }
            }
        }

        public string FileFailed
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return FILEFAILED_DE;
                    default:
                        return FILEFAILED_EN;
                }
            }
        }

        public string PatchDownloadFailed
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return PATCHDOWNLOADFAILED_DE;
                    default:
                        return PATCHDOWNLOADFAILED_EN;
                }
            }
        }

        public string FileDownloaded
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return FILEDOWNLOADED_DE;
                    default:
                        return FILEDOWNLOADED_EN;
                }
            }
        }

        public string DownloadingPatch
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return DOWNLOADINGPATCH_DE;
                    default:
                        return DOWNLOADINGPATCH_EN;
                }
            }
        }

        public string ScanningInit
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return SCANNINGFILES_DE;
                    default:
                        return SCANNINGFILES_EN;
                }
            }
        }

        public string DownloadInit
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return DOWNLOADINIT_DE;
                    default:
                        return DOWNLOADINIT_EN;
                }
            }
        }

        public string NgenInit
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return NGENINIT_DE;
                    default:
                        return NGENINIT_EN;
                }
            }
        }
  
        public string ClientExecutableMissing
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return CLIENTEXEMISSINGERROR_DE;
                    default:
                        return CLIENTEXEMISSINGERROR_EN;
                }
            }
        }

        public string ClientUpToDate
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return CLIENTUPTODATE_DE;
                    default:
                        return CLIENTUPTODATE_EN;
                }
            }
        }

        public string ClientWasUpdated
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return CLIENTWASUPDATED_DE;
                    default:
                        return CLIENTWASUPDATED_EN;
                }
            }
        }

        public string NumFiles
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return NUMFILES_DE;
                    default:
                        return NUMFILES_EN;
                }
            }
        }

        public string UpdateAborted
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return UPDATEABORTED_DE;
                    default:
                        return UPDATEABORTED_EN;
                }
            }
        }

        public string ProgressFinished
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return PROGRESSFINISHED_DE;
                    default:
                        return PROGRESSFINISHED_EN;
                }
            }
        }

        public string ProgressAborted
        {
            get
            {
                switch (languageIdentifier)
                {
                    case LanguageIdentifier.German:
                        return PROGRESSABORTED_DE;
                    default:
                        return PROGRESSABORTED_EN;
                }
            }
        }
    }
}
