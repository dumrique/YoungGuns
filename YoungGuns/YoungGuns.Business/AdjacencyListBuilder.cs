using System;
using System.Collections.Generic;
using YoungGuns.Shared;

namespace YoungGuns.Business
{
    public class AdjacencyListBuilder
    {

        public static void ExtractAndStoreAdjacencyList(TaxSystemDto dto)
        {
            Dictionary<uint, List<uint>> adjLists = new Dictionary<uint, List<uint>>();
            foreach (FieldDto field in dto.form_fields)
            {
                adjLists[field.field_id] = BuildAdjList(dto.form_fields, field.field_id);
            }
        }

        private static List<uint> BuildAdjListWorker(List<FieldDto> allFields, object field_id)
        {
            return null;
        }
    }
}
