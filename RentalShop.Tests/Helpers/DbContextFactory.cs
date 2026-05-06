using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RentalShop.Infrastructure.Persistence;

namespace RentalShop.Tests.Helpers
{
    /// <summary>
    /// Creates SQLite in-memory <see cref="RentalDbContext"/> instances for tests.
    ///
    /// SQLite in-memory (not EF InMemory provider) is used deliberately:
    /// it applies <see cref="Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter{TModel,TProvider}"/>
    /// on every read and write, which is required to exercise the State-pattern
    /// persistence mapping. The caller owns the <see cref="SqliteConnection"/>
    /// and must dispose it — the DB is lost when the last connection closes.
    /// </summary>
    internal static class DbContextFactory
    {
        /// <summary>
        /// Opens a connection, creates the schema (including seed data), and
        /// returns both the connection and the seeding context.
        /// </summary>
        internal static (SqliteConnection connection, RentalDbContext context) Create()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var ctx = BuildContext(connection);
            ctx.Database.EnsureCreated();
            return (connection, ctx);
        }

        /// <summary>
        /// Returns a fresh, independent <see cref="RentalDbContext"/> that shares
        /// the same in-memory DB as the original connection. Use to simulate a
        /// new request / unit-of-work boundary without touching the DB file.
        /// </summary>
        internal static RentalDbContext CreateFresh(SqliteConnection connection)
            => BuildContext(connection);

        private static RentalDbContext BuildContext(SqliteConnection connection)
        {
            var options = new DbContextOptionsBuilder<RentalDbContext>()
                .UseSqlite(connection)
                .Options;
            return new RentalDbContext(options);
        }
    }
}
