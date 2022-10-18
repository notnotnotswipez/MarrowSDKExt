using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;

[assembly: AssemblyTitle($safeprojectname$.BuildInfo.Name)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany($safeprojectname$.BuildInfo.Company)]
[assembly: AssemblyProduct($safeprojectname$.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + $safeprojectname$.BuildInfo.Author)]
[assembly: AssemblyTrademark($safeprojectname$.BuildInfo.Company)]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion($safeprojectname$.BuildInfo.Version)]
[assembly: AssemblyFileVersion($safeprojectname$.BuildInfo.Version)]
[assembly: NeutralResourcesLanguage("en")]
[assembly: MelonInfo(typeof($safeprojectname$.$safeprojectname$), $safeprojectname$.BuildInfo.Name, $safeprojectname$.BuildInfo.Version, $safeprojectname$.BuildInfo.Author, $safeprojectname$.BuildInfo.DownloadLink)]
[assembly: MelonGame("Stress Level Zero", "BONEWORKS")]