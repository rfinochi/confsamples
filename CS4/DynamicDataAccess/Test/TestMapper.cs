using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicDataAccess;
using System.Data;

namespace ZPod.DynamicDataAccess.Test
{
    class CountryNameMapper : IRecordMapper
    {
        public dynamic Map(IDataRecord record, string fieldName, dynamic currentValue)
        {
            if (fieldName == "Name")
            {
                currentValue = "Country Name: " + currentValue;
            }

            return currentValue;
        }


        public bool CanProcessField(IDataRecord record, string fieldName)
        {
            for (int i = 0; i < record.FieldCount; i++)
            {
                if (record.GetName(i) == fieldName)
                {
                    return true;
                }
            }

            return false;
        }
    }

    class SquareBracketsMapper : IRecordMapper
    {
        public dynamic Map(IDataRecord record, string fieldName, dynamic currentValue)
        {
            if (fieldName == "Name")
            {
                currentValue = "[" + currentValue + "]";
            }

            return currentValue;
        }

        public bool CanProcessField(IDataRecord record, string fieldName)
        {
            for (int i = 0; i < record.FieldCount; i++)
            {
                if (record.GetName(i) == fieldName)
                {
                    return true;
                }
            }

            return false;
        }
    }

    class CountFieldMapper : IRecordMapper
    {
        public dynamic Map(IDataRecord record, string fieldName, dynamic currentValue)
        {
            if (fieldName == "FieldCount")
            {
                currentValue = record.FieldCount;
            }

            return currentValue;
        }

        public bool CanProcessField(IDataRecord record, string fieldName)
        {
            return fieldName == "FieldCount";
        }
    }

}
