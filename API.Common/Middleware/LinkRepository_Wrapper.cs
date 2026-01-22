namespace Skyline.DataMiner.SDM.ObjectLinking.Middleware
{
	using System;
	using System.Collections.Generic;

	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.SDM;

	using SLDataGateway.API.Types.Querying;

	internal sealed partial class LinkRepository_Middleware : IBulkRepository<Link>
	{
		private readonly IBulkRepository<Link> _inner;
		private readonly IMiddlewareMarker<Link> _middleware;

		public LinkRepository_Middleware(IBulkRepository<Link> inner, IMiddlewareMarker<Link> middleware)
		{
			_inner = inner ?? throw new ArgumentNullException(nameof(inner));
			_middleware = middleware;
		}

		public IEnumerable<IPagedResult<Link>> ReadPaged(FilterElement<Link> filter)
		{
			if (_middleware is IPageableMiddleware<Link> middleware)
			{
				return middleware.OnReadPaged(filter, _inner.ReadPaged);
			}
			else
			{
				return _inner.ReadPaged(filter);
			}
		}

		public IEnumerable<IPagedResult<Link>> ReadPaged(IQuery<Link> query)
		{
			if (_middleware is IPageableMiddleware<Link> middleware)
			{
				return middleware.OnReadPaged(query, _inner.ReadPaged);
			}
			else
			{
				return _inner.ReadPaged(query);
			}
		}

		public IEnumerable<IPagedResult<Link>> ReadPaged(FilterElement<Link> filter, int pageSize)
		{
			if (_middleware is IPageableMiddleware<Link> middleware)
			{
				return middleware.OnReadPaged(filter, pageSize, _inner.ReadPaged);
			}
			else
			{
				return _inner.ReadPaged(filter, pageSize);
			}
		}

		public IEnumerable<IPagedResult<Link>> ReadPaged(IQuery<Link> query, int pageSize)
		{
			if (_middleware is IPageableMiddleware<Link> middleware)
			{
				return middleware.OnReadPaged(query, pageSize, _inner.ReadPaged);
			}
			else
			{
				return _inner.ReadPaged(query, pageSize);
			}
		}

		public IEnumerable<Link> Read(FilterElement<Link> filter)
		{
			if (_middleware is IReadableMiddleware<Link> middleware)
			{
				return middleware.OnRead(filter, _inner.Read);
			}
			else
			{
				return _inner.Read(filter);
			}
		}

		public IEnumerable<Link> Read(IQuery<Link> query)
		{
			if (_middleware is IReadableMiddleware<Link> middleware)
			{
				return middleware.OnRead(query, _inner.Read);
			}
			else
			{
				return _inner.Read(query);
			}
		}

		public long Count(FilterElement<Link> filter)
		{
			if (_middleware is ICountableMiddleware<Link> middleware)
			{
				return middleware.OnCount(filter, _inner.Count);
			}
			else
			{
				return _inner.Count(filter);
			}
		}

		public long Count(IQuery<Link> query)
		{
			if (_middleware is ICountableMiddleware<Link> middleware)
			{
				return middleware.OnCount(query, _inner.Count);
			}
			else
			{
				return _inner.Count(query);
			}
		}

		public IReadOnlyCollection<Link> Create(IEnumerable<Link> oToCreate)
		{
			if (_middleware is IBulkCreatableMiddleware<Link> middleware)
			{
				return middleware.OnCreate(oToCreate, _inner.Create);
			}
			else
			{
				return _inner.Create(oToCreate);
			}
		}

		public Link Create(Link oToCreate)
		{
			if (_middleware is ICreatableMiddleware<Link> middleware)
			{
				return middleware.OnCreate(oToCreate, _inner.Create);
			}
			else
			{
				return _inner.Create(oToCreate);
			}
		}

		public IReadOnlyCollection<Link> Update(IEnumerable<Link> oToUpdate)
		{
			if (_middleware is IBulkUpdatableMiddleware<Link> middleware)
			{
				return middleware.OnUpdate(oToUpdate, _inner.Update);
			}
			else
			{
				return _inner.Update(oToUpdate);
			}
		}

		public Link Update(Link oToUpdate)
		{
			if (_middleware is IUpdatableMiddleware<Link> middleware)
			{
				return middleware.OnUpdate(oToUpdate, _inner.Update);
			}
			else
			{
				return _inner.Update(oToUpdate);
			}
		}

		public void Delete(IEnumerable<Link> oToDelete)
		{
			if (_middleware is IBulkDeletableMiddleware<Link> middleware)
			{
				middleware.OnDelete(oToDelete, _inner.Delete);
			}
			else
			{
				_inner.Delete(oToDelete);
			}
		}

		public void Delete(Link oToDelete)
		{
			if (_middleware is IDeletableMiddleware<Link> middleware)
			{
				middleware.OnDelete(oToDelete, _inner.Delete);
			}
			else
			{
				_inner.Delete(oToDelete);
			}
		}

		public IReadOnlyCollection<Link> CreateOrUpdate(IEnumerable<Link> oToCreateOrUpdate)
		{
			if (_middleware is IBulkRepositoryMiddleware<Link> middleware)
			{
				return middleware.OnCreateOrUpdate(oToCreateOrUpdate, _inner.CreateOrUpdate);
			}
			else
			{
				return _inner.CreateOrUpdate(oToCreateOrUpdate);
			}
		}
	}
}
