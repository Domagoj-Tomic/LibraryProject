using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace LibraryBackend.Controllers
{
	public class GenericController<TEntity> : ApiController where TEntity : class
	{
		protected readonly DbContext _context;
		protected readonly DbSet<TEntity> _dbSet;

		public GenericController(DbContext context)
		{
			_context = context;
			_dbSet = _context.Set<TEntity>();
		}

		// GET: api/Generic
		public IQueryable<TEntity> Get()
		{
			return _dbSet;
		}

		// GET: api/Generic/5
		public IHttpActionResult Get(int id)
		{
			TEntity entity = _dbSet.Find(id);
			if (entity == null)
			{
				return NotFound();
			}

			return Ok(entity);
		}

		// PUT: api/Generic/5
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

		// POST: api/Generic
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

		// DELETE: api/Generic/5
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
