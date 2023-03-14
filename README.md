![Unified Device Manager Banner](https://user-images.githubusercontent.com/42884387/225069757-bec7dd10-0d8d-4b87-be2a-2dea785a7a2c.png)

<b>Quickly and easliy build and deploy on target devices for Unity</b>
## Unified Device Manager (Android)

Features:
- Build, Install/Deploy and Run
- Install last build and Run
- Run installed
- Start wireless adb on connected device (port 5555) and connect to it

<br>

<p align="center">
  <img width="700" align="center" alt="demo" src="https://user-images.githubusercontent.com/42884387/225070929-a354a6a1-0322-419a-8d3d-302d3bf25bde.png">
</p>

<br>

### Installation

This repository works with upm. 
<br>Simply add it via package manager (get the link from <>Code button in top right corner or view releases).
<br>To specify any version, add #version number like this: giturl#1.0.0 
<br>(more information using upm at: https://docs.unity3d.com/Manual/upm-git.html)

Manual: It is also possible to download zip and put its content anywhere in the project.
<br>

### Usage

After installation, new button should appear in the toolbar <img width="36" alt="image" src="https://user-images.githubusercontent.com/42884387/225072771-212fb036-dbd2-45ed-a5f2-7c9b05211749.png"> (left side, next to cloud and version control buttons).
<br>

### Notes

- Currently, this packge only supports Android build and it makes an assumption that default build/development platform is Android.
- Apk build location is hardcoded at this time, and after building, you should consider adding that folder (_vcignore) to .gitignore or any other version control ignore file.


### Credits

- Toolbar button feature is based on: https://github.com/marijnz/unity-toolbar-extender
