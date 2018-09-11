namespace CSharpComicViewerLib.Service
{
    /// <summary>
    /// Service that provides migration from legacy data format to the new data format.
    /// </summary>
    public interface ILegacyConfigurationMigrationService
    {
        /// <summary>
        /// Migrates legacy data (v1.5.4) to the latest format.
        /// </summary>
        void Migrate();
    }
}