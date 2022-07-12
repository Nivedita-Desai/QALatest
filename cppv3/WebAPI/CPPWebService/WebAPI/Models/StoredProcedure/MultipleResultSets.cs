using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

public static class MultipleResultSets
{

    public static MultipleResultSetWrapper MultipleResults(this DbContext db, string query, IEnumerable<MySqlParameter> parameters = null) => new MultipleResultSetWrapper(db: db, query: query, parameters: parameters);
    
        
    public class MultipleResultSetWrapper
    {

        
        public List<Func<DbDataReader, IEnumerable>> _resultSets;

       
        private readonly IObjectContextAdapter _Adapter;
        private readonly string _CommandText;
        private readonly DbContext _db;
        private readonly IEnumerable<MySqlParameter> _parameters;
        

        
        public MultipleResultSetWrapper(DbContext db, string query, IEnumerable<MySqlParameter> parameters = null)
        {
            _db = db;
            _Adapter = db;
            _CommandText = query;
            _parameters = parameters;
            _resultSets = new List<Func<DbDataReader, IEnumerable>>();
        }
        

       
        public MultipleResultSetWrapper AddResult<TResult>()
        {
            _resultSets.Add(OneResult<TResult>);
            return this;
        }

        public List<IEnumerable> Execute()
        {
            var results = new List<IEnumerable>();

            using (var connection = _db.Database.Connection)
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = _CommandText;
                if (_parameters?.Any() ?? false) { command.Parameters.AddRange(_parameters.ToArray()); }
                using (var reader = command.ExecuteReader())
                {
                    foreach (var resultSet in _resultSets)
                    {
                        results.Add(resultSet(reader));
                    }
                }

                return results;
            }
        }
       
        private IEnumerable OneResult<TResult>(DbDataReader reader)
        {
            var result = _Adapter
                .ObjectContext
                .Translate<TResult>(reader)
                .ToArray();
            reader.NextResult();
            return result;
        }

    }

}