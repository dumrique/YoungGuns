using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoungGuns.Shared
{
    public class FormDto
    {
        public uint form_id;
        public string form_name;
        public List<FieldDto> form_fields;
        public bool submitted;
    }
}
