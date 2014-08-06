using System.Data;
using System.Data.Common;

namespace DbTransmogrifier.Database
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
            if (string.IsNullOrEmpty(_masterConnectionString))
            {
                throw new DbTransmogrifierException("Master connection string required for this operation");
            }
            var connection = _dbProviderFactory.CreateConnection();
            if (connection == null)
            {
                throw new DbTransmogrifierException("Failed to create a Master connection using the specified DbProviderFactory: " + _dbProviderFactory.GetType().Name);
            }
            connection.ConnectionString = _masterConnectionString;
            connection.Open();
            return connection;
        }

        public IDbConnection OpenTarget()
        {
            if (string.IsNullOrEmpty(_targetConnectionString))
            {
                throw new DbTransmogrifierException("Target connection string required for this operation");
            }
            var connection = _dbProviderFactory.CreateConnection();
            if (connection == null)
            {
                throw new DbTransmogrifierException("Failed to create a Target connection using the specified DbProviderFactory: " + _dbProviderFactory.GetType().Name);
            }
            connection.ConnectionString = _targetConnectionString;
            connection.Open();
            return connection;
        }
    }
}