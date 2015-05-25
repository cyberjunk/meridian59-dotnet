#include "stdafx.h"

#include <windows.h>
#include <stdio.h>
#include <fcntl.h>
#include <io.h>
#include <iostream>
#include <string>

using namespace Meridian59::Ogre;
using namespace System::Windows::Forms;

extern "C"{
CEGUI::FactoryModule& getWindowFactoryModule()
{
    static CEGUI::FactoryModule mod;
    return mod;
}
}

void showWin32Console()
{
    static const WORD MAX_CONSOLE_LINES = 500;
    int hConHandle;
    long lStdHandle;
    CONSOLE_SCREEN_BUFFER_INFO coninfo;
    FILE *fp;
    // allocate a console for this app
    AllocConsole();
    // set the screen buffer to be big enough to let us scroll text
    GetConsoleScreenBufferInfo(GetStdHandle(STD_OUTPUT_HANDLE), &coninfo);
    coninfo.dwSize.Y = MAX_CONSOLE_LINES;
    SetConsoleScreenBufferSize(GetStdHandle(STD_OUTPUT_HANDLE),
    coninfo.dwSize);
    // redirect unbuffered STDOUT to the console
    lStdHandle = (long)GetStdHandle(STD_OUTPUT_HANDLE);
    hConHandle = _open_osfhandle(lStdHandle, _O_TEXT);
    fp = _fdopen( hConHandle, "w" );
    *stdout = *fp;
    setvbuf( stdout, NULL, _IONBF, 0 );
    // redirect unbuffered STDIN to the console
    lStdHandle = (long)GetStdHandle(STD_INPUT_HANDLE);
    hConHandle = _open_osfhandle(lStdHandle, _O_TEXT);
    fp = _fdopen( hConHandle, "r" );
    *stdin = *fp;
    setvbuf( stdin, NULL, _IONBF, 0 );
    // redirect unbuffered STDERR to the console
    lStdHandle = (long)GetStdHandle(STD_ERROR_HANDLE);
    hConHandle = _open_osfhandle(lStdHandle, _O_TEXT);
    fp = _fdopen( hConHandle, "w" );
    *stderr = *fp;
    setvbuf( stderr, NULL, _IONBF, 0 );
    // make cout, wcout, cin, wcin, wcerr, cerr, wclog and clog
    // point to console as well
    std::ios::sync_with_stdio();
}

/// <summary>
/// Application entry point. 
/// </summary>
int __stdcall WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, 
              LPSTR lpStrCmdString, int nCmdShow)
{

#if _DEBUG
	showWin32Console();
#endif

	// .net stuff
	Application::EnableVisualStyles();
	Application::SetCompatibleTextRenderingDefault(false);

	// GC settings for low latency
	::System::Runtime::GCSettings::LatencyMode = ::System::Runtime::GCLatencyMode::SustainedLowLatency; // .net 4.5+
	//::System::Runtime::GCSettings::LatencyMode = ::System::Runtime::GCLatencyMode::LowLatency; // .net 4

	// Make sure the mainthread has highest prio within the process (there are background workers!)
	::System::Threading::Thread::CurrentThread->Priority = ::System::Threading::ThreadPriority::Highest;

	// init client and start (locks thread)
	OgreClient::Singleton->Start(true);

#if _DEBUG
	//FreeConsole();
#endif

    return 0;
}
