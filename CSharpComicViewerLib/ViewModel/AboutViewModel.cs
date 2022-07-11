﻿using CSharpComicViewerLib.Service;
using System;
using System.Net;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace CSharpComicViewerLib.ViewModel
{
    public class AboutViewModel : ObservableRecipient, IAboutViewModel
    {
        private readonly IApplicationService applicationService;
        private ICommand checkUpdateCommand;
        private string latestVersion;
        private Uri latestVersionUrl;
        private bool latestVersionIsDifferent;


        public AboutViewModel(IApplicationService applicationService)
        {
            this.applicationService = applicationService;
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
                return applicationService.GetProgramName();
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
                return applicationService.GetCopyright();
            }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        public string Version
        {
            get
            {
                return applicationService.GetFileVersion();
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
                return applicationService.GetGitHubUrl();
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
            private set
            {
                SetProperty(ref latestVersion, value, true);
            }
        }

        /// <summary>
        /// Gets or sets the latest version URL.
        /// </summary>
        /// <value>
        /// The latest version URL.
        /// </value>
        public Uri LatestVersionUrl
        {
            get
            {
                return latestVersionUrl;
            }
            set
            {
                SetProperty(ref latestVersionUrl, value,true);
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
                SetProperty(ref latestVersionIsDifferent, value,true);
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
                    checkUpdateCommand = new AsyncRelayCommand(async () =>
                    {
                        try
                        {
                            using (WebClient wc = new WebClient())
                            {
                                //Use custom user agent, github api requires a user agent.
                                wc.Headers.Add("user-agent", "CSharpComicViewer version checker");
                                wc.Headers.Add("Accept", "application/vnd.github.v3+json");
                                var url = applicationService.GetLatestVersionUrl();
                                var json = await wc.DownloadStringTaskAsync(url);
                                dynamic data = System.Text.Json.JsonDocument.Parse(json);
                                LatestVersion = data.name;
                                LatestVersionUrl = new Uri((string)data.html_url);
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