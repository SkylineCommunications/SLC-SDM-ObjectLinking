// Ignore Spelling: SDM

namespace Skyline.DataMiner.SDM.ObjectLinking
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Net.Messages.SLDataGateway;

	/// <summary>
	/// Provides extension methods for creating and retrieving <see cref="Link"/> instances.
	/// </summary>
	public static class LinkProvider
	{
		/// <summary>
		/// Creates a new <see cref="Link"/> between two entities.
		/// </summary>
		/// <param name="provider">The provider used to create the link.</param>
		/// <param name="entityA">The first entity to link.</param>
		/// <param name="entityB">The second entity to link.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entityA"/> or <paramref name="entityB"/> is <c>null</c>.</exception>
		public static void Create(this ICreatable<Link> provider, Entity entityA, Entity entityB)
		{
			provider.Create(new Link
			{
				Entities =
				{
					entityA ?? throw new ArgumentNullException(nameof(entityA)),
					entityB ?? throw new ArgumentNullException(nameof(entityB)),
				},
			});
		}

		/// <summary>
		/// Retrieves all <see cref="Link"/> instances associated with the specified entity.
		/// </summary>
		/// <param name="provider">The provider used to retrieve the links.</param>
		/// <param name="entity">The entity for which to retrieve links.</param>
		/// <returns>A list of <see cref="Link"/> objects associated with the entity.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is <c>null</c> or its <c>ID</c> is <c>null</c>.</exception>
		public static List<Link> GetLinksByEntity(this IPageable<Link> provider, Entity entity)
		{
			return GetLinksByEntity(provider, entity?.ID ?? throw new ArgumentNullException(nameof(entity)));
		}

		/// <summary>
		/// Retrieves all <see cref="Link"/> instances associated with the specified entity ID.
		/// </summary>
		/// <param name="provider">The provider used to retrieve the links.</param>
		/// <param name="entityId">The ID of the entity for which to retrieve links.</param>
		/// <returns>A list of <see cref="Link"/> objects associated with the entity ID.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entityId"/> is <c>null</c> or empty.</exception>
		public static List<Link> GetLinksByEntity(this IPageable<Link> provider, string entityId)
		{
			if (String.IsNullOrEmpty(entityId))
			{
				throw new ArgumentNullException(nameof(entityId));
			}

			var result = new List<Link>();

			foreach (var page in provider.ReadPaged(LinkExposers.Entities.ID.Equal(entityId)))
			{
				result.AddRange(page);
			}

			return result;
		}

		/// <summary>
		/// Retrieves all related entities that are linked with the specified entity.
		/// </summary>
		/// <param name="provider">The provider used to retrieve the links.</param>
		/// <param name="entity">The entity for which to retrieve links.</param>
		/// <returns>A list of <see cref="Link"/> objects associated with the entity.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is <c>null</c> or its <c>ID</c> is <c>null</c>.</exception>
		public static List<Entity> GetLinkedEntities(this IPageable<Link> provider, Entity entity)
		{
			return GetLinkedEntities(provider, entity?.ID ?? throw new ArgumentNullException(nameof(entity)));
		}

		/// <summary>
		/// Retrieves all related entities that are linked with the specified entity.
		/// </summary>
		/// <param name="provider">The provider used to retrieve the links.</param>
		/// <param name="entityId">The ID of the entity for which to retrieve links.</param>
		/// <returns>A list of <see cref="Link"/> objects associated with the entity ID.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="entityId"/> is <c>null</c> or empty.</exception>
		public static List<Entity> GetLinkedEntities(this IPageable<Link> provider, string entityId)
		{
			if (String.IsNullOrEmpty(entityId))
			{
				throw new ArgumentNullException(nameof(entityId));
			}

			var result = new List<Entity>();

			foreach (var page in provider.ReadPaged(LinkExposers.Entities.ID.Equal(entityId)))
			{
				result.AddRange(
					page.SelectMany(l =>
						l.Entities.Where(e => e.ID != entityId)));
			}

			return result;
		}

		/// <summary>
		/// Retrieves a <see cref="Link"/> instance by its unique identifier.
		/// </summary>
		/// <param name="provider">The provider used to retrieve the link.</param>
		/// <param name="linkId">The unique identifier of the link.</param>
		/// <returns>The <see cref="Link"/> object with the specified ID.</returns>
		/// <exception cref="ArgumentException">Thrown when <paramref name="linkId"/> is an empty <see cref="Guid"/>.</exception>
		/// <exception cref="KeyNotFoundException">Thrown when no link is found with the specified <paramref name="linkId"/>.</exception>
		public static Link GetLinkById(IReadable<Link> provider, Guid linkId)
		{
			if (linkId == Guid.Empty)
			{
				throw new ArgumentException("Identifier cannot be an empty GUID.", nameof(linkId));
			}

			var link = provider.Read(LinkExposers.Guid.Equal(linkId))?.FirstOrDefault();
			if (link is null)
			{
				throw new KeyNotFoundException($"No link found with ID: {linkId}");
			}

			return link;
		}
	}
}
