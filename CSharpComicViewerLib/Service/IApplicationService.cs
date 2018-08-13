using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpComicViewerLib.Service
{
    public interface IApplicationService
    {
        string GetFileVersion();

        string GetProgramName();

        string GetCopyright();

        string GetGitHubUrl();

        string GetLatestVersionUrl();

        void ApplicationShutdown();
    }
}
