using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace Lyu.Core.IO
{
    public static class IOHelper
    {
        private static string _rootDir = "";
        // static compiled regex for faster performance
        private readonly static Regex ResolveUrlPattern = new Regex("(=[\"\']?)(\\W?\\~(?:.(?![\"\']?\\s+(?:\\S+)=|[>\"\']))+.)[\"\']?", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        public static char DirSepChar
        {
            get
            {
                return Path.DirectorySeparatorChar;
            }
        }

        //尝试匹配旧路径到一个新的虚拟路径
        public static string FindFile(string virtualPath)
        {
            string retval = virtualPath;

            if (virtualPath.StartsWith("~"))
                retval = virtualPath.Replace("~", SystemDirectories.Root);

            if (virtualPath.StartsWith("/") && virtualPath.StartsWith(SystemDirectories.Root) == false)
                retval = SystemDirectories.Root + "/" + virtualPath.TrimStart('/');

            return retval;
        }
        //替换根目录符号
        public static string ResolveUrl(string virtualPath)
        {
            if (virtualPath.StartsWith("~"))
                return virtualPath.Replace("~", SystemDirectories.Root).Replace("//", "/");
            else if (Uri.IsWellFormedUriString(virtualPath, UriKind.Absolute))
                return virtualPath;
            else
                return VirtualPathUtility.ToAbsolute(virtualPath, SystemDirectories.Root);
        }
        public static Attempt<string> TryResolveUrl(string virtualPath)
        {
            try
            {
                if (virtualPath.StartsWith("~"))
                    return Attempt.Succeed(virtualPath.Replace("~", SystemDirectories.Root).Replace("//", "/"));
                if (Uri.IsWellFormedUriString(virtualPath, UriKind.Absolute))
                    return Attempt.Succeed(virtualPath);
                return Attempt.Succeed(VirtualPathUtility.ToAbsolute(virtualPath, SystemDirectories.Root));
            }
            catch (Exception ex)
            {
                return Attempt.Fail(virtualPath, ex);
            }
        }
        public static string MapPath(string path)
        {
            return MapPath(path, true);
        }

        public static string MapPath(string path, bool useHttpContext)
        {
            //检查路径是否已映射
            if ((path.Length >= 2 && path[1] == Path.VolumeSeparatorChar)
                || path.StartsWith(@"\\")) //UNC Paths start with "\\". If the site is running off a network drive mapped paths will look like "\\Whatever\Boo\Bar"
            {
                return path;
            }
            //检查我们至少有一个HttpContext
            // http://umbraco.codeplex.com/workitem/30946

            if (useHttpContext && HttpContext.Current != null)
            {
                //string retval;
                if (String.IsNullOrEmpty(path) == false && (path.StartsWith("~") || path.StartsWith(SystemDirectories.Root)))
                    return HostingEnvironment.MapPath(path);
                else
                    return HostingEnvironment.MapPath("~/" + path.TrimStart('/'));
            }

            var root = GetRootDirectorySafe();
            var newPath = path.TrimStart('~', '/').Replace('/', IOHelper.DirSepChar);
            var retval = root + IOHelper.DirSepChar.ToString(CultureInfo.InvariantCulture) + newPath;

            return retval;
        }
        /// <summary>
        /// 通过获取该程序集的路径 返回到应用程序的根的路径
        //  即使是在一个 bin/debug 或 /bin/release 文件夹
        /// </summary>
        /// <returns></returns>
        internal static string GetRootDirectorySafe()
        {
            if (String.IsNullOrEmpty(_rootDir) == false)
            {
                return _rootDir;
            }

            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new Uri(codeBase);
            var path = uri.LocalPath;
            var baseDirectory = Path.GetDirectoryName(path);
            if (String.IsNullOrEmpty(baseDirectory))
                throw new Exception("没有根目录. 请确保您的配置正确.");

            _rootDir = baseDirectory.Contains("bin")
                           ? baseDirectory.Substring(0, baseDirectory.LastIndexOf("bin", StringComparison.OrdinalIgnoreCase) - 1)
                           : baseDirectory;

            return _rootDir;
        }
        internal static string GetRootDirectoryBinFolder()
        {
            string binFolder = String.Empty;
            if (String.IsNullOrEmpty(_rootDir))
            {
                binFolder = Assembly.GetExecutingAssembly().GetAssemblyFile().Directory.FullName;
                return binFolder;
            }

            binFolder = Path.Combine(GetRootDirectorySafe(), "bin");

#if DEBUG
            var debugFolder = Path.Combine(binFolder, "debug");
            if (Directory.Exists(debugFolder))
                return debugFolder;
#endif   
            var releaseFolder = Path.Combine(binFolder, "release");
            if (Directory.Exists(releaseFolder))
                return releaseFolder;

            if (Directory.Exists(binFolder))
                return binFolder;

            return _rootDir;
        }
        /// <summary>
        /// Allows you to overwrite RootDirectory, which would otherwise be resolved
        /// automatically upon application start.
        /// </summary>
        /// <remarks>The supplied path should be the absolute path to the root of the umbraco site.</remarks>
        /// <param name="rootPath"></param>
        internal static void SetRootDirectory(string rootPath)
        {
            _rootDir = rootPath;
        }
        /// <summary>
        /// Check to see if filename passed has any special chars in it and strips them to create a safe filename.  Used to overcome an issue when Umbraco is used in IE in an intranet environment.
        /// </summary>
        /// <param name="filePath">The filename passed to the file handler from the upload field.</param>
        /// <returns>A safe filename without any path specific chars.</returns>
        internal static string SafeFileName(string filePath)
        {
            // use string extensions
            return filePath.ToSafeFileName();
        }
        internal static string ReturnPath(string settingsKey, string standardPath)
        {
            return ReturnPath(settingsKey, standardPath, false);

        }
        //use a tilde character instead of the complete path
        internal static string ReturnPath(string settingsKey, string standardPath, bool useTilde)
        {
            string retval = ConfigurationManager.AppSettings[settingsKey];

            if (String.IsNullOrEmpty(retval))
                retval = standardPath;

            return retval.TrimEnd('/');
        }
    }
}