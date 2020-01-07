﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MrCMS.Entities;

namespace MrCMS.Data
{
    public class DataReader : IDataReader
    {
        private readonly IRepositoryResolver _repositoryResolver;

        public DataReader(IRepositoryResolver repositoryResolver)
        {
            _repositoryResolver = repositoryResolver;
        }

        public IQueryable<T> Readonly<T>() where T : class, IHaveId, IHaveSite
        {
            return _repositoryResolver.GetRepository<T>().Readonly();
        }

        public IQueryable<T> GlobalReadonly<T>() where T : class, IHaveId
        {
            return _repositoryResolver.GetGlobalRepository<T>().Readonly();
        }

        public Task<T> Get<T>(int id, CancellationToken token) where T : class, IHaveId, IHaveSite
        {
            return _repositoryResolver.GetRepository<T>().GetData(id, token);
        }

        public async Task<object> Get(Type type, int id, CancellationToken token = default)
        {
            var methodInfo = GetType().GetMethods().First(x => (x.Name == nameof(Get)) && x.IsGenericMethod);
            var genericMethod = methodInfo.MakeGenericMethod(type);
            return await (Task<object>)genericMethod.Invoke(this, new object[] { id, token });
        }

        public Task<T> GlobalGet<T>(int id, CancellationToken token) where T : class, IHaveId
        {
            return _repositoryResolver.GetGlobalRepository<T>().GetData(id, token);
        }

        public async Task<object> GlobalGet(Type type, int id, CancellationToken token = default)
        {
            var methodInfo = GetType().GetMethods().First(x => (x.Name == nameof(GlobalGet)) && x.IsGenericMethod);
            var genericMethod = methodInfo.MakeGenericMethod(type);
            return await (Task<object>)genericMethod.Invoke(this, new object[] { id, token });
        }
    }
}