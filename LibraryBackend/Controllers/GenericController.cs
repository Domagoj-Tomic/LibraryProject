using Serilog;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web.Http;

namespace LibraryBackend.Controllers
{
	[RoutePrefix("api/[controller]")]
	public class GenericController<TEntity> : ApiController where TEntity : class
	{
		protected readonly DbContext _context;
		protected readonly DbSet<TEntity> _dbSet;

		public GenericController(DbContext context)
		{
			_context = context;
			_dbSet = _context.Set<TEntity>();

			// Configure Serilog to log to a file on the desktop
			/*Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug() // Ensure the minimum level is set to Debug
				.WriteTo.File(
					path: System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "log.txt"),
					rollingInterval: RollingInterval.Day)
				.CreateLogger();*/

			// Enable SQL logging
			_context.Database.Log = sql => Log.Debug(sql);

			Log.Information("" +
				"\n~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~" +
				"\n~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~O~" +
				"\n"
				);
		}

		// GET: api/[controller]/{id}
		[HttpGet]
		[Route("/{id:int}")]
		public IHttpActionResult Get(int id)
		{
			Log.Information("Fetching entity with ID {Id}", id);
			TEntity entity = _dbSet.Find(id);
			if (entity == null)
			{
				Log.Warning("Entity with ID {Id} not found", id);
				return NotFound();
			}

			Log.Information("Entity with ID {Id} found", id);
			return Ok(entity);
		}

		// GET: api/[controller]
		[HttpGet]
		public IHttpActionResult Search([FromUri] string search = null, [FromUri] int limit = 50)
		{
			Log.Information("Search initiated with search term '{SearchTerm}' and limit {Limit}", search, limit);

			// Trim the search term to remove any leading/trailing whitespace and quotes
			search = search?.Trim().Trim('"');

			var entities = _dbSet.AsQueryable();

			if (!string.IsNullOrEmpty(search))
			{
				var parameter = Expression.Parameter(typeof(TEntity), "e");
				var properties = typeof(TEntity).GetProperties()
					.Where(p => p.PropertyType == typeof(string));

				Log.Debug("Properties to be searched: {@Properties}", properties.Select(p => p.Name).ToList());

				Expression predicate = null;

				foreach (var property in properties)
				{
					var propertyAccess = Expression.Property(parameter, property);
					var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
					var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
					var searchExpression = Expression.Constant(search.ToLower(), typeof(string));
					var toLowerExpression = Expression.Call(propertyAccess, toLowerMethod);
					var containsExpression = Expression.Call(toLowerExpression, containsMethod, searchExpression);

					if (predicate == null)
					{
						predicate = containsExpression;
					}
					else
					{
						predicate = Expression.OrElse(predicate, containsExpression);
					}
				}

				if (predicate != null)
				{
					var lambda = Expression.Lambda<Func<TEntity, bool>>(predicate, parameter);
					Log.Debug("Search predicate: {Predicate}", lambda);
					entities = entities.Where(lambda);
				}
			}

			var result = entities.Take(limit).ToList();
			// Log the result count and details
			Log.Information("Search result count: {ResultCount}", result.Count);
			Log.Debug("Search results: {@Results}", result);
			return Ok(result);
		}

		// PUT: api/[controller]/id/{id}
		[HttpPut]
		[Route("id/{id:int}")]
		public IHttpActionResult Put(int id, TEntity entity)
		{
			Log.Information("Updating entity with ID {Id}", id);
			if (!ModelState.IsValid)
			{
				Log.Warning("Invalid model state for entity with ID {Id}", id);
				return BadRequest(ModelState);
			}

			_context.Entry(entity).State = EntityState.Modified;

			try
			{
				_context.SaveChanges();
				Log.Information("Entity with ID {Id} updated successfully", id);
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!EntityExists(id))
				{
					Log.Warning("Entity with ID {Id} not found during update", id);
					return NotFound();
				}
				else
				{
					Log.Error("Concurrency exception occurred while updating entity with ID {Id}", id);
					throw;
				}
			}

			return StatusCode(HttpStatusCode.NoContent);
		}

		// POST: api/[controller]
		[HttpPost]
		[Route("")]
		public IHttpActionResult Post(TEntity entity)
		{
			Log.Information("Creating a new entity");
			if (!ModelState.IsValid)
			{
				Log.Warning("Invalid model state for new entity");
				return BadRequest(ModelState);
			}

			_dbSet.Add(entity);
			_context.SaveChanges();

			var key = GetPrimaryKeyValue(entity);
			Log.Information("New entity created with ID {Id}", key);
			return CreatedAtRoute("DefaultApi", new { id = key }, entity);
		}

		// DELETE: api/[controller]/id/{id}
		[HttpDelete]
		[Route("id/{id:int}")]
		public IHttpActionResult Delete(int id)
		{
			Log.Information("Deleting entity with ID {Id}", id);
			TEntity entity = _dbSet.Find(id);
			if (entity == null)
			{
				Log.Warning("Entity with ID {Id} not found for deletion", id);
				return NotFound();
			}

			_dbSet.Remove(entity);
			_context.SaveChanges();
			Log.Information("Entity with ID {Id} deleted successfully", id);

			return Ok(entity);
		}

		private bool EntityExists(int id)
		{
			return _dbSet.Find(id) != null;
		}

		private object GetPrimaryKeyValue(TEntity entity)
		{
			var keyProperty = typeof(TEntity).GetProperties()
				.FirstOrDefault(p => p.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true).Length > 0);

			if (keyProperty == null)
			{
				// Fallback to convention-based key property names
				keyProperty = typeof(TEntity).GetProperties()
					.FirstOrDefault(p => p.Name.Equals(typeof(TEntity).Name + "ID", StringComparison.OrdinalIgnoreCase) ||
p.Name.Equals("ID", StringComparison.OrdinalIgnoreCase));
			}

			if (keyProperty == null)
			{
				throw new Exception("Entity does not have a key property");
			}

			return keyProperty.GetValue(entity);
		}
	}
}
