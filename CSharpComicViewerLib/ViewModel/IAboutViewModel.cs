using System;
using System.Windows.Input;

namespace CSharpComicViewerLib.ViewModel
{
    public interface IAboutViewModel
    {
        ICommand CheckUpdateCommand { get; }
        string Copyright { get; }
        string GitHubUrl { get; }
        string LatestVersion { get; }
        bool LatestVersionIsDifferent { get; }
        Uri LatestVersionUrl { get; }
        string ProgramName { get; }
        string Version { get; }
    }
}