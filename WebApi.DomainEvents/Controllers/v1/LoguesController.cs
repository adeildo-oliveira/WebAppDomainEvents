using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DomainEvents.Models.MongoView;
using WebAppDomainEvents.Domain.Interfaces.Repository.Logue;

namespace WebApi.DomainEvents.Controllers.v1
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class LoguesController : ControllerBase
    {
        private readonly ILogueMongodb<LogueView> _logs;
        public LoguesController(ILogueMongodb<LogueView> logs) => _logs = logs;

        [HttpGet]
        [Route("GetAsync")]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var resultado = await _logs.GetMongoCollection().FindAsync(_ => true);
                return StatusCode(200, resultado.ToList().OrderByDescending(x => x.Timestamp));
            }
            catch (Exception erro)
            {
                return StatusCode(500, erro.Message);
            }
        }

        [HttpGet]
        [Route("GetAsync/{level}")]
        public async Task<IActionResult> GetAsync(string level)
        {
            try
            {
                var resultado = await _logs.GetMongoCollection().FindAsync(x => x.Level == level);
                return StatusCode(200, resultado.ToList().OrderByDescending(x => x.Timestamp));
            }
            catch (Exception erro)
            {
                return StatusCode(500, erro.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteAllAsync")]
        public async Task<IActionResult> DeleteAllAsync()
        {
            try
            {
                await _logs.GetMongoCollection().DeleteManyAsync(x => x.Timestamp <= DateTime.Now);
                return StatusCode(200, "Exclusão executada com sucesso");
            }
            catch (Exception erro)
            {
                return StatusCode(500, erro.Message);
            }
        }
    }
}
