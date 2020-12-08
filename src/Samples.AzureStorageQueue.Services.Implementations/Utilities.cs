#region Using Statements
using Samples.AzureStorageQueue.Models;
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace Samples.AzureStorageQueue.Services.Implementations
{
    public class Utilities
    {
        public static QueueItem GetQueueItem()
        {
            QueueItem toReturn = new QueueItem();
            toReturn.Id = Guid.NewGuid();
            toReturn.Timestamp = DateTime.UtcNow;
            toReturn.Message = new QueueItemMesssage();
            //toReturn.Message.Body = Convert.ToBase64String(Encoding.Default.GetBytes(toReturn.Id.ToString()));
            toReturn.Message.Body = Shared.Utilities.StringHelper.Reverse(toReturn.Id.ToString());

            toReturn.Message.AdditionalProperties = new Dictionary<string, string>();
            int op1 = new Random().Next(0, 2);
            if (op1 == 0)
            {
                toReturn.Message.AdditionalProperties.Add("source", "alpha");
            }
            else
            {
                toReturn.Message.AdditionalProperties.Add("source", "beta");
            }

            int op2 = new Random().Next(0, 2);
            if (op2 != 0)
            {
                toReturn.Message.AdditionalProperties.Add("foreignKey", new Random().Next(0, 9999999).ToString());
            }
            return toReturn;
        }

        public static QueueItem GetQueueItemForApi(string message)
        {
            QueueItem toReturn = new QueueItem();
            toReturn.Id = Guid.NewGuid();
            toReturn.Timestamp = DateTime.UtcNow;
            toReturn.Message = new QueueItemMesssage();
            toReturn.Message.Body = message;

            toReturn.Message.AdditionalProperties = new Dictionary<string, string>();
            toReturn.Message.AdditionalProperties.Add("source", "api");
            return toReturn;
        }

        /// <summary>
        /// Used to convert a string into the Base64 encoded representation.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>string</returns>
        public static string ToBase64EncodedString(string value)
        {
            string result = Convert.ToBase64String(Encoding.Default.GetBytes(value.Trim()));
            return result;
        }

        /// <summary>
        /// Used to decode a Based64 encoded string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>string</returns>
        public static string ToBase64DecodedString(string value)
        {
            string result = Encoding.Default.GetString(Convert.FromBase64String(value.Trim()));
            return result;
        }

    }
}
