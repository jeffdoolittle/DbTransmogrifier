using System.Data;
using System.Data.Common;

namespace DbTransmogrifier
{
    public interface IConnectionFactory
    {
        IDbConnection OpenMaster();
        IDbConnection OpenTarget();
    }

    public class ConnectionFactory : IConnectionFactory
    {
        private readonly DbProviderFactory _dbProviderFactory;
        private readonly string _masterConnectionString;
        private readonly string _targetConnectionString;

        public ConnectionFactory(DbProviderFactory dbProviderFactory, string masterConnectionString, string targetConnectionString)
        {
            _dbProviderFactory = dbProviderFactory;
            _masterConnectionString = masterConnectionString;
            _targetConnectionString = targetConnectionString;
        }

        public IDbConnection OpenMaster()
        {
            var connection = _dbProviderFactory.CreateConnection();
            connection.ConnectionString = _masterConnectionString;
            connection.Open();
            return connection;
        }

        public IDbConnection OpenTarget()
        {
            var connection = _dbProviderFactory.CreateConnection();
            connection.ConnectionString = _targetConnectionString;
            connection.Open();
            return connection;
        }
    }
}