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

        private const string FILEDOWNLOADED_EN = "Downloaded file {0}\n";
        private const string FILEDOWNLOADED_DE = "Datei {0} heruntergeladen\n";

        private const string DOWNLOADINGPATCH_EN = "Downloading patch information...\n";
        private const string DOWNLOADINGPATCH_DE = "Herunterladen der Patch-Informationen...\n";

        private const string PATCHDOWNLOADFAILED_EN = "Patch information download failed!\n";
        private const string PATCHDOWNLOADFAILED_DE = "Herunterladen der Patch-Informationen fehlgeschlagen!\n";

        private const string DOWNLOADINIT_EN = "Initializing client file download...";
        private const string DOWNLOADINIT_DE = "Initialisierung der Client-Dateien zum herunterladen...";

        private const string SCANNINGFILES_EN = "Calculating client files to download...";
        private const string SCANNINGFILES_DE = "Berechnen der Client-Dateien zum herunterladen...";

        private const string CLIENTUPTODATE_EN = "Client is up to date, nothing to download. Click OK to launch the client.";
        private const string CLIENTUPTODATE_DE = "Client ist auf dem neusten Stand. Klicke OK um das Spiel zu starten.";

        private const string CLIENTWASUPDATED_EN = "Client updated. Click OK to launch the client.";
        private const string CLIENTWASUPDATED_DE = "Client wurde aktualisiert. Klicke OK um das Spiel zu starten.";

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

        public string ScanningFiles
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
    }
}
