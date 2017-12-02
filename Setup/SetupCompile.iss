; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{8C88D80A-F9F9-4DEF-8510-DA9E438E729A}
AppName=C# Comicviewer
AppVersion=V2.0.0-beta
AppPublisherURL=http://riuujin.github.io/charpcomicviewer-sf
AppSupportURL=https://github.com/Riuujin/charpcomicviewer-sf/issues
AppUpdatesURL=http://riuujin.github.io/charpcomicviewer-sf
DefaultDirName={pf}\C# Comicviewer
DefaultGroupName=C# Comicviewer
LicenseFile=Licence.txt
OutputBaseFilename=CSharp.Comic.viewer.2.0.0-beta
Compression=lzma
SolidCompression=yes
ChangesAssociations=yes
ArchitecturesInstallIn64BitMode=x64

[Registry]
Root: HKCR; Subkey: ".CBR"; ValueType: string; ValueName: ""; ValueData: "ComicBookRarFile"; Flags: uninsdeletevalue
Root: HKCR; Subkey: "ComicBookRarFile"; ValueType: string; ValueName: ""; ValueData: "Comic Book Rar File"; Flags: uninsdeletekey
Root: HKCR; Subkey: "ComicBookRarFile\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\csharp-comicviewer.exe,0"
Root: HKCR; Subkey: "ComicBookRarFile\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\csharp-comicviewer.exe"" ""%1"""
Root: HKCR; Subkey: ".CBZ"; ValueType: string; ValueName: ""; ValueData: "ComicBookZipFile"; Flags: uninsdeletevalue
Root: HKCR; Subkey: "ComicBookRarFile"; ValueType: string; ValueName: ""; ValueData: "Comic Book Zip File"; Flags: uninsdeletekey
Root: HKCR; Subkey: "ComicBookZipFile\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\csharp-comicviewer.exe,0"
Root: HKCR; Subkey: "ComicBookZipFile\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\csharp-comicviewer.exe"" ""%1"""

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"



[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
            

[Files]
Source: "..\CSharpComicViewer\bin\Release\csharp-comicviewer.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\CSharpComicViewer\bin\Release\csharp-comicviewer.exe.config"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\C# Comicviewer"; Filename: "{app}\csharp-comicviewer.exe"
Name: "{group}\{cm:ProgramOnTheWeb,C# Comicviewer}"; Filename: "http://riuujin.github.io/charpcomicviewer-sf"
Name: "{group}\{cm:UninstallProgram,C# Comicviewer}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\C# Comicviewer"; Filename: "{app}\csharp-comicviewer.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\csharp-comicviewer.exe"; Description: "{cm:LaunchProgram,C# Comicviewer}"; Flags: nowait postinstall skipifsilent

[Code]
/////////////////////////////////////////////////////////////////////
function GetUninstallString(): String;
var
  sUnInstPath: String;
  sUnInstallString: String;
begin
  sUnInstPath := ExpandConstant('Software\Microsoft\Windows\CurrentVersion\Uninstall\{#emit SetupSetting("AppId")}_is1');
  sUnInstallString := '';
  if not RegQueryStringValue(HKLM, sUnInstPath, 'UninstallString', sUnInstallString) then
    RegQueryStringValue(HKCU, sUnInstPath, 'UninstallString', sUnInstallString);
  Result := sUnInstallString;
end;


/////////////////////////////////////////////////////////////////////
function IsUpgrade(): Boolean;
begin
  Result := (GetUninstallString() <> '');
end;


/////////////////////////////////////////////////////////////////////
function UnInstallOldVersion(): Integer;
var
  sUnInstallString: String;
  iResultCode: Integer;
begin
// Return Values:
// 1 - uninstall string is empty
// 2 - error executing the UnInstallString
// 3 - successfully executed the UnInstallString

  // default return value
  Result := 0;

  // get the uninstall string of the old app
  sUnInstallString := GetUninstallString();
  if sUnInstallString <> '' then begin
    sUnInstallString := RemoveQuotes(sUnInstallString);
    if Exec(sUnInstallString, '/SILENT /NORESTART /SUPPRESSMSGBOXES','', SW_HIDE, ewWaitUntilTerminated, iResultCode) then
      Result := 3
    else
      Result := 2;
  end else
    Result := 1;
end;

/////////////////////////////////////////////////////////////////////
procedure CurStepChanged(CurStep: TSetupStep);
begin
  if (CurStep=ssInstall) then
  begin
    if (IsUpgrade()) then
    begin
      UnInstallOldVersion();
    end;
  end;
end;
