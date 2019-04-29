﻿using StackExchange.Redis;
using System;
using System.Net;
namespace NopFramework.Core.Caching
{
    /// <summary>
    /// Redis连接包装器
    /// </summary>
    public interface IRedisConnectionWrapper: IDisposable
    {
        IDatabase Database(int? db = null);
        IServer Server(EndPoint endPoint);
        EndPoint[] GetEndpoints();
        void FlushDb(int? db = null);
    }
}
