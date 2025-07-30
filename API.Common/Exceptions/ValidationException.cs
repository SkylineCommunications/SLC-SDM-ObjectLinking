namespace Skyline.DataMiner.SDM.ObjectLinking.Exceptions
{
	using System;
	using System.Runtime.Serialization;

	using DomHelpers.SlcObject_Linking;

	[Serializable]
	public class ValidationException : Exception
	{
		public ValidationException()
		{
		}

		public ValidationException(string message) : base(message)
		{
		}

		public ValidationException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}

	public class LinkValidationException : ValidationException
	{
		protected LinkValidationException(string message)
			: base(message)
		{
		}

		public LinkValidationException(string message, Guid linkGuid)
			: base($"{message} Guid: {linkGuid}")
		{
			Link = new SdmObjectReference<Link>(linkGuid);
		}

		public SdmObjectReference<Link> Link { get; protected set; }
	}

	public class LinkEntityValidationException : LinkValidationException
	{
		public LinkEntityValidationException(string message, Guid linkGuid, string entityId)
			: base($"{message} (Guid: {linkGuid}, entity: {entityId})")
		{
			Link = new SdmObjectReference<Link>(linkGuid);
			EntityID = entityId;
		}

		public string EntityID { get; }
	}
}
