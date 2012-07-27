using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Dynamic;

namespace DynamicDataAccess
{
    public class DynamicRecord : DynamicObject
    {
        IRecordMapper[] _mappers;
        IDataRecord _record;
        List<string> _fieldNames;

        public DynamicRecord(IDataRecord record, IRecordMapper[] mappers)
        {
            _mappers = mappers;
            _record = record;
            _fieldNames = GetDynamicMemberNames().ToList(); ;
        }

        private dynamic ProcessField(IDataRecord record, string fieldName, object currentValue)
        {
            foreach (var mapper in _mappers)
            {
                if (mapper.CanProcessField(record, fieldName))
                {
                    currentValue = mapper.Map(record, fieldName, currentValue);
                }
            }

            return currentValue;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            for (int i = 0; i < _record.FieldCount; i++)
            {
                yield return _record.GetName(i);
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            try
            {
                result = GetMember(binder.Name);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        private object GetMember(string memberName)
        {
            object currentValue = null;
            if (_fieldNames.Contains(memberName))
            {
                currentValue = _record[memberName];
            }
            
            return ProcessField(_record, memberName, currentValue);
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length > 1)
            {
                result = null;
                return false;
            }

            try
            {
                if (indexes[0] is int)
                {
                    result = GetMember(_record.GetName((int)indexes[0]));
                    return true;
                }
                else if (indexes[0] is string)
                {
                    result = GetMember((string)indexes[0]);
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }
            catch
            {
                result = null;
                return false;
            }
        }
    }
}
