using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository _campRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        public CampsController(ICampRepository campRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            this._campRepository = campRepository;
            this._mapper = mapper;
            this._linkGenerator = linkGenerator;

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
        public async Task<ActionResult<CampModel[]>> SearchByDate(DateTime date, bool includeTalks)
        {
            try
            {
                var res = await _campRepository.GetAllCampsByEventDate(date, includeTalks);
                if (!res.Any()) return NotFound();
                return _mapper.Map<CampModel[]>(res);
            }
            catch (Exception)
            {

                throw;
            }



        }

        public async Task<ActionResult<CampModel>> Post(CampModel model)
        {

            try
            {
                var existingCamp = await _campRepository.GetCampAsync(model.Moniker);
                if(existingCamp != null)
                {
                    return BadRequest("Moniker in use");
                }
                var location = _linkGenerator.GetPathByAction("GET","Camps",new {monkier = model.Moniker});

                if (string.IsNullOrEmpty(location))
                {
                    return BadRequest("Could not use current moniker");
                }
                var camp=_mapper.Map<Camp>(model);
                _campRepository.Add(camp);
              if(  await _campRepository.SaveChangesAsync())
                {
                    //using the hard coded way and its not practicle to use
                    return this.Created($"/api/camps/{camp.Moniker}",_mapper.Map<CampModel>(camp));
                   //Using LinkGenerator to generate URI
                  //  return _linkGenerator.
                }
      
            }
            catch (Exception)
            {

                  return this.StatusCode(StatusCodes.Status500InternalServerError, "DataBase Failure");
                // throw;
            }
            return this.BadRequest();
        }


        [HttpPut("{moniker}")]
        //In put method we put the entire obejct in the body to modify the old object
        public  async Task<ActionResult<CampModel>> Put(string moniker,CampModel model)
        {
            try
            {

                var oldCamp = await _campRepository.GetCampAsync(moniker);
                if (oldCamp == null) return NotFound($"Could not find camp with moniker of {moniker}");

                _mapper.Map(model, oldCamp);
                if(await _campRepository.SaveChangesAsync())
                {
                    return _mapper.Map<CampModel>(oldCamp);
                }

            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "DataBase Failure");
                // throw;
            }
            return this.BadRequest();

        }

        [HttpDelete("{moniker}")]
        public async Task<IActionResult> Delete(string moniker)
        {
            try
            {
                var camp = await _campRepository.GetCampAsync(moniker);
                if (camp == null) { return NotFound("Failed to find the camp to delete"); }
                _campRepository.Delete(camp);
                if (await _campRepository.SaveChangesAsync()) return Ok();
                else return BadRequest("Failed to delete Camp");

            }
            catch (System.Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "DataBase Failure");
            }
        }

    }
}
