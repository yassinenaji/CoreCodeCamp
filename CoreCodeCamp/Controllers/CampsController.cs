using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository _campRepository;
        private readonly IMapper _mapper;
        public CampsController(ICampRepository campRepository, IMapper mapper)
        {
            this._campRepository = campRepository;
            this._mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<CampModel[]>> Get(bool includeTalks = false)
        {

            try
            {


                var results = await _campRepository.GetAllCampsAsync(includeTalks);
                //Use the model
                return _mapper.Map<CampModel[]>(results);
                //Use the entity
                //return results;



            }
            catch (Exception)
            {

                //return this.StatusCode(StatusCodes.Status500InternalServerError,"DataBase Failure");
                throw;
            }
        }

        [HttpGet("{moniker}")]
        //[HttpGet("{moniker:int}")] to ensure it matches both type and name
        public async Task<ActionResult<CampModel>> Get(string moniker)
        {

            try
            {


                var results = await _campRepository.GetCampAsync(moniker);
                if (results == null) return NotFound();
                return _mapper.Map<CampModel>(results);




            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "DataBase Failure");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<CampModel[]>> SearchByDate(DateTime date,bool includeTalks)
        {
            try
            {
                var res = await _campRepository.GetAllCampsByEventDate(date,includeTalks);
                if(!res.Any()) return NotFound(); 
                return _mapper.Map<CampModel[]>(res);
            }
            catch (Exception)
            {

                throw;
            }
            
            

        }
    }
}
