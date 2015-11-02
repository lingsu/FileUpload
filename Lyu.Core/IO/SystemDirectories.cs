﻿using System;
using System.Web;

namespace Lyu.Core.IO
{
    //all paths has a starting but no trailing /
    public class SystemDirectories
    {
        public static string Bin
        {
            get
            {
                return IOHelper.ReturnPath("umbracoBinDirectory", "~/bin");
            }
        }
        public static string Base
        {
            get
            {
                return IOHelper.ReturnPath("umbracoBaseDirectory", "~/base");
            }
        }
        public static string Config
        {
            get
            {
                return IOHelper.ReturnPath("umbracoConfigDirectory", "~/config");
            }
        }
        public static string Css
        {
            get
            {
                return IOHelper.ReturnPath("umbracoCssDirectory", "~/css");
            }
        }
        public static string Data
        {
            get
            {
                return IOHelper.ReturnPath("umbracoStorageDirectory", "~/App_Data");
            }
        }
     

        public static string AppPlugins
        {
            get
            {
                //NOTE: this is not configurable and shouldn't need to be
                return "~/App_Plugins";
            }
        }

        public static string MvcViews
        {
            get
            {
                //NOTE: this is not configurable and shouldn't need to be
                return "~/Views";
            }
        }


        public static string Media
        {
            get
            {
                return IOHelper.ReturnPath("umbracoMediaPath", "~/media");
            }
        }


        public static string MacroScripts
        {
            get
            {
                // for legacy we test for the python path first, but else we use the new default location
                string tempPath = IOHelper.ReturnPath("umbracoPythonPath", "") == String.Empty
                                      ? IOHelper.ReturnPath("umbracoMacroScriptPath", "~/macroScripts")
                                      : IOHelper.ReturnPath("umbracoPythonPath", "~/python");
                return tempPath;
            }
        }

        public static string Scripts
        {
            get
            {
                return IOHelper.ReturnPath("umbracoScriptsPath", "~/scripts");
            }
        }

        public static string Umbraco
        {
            get
            {
                return IOHelper.ReturnPath("umbracoPath", "~/umbraco");
            }
        }

        public static string UmbracoClient
        {
            get
            {
                return IOHelper.ReturnPath("umbracoClientPath", "~/umbraco_client");
            }
        }

        public static string UserControls
        {
            get
            {
                return IOHelper.ReturnPath("umbracoUsercontrolsPath", "~/usercontrols");
            }
        }

        public static string WebServices
        {
            get
            {
                return IOHelper.ReturnPath("umbracoWebservicesPath", "~/umbraco/webservices");
            }
        }

        public static string Xslt
        {
            get
            {
                return IOHelper.ReturnPath("umbracoXsltPath", "~/xslt");
            }
        }

        public static string Packages
        {
            get
            {
                //by default the packages folder should exist in the data folder
                return IOHelper.ReturnPath("umbracoPackagesPath", Data + IOHelper.DirSepChar + "packages");
            }
        }

        public static string Preview
        {
            get
            {
                //by default the packages folder should exist in the data folder
                return IOHelper.ReturnPath("umbracoPreviewPath", Data + IOHelper.DirSepChar + "preview");
            }
        }

        private static string _root;
        /// <summary>
        /// Gets the root path of the application
        /// </summary>
        public static string Root
        {
            get
            {
                if (_root == null)
                {
                    string appPath = HttpRuntime.AppDomainAppVirtualPath ?? string.Empty;
                    if (appPath == "/")
                        appPath = string.Empty;

                    _root = appPath;
                }
                return _root;
            }
            //Only required for unit tests
            internal set { _root = value; }
        }
    }
}