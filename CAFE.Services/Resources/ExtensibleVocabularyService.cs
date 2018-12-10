
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using CAFE.Core.Resources;
using CAFE.DAL.Interfaces;
using CAFE.DAL.Models;
using CAFE.DAL.Models.Resources;
using AutoMapper;
using CAFE.Core.Integration;
using System.Text.RegularExpressions;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Reflection;

namespace CAFE.Services.Resources
{
    public static class ContextExtensions
    {
        public static string GetTableNameByDbContext<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;

            return objectContext.GetTableName<T>();
        }

        public static string GetTableName<T>(this ObjectContext context) where T : class
        {
            string sql = context.CreateObjectSet<T>().ToTraceString();
            Regex regex = new Regex("FROM (?<table>.*) AS");
            Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }
    }

    /// <summary>
    /// Service that manage extensions for *Vocabulary values
    /// That mean, vocabulary may be extended (add new value or remove)
    /// </summary>
    public class ExtensibleVocabularyService : IExtensibleVocabularyService
    {
        private readonly IRepository<DbVocabularyValue> _vocabularyValuesRepository;
        private readonly IRepository<DbVocabularyUserValue> _vocabularyUserValuesRepository;
        private readonly IRepository<DbUser> _usersRepository;

        /// <summary>
        /// Default ctor with dependencies
        /// </summary>
        public ExtensibleVocabularyService(
            IRepository<DbVocabularyValue> vocabularyValuesRepository,
            IRepository<DbVocabularyUserValue> vocabularyUserValuesRepository,
            IRepository<DbUser> usersRepository)
        {
            _vocabularyValuesRepository = vocabularyValuesRepository;
            _vocabularyUserValuesRepository = vocabularyUserValuesRepository;
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// Returns all(globaly) extended values
        /// </summary>
        /// <returns>Collection of extended values</returns>
        public IEnumerable<VocabularyUserValue> GetAllExtenededUsersValues()
        {
            var dbData = _vocabularyUserValuesRepository.Select().ToList();
            var mappedData = Mapper.Map<IEnumerable<DbVocabularyUserValue>, List<VocabularyUserValue>>(dbData);
            return mappedData.OrderBy(o => o.Value);
        }

        /// <summary>
        /// Returns all(globaly) extended values for vocabulary type T (that inherited from enum)
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <returns>Collection of extended values</returns>
        public IEnumerable<VocabularyValue> GetAllExtenededValues<T>(string userId) where T : struct, IConvertible
        {
            return GetAllExtenededValues(typeof (T), userId);
        }

        /// <summary>
        /// Returns all(globaly) extended values for vocabulary type T (that inherited from enum)
        /// </summary>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <returns>Collection of extended values</returns>
        public IEnumerable<VocabularyValue> GetAllExtenededValues(Type vocabularyType, string userId)
        {
            Contract.Requires(vocabularyType != null);

            var result = new List<VocabularyValue>();

            if (!string.IsNullOrEmpty(userId))
            {
                var userIdGuid = Guid.Parse(userId);

                result.AddRange(_vocabularyUserValuesRepository.
                    FindCollection(w => w.Type == vocabularyType.Name && w.User.Id == userIdGuid).
                    Select(s => new VocabularyValue { Value = s.Value, Description = s.Description, Type = s.Type, Id = s.Id }));
            }

            result.AddRange(_vocabularyValuesRepository.FindCollection(w => w.Type == vocabularyType.Name).Select(s => new VocabularyValue { Value = s.Value, Description = s.Description, Type = s.Type, Id = s.Id }));

            return result.OrderBy(o => o.Value);
        }


        /// <summary>
        /// Returns all(globaly) filtered extended values for vocabulary type T (that inherited from enum)
        /// </summary>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <param name="searchToken">Filtered by this value</param>
        /// <returns>Collection of extended values</returns>
        public IEnumerable<VocabularyValue> GetAllExtenededValues(Type vocabularyType, string searchToken,
            string userId)
        {
            Contract.Requires(vocabularyType != null);

            var result = new List<VocabularyValue>();

            if (!string.IsNullOrEmpty(userId))
            {
                var userIdGuid = Guid.Parse(userId);

                result.AddRange(_vocabularyUserValuesRepository.
                    FindCollection(w => w.Type == vocabularyType.Name && w.User.Id == userIdGuid && w.Value.StartsWith(searchToken)).
                    Select(s => new VocabularyValue { Value = s.Value, Description = s.Description, Type = s.Type, Id = s.Id }));
            }

            result.AddRange(_vocabularyValuesRepository.FindCollection(w => w.Type == vocabularyType.Name && w.Value.StartsWith(searchToken)).Select(s => new VocabularyValue { Value = s.Value, Description = s.Description, Type = s.Type, Id = s.Id }));

            return result.OrderBy(o => o.Value);
        }


        /// <summary>
        /// Returns all(globaly) extended values
        /// </summary>
        /// <returns>Collection of extended values</returns>
        public IEnumerable<VocabularyValue> GetAllExtenededValues()
        {
            return _vocabularyValuesRepository.
                Select(s => new VocabularyValue { Value = s.Value, Description = s.Description, Type = s.Type, Id = s.Id}).Where(s => s.Value != "-1").OrderBy(o => o.Value);
        }

        /// <summary>
        /// Returns all(globaly) extended values for vocabulary type T (that inherited from enum)
        /// </summary>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <returns>Collection of extended values</returns>
        public Dictionary<string, string> GetAllSimpleTypesVocabularies()
        {
            var result = new Dictionary<string, string>();
            var allValues = _vocabularyValuesRepository.FindCollection(w => w.Value == "-1");

            foreach (var item in allValues)
                if(!result.ContainsKey(item.Type))
                    result.Add(item.Type, item.Description);

            return result;
        }

        /// <summary>
        /// Returns user's defined extended values for vocabulary type T (that inherited from enum)
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <param name="by">User's id that added extended values</param>
        /// <returns>Collection of extended values</returns>
        public IEnumerable<VocabularyValue> GetExtenededValuesBy<T>(string @by) where T : struct, IConvertible
        {
            return GetExtenededValuesBy(@by, typeof (T));
        }

        /// <summary>
        /// Returns user's defined extended values for vocabulary type T (that inherited from enum)
        /// </summary>
        /// <param name="by">User's id that added extended values</param>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <returns>Collection of extended values</returns>
        public IEnumerable<VocabularyValue> GetExtenededValuesBy(string @by, Type vocabularyType)
        {
            //Contract.Requires(!string.IsNullOrEmpty(@by));
            Contract.Requires(vocabularyType != null);

            if (string.IsNullOrEmpty(@by)) return new List<VocabularyValue>();

            return _vocabularyUserValuesRepository
                .FindCollection(w => w.Type == vocabularyType.Name && w.User.Id.ToString() == by).Select(s => new VocabularyValue { Value = s.Value, Description = s.Description }).OrderBy(o => o.Value);
        }


        /// <summary>
        /// Returns user's defined extended filtered values for vocabulary type T (that inherited from enum)
        /// </summary>
        /// <param name="by">User's id that added extended values</param>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <param name="searchToken">Filtered by this value</param>
        /// <returns>Collection of extended values</returns>
        public IEnumerable<VocabularyValue> GetExtenededValuesBy(string by, Type vocabularyType, string searchToken)

        {
            Contract.Requires(vocabularyType != null);

            if (string.IsNullOrEmpty(@by)) return new List<VocabularyValue>();

            return _vocabularyUserValuesRepository
                .FindCollection(w => w.Type == vocabularyType.Name && w.User.Id.ToString() == by && w.Value.StartsWith(searchToken)).Select(s => new VocabularyValue { Value = s.Value, Description = s.Description }).OrderBy(o => o.Value);
        }

        /// <summary>
        /// Add new user's defined extended value for vocabulary type T (that inherited from enum)
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <param name="by">User's id that added extended values</param>
        /// <param name="values">Extended values</param>
        public void AddNewExtendedValuesBy<T>(string @by, Dictionary<string, string> values) where T : struct, IConvertible
        {
            AddNewExtendedValuesBy(by, typeof (T), values);
        }

        /// <summary>
        /// Add new user's defined extended value for vocabulary type T (that inherited from enum)
        /// </summary>
        /// <param name="by">User's id that added extended values</param>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <param name="values">Extended values</param>
        public void AddNewExtendedValuesBy(string @by, Type vocabularyType, Dictionary<string, string> values)
        {
            Contract.Requires(!string.IsNullOrEmpty(@by));
            Contract.Requires(vocabularyType != null);
            Contract.Requires(values != null);
            Contract.Requires(values.Any());

            foreach (var value in values)
            {
                if (_vocabularyUserValuesRepository.Find(f => f.Value == value.Key) == null)
                {
                    var newVocabulary = new DbVocabularyUserValue()
                    {
                        CreationDate = DateTime.Now,
                        Type = vocabularyType.Name,
                        User = _usersRepository.Find(f => f.Id.ToString() == by),
                        Value = value.Key,
                        Description = value.Value
                    };

                    _vocabularyUserValuesRepository.Insert(newVocabulary);
                }
            }
        }

        /// <summary>
        /// Make user's defined extended values globaly (available for all)
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <param name="by">User's id that added extended values</param>
        /// <param name="values">user's defined extended values</param>
        /// <returns>Globaly available extended values</returns>
        public void MakeExtenededValuesGlobaly<T>(IEnumerable<long> values) where T : struct, IConvertible
        {
            MakeExtenededValuesGlobaly(typeof (T), values);
        }

        /// <summary>
        /// Make user's defined extended values globaly (available for all)
        /// </summary>
        /// <param name="by">User's id that added extended values</param>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <param name="values">user's defined extended values</param>
        /// <returns>Globaly available extended values</returns>
        public void MakeExtenededValuesGlobaly(Type vocabularyType, IEnumerable<long> ids)
        {
            Contract.Requires(ids.Any());

            var userValues = _vocabularyUserValuesRepository
                .FindCollection(w => ids.Contains(w.Id));

            var dbVocabularyUserValues = userValues as IList<DbVocabularyUserValue> ?? userValues.ToList();
            foreach (var value in dbVocabularyUserValues)
            {
                if (_vocabularyValuesRepository.Find(f => f.Value == value.Value) == null)
                {
                    var newVocabulary = new DbVocabularyValue()
                    {
                        Type = vocabularyType.Name,
                        Value = value.Value,
                        Description = value.Description
                    };

                    _vocabularyValuesRepository.Insert(newVocabulary);
                }
            }

            foreach (var dbVocabularyUserValue in dbVocabularyUserValues)
            {
                _vocabularyUserValuesRepository.Delete(dbVocabularyUserValue);
            }
        }

        public IEnumerable<VocabularyValue> GetAllExtenededValues_(Type vocabularyType)
        {
            Contract.Requires(vocabularyType != null);

            return Mapper.Map<IEnumerable<VocabularyValue>>(_vocabularyValuesRepository
                .FindCollection(w => w.Type == vocabularyType.Name)).OrderBy(o => o.Value);
        }

        public IEnumerable<VocabularyUserValue> GetExtenededValuesBy_(string @by, Type vocabularyType)
        {
            //Contract.Requires(!string.IsNullOrEmpty(@by));
            Contract.Requires(vocabularyType != null);

            if (string.IsNullOrEmpty(@by)) return new List<VocabularyUserValue>();

            return Mapper.Map<IEnumerable<VocabularyUserValue>>(_vocabularyUserValuesRepository
                .FindCollection(w => w.Type == vocabularyType.Name && w.User.Id.ToString() == by)).OrderBy(o => o.Value);
        }

        public IEnumerable<VocabularyValue> GetAllExtenededValues_<T>() where T : struct, IConvertible
        {
            return GetAllExtenededValues_(typeof(T));
        }

        public IEnumerable<VocabularyUserValue> GetExtenededValuesBy_<T>(string @by) where T : struct, IConvertible
        {
            return GetExtenededValuesBy_(@by, typeof(T));
        }

        public VocabularyValue AddGlobalValue(VocabularyValue value)
        {
            var mappedValue = Mapper.Map<DbVocabularyValue>(value);
            var newDbValue = _vocabularyValuesRepository.Insert(mappedValue);
            var newMappedValue = Mapper.Map<VocabularyValue>(newDbValue);

            return newMappedValue;
        }

        public void UpdateGlobalValue(VocabularyValue value)
        {
            var oldValue = _vocabularyValuesRepository.FindWithDetaching(v => v.Id == value.Id);
            var mappedValue = Mapper.Map<DbVocabularyValue>(value);

            if (oldValue != null)
            {
                var context = new DAL.DbContexts.ApplicationDbContext();
                var typeName = mappedValue.Type.Replace("Vocabulary", "");
                var type = Type.GetType("CAFE.DAL.Models.Db" + typeName + ", CAFE.DAL");

                MethodInfo method = typeof(ContextExtensions).GetMethod("GetTableNameByDbContext");
                MethodInfo genericMethod = method.MakeGenericMethod(type);
                var tableName = (string)genericMethod.Invoke(this, new []{ context });

                var updateResult = 
                    context.
                    Database.
                    ExecuteSqlCommand($"update {tableName} set Value = '{mappedValue.Value}' where Value = '{oldValue.Value}'");
            }

            _vocabularyValuesRepository.Update(mappedValue);
        }

        public void DeleteGlobalValue(long id)
        {
            var foundValue = _vocabularyValuesRepository.Select().Where(w => w.Id == id).FirstOrDefault();
            _vocabularyValuesRepository.Delete(foundValue);
        }
    }
}
