﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebAPIInsuranceManagementSystem.DataAccess.Models;
using WebAPIInsuranceManagementSystem.Services.DTOs;
using WebAPIInsuranceManagementSystem.Services.Services.IServices;

namespace WebAPIInsuranceManagementSystem.Controllers
{
    [Route("api/claim")]
    [ApiController]

    public class ClaimController : ControllerBase
    {
        private readonly IClaimService _claimService;

        public ClaimController(IClaimService claimService)
        {
            _claimService = claimService;
            
        }

        [HttpPost("claims")]
        public async Task<IActionResult> CreateClaim(ClaimDTO claimDTO)
        {
            try
            {
                if (claimDTO == null)
                {
                    return BadRequest("Invalid claim data");
                }

                int claimId = await _claimService.CreateClaim(claimDTO);

                if (claimId == 0)
                {
                    return StatusCode(500, "Failed to create claim");
                }

                return Ok($"Claim created successfully with ID: {claimId}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        
        [HttpPut("claims/{claimId}")]
        public async Task<IActionResult> UpdateClaimStatus(int claimId, ClaimUpdateDTO claimUpdateDTO)
        {
            try
            {
                if (claimId <= 0)
                {
                    return BadRequest("Invalid claim ID");
                }

                await _claimService.UpdateClaimStatus(claimId, claimUpdateDTO.Status);

                return Ok($"Claim status updated successfully to: {claimUpdateDTO.Status}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating claim status: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetClaimDTO>>> GetClaims()
        {
           try
            {
                List<GetClaimDTO> claims = await _claimService.GetAllClaims();

                if(claims == null)
                {
                    return NoContent();
                }

                return Ok(claims);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
