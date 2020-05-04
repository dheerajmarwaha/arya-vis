using System;
using System.Collections.Generic;
using Arya.Exceptions;
using Arya.Vis.Core.Exceptions;

namespace Arya.Vis.Core.Utils
{
        public static class ValidationUtil {
        public static void NotNull<T>(T obj) where T : class {
            if (obj == null) {
                throw new InvalidArgumentException(typeof(T).Name, null, $"non null {typeof(T).Name}");
            }
        }

        public static void NotEmptyGuid(Guid value, string parameterName) {
            if (value == Guid.Empty) {
                throw new InvalidArgumentException(parameterName, Guid.Empty.ToString(), $"non empty {parameterName}");
            }
        }

        public static void NotEmptyString(string value, string parameterName) {
            if (string.IsNullOrWhiteSpace(value)) {
                throw new InvalidArgumentException(parameterName, value, $"non null or empty {parameterName}");
            }
        }

        public static void NotEmptyCollection<T>(ICollection<T> collection) {
            if (collection == null || collection.Count == 0) {
                throw new InvalidArgumentException(typeof(T).Name, null, $"non null or empty collection of {typeof(T).Name}");
            }
        }

        public static void NotZero(int value, string parameterName) {
            if (value < 1) {
                throw new InvalidArgumentException(parameterName, value.ToString(), $"non zero value for {parameterName}");
            }
        }

        public static void NotFound<T>(T obj, string objectId) where T : class {
            if (obj == null) {
                throw new EntityNotFoundException(typeof(T).Name, objectId);
            }
        }
    }
}