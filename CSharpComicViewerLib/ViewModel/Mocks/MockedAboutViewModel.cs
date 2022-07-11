#if DEBUG
using System;
using System.Windows.Input;

namespace CSharpComicViewerLib.ViewModel.Mocks
{
    public class MockedAboutViewModel : IAboutViewModel
    {
        /// <summary>
        /// Gets the name of the program.
        /// </summary>
        /// <value>
        /// The name of the program.
        /// </value>
        public string ProgramName => "C# ComicViewer";

        /// <summary>
        /// Gets the copyright.
        /// </summary>
        /// <value>
        /// The copyright.
        /// </value>
        public string Copyright => "Copyright © Rutger Spruyt 2018";

        /// <summary>
        /// Gets the version.
        /// </summary>
        public string Version => "2.0.0";

        /// <summary>
        /// Gets the git hub URL.
        /// </summary>
        /// <value>
        /// The git hub URL.
        /// </value>
        public string GitHubUrl => "https://riuujin.github.io/charpcomicviewer-sf";

        /// <summary>
        /// Gets or sets the latest version.
        /// </summary>
        /// <value>
        /// The latest version.
        /// </value>
        public string LatestVersion => "2.0.1";

        /// <summary>
        /// Gets or sets the latest version URL.
        /// </summary>
        /// <value>
        /// The latest version URL.
        /// </value>
        public Uri LatestVersionUrl => new Uri("https://riuujin.github.io/charpcomicviewer-sf");

        /// <summary>
        /// Gets or sets a value indicating whether the latest version is different.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the latest version is different; otherwise, <c>false</c>.
        /// </value>
        public bool LatestVersionIsDifferent => true;

        /// <summary>
        /// Gets the check update command.
        /// </summary>
        /// <value>
        /// The check update command.
        /// </value>
        public ICommand CheckUpdateCommand { get; }
    }
}
#endif