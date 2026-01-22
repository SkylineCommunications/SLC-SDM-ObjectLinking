// Ignore Spelling: SDM Middleware

namespace Skyline.DataMiner.SDM.ObjectLinking.Middleware
{
	using System;
	using System.Collections.Generic;

	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.SDM.ObjectLinking.Exceptions;
	using Skyline.DataMiner.SDM.ObjectLinking.Validation;

	using SLDataGateway.API.Types.Querying;

	internal class LinkValidationMiddleware : IBulkRepositoryMiddleware<Link>
	{
		public long OnCount(FilterElement<Link> filter, Func<FilterElement<Link>, long> next)
		{
			if (filter == null)
			{
				throw new ArgumentNullException(nameof(filter), "Filter cannot be null.");
			}

			return next(filter);
		}

		public long OnCount(IQuery<Link> query, Func<IQuery<Link>, long> next)
		{
			if (query == null)
			{
				throw new ArgumentNullException(nameof(query), "Query cannot be null.");
			}

			return next(query);
		}

		public void OnCreate(IEnumerable<Link> oToCreate, Action<IEnumerable<Link>> next)
		{
			var builder = new ValidationResult.Builder();
			foreach (var link in oToCreate)
			{
				var entry = Validate(link);
				builder.Add(entry);
			}

			var result = builder.Build();
			if (!result.IsValid)
			{
				throw result.ToException();
			}

			next(oToCreate);
		}

		public void OnCreate(Link oToCreate, Action<Link> next)
		{
			var result = Validate(oToCreate);
			if (!result.IsValid)
			{
				throw result.ToException();
			}

			next(oToCreate);
		}

		public void OnCreateOrUpdate(IEnumerable<Link> oToCreateOrUpdate, Action<IEnumerable<Link>> next)
		{
			var builder = new ValidationResult.Builder();
			foreach (var link in oToCreateOrUpdate)
			{
				var entry = Validate(link);
				builder.Add(entry);
			}

			var result = builder.Build();
			if (!result.IsValid)
			{
				throw result.ToException();
			}

			next(oToCreateOrUpdate);
		}

		public void OnDelete(IEnumerable<Link> oToDelete, Action<IEnumerable<Link>> next)
		{
			if(oToDelete is null)
			{
				throw new ArgumentNullException(nameof(oToDelete), "The collection of links to delete cannot be null.");
			}

			next(oToDelete);
		}

		public void OnDelete(Link oToDelete, Action<Link> next)
		{
			if (oToDelete is null)
			{
				throw new ArgumentNullException(nameof(oToDelete), "The link to delete cannot be null.");
			}

			next(oToDelete);
		}

		public IEnumerable<Link> OnRead(FilterElement<Link> filter, Func<FilterElement<Link>, IEnumerable<Link>> next)
		{
			if (filter == null)
			{
				throw new ArgumentNullException(nameof(filter), "Filter cannot be null.");
			}

			return next(filter);
		}

		public IEnumerable<Link> OnRead(IQuery<Link> query, Func<IQuery<Link>, IEnumerable<Link>> next)
		{
			if (query == null)
			{
				throw new ArgumentNullException(nameof(query), "Query cannot be null.");
			}

			return next(query);
		}

		public IEnumerable<IPagedResult<Link>> OnReadPaged(FilterElement<Link> filter, Func<FilterElement<Link>, IEnumerable<IPagedResult<Link>>> next)
		{
			if (filter == null)
			{
				throw new ArgumentNullException(nameof(filter), "Filter cannot be null.");
			}

			return next(filter);
		}

		public IEnumerable<IPagedResult<Link>> OnReadPaged(IQuery<Link> query, Func<IQuery<Link>, IEnumerable<IPagedResult<Link>>> next)
		{
			if (query == null)
			{
				throw new ArgumentNullException(nameof(query), "Query cannot be null.");
			}

			return next(query);
		}

		public IEnumerable<IPagedResult<Link>> OnReadPaged(FilterElement<Link> filter, int pageSize, Func<FilterElement<Link>, int, IEnumerable<IPagedResult<Link>>> next)
		{
			if (filter == null)
			{
				throw new ArgumentNullException(nameof(filter), "Filter cannot be null.");
			}

			if (pageSize <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");
			}

			return next(filter, pageSize);
		}

		public IEnumerable<IPagedResult<Link>> OnReadPaged(IQuery<Link> query, int pageSize, Func<IQuery<Link>, int, IEnumerable<IPagedResult<Link>>> next)
		{
			if (query == null)
			{
				throw new ArgumentNullException(nameof(query), "Query cannot be null.");
			}

			if (pageSize <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");
			}

			return next(query, pageSize);
		}

		public void OnUpdate(IEnumerable<Link> oToUpdate, Action<IEnumerable<Link>> next)
		{
			var builder = new ValidationResult.Builder();
			foreach (var link in oToUpdate)
			{
				var entry = Validate(link);
				builder.Add(entry);
			}

			var result = builder.Build();
			if (!result.IsValid)
			{
				throw result.ToException();
			}

			next(oToUpdate);
		}

		public void OnUpdate(Link oToUpdate, Action<Link> next)
		{
			var result = Validate(oToUpdate);
			if (!result.IsValid)
			{
				throw new ValidationResult.Builder()
					.Add(result)
					.Build()
					.ToException();
			}

			next(oToUpdate);
		}

		private static ValidationEntry Validate(Link link)
		{
			var entry = new ValidationEntry();
			if (link is null)
			{
				entry.Exceptions.Add(new ArgumentNullException(nameof(link), "Link object cannot be null."));
				return entry;
			}

			// Validate that the Link has at least two entities.
			if (link.EntityDescriptors.Count < 2)
			{
				entry.Exceptions.Add(new ArgumentException($"A Link (Identifier: {link.Identifier}) must contain at least two entities.", nameof(link)));
			}

			// Optionally, check for a valid Link Id (if required).
			if (!Guid.TryParse(link.Identifier, out var guid) ||
				guid == Guid.Empty)
			{
				entry.Exceptions.Add(new ArgumentException("Link Identifier should be a valid non empty guid.", nameof(link)));
			}

			// Check the entities in the link.
			var entitySet = new HashSet<string>();
			for (int i = 0; i < link.EntityDescriptors.Count; i++)
			{
				var entity = link.EntityDescriptors[i];
				if (entity is null)
				{
					entry.Exceptions.Add(new ArgumentException($"Entity at index {i} is null in Link (Identifier: {link.Identifier}).", nameof(link)));
					continue;
				}

				if (!entitySet.Add(entity.ID))
				{
					entry.Exceptions.Add(new ArgumentException($"Entity at index '{i}' (Id: {entity.ID}) is a duplicate in Link (Identifier: {link.Identifier}).", nameof(link)));
				}

				if (String.IsNullOrEmpty(entity.ID))
				{
					entry.Exceptions.Add(new LinkEntityValidationException($"The ID of an entity in a Link cannot be null or empty.", link.Identifier, entity.ID));
				}

				if (String.IsNullOrEmpty(entity.DisplayName))
				{
					entry.Exceptions.Add(new ValidationException($"The DisplayName of an entity (Id: {entity.ID}) in Link (Identifier: {link.Identifier}) cannot be null or empty."));
				}
			}

			return entry;
		}
	}
}