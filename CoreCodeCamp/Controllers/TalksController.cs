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
                var talks = await _repository.GetTalksByMonikerAsync(moniker,true);
              
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
                var talk = await _repository.GetTalkByMonikerAsync(moniker,id,true) ;
                if(talk == null) return NotFound("could not find talk");
                return _mapper.Map<TalkModel>(talk);

            }
            catch (System.Exception)
            {
                //throw;
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get Talk");
            }

        }
        
        //TODO: Test the Post Action 
        [HttpPost]
        public async Task<ActionResult<TalkModel>> Post(string moniker,TalkModel model)
        {

            try
            {
                var camp = await _repository.GetCampAsync(moniker);
                if (camp == null) return BadRequest("Camp does not pexist");

                var talk=_mapper.Map<Talk>(model);
                talk.Camp = camp;
                if (model.Speaker == null) return BadRequest("Speaker ID is Required");
                var speaker = await _repository.GetSpeakerAsync(model.Speaker.SpeakerId);
                if (speaker == null) return BadRequest("Speaker could not be found");
                talk.Speaker = speaker;
               
                _repository.Add(talk);
                if(await _repository.SaveChangesAsync())
                {
                    var url = _linkGenerator.GetPathByAction("Get", "Talks", values : new {moniker,id=talk.TalkId });
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



        [HttpPut("{id:int}")]
        public async Task<ActionResult<TalkModel>> Put(string moniker,int id,TalkModel model)
        {

            try
            {
                var oldTalk = await _repository.GetTalkByMonikerAsync(moniker, id,true);

                if (oldTalk == null) return NotFound($"Could not find talk with moniker of {moniker} and id of {id}");

                _mapper.Map(model, oldTalk);

                if (model.Speaker != null)
                {
                    var speaker=await _repository.GetSpeakerAsync(model.Speaker.SpeakerId);
                    if(speaker != null)
                    {
                      oldTalk.Speaker = speaker;
                    }

                }
                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<TalkModel>(oldTalk);
                }
                else return BadRequest("Faild to Update DataBase");

            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update Talk");

               // throw;
            }
          //  return this.BadRequest();


        }

        [HttpDelete("{id:int}")]
        public async  Task<IActionResult> Delete(string moniker,int id)
        {
            try
            {
                var talk = await _repository.GetTalkByMonikerAsync(moniker, id);
                if(talk == null) { return NotFound("Failed to find the talk to delete"); }
                _repository.Delete(talk);
                if (await _repository.SaveChangesAsync()) return Ok();
                else return BadRequest("Failed to delete Talk");

            }
            catch (System.Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "DataBase Failure");
            }
        }


    }
}
