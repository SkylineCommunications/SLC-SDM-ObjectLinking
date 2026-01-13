// Ignore Spelling: SDM

namespace Skyline.DataMiner.SDM.ObjectLinking
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
#if NETSTANDARD2_0_OR_GREATER
	using Skyline.DataMiner.SDM.Middleware;
#endif
	using Skyline.DataMiner.SDM.ObjectLinking.Middleware;

	/// <summary>
	/// Provides functionality to create and query links between entities in the SDM object model.
	/// </summary>
	public class ObjectLinker
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectLinker"/> class.
		/// </summary>
		/// <param name="connection">The connection to use for link storage operations.</param>
		public ObjectLinker(IConnection connection)
		{
			Links = Sdm.CreateProviderBuilder(new LinkDomStorageProvider(connection))
				.AddMiddleware(new LinkValidationMiddleware())
#if NETSTANDARD2_0_OR_GREATER
				.AddMiddleware(new SdmTracingMiddleware<Link>())
#endif
				.Build();
		}

		/// <summary>
		/// Gets the storage provider for <see cref="Link"/> objects.
		/// </summary>
		public IObservableBulkStorageProvider<Link> Links { get; }

		/// <summary>
		/// Creates a new link between two entities.
		/// </summary>
		/// <param name="entityA">The first entity to link.</param>
		/// <param name="entityB">The second entity to link.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="entityA"/> or <paramref name="entityB"/> is <c>null</c>.</exception>
		public void Create(EntityDescriptor entityA, EntityDescriptor entityB)
		{
			Links.Create(new Link
			{
				EntityDescriptors =
				{
					entityA ?? throw new ArgumentNullException(nameof(entityA)),
					entityB ?? throw new ArgumentNullException(nameof(entityB)),
				},
			});
		}

		/// <summary>
		/// Gets all links that reference the specified entity.
		/// </summary>
		/// <param name="entity">The entity to search for.</param>
		/// <returns>A list of <see cref="Link"/> objects referencing the entity.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="entity"/> is <c>null</c>.</exception>
		public List<Link> GetLinksByEntity(EntityDescriptor entity)
		{
			return GetLinksByEntity(entity?.ID ?? throw new ArgumentNullException(nameof(entity)));
		}

		/// <summary>
		/// Gets all links that reference the specified SDM object.
		/// </summary>
		/// <typeparam name="T">The type of the SDM object.</typeparam>
		/// <param name="sdmObject">The SDM object to search for.</param>
		/// <returns>A list of <see cref="Link"/> objects referencing the SDM object.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="sdmObject"/> is <c>null</c>.</exception>
		public List<Link> GetLinksByEntity<T>(SdmObject<T> sdmObject)
			where T : SdmObject<T>
		{
			if (sdmObject is null)
			{
				throw new ArgumentNullException(nameof(sdmObject));
			}

			return GetLinksByEntity(Convert.ToString(sdmObject.Guid));
		}

		/// <summary>
		/// Gets all links that reference the specified entity ID.
		/// </summary>
		/// <param name="entityId">The ID of the entity to search for.</param>
		/// <returns>A list of <see cref="Link"/> objects referencing the entity ID.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="entityId"/> is <c>null</c> or empty.</exception>
		public List<Link> GetLinksByEntity(string entityId)
		{
			if (String.IsNullOrEmpty(entityId))
			{
				throw new ArgumentNullException(nameof(entityId));
			}

			var result = new List<Link>();

			foreach (var page in Links.ReadPaged(LinkExposers.EntityDescriptors.ID.Equal(entityId)))
			{
				result.AddRange(page);
			}

			return result;
		}

		/// <summary>
		/// Gets all entities that are linked to the specified entity.
		/// </summary>
		/// <param name="entity">The entity to find linked entities for.</param>
		/// <returns>A list of <see cref="EntityDescriptor"/> objects linked to the specified entity.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="entity"/> is <c>null</c>.</exception>
		public List<EntityDescriptor> GetLinkedEntities(EntityDescriptor entity)
		{
			return GetLinkedEntities(entity?.ID ?? throw new ArgumentNullException(nameof(entity)));
		}

		/// <summary>
		/// Gets all entities that are linked to the specified SDM object.
		/// </summary>
		/// <typeparam name="T">The type of the SDM object.</typeparam>
		/// <param name="sdmObject">The SDM object to find linked entities for.</param>
		/// <returns>A list of <see cref="EntityDescriptor"/> objects linked to the specified SDM object.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="sdmObject"/> is <c>null</c>.</exception>
		public List<EntityDescriptor> GetLinkedEntities<T>(SdmObject<T> sdmObject)
			where T : SdmObject<T>
		{
			if (sdmObject is null)
			{
				throw new ArgumentNullException(nameof(sdmObject));
			}

			return GetLinkedEntities(Convert.ToString(sdmObject.Guid));
		}

		/// <summary>
		/// Gets all entities that are linked to the specified entity ID.
		/// </summary>
		/// <param name="entityId">The ID of the entity to find linked entities for.</param>
		/// <returns>A list of <see cref="EntityDescriptor"/> objects linked to the specified entity ID.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="entityId"/> is <c>null</c> or empty.</exception>
		public List<EntityDescriptor> GetLinkedEntities(string entityId)
		{
			if (String.IsNullOrEmpty(entityId))
			{
				throw new ArgumentNullException(nameof(entityId));
			}

			var result = new List<EntityDescriptor>();

			foreach (var page in Links.ReadPaged(LinkExposers.EntityDescriptors.ID.Equal(entityId)))
			{
				result.AddRange(
					page.SelectMany(l =>
						l.EntityDescriptors.Where(e => e.ID != entityId)));
			}

			return result;
		}

		/// <summary>
		/// Gets a link by its unique identifier.
		/// </summary>
		/// <param name="linkId">The unique identifier of the link.</param>
		/// <returns>The <see cref="Link"/> with the specified identifier.</returns>
		/// <exception cref="ArgumentException">Thrown if <paramref name="linkId"/> is <see cref="Guid.Empty"/>.</exception>
		/// <exception cref="KeyNotFoundException">Thrown if no link with the specified identifier is found.</exception>
		public Link GetLinkById(Guid linkId)
		{
			if (linkId == Guid.Empty)
			{
				throw new ArgumentException("Identifier cannot be an empty GUID.", nameof(linkId));
			}

			var link = Links.Read(LinkExposers.Guid.Equal(linkId)).FirstOrDefault();
			if (link is null)
			{
				throw new KeyNotFoundException($"No link found with ID: {linkId}");
			}

			return link;
		}
	}
}
