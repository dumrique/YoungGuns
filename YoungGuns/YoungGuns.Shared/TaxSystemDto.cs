using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoungGuns.Shared
{
    public class TaxSystemDto
    {
        public string taxsystem_id;
        public string taxsystem_name;
        public List<FieldDto> taxsystem_fields;
        public bool submitted;
    }
}
