using CSharpComicViewerLib.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Windows.Input;

namespace CSharpComicViewerLib.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        private Assembly assembly;
        private ICommand checkUpdateCommand;
        private string latestVersion;
        private string latestVersionUrl;
        private bool latestVersionIsDifferent;


        public AboutViewModel()
        {
            if (ViewModelBase.IsInDesignModeStatic)
            {
                LatestVersion = Version;
                latestVersionIsDifferent = false;
            }
        }

        /// <summary>
        /// Gets the name of the program.
        /// </summary>
        /// <value>
        /// The name of the program.
        /// </value>
        public string ProgramName
        {
            get
            {
                return CommonServiceLocator.ServiceLocator.Current.GetInstance<IApplicationService>().GetProgramName();
            }
        }

        /// <summary>
        /// Gets the copyright.
        /// </summary>
        /// <value>
        /// The copyright.
        /// </value>
        public string Copyright
        {
            get
            {
                return CommonServiceLocator.ServiceLocator.Current.GetInstance<IApplicationService>().GetCopyright();
            }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        public string Version
        {
            get
            {
                return CommonServiceLocator.ServiceLocator.Current.GetInstance<IApplicationService>().GetFileVersion();
            }
        }

        /// <summary>
        /// Gets the git hub URL.
        /// </summary>
        /// <value>
        /// The git hub URL.
        /// </value>
        public string GitHubUrl
        {
            get
            {
                if (ViewModelBase.IsInDesignModeStatic)
                {
                    return "http://riuujin.github.io/charpcomicviewer-sf";
                }

                return CommonServiceLocator.ServiceLocator.Current.GetInstance<IApplicationService>().GetGitHubUrl();
            }
        }

        /// <summary>
        /// Gets or sets the latest version.
        /// </summary>
        /// <value>
        /// The latest version.
        /// </value>
        public string LatestVersion
        {
            get
            {
                return latestVersion;
            }
            set
            {
                Set(ref latestVersion, value);
            }
        }

        /// <summary>
        /// Gets or sets the latest version URL.
        /// </summary>
        /// <value>
        /// The latest version URL.
        /// </value>
        public string LatestVersionUrl
        {
            get
            {
                return latestVersionUrl;
            }
            set
            {
                Set(ref latestVersionUrl, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the latest version is different.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the latest version is different; otherwise, <c>false</c>.
        /// </value>
        public bool LatestVersionIsDifferent
        {
            get
            {
                return latestVersionIsDifferent;
            }
            set
            {
                Set(ref latestVersionIsDifferent, value);
            }
        }

        /// <summary>
        /// Gets the check update command.
        /// </summary>
        /// <value>
        /// The check update command.
        /// </value>
        public ICommand CheckUpdateCommand
        {
            get
            {
                if (checkUpdateCommand == null)
                {
                    checkUpdateCommand = new RelayCommand(async () =>
                    {
                        try
                        {
                            using (WebClient wc = new WebClient())
                            {
                                //Use chrome user agent, github api requires a user agent.
                                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36");
                                wc.Headers.Add("Accept", "application/vnd.github.v3+json");
                                var url = CommonServiceLocator.ServiceLocator.Current.GetInstance<IApplicationService>().GetLatestVersionUrl();
                                var json = await wc.DownloadStringTaskAsync(url);
                                dynamic data = JObject.Parse(json);
                                LatestVersion = data.name;
                                LatestVersionUrl = data.html_url;
                                LatestVersionIsDifferent = data.tag_name != "v" + Version;
                            }
                        }
                        catch (System.Exception)
                        {
                            //This should never cause the app to crash
                        }

                    });
                }

                return checkUpdateCommand;
            }
        }

    }
}