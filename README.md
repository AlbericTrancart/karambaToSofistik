GhToSofistik
============

A Grasshopper plugin which takes a Karamba Model and converts it to a .dat file readable by Sofistik.

------------
Installation
------------
To install the plugin follow these steps :

- [Download the plugin][1].
- Place the GHA file into your plugin directory in Grasshopper. You may find it in *"%APPDATA%\Grasshopper\Libraries"*.

> **NOTE:**
> To find the *%APPDATA%* folder just open the **Run** window (hit WIN+R) then type *"%APPDATA%"* and then OK.


-----
Usage
-----

*Work In Progress...*

-----------------------
Building it from source
-----------------------

As an alternative you can build this plugin from the source and tweak the code if you want.
You will need **Microsoft Visual Studio** with the [Grasshopper Assembly][2] (you can download it directly from Visual Studio in *"Tools > Extensions and updates..."*).

- Either [download the source zip][3] ork fork the repo.
- Open the *Source/GhToSofistik.sln* project file with Visual Studio.
- In the project tree **check that all the references are available**. If not, fix them (the required libraries can be found in the *Source/Libraries/ Folder*).
- Build the solution **(Ctrl+Shift+B)**. You should get the GHA file in the *Source/bin* folder.

> **NOTE**
> Visual Studio may give you a warning which says that it can't copy the GHA file to the Grasshopper plugin directory. To change this, edit the project properties and go to **Build Events**.
> 
> Replace these lines:
> `Copy "$(TargetDir)$(ProjectName).gha" "C:\Users\alberic\AppData\Roaming\Grasshopper\Libraries"`
> `Copy "$(TargetDir)$(ProjectName).gha" "E:\GhToSofistik\Bin"`
> 
> With this one:
> `Copy "$(TargetDir)$(ProjectName).gha" "YOUR\GRASSHOPPER\PLUGIN\DIRECTORY"`
>
> With this setup, it will not have to copy again and again the GHA file with every build.
