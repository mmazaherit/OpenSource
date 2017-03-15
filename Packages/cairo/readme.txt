Cairo Solution for Visual Studio 2012 - README
----------------------------------------------

Please read COPYING for license information. This readme file is for
describing the purpose and use of this solution. Solution is just a fancy
name for "a bunch of projects that might depend on each other" in VS.

What is cairo:
---------------------------------------
"Cairo is a 2D graphics library with support for multiple output devices."
http://cairographics.org/

What is pixman:
---------------------------------------
It is a library that cairo uses, but you can find out more about it on the
cairo website. It is included in the download section of cairo as a
separate package file.

Purpose of this solution:
---------------------------------------
This solution is for building 32bit pixman and cairo with Visual Studio
2012 without having to install scripting languages and shell environments
that emulate linux, that the original makefiles of cairo need even when
building cairo with Visual Studio under Windows.

How to compile:
---------------------------------------
Download the latest cairo and pixman sources from
http://cairographics.org/releases/
*Please note that this solution does not compile cairomm nor use it
for anything.

At the time of creating this solution the latest cairo version is 1.12.16.
and the latest pixman version is 0.32.4.

1. Unzip the files of this solution to any location, but make sure the
folders inside get created.

2. Unpack the cairo source files to the subfolder cairo-1.12.16 and the
pixman sources to pixman-0.32.4 (both created when unzipping the solution,
but should only contain single readme files.)

* The packages for cairo/pixman might contain the files inside a folder. If
you only see a folder at the root of the packages, and no makefiles, no
readmes no nothing, only extract the files from that folder and not the
folder itself.

3. Open projects\cairo.sln with visual studio. You can skip to the last
step if you don't need SVG or PNG etc. support, but need to work with
Win32 GDI "surfaces" (HDC) and the basic drawing functionality of cairo.

4.a Edit projects\cairo_1_12_16\src\cairo-features.h to change what
additional features you need, or comment out what you don't want. For
example if you don't need to work with win32 GDI "surfaces" (HDC) you can
comment out the define for CAIRO_HAS_WIN32_SURFACE.
*Please note that if you want some features you might have to supply zlib
and/or libpng beside pixman/cairo.

4.b The files for the unused features are included in the solution, but
disabled. Enable what you need in the Solution Explorer in VS by selecting
the files and right-click/properties and erase the "Yes" string from
"Excluded From Build".

5. If you want to create a DLL instead of the static libraries this solution
creates, you can change that in the project settings with the VS interface.
You might have to edit the cairo project properties and add additional
libraries and their paths if you use features that require it. If you need
help with that, please learn to use the user interface of Visual Studio.

* This solution creates static libraries with the platform toolset for
vs2012 with WinXP support. It is not a requirement for cairo, just my
preference.

6. Compile and link the resulting static libraries (both cairo and pixman)
with your project. *

* When using cairo as a static library you will have to specify the
CAIRO_WIN32_STATIC_BUILD preprocessor directive in YOUR OWN PROJECT THAT
USES cairo, or define it before including the cairo header files.

Contact, bug report etc:
---------------------------------------
Please only contact me (via sourceforge) if you found a "bug" in the
solution. The only way a bug can be present is if a .c file is missing,
that is, not added to the solution thus not compiled into the library. I
don't intend to update this project when new versions of cairo or pixman
are out. It is easy to do-it-yourself just by renaming a few folders and
adding files to the project that were missing from previous cairo versions
if they are needed.

You can contact me via the project's sourceforge page at:
https://sourceforge.net/projects/cairosolutionvs2012
(Use the ticket system for bug reports.)