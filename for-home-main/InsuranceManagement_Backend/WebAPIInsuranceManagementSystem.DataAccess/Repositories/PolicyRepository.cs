using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIInsuranceManagementSystem.DataAccess.Models;
using WebAPIInsuranceManagementSystem.DataAccess.Repositories.IRepositories;

namespace WebAPIInsuranceManagementSystem.DataAccess.Repositories
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly InsuranceandclaimmanagementdbContext _context;
        public PolicyRepository(InsuranceandclaimmanagementdbContext context)
        {
            _context = context;
        }

        public async Task<List<Policy>> GetAllPolicies()
        {
            try
            {
                List<Policy> policies = await _context.Policies.Include(p => p.PolicyType).ToListAsync();
                return policies;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve policies from the database", ex);
            }
        }

        public async Task<List<UserPolicy>> GetAllUserPolicies()
        {
            try
            {
                List<UserPolicy> userPolicies = await _context.UserPolicies
                    .Include(p => p.Policy)
                    .Include(u => u.User)
                    .Include (u => u.Agent)
                    .ToListAsync();
                return userPolicies;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve policies from the database", ex);
            }
        }
    }
}
