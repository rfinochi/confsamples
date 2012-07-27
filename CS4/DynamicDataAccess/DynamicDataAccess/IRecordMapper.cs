using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DynamicDataAccess
{
    public interface IRecordMapper
    {
        dynamic Map(IDataRecord record, string fieldName, dynamic currentValue);

        bool CanProcessField(IDataRecord record, string fieldName);
    }
}
