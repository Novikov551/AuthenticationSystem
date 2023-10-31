using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using AuthenticationSystem.Domain;
using AuthenticationSystem.Domain.Core;
using AuthenticationSystem.Domain.Extensions;
using AuthenticationSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

using NpgsqlTypes;
using UserAuthenticationSystem.Domain.Core;

namespace Infrastructure.EF;

public class AuthenticationSystemDbContext : DbContext, IUnitOfWork
{
    public DbSet<User> Users { get; set; }

    public AuthenticationSystemDbContext(DbContextOptions<AuthenticationSystemDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public AuthenticationSystemDbContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
    }

    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        var type = typeof(T);
        if (_cachedRepositories.TryGetValue(type, out var repository))
        {
            return (IRepository<T>)repository;
        }

        var concreteRepository = new BaseEfRepository<T>(Set<T>());
        _cachedRepositories.TryAdd(type, concreteRepository);

        return concreteRepository;
    }

    private readonly Dictionary<Type, object> _cachedRepositories = new();

    public async Task BulkInsertAsync<T>(IEnumerable<T> data, CancellationToken cancellationToken)
    {
        var tablename = Model.FindEntityType(typeof(T)).GetTableName() ?? string.Empty;

        if (tablename.IsNullOrEmpty())
        {
            throw new InvalidDataException($"`{tablename}` тип не найден в контексте");
        }

        using var transaction = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);

        // Берем connection и !не закрываем! это connection контекста, поэтому он сам этим управляет
        if (Database.GetDbConnection() is NpgsqlConnection connection)
        {
            // Берем connection и !не закрываем! это connection контекста, поэтому он сам этим управляет
            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync(cancellationToken);
            }

            connection.EnlistTransaction(Transaction.Current);

            try
            {
                await WriteToServerAsync(data, connection, tablename, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing NpgSqlBulkCopy.WriteToServer().  See inner exception for details", ex);
            }

            transaction.Complete();
        }
    }

    private async Task WriteToServerAsync<T>(IEnumerable<T> data, NpgsqlConnection connection, string tablename, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(tablename))
        {
            throw new ArgumentOutOfRangeException(nameof(tablename), "Destination table must be set");
        }

        var properties = typeof(T).GetProperties();
        var colCount = properties.Length;

        var types = new NpgsqlDbType[colCount];
        var lengths = new int[colCount];
        var fieldNames = new string[colCount];

        await using (var cmd = new NpgsqlCommand($"SELECT * FROM {tablename} LIMIT 1", connection))
        {
            await using var rdr = cmd.ExecuteReader();

            if (rdr.FieldCount != colCount)
            {
                throw new ArgumentOutOfRangeException("dataTable",
                    "Column count in Destination Table does not match column count in source table.");
            }

            var columns = await rdr.GetColumnSchemaAsync(cancellationToken);
            for (var i = 0; i < colCount; i++)
            {
                types[i] = (NpgsqlDbType)columns[i].NpgsqlDbType;
                lengths[i] = columns[i].ColumnSize == null ? 0 : (int)columns[i].ColumnSize;
                fieldNames[i] = columns[i].ColumnName;
            }

        }

        var sB = new StringBuilder(fieldNames[0]);
        for (var p = 1; p < colCount; p++)
        {
            sB.Append(", " + fieldNames[p]);
        }

        await using var writer = await connection.BeginBinaryImportAsync($"COPY {tablename} ({sB}) FROM STDIN (FORMAT BINARY)", cancellationToken);
        foreach (var t in data)
        {
            await writer.StartRowAsync(cancellationToken);

            for (var i = 0; i < colCount; i++)
            {
                if (properties[i].GetValue(t) == null)
                {
                    await writer.WriteNullAsync(cancellationToken);
                }
                else
                {
                    switch (types[i])
                    {
                        case NpgsqlDbType.Bigint:
                            await writer.WriteAsync((long)properties[i].GetValue(t), types[i], cancellationToken);
                            break;
                        case NpgsqlDbType.Bit:
                            if (lengths[i] > 1)
                            {
                                await writer.WriteAsync((byte[])properties[i].GetValue(t), types[i], cancellationToken);
                            }
                            else
                            {
                                await writer.WriteAsync((byte)properties[i].GetValue(t), types[i], cancellationToken);
                            }

                            break;
                        case NpgsqlDbType.Boolean:
                            await writer.WriteAsync((bool)properties[i].GetValue(t), types[i], cancellationToken);
                            break;
                        case NpgsqlDbType.Bytea:
                            await writer.WriteAsync((byte[])properties[i].GetValue(t), types[i], cancellationToken);
                            break;
                        case NpgsqlDbType.Char:
                            if (properties[i].PropertyType == typeof(string))
                            {
                                await writer.WriteAsync((string)properties[i].GetValue(t), types[i], cancellationToken);
                            }
                            else if (properties[i].PropertyType == typeof(Guid))
                            {
                                var value = properties[i].GetValue(t).ToString();
                                await writer.WriteAsync(value, types[i], cancellationToken);
                            }
                            else if (lengths[i] > 1)
                            {
                                await writer.WriteAsync((char[])properties[i].GetValue(t), types[i], cancellationToken);
                            }
                            else
                            {
                                var s = properties[i].GetValue(t).ToString().ToCharArray();
                                await writer.WriteAsync(s[0], types[i], cancellationToken);
                            }

                            break;
                        case NpgsqlDbType.Time:
                        case NpgsqlDbType.Timestamp:
                        case NpgsqlDbType.TimestampTz:
                        case NpgsqlDbType.Date:
                            await writer.WriteAsync((DateTime)properties[i].GetValue(t), types[i], cancellationToken);
                            break;
                        case NpgsqlDbType.Double:
                            await writer.WriteAsync((double)properties[i].GetValue(t), types[i], cancellationToken);
                            break;
                        case NpgsqlDbType.Integer:
                            try
                            {
                                if (properties[i].PropertyType == typeof(int))
                                {
                                    await writer.WriteAsync((int)properties[i].GetValue(t), types[i], cancellationToken);
                                    break;
                                }
                                else if (properties[i].PropertyType == typeof(string))
                                {
                                    var swap = Convert.ToInt32(properties[i].GetValue(t));
                                    await writer.WriteAsync(swap, types[i], cancellationToken);
                                    break;
                                }
                            }
                            catch
                            {
                                // ignored
                            }

                            await writer.WriteAsync(properties[i].GetValue(t), types[i], cancellationToken);
                            break;
                        case NpgsqlDbType.Interval:
                            await writer.WriteAsync((TimeSpan)properties[i].GetValue(t), types[i], cancellationToken);
                            break;
                        case NpgsqlDbType.Numeric:
                        case NpgsqlDbType.Money:
                            await writer.WriteAsync((decimal)properties[i].GetValue(t), types[i], cancellationToken);
                            break;
                        case NpgsqlDbType.Real:
                            await writer.WriteAsync((float)properties[i].GetValue(t), types[i], cancellationToken);
                            break;
                        case NpgsqlDbType.Smallint:

                            try
                            {
                                if (properties[i].PropertyType == typeof(byte))
                                {
                                    var swap = Convert.ToInt16(properties[i].GetValue(t));
                                    await writer.WriteAsync(swap, types[i], cancellationToken);
                                    break;
                                }

                                await writer.WriteAsync((short)properties[i].GetValue(t), types[i], cancellationToken);
                            }
                            catch
                            {
                                // ignored
                            }

                            break;
                        case NpgsqlDbType.Varchar:
                        case NpgsqlDbType.Text:
                            await writer.WriteAsync((string)properties[i].GetValue(t), types[i], cancellationToken);
                            break;
                        case NpgsqlDbType.Uuid:
                            await writer.WriteAsync((Guid)properties[i].GetValue(t), types[i], cancellationToken);
                            break;
                        case NpgsqlDbType.Xml:
                            await writer.WriteAsync((string)properties[i].GetValue(t), types[i], cancellationToken);
                            break;
                        case NpgsqlDbType.Jsonb:
                            await writer.WriteAsync((string)properties[i].GetValue(t), types[i], cancellationToken);
                            break;
                    }
                }
            }
        }

        await writer.CompleteAsync(cancellationToken);
    }

    public override void Dispose()
    {
        base.Dispose();
        _cachedRepositories.Clear();
    }

    public override ValueTask DisposeAsync()
    {
        _cachedRepositories.Clear();
        return base.DisposeAsync();
    }
}