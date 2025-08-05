// Ignore Spelling: SDM Middleware

namespace Skyline.DataMiner.SDM.ObjectLinking.Middleware
{
	using System;
	using System.Collections.Generic;

	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.SDM.ObjectLinking.Models;
	using Skyline.DataMiner.SDM.ObjectLinking.Validation;

	using SLDataGateway.API.Types.Querying;

	internal class SolutionValidationMiddleware : IBulkStorageProviderMiddleware<SolutionRegistration>
	{
		public long OnCount(FilterElement<SolutionRegistration> filter, Func<FilterElement<SolutionRegistration>, long> next)
		{
			if (filter == null)
			{
				throw new ArgumentNullException(nameof(filter), "Filter cannot be null.");
			}

			return next(filter);
		}

		public long OnCount(IQuery<SolutionRegistration> query, Func<IQuery<SolutionRegistration>, long> next)
		{
			if (query == null)
			{
				throw new ArgumentNullException(nameof(query), "Query cannot be null.");
			}

			return next(query);
		}

		public void OnCreate(IEnumerable<SolutionRegistration> oToCreate, Action<IEnumerable<SolutionRegistration>> next)
		{
			var builder = new ValidationResult.Builder();
			foreach (var solutionRegistration in oToCreate)
			{
				var entry = Validate(solutionRegistration);
				builder.Add(entry);
			}

			var result = builder.Build();
			if (!result.IsValid)
			{
				throw result.ToException();
			}

			next(oToCreate);
		}

		public void OnCreate(SolutionRegistration oToCreate, Action<SolutionRegistration> next)
		{
			var result = Validate(oToCreate);
			if(!result.IsValid)
			{
				throw result.ToException();
			}

			next(oToCreate);
		}

		public void OnCreateOrUpdate(IEnumerable<SolutionRegistration> oToCreateOrUpdate, Action<IEnumerable<SolutionRegistration>> next)
		{
			var builder = new ValidationResult.Builder();
			foreach(var solutionRegistration in oToCreateOrUpdate)
			{
				var entry = Validate(solutionRegistration);
				builder.Add(entry);
			}

			var result = builder.Build();
			if (!result.IsValid)
			{
				throw result.ToException();
			}

			next(oToCreateOrUpdate);
		}

		public void OnDelete(IEnumerable<SolutionRegistration> oToDelete, Action<IEnumerable<SolutionRegistration>> next)
		{
			if (oToDelete is null)
			{
				throw new ArgumentNullException(nameof(oToDelete), "The collection of solution registrations to delete cannot be null.");
			}

			next(oToDelete);
		}

		public void OnDelete(SolutionRegistration oToDelete, Action<SolutionRegistration> next)
		{
			if (oToDelete is null)
			{
				throw new ArgumentNullException(nameof(oToDelete), "The solution registration to delete cannot be null.");
			}

			next(oToDelete);
		}

		public IEnumerable<SolutionRegistration> OnRead(FilterElement<SolutionRegistration> filter, Func<FilterElement<SolutionRegistration>, IEnumerable<SolutionRegistration>> next)
		{
			if (filter == null)
			{
				throw new ArgumentNullException(nameof(filter), "Filter cannot be null.");
			}

			return next(filter);
		}

		public IEnumerable<SolutionRegistration> OnRead(IQuery<SolutionRegistration> query, Func<IQuery<SolutionRegistration>, IEnumerable<SolutionRegistration>> next)
		{
			if (query == null)
			{
				throw new ArgumentNullException(nameof(query), "Query cannot be null.");
			}

			return next(query);
		}

		public IEnumerable<IPagedResult<SolutionRegistration>> OnReadPaged(FilterElement<SolutionRegistration> filter, Func<FilterElement<SolutionRegistration>, IEnumerable<IPagedResult<SolutionRegistration>>> next)
		{
			if (filter == null)
			{
				throw new ArgumentNullException(nameof(filter), "Filter cannot be null.");
			}

			return next(filter);
		}

		public IEnumerable<IPagedResult<SolutionRegistration>> OnReadPaged(IQuery<SolutionRegistration> query, Func<IQuery<SolutionRegistration>, IEnumerable<IPagedResult<SolutionRegistration>>> next)
		{
			if (query == null)
			{
				throw new ArgumentNullException(nameof(query), "Query cannot be null.");
			}

			return next(query);
		}

		public IEnumerable<IPagedResult<SolutionRegistration>> OnReadPaged(FilterElement<SolutionRegistration> filter, int pageSize, Func<FilterElement<SolutionRegistration>, int, IEnumerable<IPagedResult<SolutionRegistration>>> next)
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

		public IEnumerable<IPagedResult<SolutionRegistration>> OnReadPaged(IQuery<SolutionRegistration> query, int pageSize, Func<IQuery<SolutionRegistration>, int, IEnumerable<IPagedResult<SolutionRegistration>>> next)
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

		public void OnUpdate(IEnumerable<SolutionRegistration> oToUpdate, Action<IEnumerable<SolutionRegistration>> next)
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

		public void OnUpdate(SolutionRegistration oToUpdate, Action<SolutionRegistration> next)
		{
			var result = Validate(oToUpdate);
			if (!result.IsValid)
			{
				throw result.ToException();
			}

			next(oToUpdate);
		}

		private static ValidationEntry Validate(SolutionRegistration solutionRegistration)
		{
			var entry = new ValidationEntry();
			if (solutionRegistration is null)
			{
				entry.Exceptions.Add(new ArgumentNullException(nameof(solutionRegistration), "SolutionRegistration object cannot be null."));
				return entry;
			}

			if (String.IsNullOrEmpty(solutionRegistration.ID))
			{
				entry.Exceptions.Add(new ArgumentException("SolutionRegistration ID cannot be null or empty.", nameof(solutionRegistration)));
			}

			if (String.IsNullOrEmpty(solutionRegistration.DisplayName))
			{
				entry.Exceptions.Add(new ArgumentException("SolutionRegistration Display Name cannot be null or empty.", nameof(solutionRegistration)));
			}

			return entry;
		}
	}
}
