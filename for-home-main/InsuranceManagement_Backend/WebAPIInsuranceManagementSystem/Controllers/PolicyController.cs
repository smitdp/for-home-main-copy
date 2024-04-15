using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIInsuranceManagementSystem.Services.DTOs;
using WebAPIInsuranceManagementSystem.Services.Services.IServices;

namespace WebAPIInsuranceManagementSystem.Controllers
{
    [Route("api/policy")]
    [ApiController]
    //[Authorize]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _policyService;

        public PolicyController(IPolicyService policyService)
        {
            _policyService = policyService;
        }


        //[Authorize(Roles = "Insurer, Claimant")]
        [HttpGet]
        public async Task<IActionResult> GetPolicies()
        {
            try
            {
                List<PolicyDTO> policies = await _policyService.GetAllPolicies();
                if (policies == null || !policies.Any())
                {
                    return NoContent(); // 204
                }
                return Ok(policies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("user-policies")]
        public async Task<IActionResult> GetAllUserPolicies()
        {
            try
            {
                List<UserPolicyInfoDTO> userPolicies = await _policyService.GetAllUserPolicies();

                if (userPolicies == null || !userPolicies.Any())
                {
                    return NoContent();
                }

                return Ok(userPolicies);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
