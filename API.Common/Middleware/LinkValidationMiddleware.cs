// Ignore Spelling: SDM Middleware

namespace Skyline.DataMiner.SDM.ObjectLinking.Middleware
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;

	using DomHelpers.SlcObject_Linking;

	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.SDM.ObjectLinking.Exceptions;

	using SLDataGateway.API.Types.Querying;

	internal class LinkValidationMiddleware : IBulkStorageProviderMiddleware<Link>
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
				throw new ValidationResult.Builder()
					.Add(result)
					.Build()
					.ToException();
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
			if (link.Entities.Count < 2)
			{
				entry.Exceptions.Add(new ArgumentException($"A Link (Guid: {link.Guid}) must contain at least two entities.", nameof(link)));
			}

			// Optionally, check for a valid Link Id (if required).
			if (link.Guid == Guid.Empty)
			{
				entry.Exceptions.Add(new ArgumentException("Link Guid cannot be an empty guid.", nameof(link)));
			}

			// Check the entities in the link.
			var entitySet = new HashSet<string>();
			for (int i = 0; i < link.Entities.Count; i++)
			{
				var entity = link.Entities[i];
				if (entity is null)
				{
					entry.Exceptions.Add(new ArgumentException($"Entity at index {i} is null in Link (Guid: {link.Guid}).", nameof(link)));
					continue;
				}

				if (!entitySet.Add(entity.ID))
				{
					entry.Exceptions.Add(new ArgumentException($"Entity at index '{i}' (Id: {entity.ID}) is a duplicate in Link (Guid: {link.Guid}).", nameof(link)));
				}

				if (String.IsNullOrEmpty(entity.ID))
				{
					entry.Exceptions.Add(new LinkEntityValidationException($"The ID of an entity in a Link cannot be null or empty.", link.Guid, entity.ID));
				}

				if (String.IsNullOrEmpty(entity.DisplayName))
				{
					entry.Exceptions.Add(new ValidationException($"The DisplayName of an entity (Id: {entity.ID}) in Link (Guid: {link.Guid}) cannot be null or empty."));
				}
			}

			return entry;
		}
	}

	internal class ValidationResult
	{
		private ValidationResult(List<ValidationEntry> entries)
		{
			Entries = new ReadOnlyCollection<ValidationEntry>(entries ?? throw new ArgumentNullException(nameof(entries), "Entries cannot be null."));
			IsValid = !Entries.Any(x => !x.IsValid);
		}

		public bool IsValid { get; }

		public IReadOnlyList<ValidationEntry> Entries { get; }

		public Exception ToException()
		{
			var exceptionMessage = ToString();
			return new ValidationException(exceptionMessage);
		}

		public override string ToString()
		{
			if (Entries == null || Entries.Count == 0)
			{
				return "Validation succeeded.";
			}

			var builder = new StringBuilder();
			for (int i = 0; i < Entries.Count; i++)
			{
				var entry = Entries[i];
				if (entry.IsValid)
				{
					continue; // Skip valid entries
				}

				builder.AppendLine($"Entry {i}:");
				foreach (var ex in entry.Exceptions)
				{
					builder.AppendLine($" - {ex.Message}");
				}
			}

			return builder.ToString();
		}

		internal class Builder
		{
			private readonly List<ValidationEntry> _entries = new List<ValidationEntry>();

			public Builder Add(ValidationEntry result)
			{
				if (result is null)
				{
					throw new ArgumentNullException(nameof(result), "Validation result cannot be null.");
				}

				_entries.Add(result);
				return this;
			}

			public void AddRange(IEnumerable<ValidationEntry> result)
			{
				if (result is null)
				{
					throw new ArgumentNullException(nameof(result), "Validation results cannot be null.");
				}

				_entries.AddRange(result);
			}

			public ValidationResult Build()
			{
				return new ValidationResult(_entries);
			}
		}
	}

	internal class ValidationEntry
	{
		public bool IsValid => Exceptions.Count == 0;

		public List<Exception> Exceptions { get; } = new List<Exception>();
	}
}