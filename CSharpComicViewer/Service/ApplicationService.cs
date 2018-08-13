using CSharpComicViewerLib.Service;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace CSharpComicViewer.Service
{
    class ApplicationService : IApplicationService
    {
        private const string GITHUB_URL = "http://riuujin.github.io/charpcomicviewer-sf";
        private const string LATEST_VERSION_URL = "https://api.github.com/repos/Riuujin/charpcomicviewer-sf/releases/latest";
        private Assembly assembly;

        public void ApplicationShutdown()
        {
            Application.Current.Shutdown();
        }

        public string GetFileVersion()
        {
            return FileVersionInfo.GetVersionInfo(GetAssembly().Location).FileVersion;
        }

        private Assembly GetAssembly()
        {
            if (assembly == null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }
            return assembly;
        }

        public string GetProgramName()
        {
            return ((AssemblyTitleAttribute)AssemblyTitleAttribute.GetCustomAttribute(GetAssembly(), typeof(AssemblyTitleAttribute))).Title;
        }

        public string GetCopyright()
        {
            return ((AssemblyCopyrightAttribute)AssemblyTitleAttribute.GetCustomAttribute(GetAssembly(), typeof(AssemblyCopyrightAttribute))).Copyright;
        }

        public string GetGitHubUrl()
        {
            return GITHUB_URL;
        }

        public string GetLatestVersionUrl()
        {
            return LATEST_VERSION_URL;
        }
    }
}
