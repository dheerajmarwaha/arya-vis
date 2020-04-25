using System;
using System.Runtime.Serialization;
using System.Security.Permissions;


namespace Arya.Vis.Core.Exceptions
{
[Serializable]
    public class EntityNotFoundException : Exception {

        private readonly string _entityName;
        private readonly string _entityId;
        public EntityNotFoundException(string entityName, string entityId):
            base(GetMessage(entityName, entityId)) {
                _entityName = entityName;
                _entityId = entityId;
            }

        public EntityNotFoundException(string entityName, string entityId, string message):
            base(GetMessage(entityName, entityId, message)) {
                _entityName = entityName;
                _entityId = entityId;
            }

        public EntityNotFoundException(string entityName, string entityId, string message, Exception innerException):
            base(GetMessage(entityName, entityId, message), innerException) {
                _entityName = entityName;
                _entityId = entityId;
            }

        private static string GetMessage(string entityName, string entityId, string message = null) {
            var errorMessage = $"Unable to find requested entity: {entityName} with id: {entityId}";
            if (!string.IsNullOrWhiteSpace(message)) {
                errorMessage += "; " + message;
            }
            return errorMessage;
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected EntityNotFoundException(SerializationInfo info, StreamingContext context):
            base(info, context) {
                _entityName = info.GetString("EntityName");
                _entityId = info.GetString("EntityId");
            }

        public string EntityName {
            get { return _entityName; }
        }

        public string EntityId {
            get { return _entityId; }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            if (info == null) {
                throw new ArgumentNullException(nameof(info));
            }
            info.AddValue("EntityName", _entityName);
            info.AddValue("EntityId", _entityId);
            base.GetObjectData(info, context);
        }
    }

}