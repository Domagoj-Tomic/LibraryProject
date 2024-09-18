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
		}

/*		// GET: api/[controller]		//Obriši ovo kasnije
		[HttpGet]
		[Route("")]
		public IQueryable<TEntity> Get()
		{
			return _dbSet;
		}*/

		// GET: api/[controller]/{id}
		[HttpGet]
		[Route("/{id:int}")]
		public IHttpActionResult Get(int id)
		{
			TEntity entity = _dbSet.Find(id);
			if (entity == null)
			{
				return NotFound();
			}

			return Ok(entity);
		}

		// GET: api/[controller]
		[HttpGet]
		public IHttpActionResult Search([FromUri] string search = null, [FromUri] int limit = 50)
		{
			var entities = _dbSet.AsQueryable();

			if (!string.IsNullOrEmpty(search))
			{
				var parameter = Expression.Parameter(typeof(TEntity), "e");
				var properties = typeof(TEntity).GetProperties()
					.Where(p => p.PropertyType == typeof(string));

				Expression predicate = null;

				foreach (var property in properties)
				{
					var propertyAccess = Expression.Property(parameter, property);
					var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
					var searchExpression = Expression.Constant(search, typeof(string));
					var containsExpression = Expression.Call(propertyAccess, containsMethod, searchExpression);

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
					entities = entities.Where(lambda);
				}
			}

			return Ok(entities.Take(limit).ToList());
		}

		// PUT: api/[controller]/id/{id}
		[HttpPut]
		[Route("id/{id:int}")]
		public IHttpActionResult Put(int id, TEntity entity)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_context.Entry(entity).State = EntityState.Modified;

			try
			{
				_context.SaveChanges();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!EntityExists(id))
				{
					return NotFound();
				}
				else
				{
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
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_dbSet.Add(entity);
			_context.SaveChanges();

			var key = GetPrimaryKeyValue(entity);
			return CreatedAtRoute("DefaultApi", new { id = key }, entity);
		}

		// DELETE: api/[controller]/id/{id}
		[HttpDelete]
		[Route("id/{id:int}")]
		public IHttpActionResult Delete(int id)
		{
			TEntity entity = _dbSet.Find(id);
			if (entity == null)
			{
				return NotFound();
			}

			_dbSet.Remove(entity);
			_context.SaveChanges();

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
