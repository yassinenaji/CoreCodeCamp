using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace CoreCodeCamp.Controllers
{
    [Route("api/camps/{moniker}/[controller]")]
    [ApiController]
    public class TalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICampRepository _repository;
        private readonly LinkGenerator _linkGenerator;

        public TalksController(ICampRepository repository,IMapper mapper,LinkGenerator linkGenerator)
        {
            this._mapper = mapper; 
            this._repository = repository;
            this._linkGenerator = linkGenerator;

        }

        [HttpGet]
        public async Task<ActionResult<TalkModel[]>> Get(string moniker)
        {

            try
            {
                var talks = await _repository.GetTalksByMonikerAsync(moniker);
                return _mapper.Map<TalkModel[]>(talks);

            }
            catch (System.Exception)
            {
                //throw;
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get Talks");
            }

        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<TalkModel>> Get(string moniker, int id)
        {

            try
            {
                var talk = await _repository.GetTalkByMonikerAsync(moniker,id) ;
                return _mapper.Map<TalkModel>(talk);

            }
            catch (System.Exception)
            {
                //throw;
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get Talk");
            }

        }
        
        [HttpPost]
        public async Task<ActionResult<TalkModel>> Post(string moniker,TalkModel model)
        {

            try
            {
                var camp = await _repository.GetCampAsync(moniker);
                if (camp == null) return BadRequest("Camp does not exist");

                var talk=_mapper.Map<Talk>(model);
                talk.Camp = camp;
                _repository.Add(talk);
                if(await _repository.SaveChangesAsync())
                {
                    var url = _linkGenerator.GetPathByAction("Get", "Talks", new {moniker,id=talk.TalkId });
                    return Created(url,_mapper.Map<TalkModel>(talk));
                }
                else
                {
                    return BadRequest("Failed to save new Talk");
                }
                        }
            catch (System.Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add Talk");
            }
        }


    }
}
