using Microsoft.AspNetCore.Mvc;
using Picklerick.Data;
using Picklerick.Models;
using Picklerick.Dtos;
using AutoMapper;


namespace Picklerick.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RicksController : ControllerBase
    {
        private readonly DataContextEF _entityFramework;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RickToAddOrUpdateDto, Rick>();
            }));
        public RicksController(IConfiguration config, DataContextEF? mockedDataContextEF = null)
        {
            if (mockedDataContextEF != null)
            {
                _entityFramework = mockedDataContextEF;
            }
            else
            {
                _entityFramework = new DataContextEF(config);
            }

            _config = config;
        }


        [HttpGet("")]
        [ProducesResponseType<IEnumerable<Rick>>(StatusCodes.Status200OK)]
        public IResult GetRicks()
        {
            IEnumerable<Rick> Ricks = _entityFramework.Ricks.ToList<Rick>();
            return Results.Ok(Ricks);
        }

        [HttpGet("{rickId}")]
        [ProducesResponseType<Rick>(StatusCodes.Status200OK)]
        public IResult GetSingleRick(int rickId)
        {
            Rick? rick = _entityFramework.Ricks
                .Where(u => u.Id == rickId)
                .FirstOrDefault<Rick>();

            return rick != null ? Results.Ok(rick) : Results.NotFound();
        }

        [HttpPost("")]
        [ProducesResponseType<Rick>(StatusCodes.Status201Created)]

        public IResult AddRick(RickToAddOrUpdateDto rick)
        {
            if (_entityFramework.Ricks.Any(r => r.Universe == rick.Universe))
            {
                return Results.BadRequest("There is already a Rick in this universe.");
            }

            Rick RickDb = _mapper.Map<Rick>(rick);

            using var dbContext = new DataContextEF(_config);
            _entityFramework.Add(RickDb);

            if (_entityFramework.SaveChanges() > 0)
            {
                return Results.Created("Success", RickDb);
            }

            throw new Exception("Failed to Add Rick");
        }

        [HttpPut("{rickId}")]
        [ProducesResponseType<Rick>(StatusCodes.Status200OK)]
        public IResult EditRick(RickToAddOrUpdateDto rick, int rickId)
        {
            if (_entityFramework.Ricks.Any(r => r.Id != rickId && r.Universe == rick.Universe))
            {
                return Results.BadRequest("There is already a Rick in this universe.");
            }
            Rick? RickDb = _entityFramework.Ricks
                .Where(r => r.Id == rickId)
                .FirstOrDefault<Rick>();

            if (RickDb != null)
            {
                using var dbContext = new DataContextEF(_config);
                RickDb.Universe = rick.Universe;
                RickDb.IsMortyAlive = rick.IsMortyAlive;
                if (_entityFramework.SaveChanges() > 0)
                {
                    return Results.Ok(RickDb);
                }

                throw new Exception("Failed to update Rick.");
            }
            return Results.NotFound();
        }

        [HttpDelete("{rickId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IResult DeleteRick(int rickId)
        {
            Rick? RickDb = _entityFramework.Ricks
                .Where(r => r.Id == rickId)
                .FirstOrDefault<Rick>();

            if (RickDb != null)
            {
                _entityFramework.Ricks.Remove(RickDb);
                if (_entityFramework.SaveChanges() > 0)
                {
                    return Results.NoContent();
                }

                throw new Exception("Failed to Delete Rick.");
            }

            return Results.NotFound();
        }
    }
}