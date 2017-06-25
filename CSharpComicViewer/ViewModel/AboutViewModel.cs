using GalaSoft.MvvmLight;
using System.Diagnostics;
using System.Reflection;

namespace CSharpComicViewer.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        private Assembly assembly;

        private Assembly GetAssembly()
        {
            if (assembly == null)
            {

                assembly = Assembly.GetExecutingAssembly();
            }
            return assembly;
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
                return ((AssemblyTitleAttribute)AssemblyTitleAttribute.GetCustomAttribute(GetAssembly(), typeof(AssemblyTitleAttribute))).Title;
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
                return ((AssemblyCopyrightAttribute)AssemblyTitleAttribute.GetCustomAttribute(GetAssembly(), typeof(AssemblyCopyrightAttribute))).Copyright;
            }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        public string Version
        {
            get
            {
                return FileVersionInfo.GetVersionInfo(GetAssembly().Location).FileVersion;
            }
        }

    }
}