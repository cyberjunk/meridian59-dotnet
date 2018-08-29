# Meridian 59: New 3D client and developer tools

# Latest News

## 29 August 2018
New OgreClient version 1.0.5.4 has been released, visit the wiki [changelog](https://github.com/cyberjunk/meridian59-dotnet/wiki/Meridian59.Ogre.Client-ChangeLog) for more details.

Check out the [news archive](https://github.com/cyberjunk/meridian59-dotnet/wiki) for older news.

# OgreClient Screenshot

<img src="https://raw.githubusercontent.com/wiki/cyberjunk/meridian59-dotnet/images/ogreclient1.png" />

# Compatibility / Downloads
I do not provide any binaries of OgreClient myself. Instead you can download it from various server operators that support it.

By default, software included in this repository (e.g. [OgreClient](https://github.com/cyberjunk/meridian59-dotnet/wiki/Meridian59.Ogre.Client)) is only compatible with the technically most advanced servers/versions of the game. As of today these are:

 * Server 105 | [MeridianNext](https://meridiannext.com) | US | Multi-Language
 * Server 112 | [Meridian59de](https://meridian59.de) | EU | Multi-Language


I also try to maintain basic compatibility with older/less technically advanced versions of the game such as [OpenMeridian](http://openmeridian.org/) or [Meridian59com](https://www.meridian59.com), but due to certain important features not being implemented on these branches the experience when playing OgreClient can suffer and I do not guarantee any compatibility. You are also advised to make sure that OgreClient is actually allowed on your server before using it!

## Supported Versions of the Game

You can build binaries compatible with different branches of the game by using preprocessor macros. See the table below for the options and the supported branches. Others might work if they build upon of these and haven't been technically changed too much.

|  Server  | Macro            | Compatibility | Website             |
|----------|------------------|---------------|---------------------|
|   105    | -                |  **Full**     | [MeridianNext](https://meridiannext.com) |
|   112    | -                |  **Full**     | [Meridian59de](https://meridian59.de)    |
|   103    | OPENMERIDIAN     |  Low          | [OpenMeridian](http://openmeridian.org/) |
| 101/102  | VANILLA          |  Minimal      | [Meridian59com](https://www.meridian59.com) |

# Wiki

Visit the <a href="https://github.com/cyberjunk/meridian59-dotnet/wiki">wiki</a> for more information about the different projects/tools.

