
using System;
using System.Linq;
using System.Reflection;
using CAFE.DAL.DbContexts;
using CAFE.DAL.Models;
using CAFE.DAL.Repositories;

namespace CAFE.Services
{
    public sealed class VersionController
    {
        public DbRemoteVersion GetLocalVersion()
        {
            var rep = new Repository<DbRemoteVersion>(new ApplicationDbContext());
            var foundVersion = rep.Select().FirstOrDefault();
            if (foundVersion == null)
            {
                foundVersion = new DbRemoteVersion();
                foundVersion.IsNew = false;
                foundVersion.Version = Assembly.GetCallingAssembly().GetName().Version.ToString();

                rep.Insert(foundVersion);
            }

            return foundVersion;
        }

        public void SetLocalVersionObsoluted(Version serverVersion)
        {
            var rep = new Repository<DbRemoteVersion>(new ApplicationDbContext());

            var foundVersion = rep.Select().FirstOrDefault();
            if (foundVersion == null)
            {
                foundVersion = new DbRemoteVersion();
                foundVersion.IsNew = true;
                foundVersion.Version = serverVersion.ToString();

                rep.Insert(foundVersion);
            }
            else
            {
                foundVersion.IsNew = true;
                foundVersion.Version = serverVersion.ToString();

                rep.Update(foundVersion);
            }
        }
    }
}
