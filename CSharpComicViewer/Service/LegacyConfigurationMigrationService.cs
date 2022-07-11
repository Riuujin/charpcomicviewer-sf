﻿using CSharpComicViewerLib.Data;
using CSharpComicViewerLib.Service;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CSharpComicViewer.Service
{
    /// <summary>
    /// Service that provides migration from legacy data format to the new data format.
    /// </summary>
    /// <seealso cref="CSharpComicViewerLib.Service.ILegacyConfigurationMigrationService" />
    public class LegacyConfigurationMigrationService : ILegacyConfigurationMigrationService
    {
        /// <summary>
        /// Migrates legacy data (v1.5.4) to the latest format.
        /// </summary>
        public void Migrate()
        {
            var service = Ioc.Default.GetRequiredService<IDataStorageService>();
            string configurationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "C# Comicviewer", "Configuration.xml");

            if (File.Exists(configurationPath))
            {
                dynamic configuration = DynamicXml.Parse(File.ReadAllText(configurationPath));
                var resume = configuration.Resume;

                if (resume != null)
                {
                    var bookmark = GetBookmark(resume);
                    service.Save("resumeData", bookmark);
                }

                var bookmarks = configuration.Bookmarks;

                if (bookmarks != null)
                {
                    List<Bookmark> newBookmarks = new List<Bookmark>();

                    foreach (var bookmark in bookmarks.Bookmark)
                    {
                        newBookmarks.Add(GetBookmark(bookmark));
                    }

                    service.Save("bookmarks", newBookmarks.ToArray());
                }

                //TODO: Remove old Configuration.xml
            }
        }

        private static Bookmark GetBookmark(dynamic bookmark)
        {
            List<string> files = new List<string>();
            int currentFile = int.Parse(bookmark.CurrentFile);
            int currentPage = int.Parse(bookmark.CurrentPage);
            string name = null;

            if (currentFile > 0 && currentPage > 0)
            {
                //TODO: Determine what is should do here
            }

            if (bookmark.Files.@string.GetType().Equals(typeof(string)))
            {
                name = Path.GetFileNameWithoutExtension(bookmark.Files.@string);
                files.Add(bookmark.Files.@string);
            }
            else
            {
                foreach (string file in bookmark.Files.@string)
                {
                    files.Add(file);

                    if (name == null)
                    {
                        name = Path.GetFileNameWithoutExtension(file);
                    }
                }
            }

            return new Bookmark
            {
                Name = name,
                FilePaths = files.ToArray(),
                Page = currentPage > 0 ? currentPage : currentFile
            };
        }

        private class DynamicXml : DynamicObject
        {
            XElement _root;
            private DynamicXml(XElement root)
            {
                _root = root;
            }

            public static DynamicXml Parse(string xmlString)
            {
                return new DynamicXml(XDocument.Parse(xmlString).Root);
            }

            public static DynamicXml Load(string filename)
            {
                return new DynamicXml(XDocument.Load(filename).Root);
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                result = null;

                var att = _root.Attribute(binder.Name);
                if (att != null)
                {
                    result = att.Value;
                    return true;
                }

                var nodes = _root.Elements(binder.Name);
                if (nodes.Count() > 1)
                {
                    result = nodes.Select(n => n.HasElements ? (object)new DynamicXml(n) : n.Value).ToList();
                    return true;
                }

                var node = _root.Element(binder.Name);
                if (node != null)
                {
                    result = node.HasElements ? (object)new DynamicXml(node) : node.Value;
                    return true;
                }

                return true;
            }
        }
    }
}
