using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DynamicDataAccess
{
    public class DynamicConnection : IDisposable
    {
        IRecordMapper[] _mappers;
        IDbConnection _conn;

        public DynamicConnection(IDbConnection realConnection, params IRecordMapper[] mappers)
        {
            if (realConnection == null)
            {
                throw new ArgumentNullException("realConnection");
            }

            if (mappers == null)
            {
                mappers = new IRecordMapper[0];
            }

            _conn = realConnection;
            _mappers = mappers;
        }

        public static Parameter CreateParameter(string name, object value)
        {
            return new Parameter() { Name = name, Value = value };
        }

        private static Parameter[] CreateParameters(object[] values)
        {
            Parameter[] ret = new Parameter[values == null ? 0 : values.Length];
   
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = CreateParameter("@" + i.ToString(), values[i]);
            }

            return ret;
        }

        public IEnumerable<DynamicRecord> Query(string query)
        {
            return Query(query, new Parameter[0]);
        }

        public IEnumerable<DynamicRecord> Query(string query, params object[] parameters)
        {
            return Query(query, CreateParameters(parameters));
        }

        public IEnumerable<DynamicRecord> Query(string query, params Parameter[] parameters)
        {
            OpenConnection();
            using (var command = _conn.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = query;

                if (parameters != null)
                {
                    parameters.AsParallel().ForAll(p =>
                        {
                            command.Parameters.Add(p.ToDataParameter(command));
                        });
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return new DynamicRecord(reader, _mappers);
                    }
                }
            }
        }

        public dynamic QueryValue(string query, params object[] parameters)
        {
            return QueryValue(query, CreateParameters(parameters));
        }

        public dynamic QueryValue(string query, params Parameter[] parameters)
        {
            OpenConnection();
            using (var command = _conn.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = query;

                if (parameters != null)
                {
                    parameters.AsParallel().ForAll(p =>
                    {
                        command.Parameters.Add(p.ToDataParameter(command));
                    });
                }

                return command.ExecuteScalar();
            }
        }

        private void OpenConnection()
        {
            if (_conn.State == ConnectionState.Closed)
            {
                _conn.Open();
            }
        }

        public void Dispose()
        {
            _conn.Dispose();
        }
    }
}
