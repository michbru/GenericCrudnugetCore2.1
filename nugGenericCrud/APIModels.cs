using System;
using System.Collections.Generic;
using System.Text;

namespace nugGenericCrud
{

    public class APISend
    {

        public string p_entity { set; get; }
        public string p_entity_sql { set; get; }
        public string p_sqlWhere { set; get; }
        public string p_sqlOrder { set; get; }

    }

    public class APISendSave
    {

        public string p_entity { set; get; }
        public string p_recId { set; get; }
        public string p_primeKey { set; get; }
        public dynamic record { set; get; }
    }

}
