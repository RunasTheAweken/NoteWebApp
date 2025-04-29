
namespace Services
{
    using System;
    using BCrypt.Net;
    using Microsoft.Extensions.Logging;
    public class Hasher : IHasher
    {

        private readonly ILogger _logger;
        public Hasher(ILogger<Hasher> logger)
        {
            _logger = logger;
        }


        public string HashString(string hash)
        {
            if (string.IsNullOrEmpty(hash))
            {
                _logger.LogWarning("Hasher : [Error] Input Data null or empty");
                throw new ArgumentNullException(nameof(hash));
            }
            string result = BCrypt.HashPassword(hash);
            if(_logger.IsEnabled(LogLevel.Debug))
                _logger.LogInformation("Hasher: [Success] Data Hashed Successfully");
            return result;
        } 
        public bool IsValid(string input, string hash){
            if(string.IsNullOrEmpty(input) || string.IsNullOrEmpty(hash)){
                _logger.LogError("Hasher: [Error] Input or hash is null or empty");
                throw new ArgumentNullException("Hash or Input are missing");
            }
            bool isValid = BCrypt.Verify(input, hash);
            if(_logger.IsEnabled(LogLevel.Debug))
                _logger.LogInformation($"Hasher: [Success] Hash verification complete");
            return isValid;
        }
    }

}
public interface IHasher{
    public string HashString(string hash);
    public bool IsValid(string input, string hash);

}