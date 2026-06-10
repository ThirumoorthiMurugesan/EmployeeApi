using EmployeeApi.Models;
using EmployeeApi.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EmployeeApi.Services
{
    public class EmployeeService
    {
        private readonly IMongoCollection<Employee> _employees;

        public EmployeeService(IOptions<MongoDbSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                settings.Value.DatabaseName);

            _employees = mongoDatabase.GetCollection<Employee>(
                settings.Value.CollectionName);
        }

        public async Task<List<Employee>> GetAsync()
        {
            return await _employees.Find(_ => true).ToListAsync();
        }

        public async Task<Employee?> GetAsync(string id)
        {
            return await _employees.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Employee employee)
        {
            await _employees.InsertOneAsync(employee);
        }

        public async Task UpdateAsync(string id, Employee employee)
        {
            await _employees.ReplaceOneAsync(x => x.Id == id, employee);
        }

        public async Task DeleteAsync(string id)
        {
            await _employees.DeleteOneAsync(x => x.Id == id);
        }
    }
}