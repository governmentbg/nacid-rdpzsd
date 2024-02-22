using Logs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Logs
{
	public class LogContext : DbContext
	{
		public DbSet<Log> Logs { get; set; }
		public DbSet<ActionWorkflow> ActionWorkflows { get; set; }

		public LogContext(DbContextOptions<LogContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			ApplyConfigurations(modelBuilder);
			DisableCascadeDelete(modelBuilder);
			ConfigurePgSqlNameMappings(modelBuilder);
		}

		private static void ApplyConfigurations(ModelBuilder modelBuilder)
		{
			var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
				   .Where(t => t.GetInterfaces().Any(gi =>
					   gi.IsGenericType
					   && gi.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
				   .ToList();

			foreach (var type in typesToRegister)
			{
				dynamic configurationInstance = Activator.CreateInstance(type);
				modelBuilder.ApplyConfiguration(configurationInstance);
			}
		}

		private static void DisableCascadeDelete(ModelBuilder modelBuilder)
		{
			modelBuilder.Model.GetEntityTypes()
				.SelectMany(t => t.GetForeignKeys())
				.Where(fk => !fk.IsOwnership
					&& fk.DeleteBehavior == DeleteBehavior.Cascade)
				.ToList()
				.ForEach(e => e.DeleteBehavior = DeleteBehavior.Restrict);
		}

		private static void ConfigurePgSqlNameMappings(ModelBuilder modelBuilder)
		{
			foreach (var entity in modelBuilder.Model.GetEntityTypes())
			{
				// Configure pgsql table names convention.
				entity.SetTableName(entity.ClrType.Name.ToLower());

				// Configure pgsql column names convention.
				foreach (var property in entity.GetProperties())
					property.SetColumnName(property.Name.ToLower());
			}
		}
	}
}
