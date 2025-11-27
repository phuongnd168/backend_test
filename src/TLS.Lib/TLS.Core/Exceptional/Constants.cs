using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Core.Exceptional
{
    internal class ExceptionalConstants
    {
        //
        // Summary:
        //     Key for storing errors that happen when fetching custom data.
        public const string CustomDataErrorKey = "CustomDataFetchError";
        //
        // Summary:
        //     Key for storing errors that happen when fetching data from a collection in the
        //     request, e.g. ServerVariables, Cookies, etc.
        public const string CollectionErrorKey = "CollectionFetchError";
        //
        // Summary:
        //     Key for prefixing fields in .Data for logging to CustomData
        public const string CustomDataKeyPrefix = "ExceptionalCustom-";
    }
}
