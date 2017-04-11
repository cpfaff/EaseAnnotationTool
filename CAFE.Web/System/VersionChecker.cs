
using System;
using System.Configuration;
using System.Net.Http;
using CAFE.Core;
using CAFE.Core.Configuration;
using CAFE.Services;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace CAFE.Web
{
    /// <summary>
    /// Class that performs version checking from github hosted file
    /// </summary>
    public sealed class VersionChecker
    {
        #region Singleton

        private static readonly object _locker = new object();
        private static VersionChecker _instance = null;

        public static VersionChecker Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                            _instance = new VersionChecker();
                    }
                }
                return _instance;
            }
        }

        private VersionChecker()
        {
            _controller = new VersionController();
        }

        #endregion

        private readonly VersionController _controller;


        /// <summary>
        /// Returns flag that indicates existing new version
        /// </summary>
        /// <returns></returns>
        public bool HaveNewVersion()
        {
            var currentVersion = Version.Parse(_controller.GetLocalVersion().Version);
            try
            {
                var serverVersion = RequestLastVersion();

                if (serverVersion > currentVersion)
                {
                    _controller.SetLocalVersionObsoluted(serverVersion);
                    return true;
                }
            }
            catch (Exception e)
            {
                //TODO: Connection troubles
            }

            return false;
        }

        public bool NeedToNotify()
        {
            var finded = _controller.GetLocalVersion();
            if (finded == null) return false;

            if (finded.IsNew) return true;

            return false;
        }

        private Version RequestLastVersion()
        {
            string versionString;
            using (var client = new HttpClient())
            {
                var connectionString = ConfigurationManager.AppSettings["CheckVersionBuildLink"];
                if (string.IsNullOrEmpty(connectionString))
                    connectionString = "https://raw.githubusercontent.com/deniskozlov/temprepo/master/lastversion.txt";
                var t = client.GetStringAsync(connectionString);
                t.Wait();

                versionString = t.Result;
            }
            return Version.Parse(versionString);
        }
    }

}