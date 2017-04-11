
using System;
using AutoMapper;
using CAFE.Core.Resources;
using CAFE.Core.Security;
using CAFE.DAL.Interfaces;
using CAFE.DAL.Models;

namespace CAFE.Services.Resources
{
    /// <summary>
    /// Provide access to resource knowing only resource kind and resource unique identifier
    /// </summary>
    public class AccessibleResourcesProvider : IAccessibleResourcesProvider
    {
        private readonly IRepository<DbUserFile> _filesRepository;
        private readonly IRepository<DbAnnotationItem> _annotationsRepository;

        /// <summary>
        /// Constructor with dependencies
        /// </summary>
        /// <param name="filesRepository">Files repository</param>
        /// <param name="annotationsRepository">Annotation items repository</param>
        public AccessibleResourcesProvider(IRepository<DbUserFile> filesRepository,
            IRepository<DbAnnotationItem> annotationsRepository)
        {
            _filesRepository = filesRepository;
            _annotationsRepository = annotationsRepository;
        }


        /// <summary>
        /// Returns resource by resource kind and unique identifier
        /// </summary>
        /// <param name="resourceKind">Resource kind</param>
        /// <param name="resourceId">Resource unique identifier</param>
        /// <returns>AccessibleResource</returns>
        public AccessibleResource GetResource(AccessibleResourceKind resourceKind, Guid resourceId)
        {
            if (resourceKind == AccessibleResourceKind.File)
            {
                var dbFile = _filesRepository.Find(f => f.Id == resourceId);
                var mappedFile = Mapper.Map(dbFile, new UserFile());
                return new AccessibleResource()
                {
                    Content = mappedFile,
                    ResourceId = resourceId,
                    Kind = AccessibleResourceKind.File
                };
            }
            else
            {
                var dbAnnotationItem = _annotationsRepository.Find(f => f.Id == resourceId);
                //var mappedAnnotationItem = Mapper.Map(dbAnnotationItem, new Core.Integration.AnnotationItem());
                return new AccessibleResource()
                {
                    Content = dbAnnotationItem,
                    ResourceId = resourceId,
                    Kind = AccessibleResourceKind.AnnotationItem
                };
            }
        }
    }
}
