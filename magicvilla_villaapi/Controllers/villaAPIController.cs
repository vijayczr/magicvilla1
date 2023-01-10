using AutoMapper;
using magicvilla_villaapi.Data;
using magicvilla_villaapi.Logging;
using magicvilla_villaapi.models;
using magicvilla_villaapi.models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace magicvilla_villaapi.Controllers
{
    [Route("api/villaAPI")]
    [ApiController]
    public class villaAPIController : ControllerBase
    {
        //private readonly ILogger<villaAPIController> _lodger;      //_logger

        //public villaAPIController(ILogger<villaAPIController> lodger)    //logger
        //{
        //    this._lodger = lodger;
        //}
        

        private readonly ApplicationBDContext _db;
        public readonly IMapper _mapper;
        public villaAPIController(ApplicationBDContext db , IMapper mapper) 
        {
            _db= db;
            _mapper= mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()

        {
            IEnumerable<Villa> villalist = await _db.Villas.ToListAsync();
            return Ok(_mapper.Map<List<VillaDTO>>(villalist));
        }

        [HttpGet("{id:int}", Name = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)

        {
            if (id == 0)
            {
               
                return BadRequest();
            }
            var villa =await _db.Villas.FirstOrDefaultAsync(u => u.ID == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<VillaDTO>(villa));
        }
 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> Createvilla([FromBody] VillacreateDTO createDTO)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            if (await _db.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("customerror", "villa already exist");
                return BadRequest(ModelState);
            }
            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }
            //if (villaDTO.ID > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}
            Villa model = _mapper.Map<Villa>(createDTO);
            //Villa model = new()
            //{   Amenity = createDTO.Amenity,
            //    Details = createDTO.Details,
            //    ImageUrl = createDTO.ImageUrl,
            //    Name = createDTO.Name,
            //    Occupancy = createDTO.Occupancy,
            //    Rate = createDTO.Rate,
            //    Sqrt = createDTO.Sqrt
            //};
            await _db.Villas.AddAsync(model);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("get", new { id = model.ID }, model);
        }
        [HttpDelete("{id:int}", Name = "del")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deletevilla(int id)
        {
            
            if ( id== 0)
            {
                return BadRequest();
            }
            var villa =await _db.Villas.Where(u => u.ID == id).FirstOrDefaultAsync();
            if (villa == null)
            {
                return NotFound();
            }
            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> updatevilla(int id, [FromBody] VillaupdateDTO updateDTO)
        {
            if (updateDTO.ID == 0 || id != updateDTO.ID)
            {
                return BadRequest();
            }
            //  var villa = VillaStore.villaList.FirstOrDefault(u => u.ID == id);
            //villa.Name=villaDTO.Name;
            //villa.Srft=villaDTO.Srft;
            //villa.Occupancy = villaDTO.Occupancy;
            //var villa = _db.Villas.Where(w => w.ID == villaDTO.ID).FirstOrDefault();
            Villa model = _mapper.Map<Villa>(updateDTO);
        //    Villa model = new()
        //    {
        //        ID = updateDTO.ID,
        //    Name = updateDTO.Name,
        //    Amenity = updateDTO.Amenity,
        //    Sqrt = updateDTO.Sqrt,
        //    Rate = updateDTO.Rate,
        //    Details = updateDTO.Details,
        //    ImageUrl = updateDTO.ImageUrl,
        //    Occupancy = updateDTO.Occupancy

        //};
            _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        [HttpPatch("{id:int}", Name = "partialupdate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> partialupdatevilla(int id,JsonPatchDocument<VillaupdateDTO> patchDTO)
        {
            if(patchDTO == null || id==0)
            {
                return BadRequest();
            }
            var villa = await _db.Villas.AsNoTracking().Where(u => u.ID == id).FirstOrDefaultAsync();

            VillaupdateDTO villaDTO = _mapper.Map<VillaupdateDTO>(villa);
            //VillaupdateDTO villaDTO = new()
            //{   ID = villa.ID,
            //    Amenity = villa.Amenity,
            //    Details = villa.Details,
            //    ImageUrl = villa.ImageUrl,
            //    Name = villa.Name,
            //    Occupancy = villa.Occupancy,
            //    Rate = villa.Rate,
            //    Sqrt = villa.Sqrt
            //};
            if (villa == null)
            {
                return BadRequest();
            }
           patchDTO.ApplyTo(villaDTO,ModelState);
            Villa model = _mapper.Map<Villa>(villaDTO);
            //Villa model = new()
            //{     
            //    Amenity = villaDTO.Amenity,
            //    Details = villaDTO.Details,
            //    ImageUrl = villaDTO.ImageUrl,
            //    ID= villaDTO.ID,
            //    Name = villaDTO.Name,
            //    Occupancy = villaDTO.Occupancy,
            //    Rate = villaDTO.Rate,
            //    Sqrt = villaDTO.Sqrt
            //};
            _db.Update(model);
            await _db.SaveChangesAsync(); 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent() ;
        }
    }
} 
