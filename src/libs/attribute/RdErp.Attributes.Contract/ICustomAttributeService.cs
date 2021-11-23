using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RdErp.Attributes
{
    public interface ICustomAttributeService
    {
        /// <summary>
        /// Returns custom attribute value info for all values of custom attributes.
        /// Methods returns all existing, even not active, values.
        /// </summary>
        Task<CustomAttributeInfo[]> LookupValues(CustomAttributeValue[] values);

        /// <summary>
        /// Gets a list of allowed attribute values based on entered values.
        /// Method filters out available values based on value of parent attribute (if exists).
        ///
        /// If <paramref name="values" /> are null, returns all available attributes.
        /// If attribute value exists in <paramref name="values" /> it should be returned
        /// even if it filtered out by parent value.
        /// </summary>
        Task<Dictionary<string, CustomAttributeInfo[]>> GetAttributeValueList(CustomAttributeValue[] values);
    }
}