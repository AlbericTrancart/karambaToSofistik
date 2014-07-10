karambaToSofistik
============

A Grasshopper plugin which takes a Karamba Model and converts it to a .dat file readable by Sofistik.

Features
--------

- Takes as input a Karamba model and outputs a .dat file into CADINPUT language;
- Converts beams, materials, cross sections and loads;
- Covered cross sections types: box profile, circular, I profile, trapezoidal;
- Covered load types: line-load, point load, gravity, pretension, temperature load;
- Takes Grasshopper's ID system into account.

Installation
------------
To install the plugin follow these steps :

- [Download the plugin][1].
- Place the GHA file into your plugin directory in Grasshopper. You may find it in *"%APPDATA%\Grasshopper\Libraries"*.

> **NOTE:**
> 
> To find the *%APPDATA%* folder just open the **Run** window (hit WIN+R) then type *"%APPDATA%"* and then OK.


Usage
-----

This component is very simple to use. You can find it in the Karamba tab in the section "Extra". You have to plug in an input model, that's all!

You will get the data with "Output".
The "Status" output is for debugging purposes.

Optionnaly, you can set a input path: **the .dat file will be automatically saved and updated to this location**.

> **NOTE**
> 
> The directoy must exist in order to save the file into it.

![Usage demonstration][2]

*A simple use of this component*



Building it from source
-----------------------

As an alternative you can build this plugin from the source and tweak the code if you want.
You will need **Microsoft Visual Studio** with the [Grasshopper Assembly][3] (you can download it directly from Visual Studio in *"Tools > Extensions and updates..."*).

- Either [download the source zip][4] or fork the repo.
- Open the *Source/karambaToSofistik.sln* project file with Visual Studio.
- In the project tree **check that all the references are available**. If not, fix them (the required libraries can be found in the *Source/Libraries/* directory).
- Build the solution **(Ctrl+Shift+B)**. You should get the GHA file in the *Source/bin* folder.

> **NOTE**
> 
> Visual Studio may give you a warning which says that it can't copy the GHA file to the Grasshopper plugin directory. To change this, edit the project properties and go to **Build Events**.
> 
> Replace these lines:
> 
> `Copy "$(TargetDir)$(ProjectName).gha" "C:\Users\alberic\AppData\Roaming\Grasshopper\Libraries"`
> `Copy "$(TargetDir)$(ProjectName).gha" "E:\karambaToSofistik\Bin"`
> 
> With this one:
> 
> `Copy "$(TargetDir)$(ProjectName).gha" "YOUR\GRASSHOPPER\PLUGIN\DIRECTORY"`
>
> With this setup, it will not have to copy again and again the GHA file with every build.


  [1]: https://github.com/AlbericTrancart/karambaToSofistik/blob/master/Bin/karambaToSofistik.gha?raw=true
  [2]: http://image.noelshack.com/fichiers/2014/27/1404381095-capture-2.png "Usage demonstration"
  [3]: http://visualstudiogallery.msdn.microsoft.com/9e389515-0719-47b4-a466-04436b491cd6 "Grasshopper Assembly"
  [4]: https://github.com/AlbericTrancart/karambaToSofistik/archive/master.zip
