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

#pragma once

#if _WIN32 || _WIN64
#if _WIN64
typedef ::System::Double CLRReal;
#else
typedef ::System::Single CLRReal;
#endif
#endif

// typedef for CLR classes to avoid ambigiousness
typedef ::System::String CLRString;
typedef ::System::Math   CLRMath;
