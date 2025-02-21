﻿using System;
using System.IO;
using StackExchange.Profiling.Storage;
using Xunit;
using Xunit.Abstractions;

namespace StackExchange.Profiling.Tests.Storage
{
    public class SqliteStorageTests : StorageBaseTest, IClassFixture<SqliteStorageFixture>
    {
        public SqliteStorageTests(SqliteStorageFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }
    }

    public class SqliteStorageFixture : StorageFixtureBase<SqliteStorage>, IDisposable
    {
        private readonly string fileName;

        public SqliteStorageFixture()
        {
            fileName = Guid.NewGuid() + ".sqlite";

            Storage = new SqliteStorage(
                $"Data Source={fileName},Pooling=False",
                "MPTest" + TestId,
                "MPTimingsTest" + TestId,
                "MPClientTimingsTest" + TestId);
            try
            {
                Storage.CreateSchema();
            }
            catch (Exception e)
            {
                e.MaybeLog(fileName);
                ShouldSkip = true;
                SkipReason = e.Message;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!ShouldSkip)
            {
                Storage.DropSchema();
            }
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }
}
