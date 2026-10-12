using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ManagerStudentSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LevelController : ControllerBase
    {
        private readonly ILogger<LevelController> _logger;
        private readonly ILevelsService _levelsService;

        public LevelController(ILogger<LevelController> logger, ILevelsService levelsService)
        {
            _logger = logger;
            _levelsService = levelsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLevels()
        {
            try
            {
                var levels = await _levelsService.GetAllLevelsAsync();
                return Ok(levels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all levels.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLevelById(int id)
        {
            try
            {
                var level = await _levelsService.GetLevelByIdAsync(id);
                if (level == null)
                {
                    return NotFound();
                }
                return Ok(level);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the level with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateLevel([FromBody] LevelDTO level)
        {
            try
            {
                if (level == null)
                {
                    return BadRequest("Level data is null.");
                }

                var createdLevel = await _levelsService.CreateLevelAsync(level);
                return CreatedAtAction(nameof(GetLevelById), new { id = createdLevel.LevelName }, createdLevel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new level.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLevel(int id, [FromBody] LevelDTO level)
        {
            try
            {
                if (level == null)
                {
                    return BadRequest("Level data is null.");
                }

                var updatedLevel = await _levelsService.UpdateLevelAsync(id, level);
                if (updatedLevel == null)
                {
                    return NotFound();
                }
                return Ok(updatedLevel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the level with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLevel(int id)
        {
            try
            {
                var isDeleted = await _levelsService.DeleteLevelAsync(id);
                if (!isDeleted)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the level with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }


    }
}