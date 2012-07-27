using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DynamicDataAccess
{
    public class Parameter
    {
        public string Name
        {
            get;
            set;
        }

        public object Value
        {
            get;
            set;
        }

        internal IDataParameter ToDataParameter(IDbCommand command)
        {
            var param = command.CreateParameter();
            param.ParameterName = Name;
            param.Value = Value;

            return param;
        }
    }
}
